# Laboratorul 3 - raport de implementare

Implementarea urmeaza toate cele 22 de puncte din Laborator 3.pdf, cu exceptia crearii branch-ului din punctul 1: folderul furnizat nu este un repository Git. Nu a fost initializat un repository nou. Daca este necesar pentru predare, adauga aceasta copie in repository-ul tau si creeaza branch-ul Laborator3 din Visual Studio > Git > New Branch.

## Fisiere adaugate
- Models/Category.cs
- Models/BookCategory.cs
- Models/AssignedCategoryData.cs
- Models/BookCategoriesPageModel.cs
- Models/BookData.cs
- Pages/Categories/Index.cshtml si Index.cshtml.cs
- Pages/Categories/Create.cshtml si Create.cshtml.cs
- Pages/Categories/Edit.cshtml si Edit.cshtml.cs
- Pages/Categories/Details.cshtml si Details.cshtml.cs
- Pages/Categories/Delete.cshtml si Delete.cshtml.cs
- Migrations/20261030161930_BookCategoryModif.cs si .Designer.cs

## Fisiere modificate
- Models/Book.cs
- Stefanescu_Dimitrie_Lab2Context.cs
- Program.cs
- Pages/BookPages/Create.cshtml si Create.cshtml.cs
- Pages/BookPages/Edit.cshtml si Edit.cshtml.cs
- Pages/BookPages/Index.cshtml si Index.cshtml.cs
- Pages/BookPages/Details.cshtml si Details.cshtml.cs
- Pages/Shared/_Layout.cshtml
- Pages/Index.cshtml
- Migrations/Stefanescu_Dimitrie_Lab2ContextModelSnapshot.cs

Namespace-ul Stefanescu_Dimitrie_Lab2, clasa Authors, contextul si numele csproj au fost pastrate. Pagina cartilor este acum la /, cu ruta suplimentara /BookPages. Vechea pagina de pornire este pastrata la /Home pentru a evita conflictul de rute. Authors si Publishers raman disponibile din meniu.

## Baza de date si sarcina 22
Migrarea BookCategoryModif a fost generata cu EF Core 10.0.14 si aplicata cu succes. Nu modifica tabelele existente; adauga Category, BookCategory, cheile straine si un index unic pentru perechea BookID/CategoryID.
Backup inaintea migrarii: App_Data/BeforeLab3.bak.
Conexiunea existenta a fost pastrata: (localdb)\mssqllocaldb, baza Stefanescu_Dimitrie_Lab2Context. Daca proiectul original Lab2 foloseste aceeasi conexiune, va vedea aceeasi baza actualizata. Fisierele proiectului original nu au fost modificate.

Prin formularele HTTP ale aplicatiei au fost efectuate:
- Ion (ID 2): selectate Roman si Literatura romana. Celelalte campuri pastrate.
- Padurea spanzuratilor (ID 6): creata cu categoria Roman, autor ID 1, editura ID 1, pret 45.00, data 1922-01-01.
- Verificata afisarea categoriilor in Details pentru cartea existenta.
- Verificate adaugarea, editarea, detaliile si stergerea unei categorii temporare; aceasta a fost eliminata.
- Verificate debifarea tuturor categoriilor si restaurarea celor doua pentru Ion.
- Verificata pastrarea checkbox-urilor la un formular Create invalid.
Cele trei carti initiale, cei doi autori si cele trei edituri au fost pastrate.

## Verificari
- dotnet build --no-restore: succes, 0 erori, 1 avertisment NU1900 deoarece verificarea vulnerabilitatilor NuGet nu a putut contacta api.nuget.org.
- EF migrations has-pending-model-changes: nu exista schimbari de model nemigrate.
- Raspuns HTTP 200: /, /Categories, /AuthorsPages, /PublisherPages, /BookPages/Details?id=2.
- Teste HTTP cu cookie-uri si antiforgery pentru formularele mentionate mai sus.

## In Visual Studio
1. Deschide Stefanescu_Dimitrie_Lab2.slnx din ACEST folder Lab3.
2. Build > Build Solution, apoi Ctrl+F5.
3. Pagina de pornire trebuie sa afiseze cartile si coloana Categories. Deschide Details pentru Ion si pentru Padurea spanzuratilor.
4. Nu mai trebuie sa rulezi Add-Migration: migrarea exista si este aplicata.
5. Doar daca muti aplicatia pe alta baza, in Tools > NuGet Package Manager > Package Manager Console, cu proiectul acesta ca Default project, ruleaza:
   Update-Database -Context Stefanescu_Dimitrie_Lab2Context
6. Creeaza branch-ul Laborator3 in repository-ul tau daca este cerut la predare; aici lipseste .git.

Instrumentul dotnet-ef 10.0.14 a fost instalat local in .tools pentru efectuarea migrarii. Nu s-a inceput Laboratorul 4.
