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
//or
IniFiole Settings = new IniFile();
Settings.LoadIni("path");
```
- if the file exists and contains data, it will be downloaded to the data storage structure in the object
- if the file does not exist, it will be created when saving, i.e. when data is added to the structure for the first time
### Data reading:
* returns a single string of data from a given key in a given section, if they exist
* if there is no data about such a representation (section, key) or the key is empty, it will return an empty string  
```c#
string dane = Settings.Read("section", "key");
``` 
* Read the entire section
```c#
Dictionary<string, string> sekcja;
sekcja = GetSection("section");
//or
var section = GetSection("section");
```
### Data recording:
* adds data to the key in the selected section
* if one of them does not exist, it will be created
* if there is data about such a representation (section, key) it will be overwritten
* the last stage of adding is writing to a file
* if the file does not exist it will be createdy  
```c#
Settings.Write("sekcja", "klucz", "dane");
//lub
Settings.Write("sekcja",new Dictionary<string, string>{{ key, value },{key_1,value_1},{key_n,value_n});
```
* Or replace the entire section with a new dictionary
```c#
Settings.WriteAppendAll("section",new  Dictionary<string, string>{{ key, value },{key_1,value_1},{key_n,value_n});
``` 
### Deleting keys and sections:
* deleting individual keys:
```c#
 Settings.DeleteKey("section","key");
```
* deleting entire sections:
```c#
 Settings.DeleteSection("section");
```
### helper methods - tools:
* transaction - allows you to add data without saving to the file, should be used when saving in a loop:
```c#
 Settings.OpenTransaction();
 for(.......){
    Settings.DeleteKey("section","key");
 }
 Savetransaction();//only here will write to the file
```
* keys and sections
```c#
string[] section =  GetSectionList();
string[] keys = GetKeysList("Section");
```

***
## Change Log
### [First publication] - 2023-02-08
* adding a class in two versions

### [Adding Functionality] - 2023-02-09
* addition of helper methods
* addition of extended saving methods and alternative file loading
---
[![license](https://shields.io/badge/license-MIT-green.svg)](https://github.com/lokijfk/class-IniFile/blob/main/LICENSE)
