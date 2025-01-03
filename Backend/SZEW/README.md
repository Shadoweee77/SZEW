### NuGet
Otwierasz terminal w katalogu `/Backend/SZEW` i wpisujesz `dotnet restore`. Powinno pobrać automatycznie pakiety nuget.

### OpenAPI
`https://localhost:7035/scalar/v1`

### Dane testowe
W lokalizacji `\Backend\SZEW\SZEW` otwieramy terminal i uruchamiamy polecenie `dotnet run testdata`. Dane testowe powinny zostać dodane do bazy danych, co można sprawdzić kwerendą `SELECT * FROM "Clients"`.
Istnieje również `dotnet run testdata forced`, która najpierw usuwa wszystkie dane w bazie danych i dodaje je na nowo.

### Migracje
W konsoli menedżera pekietów:
`Add-Migration {nazwa}` - Tworzenie wzorców tabel w baze danych
`Update-Database` - Wprowadzanie zmian na podstawie migracji