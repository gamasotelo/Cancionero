using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Alabanza.utils;
using System.IO;

namespace Alabanza.songs
{
    public class SongManager
    {
        public void importSongs(string file) {
            SongList songlist = new SongList("songList.txt");
            SongList hymnlist = new SongList("hymn.txt");
            string[] lines = File.ReadAllLines(file);
            string fileName = "";
            string content = "";
            string songName = "";
            string songType = "";
            foreach (string line in lines) {
                string[] analize = line.Split(' ');
                if (analize[0].Equals("SONGTYPE")) {
                    songType = analize[1];//determinamos si es alabanza o himno
                }
                else if (analize[0].Equals("SONGNAME"))
                {
                    fileName = String.Join("", analize, 1, analize.Length - 1) + ".txt";
                    songName = String.Join(" ", analize, 1, analize.Length - 1);
                    content = line + "\n"; //agregamos SONGNAME nombre al documento
                }
                else if (analize[0].Equals("ENDSONG"))
                {
                    
                    string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), fileName);
                    File.WriteAllText(path, content);

                    if (songType.Equals("Alabanza")) {
                        //Console.WriteLine("Se guardo la cancion " + songName + " en alabanzas");
                        songlist.addSong(songName);
                    }
                    else if(songType.Equals("Himno")){
                        //Console.WriteLine("Se guardo la cancion " + songName + " en Himnos");
                        hymnlist.addSong(songName);
                    }
                    
                    content = " ";
                }
                else {
                    content = content + line + "\n";
                }
            }
        }

        public void deleteSong(string songName,string list) {
            string[] aux = songName.Split(' ');
            string fileName = String.Join("", aux) + ".txt";
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), fileName);
            File.Delete(path);
            SongList theList = new SongList(list);
            theList.DeleteSong(songName);
        }

        public void exportSongs(string[] paths) {
            DependencyService.Get<FileService>().createFile(paths);
        }
    }
}
