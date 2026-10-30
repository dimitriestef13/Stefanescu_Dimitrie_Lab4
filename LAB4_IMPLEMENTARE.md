# Laboratorul 4 - implementare si verificare

## Deschidere
Deschide Stefanescu_Dimitrie_Lab2.slnx din acest folder Stefanescu_Dimitrie_Lab4 in Visual Studio, apoi Ctrl+F5. Numele proiectului, namespace-urile, clasa Authors si contextul original au fost pastrate intentionat. Nu este necesara nicio migrare sau comanda Update-Database.

## Acoperirea cerintelor PDF
1. Proiectul existent a fost continuat. Folderul nu contine .git, deci branch-ul Laborator4 nu poate fi creat aici fara initializarea unui repository nou. Nu s-a initializat Git si nu s-a publicat pe GitHub. Branch-ul ramane de creat in repository-ul de predare.
2-5. ViewModel PublisherIndexData, incarcare eager a cartilor si autorilor, selectarea editurii prin ruta id sau query string, evidentierea randului selectat.
6. Authors.FullName, Display Full Name, proprietate calculata NotMapped. Nu necesita coloana in baza de date. Clasa existenta Authors nu a fost redenumita.
7. Sub edituri se afiseaza exclusiv cartile editurii selectate, cu titlu si autor. Editurile fara carti au mesaj explicit.
8-10. TitleSort si AuthorSort, sortare SQL ascendenta/descendenta dupa titlu si numele complet (prenume + nume). Ordine stabila prin criterii secundare. Linkuri in antet.
11-13. Cautare dupa titlu, prenume sau nume autor, formular GET, CurrentFilter, pastrarea filtrului la sortare si a sortarii la cautare, Back to full List. Cautarile goale sau cu spatii afiseaza lista completa; lipsa rezultatelor are mesaj explicit.
Sarcina pagina 9. CategoryIndexData si Razor Pages Categories/Index: Select pentru fiecare categorie, rand evidential, tabel cu titlu si autor doar pentru cartile asociate. Mesaj pentru o categorie fara carti. Nu a fost creat un folder MVC Views.

Selectiile de edituri/categorii inexistente raspund 404. Autorii optionali sunt tratati fara dereferentiere null. CRUD-ul existent si relatiile din Lab2/Lab3 au fost pastrate.

## Fisiere sursa modificate
- Models/Authors.cs
- Pages/PublisherPages/Index.cshtml
- Pages/PublisherPages/Index.cshtml.cs
- Pages/Categories/Index.cshtml
- Pages/Categories/Index.cshtml.cs
- Pages/BookPages/Index.cshtml
- Pages/BookPages/Index.cshtml.cs
- Pages/Shared/_Layout.cshtml (eticheta Lab4)
- Program.cs (alias suplimentar /BookPages/Index; / si /BookPages pastrate)

## Fisiere adaugate
- Models/ViewModels/PublisherIndexData.cs
- Models/ViewModels/CategoryIndexData.cs
- scripts/verify_lab4.py
- tmp/lab4-test-results.txt
- LAB4_IMPLEMENTARE.md

## Verificari executate
- dotnet restore: succes, inclusiv verificarea NuGet din afara sandbox-ului.
- dotnet build --no-restore -p:UseSharedCompilation=false: succes, 0 erori, 0 avertismente.
- .tools/dotnet-ef migrations has-pending-model-changes --no-build: No changes have been made to the model since the last migration.
- Teste HTTP automate: 104 verificari trecute, lista integrala in tmp/lab4-test-results.txt.
- Pagini /, /BookPages, /BookPages/Index, /Home, /Privacy, /AuthorsPages, /PublisherPages, /PublisherPages/Index, /Categories, /Categories/Index: HTTP 200.
- Toate cele patru ordini de sortare comparate cu datele SQL reale.
- Cautare dupa fiecare dintre cele trei campuri, combinata cu fiecare sortare; filtru pastrat, lipsa rezultate, spatii si resetare lista.
- Fiecare editura si categorie: titluri si autori comparati cu relatiile SQL, selectia evidentiata, ruta si query string echivalente, ID invalid: 404.
- Formulare Create si pagini Edit/Details/Delete pentru inregistrarile existente: HTTP 200. Nu au fost trimise formulare de modificare sau stergere pentru a pastra datele.
- Numar de inregistrari inainte si dupa: Book=4, Authors=2, Publisher=3, Category=2, BookCategory=3.

## Date si limite
Conexiunea si fisierele migrarilor au ramas nemodificate. Nu s-a executat nicio migrare, resetare, stergere, insertie sau actualizare SQL. Nu s-au modificat folderele Laboratoarelor 2 si 3.
LocalDB este functional; restrictia initiala de acces a fost a sandbox-ului, rezolvata prin executia autorizata a verificarilor in afara acestuia.
Testele au folosit HTTP pe portul temporar 5184; avertismentul despre lipsa portului HTTPS in aceasta rulare nu afecteaza profilurile Visual Studio existente. Profilurile de lansare nu au fost modificate.
Verificarea navigarii si formularelor s-a facut prin raspunsurile HTML/HTTP; Visual Studio si interactiunea vizuala in browser nu au fost testate manual. Cazurile fara autor si colectiile complet goale sunt tratate in cod, dar nu s-au creat date artificiale pentru ele.
