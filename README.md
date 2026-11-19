\# Wypożyczalnia szkolna — konfiguracja PostgreSQL



Aplikacja jest desktopowym programem WPF dla Windows, napisanym w .NET 10. Dane sprzętu, wypożyczeń i zwrotów zapisuje w PostgreSQL.



\## Wymagania



\- Windows

\- PostgreSQL uruchomiony lokalnie lub na serwerze, do którego komputer ma dostęp

\- .NET 10 SDK



\## 1. Utwórz użytkownika i bazę danych



W pgAdminie połącz się z serwerem PostgreSQL, otwórz \*\*Query Tool\*\* i wykonaj poniższe polecenia osobno. Zastąp `TwojeMocneHaslo` własnym hasłem.



```sql

CREATE ROLE wypozyczalnia LOGIN PASSWORD 'TwojeMocneHaslo';

```



```sql

CREATE DATABASE wypozyczalnia OWNER wypozyczalnia;

```



Użytkownik potrzebuje uprawnień do tworzenia tabel w tej bazie. Aplikacja utworzy tabele przy pierwszym uruchomieniu.



\## 2. Zapisz dane połączenia w Windows



Otwórz PowerShell i wykonaj polecenie poniżej, wpisując hasło ustawione przy tworzeniu użytkownika bazy:



```powershell

\[Environment]::SetEnvironmentVariable(

&#x20; "SCHOOL\_RENTAL\_DB",

&#x20; "Host=localhost;Port=5432;Database=wypozyczalnia;Username=wypozyczalnia;Password=TwojeMocneHaslo",

&#x20; "User"

)

```



To zapisuje zmienną dla Twojego konta Windows, więc nie trzeba ustawiać jej przy każdym uruchomieniu. Zamknij i otwórz ponownie PowerShell lub Visual Studio, aby programy odczytały nowe ustawienie. Hasła nie wpisuj do plików projektu ani repozytorium.



Aby sprawdzić, czy zmienna jest dostępna w nowym oknie PowerShella, wpisz:



```powershell

\[bool]$env:SCHOOL\_RENTAL\_DB

```



Wynik `True` oznacza, że zmienna jest ustawiona. Nie wyświetlaj całej jej wartości podczas udostępniania zrzutów ekranu, bo zawiera hasło.



\## 3. Uruchom aplikację



Otwórz PowerShell w folderze głównym projektu (tym, w którym znajduje się folder `wypozyczalnia`) i wykonaj:



```powershell

dotnet restore .\\wypozyczalnia\\wypozyczalnia.csproj

dotnet run --project .\\wypozyczalnia\\wypozyczalnia.csproj

```



Jeśli PowerShell jest otwarty folder wyżej, w folderze zawierającym `Wypozyczalnia-Szkolna-master`, użyj ścieżki:



```powershell

dotnet run --project .\\Wypozyczalnia-Szkolna-master\\wypozyczalnia\\wypozyczalnia.csproj

```



Przy pierwszym uruchomieniu aplikacja tworzy tabele `equipment` i `rental\_history`. Dodaje również przykładowe wyposażenie, jeśli tabela sprzętu jest pusta. Wypożyczenia i zwroty wraz z historią zapisują się w bazie.



\## Rozwiązywanie problemów



\- \*\*Aplikacja prosi o ustawienie `SCHOOL\_RENTAL\_DB`:\*\* uruchom ją ponownie po otwarciu nowego terminala. Jeżeli uruchamiasz ją z Visual Studio, zamknij i otwórz Visual Studio ponownie.

\- \*\*Błąd połączenia lub logowania:\*\* sprawdź, czy PostgreSQL działa oraz czy host, port, nazwa bazy, użytkownik i hasło w zmiennej są poprawne.

\- \*\*Brak uprawnień do utworzenia tabel:\*\* nadaj użytkownikowi `wypozyczalnia` uprawnienia do tworzenia tabel w bazie `wypozyczalnia`.

\- \*\*`dotnet` nie obsługuje celu `net10.0-windows`:\*\* zainstaluj .NET 10 SDK. Interfejs WPF uruchamiaj w Windows.



