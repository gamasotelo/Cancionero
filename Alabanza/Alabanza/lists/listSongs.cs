using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Alabanza.lists
{
    public class listSongs
    {
        string listPath;

        public listSongs(string listName) {
            //Creamos path de la lista
            listPath = getPath(listName);
            //Creamos lista si no existe
            if (!File.Exists(listPath))
            {
                File.WriteAllText(listPath, "");
            }
        }

        public void deleteList() {
            File.Delete(listPath);
        }

        public void addSongToList(string songName) {
            string filePath = getPath(songName);
            string[] songs = File.ReadAllLines(listPath);
            bool exist = false;
            foreach (string song in songs)
            {
                if (song.Equals(songName))
                {
                    exist = true;
                    break;
                }
            }
            if (exist == false)
            {
                string content = File.ReadAllText(listPath);
                if (content.Equals(""))
                {
                    content = songName;
                }
                else
                {
                    content = content + "\n" + songName;
                }
                File.WriteAllText(listPath, content);
            }
        }

        public void deleteSongFromList(string songName) {
            string[] content = File.ReadAllLines(listPath);
            string newContent = "";
            foreach (string song in content) {
                if (!song.Equals(songName)) {
                    if (newContent.Equals(""))
                    {
                        newContent = song;
                    }
                    else {
                        newContent += "\n" + song;
                    }
                }
            }
            File.WriteAllText(listPath, newContent);
        }

        public void subirCancion(string nameSong)
        {
            string[] content = File.ReadAllLines(listPath);
            for (int i = 0; i < content.Length;i++)
            {
                if (content[i].Equals(nameSong)) {
                    if (i != 0) {
                        string aux = content[i];
                        content[i] = content[i - 1];
                        content[i - 1] = aux;
                    }
                }
            }

            string newContent = string.Join("\n", content);
            File.WriteAllText(listPath, newContent);
        }

        public void bajarCancion(string nameSong)
        {
            string[] content = File.ReadAllLines(listPath);
            for (int i = 0; i < content.Length; i++)
            {
                if (content[i].Equals(nameSong))
                {
                    if (i != content.Length-1)
                    {
                        string aux = content[i];
                        content[i] = content[i + 1];
                        content[i + 1] = aux;
                    }
                    break;
                }
            }

            string newContent = string.Join("\n", content);
            File.WriteAllText(listPath, newContent);
        }

        public string[] getContent()
        {
            return File.ReadAllLines(listPath);
        }

        private string getPath(string name) {
            string path;
            if (!name.Contains(".txt")) {
                string[] aux = name.Split(' ');
                string fileName = string.Join("", aux);
                fileName = fileName + ".txt";
                 path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), fileName);
            }
            else {
                path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), name);
            }
            return path;
        }
            
    }
}
