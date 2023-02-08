using System;
using System.Collections.Generic;
using System.IO;

namespace ConfiguratorSH
{
    internal class IniFile
    {
        /// <summary>
        /// klasa do obsługi plików ini, zapis odczyt, modyfikacja
        /// nie obsługuje komentarzy i pustych linii - są usuwane
        /// przewidziane są pliki które zawierają sekcje
        /// pliki bez sekcji są nie obsługiwane
        /// -------------------------------------
        /// pomysły
        /// -------------------------------------
        /// korci mnie żeby tu dodać obsługęnawiasów kwadratowych i wyszukiwanie jak w dictionary
        /// https://learn.microsoft.com/pl-pl/dotnet/api/system.collections.generic.idictionary-2?view=net-6.0
        /// musiał bym poczytać co mam zaiplementować, ale musiał bym tak zrobić żeby byłą przewidziana tablica dwuwymarowa od ręki i nie większa
        /// -------------------------------------
        /// mam pomysl na komentarze, nagłówek nazywał by się KomLine + numer, tak samo ma się nzaywać podklucz
        /// a wartość to już wiadomo będzie komentarzem
        /// jak to zrobię odpowiednio to komentarze będą odczytywane i zapisywane do pliku
        /// 
        /// przygotowane dla .NET 4.8 i C# v 7.3
        /// https://learn.microsoft.com/pl-pl/dotnet/csharp/language-reference/compiler-options/language
        /// 
        /// brak testów, porobić na różne buble
        /// </summary>


        private readonly string iniPath;

        // ta konstrukcja w przybliżeniu odzwierciedla wygląd pliku ini
        private readonly Dictionary<string, Dictionary<string, string>> ListAdw = new Dictionary<string, Dictionary<string, string>>();

        public IniFile(string path)
        {
            iniPath = path;
            LoadIni();
        }

        /// <summary>
        /// przepisanie danych ze struktury do tablicy jednowymiarowej
        /// uzupełnia nawiasy w nagłówku sekcji, łączy klucz z danymi
        /// </summary>
        /// <returns></returns>
        private string[] IniToArray()
        {
            if (ListAdw.Count > 0)
            {
                List<string> lista = new List<string>();
                foreach (KeyValuePair<string, Dictionary<string, string>> item in ListAdw)
                {
                    lista.Add("[" + item.Key + "]");
                    foreach (KeyValuePair<string, string> kvp in ListAdw[item.Key])
                    {
                        lista.Add(kvp.Key + "=" + kvp.Value);
                    }
                }
                return lista.ToArray();
            }
            return new string[0];
        }

        /// <summary>
        /// zapis przekonwertowanych danych ze struktury do pliku
        /// </summary>
        /// <param name="iniPath"></param>
        private void SaveIni()
        {
            //tu zabezpieczenie żeby całkowicie nie usunąć danych ale czy potrzebne ??
            string[] tab = IniToArray();
            //if(tab.Length > 0)
            File.WriteAllLines(this.iniPath, tab);

        }

        /// <summary>
        /// załadowanie danych z pliku do struktury
        /// </summary>
        /// <param name="iniPath"></param>
        private void LoadIni()
        {
            string header = "", key, value, temp;
            int found;
            if (File.Exists(this.iniPath))
            {
                foreach (string line in File.ReadLines(this.iniPath))
                {
                    temp = line.Trim();                    
                    if (temp.StartsWith(";")|| temp.StartsWith("#")||(temp.Length == 0)) continue;
                    if (temp.StartsWith(";"))
                    {
                        found = temp.IndexOf("[");                        
                        header = temp.Substring(found + 1).Remove(temp.IndexOf("]", found) - 1);                       
                        ListAdw[header] = new Dictionary<string, string>();
                    }
                    else
                    {                           
                        found = temp.IndexOf("=");
                        if (found > 0)
                        {
                            key = temp.Remove(found); 
                            value = temp.Substring(found + 1);//temp.Substring(found + 1);                        
                            if (!header.Equals(""))
                                ListAdw[header][key] = value;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// zwraca zawartość klucza z podane sekcji
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Read(string section, string key)
        {
            //tu powinny być wyzwalane wyjątki i bardziej rozbudowany if, taki jak przy zapisie            
            if (ListAdw.ContainsKey(section) && ListAdw[section].ContainsKey(key))
            {
                return ListAdw[section][key];
            }
            return "";
        }

        /// <summary>
        /// zapis danych do struktury (i do pliku)
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Write(string section, string key, string value)
        {
            if (ListAdw.ContainsKey(section))
            {
                if (ListAdw[section].ContainsKey(key))
                {
                    ListAdw[section][key] = value;
                }
                else
                {
                    ListAdw[section].Add(key, value);
                }
            }
            else
            {
                ListAdw[section] = new Dictionary<string, string>
                {
                    { key, value }
                };
            }
            //to do poprawy a na pewno do zmiany
            SaveIni();
        }

        /// <summary>
        /// usuwa całą wskazaną sekcję
        /// </summary>
        /// <param name="section"></param>
        public void DeleteSection(string section)
        {
            if (ListAdw.ContainsKey(section))
            {
                ListAdw.Remove(section);
                SaveIni();
            }

        }

        /// <summary>
        /// zwraca zawartość całej cekcji w postaci obiektu Dictionary<string,string>
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetSection(string section)
        {
            if (ListAdw.ContainsKey(section))
            {
                return ListAdw[section];
            }
            return null;
        }

        /// <summary>
        /// usuwa klucz 'key' z wartością z sekcji 'section"
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        public void DeleteKey(string section, string key)
        {
            if (ListAdw.ContainsKey(section) && ListAdw[section].ContainsKey(key))
            {
                ListAdw[section].Remove(key);
                SaveIni();
            }
        }
    }
}
