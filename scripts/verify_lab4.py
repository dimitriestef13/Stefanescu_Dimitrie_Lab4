import subprocess,json,re,html,urllib.request,urllib.error
from pathlib import Path
from urllib.parse import urlencode
sql="SET NOCOUNT ON; SELECT (SELECT b.ID,b.Title,b.PublisherID,a.FirstName,a.LastName FROM Book b LEFT JOIN Authors a ON a.ID=b.AuthorID FOR JSON PATH) Books,(SELECT ID FROM Publisher FOR JSON PATH) Publishers,(SELECT ID FROM Category FOR JSON PATH) Categories,(SELECT BookID,CategoryID FROM BookCategory FOR JSON PATH) Links FOR JSON PATH,WITHOUT_ARRAY_WRAPPER"
r=subprocess.run(['sqlcmd','-S',r'(localdb)\mssqllocaldb','-d','Stefanescu_Dimitrie_Lab2Context','-E','-Q',sql,'-y','0','-w','65535'],capture_output=True,text=True,check=True)
data=json.loads(r.stdout.strip()); data={k:json.loads(v) if isinstance(v,str) else v for k,v in data.items()}
base='http://localhost:5184'; passed=[]
def get(path):
 with urllib.request.urlopen(base+path) as r:
  assert r.status==200
  return r.read().decode()
def rows(s):
 tables=re.findall(r'<tbody>(.*?)</tbody>',s,re.S)
 return [[html.unescape(re.sub('<[^>]+>','',c)).strip() for c in re.findall(r'<td[^>]*>(.*?)</td>',row,re.S)] for row in re.findall(r'<tr[^>]*>(.*?)</tr>',tables[-1],re.S)]
def check(label,condition):
 assert condition,label
 passed.append(label)
books=data['Books']
for route in ['/','/BookPages','/BookPages/Index','/Home','/Privacy','/PublisherPages','/PublisherPages/Index','/Categories','/Categories/Index','/AuthorsPages']:
 get(route); passed.append('HTTP 200 '+route)
for order in ['', 'title_desc','author','author_desc']:
 actual=[x[0] for x in rows(get('/?'+urlencode({'sortOrder':order}))) if len(x)==7]
 key=(lambda b:(b.get('FirstName','')+' '+b.get('LastName','')).strip().casefold()) if order.startswith('author') else lambda b:b['Title'].casefold()
 expected=[b['Title'] for b in sorted(books,key=key,reverse=order.endswith('desc'))]
 # Equal author names may occur; verify key sequence, then all titles.
 values={b['Title']:key(b) for b in books}
 check('sort '+(order or 'title_asc'),sorted(actual)==sorted(expected) and [values[x] for x in actual]==[values[x] for x in expected])
for field in ['Title','FirstName','LastName']:
 term=next(b[field] for b in books if b.get(field))
 expected=[b['Title'] for b in books if any(term.casefold() in b.get(f,'').casefold() for f in ['Title','FirstName','LastName'])]
 for order in ['', 'title_desc','author','author_desc']:
  page=get('/?'+urlencode({'SearchString':term,'sortOrder':order}))
  actual=[x[0] for x in rows(page) if len(x)==7]
  check('search '+field+' '+(order or 'default'), sorted(actual)==sorted(expected))
  check('filter retained '+field+' '+order,html.escape(term,quote=True) in page or term in html.unescape(page))
check('no results', 'Nu au fost' in get('/?SearchString=zzzz_lab4_no_match'))
check('whitespace filter', len([x for x in rows(get('/?SearchString=%20%20')) if len(x)==7])==len(books))
for entity,path,foreign in [('Publishers','PublisherPages','PublisherID'),('Categories','Categories','CategoryID')]:
 for item in data[entity]:
  i=item['ID']; page=get(f'/{path}/Index/{i}')
  expected=[b for b in books if b.get(foreign)==i] if entity=='Publishers' else [b for b in books if any(l['BookID']==b['ID'] and l['CategoryID']==i for l in data['Links'])]
  actual=rows(page)
  check(f'{entity} {i} books', [x[0] for x in actual if len(x)==2]==[b['Title'] for b in sorted(expected,key=lambda b:b['Title'].casefold())])
  check(f'{entity} {i} authors', all([b['Title'],(b.get('FirstName','')+' '+b.get('LastName','')).strip() or 'Fără autor'] in actual for b in expected))
  check(f'{entity} {i} highlight','table-success' in page)
  check(f'{entity} {i} query route', rows(get(f'/{path}/Index?id={i}'))==actual)
 for bad in [-1,2147483647]:
  try: get(f'/{path}/Index/{bad}'); raise AssertionError('expected 404')
  except urllib.error.HTTPError as e: check(f'{entity} invalid {bad}',e.code==404)
for path,ids in [('BookPages',[b['ID'] for b in books]),('AuthorsPages',[1,2]),('PublisherPages',[p['ID'] for p in data['Publishers']]),('Categories',[p['ID'] for p in data['Categories']])]:
 get('/'+path+'/Create'); passed.append('Create form '+path)
 for i in ids:
  for action in ['Edit','Details','Delete']:
   get(f'/{path}/{action}?id={i}'); passed.append(f'{path} {action} {i}')
page=get('/')
check('GET form', bool(re.search(r'<form[^>]*method="get"',page)))
reset = re.search(r'<a href="([^"]+)">Back to full List</a>',page)
check('reset link',reset is not None and '?' not in reset.group(1) and len([x for x in rows(get(reset.group(1))) if len(x)==7])==len(books))
check('book stale selection',len(rows(get('/?id=2147483647&SearchString=zzzz_lab4_no_match')))==1)
Path('tmp/lab4-test-results.txt').write_text('\n'.join(passed)+f'\nPASS: {len(passed)} checks\n',encoding='utf-8')
print(f'PASS: {len(passed)} checks; database used read-only.')

