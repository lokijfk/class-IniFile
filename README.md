# class-IniFile
Klasa do obsługi plików ini utworzona przy okazji innego projektu w C# można wykozystywać tam gdzie jest potrzebna.

The class for handling ini files created on the occasion of another C# project can be used where it is needed.

## minusy klasy
* does not support files without sections - nie obsługuje plików bez sekcji
* does not support comments, does not include them as data, and removes them on save  - nie obsługuje komentarzy, nie uwzględnia ich jako danych i usuwa przy zapisie
* does not handle blank lines, does not consider them as data and removes them on write  - nie obsługuje pustych linii, nie uwzględnia ich jako danych i usuwa przy zapisie

## Usage
### inicjalizacja i dodawanie pliku
Plik ini dodajemy do obiektu na etapie inicjalizacji

`IniFiole Settings = new IniFile(@"settings.ini");`
lub
`var Settings = new IniFile(@"settings.ini");`

- jeżeli plik istnieje i zawiera dane zostaną one pobrane do struktury przechowującej dane w obiekcie
- jeżeli pliku nie ma, zostanie on utworzony przy zapisie tj. przy pierwszym dodaniu danych do struktury
### odczyt danych:

* zwraca pojedyńczy ciąg danych z danego klucza w danej sekcji o ile te istnieją
* jeżeli nie ma danych o takiej reprezentacji (sekcja,klucz) lub klucz jest pusty zwróci pusty ciąg

`var dane = Settings.Read("sekcja", "klucz");` 

### zapis danych:

* dodaje dane  do klucza w wybranej sekcji,
* jeżeli któregoś z nich nie ma zostanie utworzony
* jeżeli są dane o takiej reprezentacji (sekcja, klucz) zostaną nadpisane
* ostatnim etapem dodania jest zapis do pliku
* jeżeli plik nie istnieje zostanie utworzony

`Settings.Write("sekcja", "klucz", "dane");`
                                      
### odczyt wszystkich danych z wybranej sekcji:

* dane są zwracane w postaci dłownika Dictionary

`Dictionary<string, string> dane = SettingsGetSection("sekcja");`
lub
`var dane = SettingsGetSection("sekcja");`

 do zrobienia (To Do)
* dodawanie pustych sekcji
* dodawanie i obsługa pustych linii
* dodawanie i obsługa komentarzy
* obsługa plików bez sekcji - tylko klucz=dane

