# class-IniFile
[![en](https://img.shields.io/badge/lang-en-red.svg)](https://github.com/lokijfk/class-IniFile/blob/main/README.md) [![pl](https://img.shields.io/badge/lang-pl-green.svg)](https://github.com/lokijfk/class-IniFile/blob/main/README.pl.md)

Klasa do obsługi plików ini utworzona przy okazji jednego z projektów w C#.
 - iniFile-609 przeznaczona do pracy z C# ver 9+
 - iniFile-4873 przeznaczona do pracy z C# ver 7.3+</br>
    Zgodnie z [dokumentacją Firmy Microsoft](https://learn.microsoft.com/pl-pl/dotnet/csharp/language-reference/configure-language-version)

## Funkcjonalności i problemy klasy
* creating, saving and reading ini files
* nie obsługuje plików bez sekcji
* nie obsługuje komentarzy, nie uwzględnia ich jako danych i usuwa przy zapisie
* nie obsługuje pustych linii, nie uwzględnia ich jako danych i usuwa przy zapisie

## Sposoby użycia
### Inicjalizacja i dodawanie pliku
Plik ini dodajemy do obiektu na etapie inicjalizacji  
```c#
IniFiole Settings = new IniFile(@"settings.ini");
```
- jeżeli plik istnieje i zawiera dane zostaną one pobrane do struktury przechowującej dane w obiekcie
- jeżeli pliku nie ma, zostanie on utworzony przy zapisie tj. przy pierwszym dodaniu danych do struktury
### Odczyt danych:
* zwraca pojedyńczy ciąg danych z danego klucza w danej sekcji o ile te istnieją
* jeżeli nie ma danych o takiej reprezentacji (sekcja,klucz) lub klucz jest pusty zwróci pusty ciąg<br>
```c#
string zmienna = Settings.Read("sekcja", "klucz");
``` 
### Zapis danych:
* dodaje dane  do klucza w wybranej sekcji,
* jeżeli któregoś z nich nie ma zostanie utworzony
* jeżeli są dane o takiej reprezentacji (sekcja, klucz) zostaną nadpisane
* ostatnim etapem dodania jest zapis do pliku
* jeżeli plik nie istnieje zostanie utworzony<br>
```c#
Settings.Write("sekcja", "klucz", "dane");
```                                      
### Odczyt wszystkich danych z wybranej sekcji:
* dane są zwracane w postaci klasy "Dictionary"<br>
```c#
Dictionary<string, string> dane = SettingsGetSection("sekcja");
```
***
## Dziennik zmian
### [Pierwsza publikacja] - 2023-02-08
* dodanie klasy w dwóch wersjach

---
[![license](https://shields.io/badge/license-MIT-green.svg)](https://github.com/lokijfk/class-IniFile/blob/main/LICENSE)
