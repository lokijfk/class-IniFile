using System;
using System.Collections.Generic;
using System.Transactions;

namespace ConfiguratorSH
{
    internal class IniFile
    {
        /// <summary>
        /// klasa do obsługi plików ini, zapis odczyt, modyfikacja
        /// nie obsługuje komentarzy i pustych linii - są usuwane
        /// przewidziane są pliki które zawierają sekcje
        /// pliki bez sekcji są nie obsługiwane
        /// 
        /// przygotowane dla .NET 6.0 i C# v 9
        /// 
        /// </summary>

        #region ptivate
        private string iniPath;
        private bool transaction = false;
        // ta konstrukcja w przybliżeniu odzwierciedla wygląd pliku ini
        private readonly Dictionary<string, Dictionary<string, string>> ListAdw = new();

        /// <summary>
        /// załadowanie danych z pliku do struktury
        /// </summary>
        private void LoadIni()
        {
            string header = "", key, value, temp;
            int found;
            if (File.Exists(this.iniPath))
            {
                foreach (string line in File.ReadLines(this.iniPath))
                {
                    temp = line.Trim();
                    if (temp.StartsWith(';') || temp.StartsWith('#') || (temp.Length == 0)) continue;
                    if (temp.StartsWith('['))
                    {
                        found = temp.IndexOf("[");
                        header = temp[(found + 1)..].Remove(temp.IndexOf("]", found) - 1);
                        ListAdw[header] = new Dictionary<string, string>();
                    }
                    else
                    {
                        found = temp.IndexOf("=");
                        key = temp.Remove(found);
                        value = temp[(found + 1)..];//temp.Substring(found + 1);                        
                        if (!header.Equals(""))
                            ListAdw[header][key] = value;
                    }
                }
            }
        }

        /// <summary>
        /// usuwa wszystkie dane i podklucze ze struktury przed ponownym załadowaniem pliku ini
        /// </summary>
        private void ClearAll()
        {
            foreach (KeyValuePair<string, Dictionary<string, string>> item in ListAdw)
            {
                ListAdw[item.Key].Clear();
            }
            ListAdw.Clear();

            foreach (KeyValuePair<string, Dictionary<string, string>> item in ListAdw)
            {
                foreach (KeyValuePair<string, string> kvp in ListAdw[item.Key])
                {
                    ListAdw[item.Key].Remove(kvp.Key);
                }
                ListAdw.Remove(item.Key);
            }
        }

        /// <summary>
        /// zapis przekonwertowanych danych ze struktury do pliku
        /// o ile nie ma włączonej tranzakcji
        /// </summary>
        private void SaveIni()
        {
            if (!this.transaction)
            {
                //tu zabezpieczenie żeby całkowicie nie usunąć danych ale czy potrzebne ??
                string[] tab = IniToArray();
                //if(tab.Length > 0)
                File.WriteAllLines(this.iniPath, tab);
            }
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
                List<string> lista = new();
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
            return Array.Empty<string>();
        }
        #endregion 


        public bool Transaction { get { return transaction; } }
        public string IniPath { get { return iniPath; } }

        //---- initialize, open the file and load the ini if it exists
        public IniFile(string path = "")
        {
            iniPath = path;
            if ((this.iniPath != "") && (this.iniPath != String.Empty))
            {
                LoadIni();
            }
        }

        public void LoadIni(String path)
        {
            if ((this.iniPath != "") && (this.iniPath != String.Empty))
            {
                ClearAll();
            }
            this.iniPath = path;
            //ListAdw.Clear();
            LoadIni();
        }

        //---- reading data from ini keys

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
        /// zwraca zawartość całej cekcji w postaci obiektu Dictionary<string,string>
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        public Dictionary<string, string>? GetSection(string section)
        {
            if (ListAdw.ContainsKey(section))
            {
                return ListAdw[section];
            }
            return null;
        }

        //--- write data

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
        /// zapis danych do struktury, nadpisuje istniejące dane
        /// </summary>
        /// <param name="section"></param>
        /// <param name="KeyVal"></param>
        public void WriteAppendAll(string section, Dictionary<string, string> KeyVal)
        {
            if (ListAdw.ContainsKey(section))
            {
                ListAdw[section].Clear();
            }
            ListAdw[section] = KeyVal;
            //to do poprawy a na pewno do zmiany
            SaveIni();
        }

        /// <summary>
        /// zapis danych do struktury, dopisuje do istniejącej struktury
        /// </summary>
        /// <param name="section"></param>
        /// <param name="KeyVal"></param>
        public void Write(string section, Dictionary<string, string> KeyVal)
        {
            if (!ListAdw.ContainsKey(section))
            {
                ListAdw[section] = new Dictionary<string, string>();
            }
            foreach (KeyValuePair<string, string> kvp in KeyVal)
            {
                //lista.Add(kvp.Key + "=" + kvp.Value);
                ListAdw[section][kvp.Key] = kvp.Value;
            }
            //to do poprawy a na pewno do zmiany
            SaveIni();
        }

        //---- delete keys and section

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

        //---- tools

        /// <summary>
        /// Zwraca listę kluczy sekcji
        /// </summary>
        /// <returns></returns>
        public string[] GetSectionList()
        {
            return ListAdw.Keys.ToArray();
            // return Array.Empty<string>();
        }

        /// <summary>
        /// Zwraca listę kluczy w wybranej sekcji
        /// </summary>
        /// <param name="Section"></param>
        /// <returns></returns>
        public string[] GetKeysList(string Section)
        {
            return ListAdw[Section].Keys.ToArray();
        } 
        
        /// <summary>
        /// otwiera tranzakcję, czyli cykliczny zapis do struktury
        /// powoduje nie zapisywanie do pliku
        /// </summary>
        public void OpenTransaction()
        {
            if (!transaction)
            {
                transaction = true;
            }
        }

        /// <summary>
        /// zamyka tranzakcję i zapisuje dane ze struktury do pliku
        /// </summary>
        public void Savetransaction()
        {
            if (transaction)
            {
                transaction = false;
                SaveIni();
            }
        }
    }
}
