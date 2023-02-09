# class-IniFile
[![en](https://img.shields.io/badge/lang-en-red.svg)](https://github.com/lokijfk/class-IniFile/blob/main/README.md) [![pl](https://img.shields.io/badge/lang-pl-green.svg)](https://github.com/lokijfk/class-IniFile/blob/main/README.pl.md)

Klasa do obsługi plików ini utworzona przy okazji jednego z projektów w C#.
 - iniFile-609 przeznaczona do pracy z C# ver 9+
 - iniFile-4873 przeznaczona do pracy z C# ver 7.3+</br>
    Zgodnie z [dokumentacją Firmy Microsoft](https://learn.microsoft.com/pl-pl/dotnet/csharp/language-reference/configure-language-version)

## Funkcjonalności i problemy klasy
* tworzenie,zapis i odczyt plików ini
* nie obsługuje plików bez sekcji
* nie obsługuje komentarzy, nie uwzględnia ich jako danych i usuwa przy zapisie
* nie obsługuje pustych linii, nie uwzględnia ich jako danych i usuwa przy zapisie

## Sposoby użycia
### Inicjalizacja i dodawanie pliku
Plik ini dodajemy do obiektu na etapie inicjalizacji  
```c#
IniFiole Settings = new IniFile(@"settings.ini");
//lub
IniFiole Settings = new IniFile();
Settings.LoadIni("path");
```
- jeżeli plik istnieje i zawiera dane zostaną one pobrane do struktury przechowującej dane w obiekcie
- jeżeli pliku nie ma, zostanie on utworzony przy zapisie tj. przy pierwszym dodaniu danych do struktury
### Odczyt danych:
* zwraca pojedyńczy ciąg danych z danego klucza w danej sekcji o ile te istnieją
* jeżeli nie ma danych o takiej reprezentacji (sekcja,klucz) lub klucz jest pusty zwróci pusty ciąg<br>
```c#
string zmienna = Settings.Read("sekcja", "klucz");
``` 
* Odczyt całej sekcji
```c#
Dictionary<string, string> sekcja;
sekcja = GetSection("section");
//lub
var section = GetSection("section");
```
### Zapis danych:
* dodaje dane  do klucza w wybranej sekcji,
* jeżeli któregoś z nich nie ma zostanie utworzony
* jeżeli są dane o takiej reprezentacji (sekcja, klucz) zostaną nadpisane
* ostatnim etapem dodania jest zapis do pliku
* jeżeli plik nie istnieje zostanie utworzony<br>
```c#
Settings.Write("sekcja", "klucz", "dane");
//lub
Settings.Write("sekcja",new Dictionary<string, string>{{ key, value },{key_1,value_1},{key_n,value_n});
```
* Lub zastąpienie całej sekcji nowym słownikiem
```c#
Settings.WriteAppendAll("section",new  Dictionary<string, string>{{ key, value },{key_1,value_1},{key_n,value_n});
``` 
### Usuwanie kluczy i sekcji:
* usuwanie pojedyńczych kluczy:
```c#
 Settings.DeleteKey("section","key");
```
* usuwanie całych sekcji:
```c#
 Settings.DeleteSection("section");
```
### metody pomocnicze - tools:
* tranzakcja - pozwala dodać dane bez zapisywania do pliku nalezy stosować przy zapisie w pętli:
```c#
 Settings.OpenTransaction();
 for(.......){
    Settings.DeleteKey("section","key");
 }
 Savetransaction();//dopiero tu nastąpi zapis do pliku
```
* klucze i sekcje 
```c#
string[] section =  GetSectionList();
string[] keys = GetKeysList("Section");
```

***
## Dziennik zmian
### [Pierwsza publikacja] - 2023-02-08
* dodanie klasy w dwóch wersjach

### [Dodanie Funkcjonalności] - 2023-02-09
* dodanie metod pomocniczych
* dodanie rozszeżonych metod zapisu i alternatywnego ładowanie pliku
---
[![license](https://shields.io/badge/license-MIT-green.svg)](https://github.com/lokijfk/class-IniFile/blob/main/LICENSE)