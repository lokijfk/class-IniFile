# class-IniFile
[![en](https://img.shields.io/badge/lang-en-red.svg)](https://github.com/lokijfk/class-IniFile/blob/main/README.md) [![pl](https://img.shields.io/badge/lang-pl-green.svg)](https://github.com/lokijfk/class-IniFile/blob/main/README.pl.md)

A class for handling ini files created on the occasion of one of the C# projects.
 - iniFile-609 designed to work with C# ver 9+
 - iniFile-4873 designed to work with C# ver 7.3+</br>
    According to [Microsoft documentation](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/configure-language-version)

## Class functionalities and problems
* does not support files without sections
* does not support comments, does not include them as data, and removes them on save
* does not handle blank lines, does not consider them as data and removes them on write

## Usage
### Initialization
The ini file is added to the object at the initialization stage  
```c#
IniFiole Settings = new IniFile(@"settings.ini");
```
- if the file exists and contains data, it will be downloaded to the data storage structure in the object
- if the file does not exist, it will be created when saving, i.e. when data is added to the structure for the first time
### Data reading:
* returns a single string of data from a given key in a given section, if they exist
* if there is no data about such a representation (section, key) or the key is empty, it will return an empty string  
```c#
string dane = Settings.Read("section", "key");
``` 
### Data recording:
* adds data to the key in the selected section
* if one of them does not exist, it will be created
* if there is data about such a representation (section, key) it will be overwritten
* the last stage of adding is writing to a file
* if the file does not exist it will be createdy  
```c#
Settings.Write("section", "key", "data");
```
### Reading all data from the selected section:
* data is returned as "Dictionary" class  
```c#
Dictionary<string, string> data = SettingsGetSection("section");
```
***
## Change Log
### [First publication] - 2023-02-08
* adding a class in two versions

---
[![license](https://shields.io/badge/license-MIT-green.svg)](https://github.com/lokijfk/class-IniFile/blob/main/LICENSE)
