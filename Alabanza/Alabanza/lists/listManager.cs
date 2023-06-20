using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Alabanza.lists
{
    public class listManager
    {
        string listsNamesFile;
        public listManager()
        {
            //Creamos archivo donde se almacenan los nombres de todas las listas
            listsNamesFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "listNames.txt");
            if (!File.Exists(listsNamesFile))
            {
                File.WriteAllText(listsNamesFile, "");
            }
        }

        public void deleteList(string listName) {
            listSongs newList = new listSongs(listName);
            newList.deleteList();
            deleteNameFromList(listName);
        }

        public bool CreateList(string listName) {
            bool listCreated = false;
            bool nameAlreadyExist = addNameToTheFile(listName);
            if (nameAlreadyExist == false)
            {
                listSongs newList = new listSongs(listName);
                listCreated = true;
                return listCreated;
            }
            return listCreated;
        }

        private bool addNameToTheFile(string listName)
        {
            bool nameExist = false;
            string[] names = File.ReadAllLines(listsNamesFile);
            if (names == null)
            {
                string content = listName;
                return nameExist;
            }

            foreach (string name in names) {
                if (name.Equals(listName))
                {
                    nameExist = true;
                    break;
                }
            }
            if (nameExist == false)
            {
                string content = File.ReadAllText(listsNamesFile);
                if (content.Equals(""))
                {
                    content = listName;
                }
                else
                {
                    content = content + "\n" + listName;
                }

                File.WriteAllText(listsNamesFile, content);
            }
            return nameExist;
        }

        private void deleteNameFromList(string listName) {
            string[] names = File.ReadAllLines(listsNamesFile);
            string newContent = "";
            foreach (string name in names)
            {
                if (!name.Equals(listName)) {
                    if (newContent.Equals(""))
                    {
                        newContent = name;
                    }
                    else
                    {
                        newContent = newContent + "\n" + name;
                    }
                }
            }
            File.WriteAllText(listsNamesFile, newContent);
        }

        public string[] getContent()
        {
            return File.ReadAllLines(listsNamesFile);
        }
    }
}
