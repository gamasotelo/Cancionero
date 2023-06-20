using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace Alabanza.songs
{
    public class SongList
    {
        string path;
        public SongList(string list) {
            path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), list);
            if (!File.Exists(path)) {
                File.WriteAllText(path, "");
            }
            
        }

        public void addSong(string songName) {
            string[] songs = File.ReadAllLines(path);
            bool exist = false;
            foreach (string song in songs) {
                if (song.Equals(songName)){
                    exist = true;
                    break;
                }
            }
            if (exist == false){
                string content = File.ReadAllText(path);
                if (content.Equals("")) {
                    content = songName;
                }
                else{
                    content = content + "\n" + songName;
                }
                
                File.WriteAllText (path, content);
            }
        }

        public void DeleteSong(string songName) {
            string[] content = readSongs();
            string newContent = "";
            for (int song = 0;song<content.Length;song++)
            {
                if (!content[song].Equals(songName)) {
                    if (newContent.Equals(""))
                    {
                        newContent = content[song];
                    }
                    else
                    {
                        newContent = newContent + "\n" + content[song];
                    }
                }
            }
            Console.WriteLine(newContent);
            File.WriteAllText(path, newContent);
        }

        public string[] readSongs() {
            string[] content = File.ReadAllLines(path);
            return content;
        }
    }
}
