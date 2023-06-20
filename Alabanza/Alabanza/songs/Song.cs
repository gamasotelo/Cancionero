using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using AlabanzaConsola.tones;
using AlabanzaConsola.utils;

namespace AlabanzaConsola.songs
{
    public class Song
    {
        string songFilePath;
        Scale scaleSharp;
        Scale scaleBemol;

        public Song(string songFilePath) {
            this.songFilePath = songFilePath;
            initScales();
        }

        //numero de caracteres default por linea = 35
        public string[] print() {
            string[] lines = File.ReadAllLines(songFilePath);
            string[] result = new string[lines.Length];
            int i = 0;
            foreach (string line in lines) {
                if (line.StartsWith("SONGNAME")) {
                    continue;
                }
                result[i] = line;
                i++;
            }
            return result;
        }

        public void upHalfTone()
        {
            handleStrings hs = new handleStrings();
            string content = "";
            string newLine = "";
            string[] lines = File.ReadAllLines(songFilePath);//leemos archivo
            for (int i = 0; i < lines.Length; i++)//leemos linea por linea(foreach no se puede :c)
            {
                string line = lines[i];
                if (line.Contains("{"))
                {
                    string[] chords = hs.pullApartChords(line);//separamos acordes
                    for (int c = 0; c < chords.Length; c++)
                    {
                        if (chords[c].Contains("/"))//PROCEDIMIENTO ACORDES CON BAJO
                        {
                            string[] notes = hs.pullApartNoteWithBass(chords[c]);
                            string[] upNotes = new string[2];
                            int j = 0;
                            foreach (string n in notes)
                            {
                                if (n.Contains("b"))
                                {
                                    upNotes[j] = scaleBemol.upTone(n);
                                }
                                else
                                {
                                    upNotes[j] = scaleSharp.upTone(n);
                                }
                                j++;
                            }
                            string[] aux = chords[c].Split('/');
                            aux[0] = aux[0].Replace(notes[0], upNotes[0]);
                            aux[1] = aux[1].Replace(notes[1], upNotes[1]);
                            string newChord = string.Join("/", aux);
                            string[] lineReplace = line.Split(' ');
                            int count = 0;

                            foreach (string element in lineReplace)
                            {
                                if (hs.countChars(element, '{') > 1)//ACORDES PEGADOS CON BAJO
                                {
                                    string multichord = element.Replace('}', ',');
                                    string[] theChords = multichord.Split(',');
                                    //Variables necesarias
                                    int nextChord = c;//para recorrer acordes
                                    List<string> list = lineReplace.ToList<string>();

                                    foreach (string n in theChords)
                                    {
                                        if (n.Equals("{" + chords[nextChord]))
                                        {
                                            if (nextChord == c)
                                            {
                                                list.RemoveAt(count);//Eliminamos los acordes de la lista
                                            }
                                            list.Insert(count, "*{" + newChord + "!}");//* para eliminar un espacio que se crea en este caso
                                        }
                                        nextChord++;//vamos al siguiente acorde
                                        count++;//vamos al siguiente espacio del arreglo
                                        if (nextChord == chords.Length)
                                            break;
                                        if (chords[nextChord].Contains("/"))
                                        {//ACORDES PEGADOS CON BAJO
                                            //aqui termina 
                                            notes = hs.pullApartNoteWithBass(chords[nextChord]);
                                            upNotes = new string[2];
                                            int u = 0;
                                            foreach (string r in notes)
                                            {
                                                if (r.Contains("b"))
                                                {
                                                    upNotes[u] = scaleBemol.upTone(r);
                                                }
                                                else
                                                {
                                                    upNotes[u] = scaleSharp.upTone(r);
                                                }
                                                u++;
                                            }
                                            aux = chords[c].Split('/');
                                            aux[0] = aux[0].Replace(notes[0], upNotes[0]);
                                            aux[1] = aux[1].Replace(notes[1], upNotes[1]);
                                            newChord = string.Join("/", aux);
                                        }
                                        else
                                        { //ACORDES PEGADOS SIN BAJO
                                            //subimos de tono el siguiente acorde
                                            string note = hs.pullApartNotes(chords[nextChord]);
                                            string upNote = "";
                                            if (note.Contains("b"))
                                            {
                                                upNote = scaleBemol.upTone(note);
                                            }
                                            else
                                            {
                                                upNote = scaleSharp.upTone(note);
                                            }
                                            newChord = chords[nextChord].Replace(note, upNote);

                                        }
                                    }
                                    lineReplace = list.ToArray();
                                }
                                else
                                {
                                    if (element.Equals("{" + chords[c] + "}"))
                                    {
                                        lineReplace[count] = "{" + newChord + "!}";//! marca de los que ya fueron cambiados
                                        break;
                                    }
                                }
                                count++;
                            }
                            line = String.Join(" ", lineReplace);
                            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                        }
                        else
                        {//PROCEDIMIENTO ACORDES SIN BAJO
                            string note = hs.pullApartNotes(chords[c]);//separamos notas
                            //subimos tono
                            String upNote;
                            if (note.Contains("b"))
                            {
                                upNote = scaleBemol.upTone(note);
                            }
                            else
                            {
                                upNote = scaleSharp.upTone(note);
                            }
                            string newChord = chords[c].Replace(note, upNote);
                            string[] lineReplace = line.Split(' ');
                            int count = 0;
                            foreach (string element in lineReplace)
                            {
                                if (hs.countChars(element, '{') > 1)//ACORDES PEGADOS CON BAJO
                                {
                                    string multichord = element.Replace('}', ',');
                                    string[] theChords = multichord.Split(',');
                                    //Variables necesarias
                                    int nextChord = c;//para recorrer acordes
                                    List<string> list = lineReplace.ToList<string>();
                                    int finalize = 0;
                                    foreach (string n in theChords)
                                    {
                                        finalize++;
                                        if (n.Equals("{" + chords[nextChord]))
                                        {
                                            if (nextChord == c)
                                            {
                                                list.RemoveAt(count);//Eliminamos los acordes de la lista
                                            }
                                            list.Insert(count, "*{" + newChord + "!}");//* para eliminar un espacio que se crea en este caso
                                            if (finalize == theChords.Length - 1)
                                                break;
                                        }
                                        nextChord++;//vamos al siguiente acorde
                                        count++;//vamos al siguiente espacio del arreglo
                                        if (nextChord == chords.Length)
                                            break;
                                        if (chords[nextChord].Contains("/"))
                                        {//ACORDES PEGADOS CON BAJO
                                            //aqui termina 
                                            string[] notes = hs.pullApartNoteWithBass(chords[nextChord]);
                                            string[] upNotes = new string[2];
                                            int u = 0;
                                            foreach (string r in notes)
                                            {
                                                if (r.Contains("b"))
                                                {
                                                    upNotes[u] = scaleBemol.upTone(r);
                                                }
                                                else
                                                {
                                                    upNotes[u] = scaleSharp.upTone(r);
                                                }
                                                u++;
                                            }
                                            string[] aux = chords[nextChord].Split('/');
                                            aux[0] = aux[0].Replace(notes[0], upNotes[0]);
                                            aux[1] = aux[1].Replace(notes[1], upNotes[1]);
                                            newChord = string.Join("/", aux);
                                        }
                                        else
                                        { //ACORDES PEGADOS SIN BAJO
                                            //subimos de tono el siguiente acorde
                                            note = hs.pullApartNotes(chords[nextChord]);
                                            upNote = "";
                                            if (note.Contains("b"))
                                            {
                                                upNote = scaleBemol.upTone(note);
                                            }
                                            else
                                            {
                                                upNote = scaleSharp.upTone(note);
                                            }
                                            newChord = chords[nextChord].Replace(note, upNote);
                                        }
                                    }
                                    lineReplace = list.ToArray();
                                    break;
                                }
                                else
                                {
                                    if (element.Equals("{" + chords[c] + "}"))
                                    {
                                        lineReplace[count] = "{" + newChord + "!}";//! marca de los que ya fueron cambiados
                                        break;
                                    }
                                }
                                count++;
                            }
                            //Juntamos array en una cadena
                            line = string.Join(" ", lineReplace);
                        }
                    }
                }
                newLine = "";
                int position = 0;//para saber si es el primer caracter de toda la linea
                bool firtTime = true;//para saber si es la primera vez que aparece * en esta linea
                foreach (char ch in line)
                {
                    if (ch.Equals('*'))
                    {
                        if (position != 0 && firtTime == false)
                        {//si "*" no esta en la primera posicion y no es el primero que aparece
                            newLine = newLine.Remove(newLine.Length - 1);
                        }
                        firtTime = false;
                        continue;
                    }
                    position++;
                    newLine = newLine + ch;
                }
                line = newLine;
                line = line.Replace("!", "");//quitamos la marca de todos los acordes de la linea
                //El if es para no agregar una linea adicional cada vez que se cambia tono
                if (i == lines.Length - 1)//Si es la ultima linea el documento
                {
                    content = content + line;
                }
                else
                {
                    content = content + line + "\n";
                }
            }
            writeNewSong(content);
        }

        public void downHalfTone()
        {
            handleStrings hs = new handleStrings();
            string content = "";
            string newLine = "";
            string[] lines = File.ReadAllLines(songFilePath);//leemos archivo
            for (int i = 0; i < lines.Length; i++)//leemos linea por linea(foreach no se puede :c)
            {
                string line = lines[i];
                if (line.Contains("{"))
                {
                    string[] chords = hs.pullApartChords(line);//separamos acordes
                    for (int c = 0; c < chords.Length; c++)
                    {
                        if (chords[c].Contains("/"))//PROCEDIMIENTO ACORDES CON BAJO
                        {
                            string[] notes = hs.pullApartNoteWithBass(chords[c]);
                            string[] upNotes = new string[2];
                            int j = 0;
                            foreach (string n in notes)
                            {
                                if (n.Contains("b"))
                                {
                                    upNotes[j] = scaleBemol.downTone(n);
                                }
                                else
                                {
                                    upNotes[j] = scaleSharp.downTone(n);
                                }
                                j++;
                            }
                            string[] aux = chords[c].Split('/');
                            aux[0] = aux[0].Replace(notes[0], upNotes[0]);
                            aux[1] = aux[1].Replace(notes[1], upNotes[1]);
                            string newChord = string.Join("/", aux);
                            string[] lineReplace = line.Split(' ');
                            int count = 0;

                            foreach (string element in lineReplace)
                            {
                                if (hs.countChars(element, '{') > 1)//ACORDES PEGADOS CON BAJO
                                {
                                    string multichord = element.Replace('}', ',');
                                    string[] theChords = multichord.Split(',');
                                    //Variables necesarias
                                    int nextChord = c;//para recorrer acordes
                                    List<string> list = lineReplace.ToList<string>();

                                    foreach (string n in theChords)
                                    {
                                        if (n.Equals("{" + chords[nextChord]))
                                        {
                                            if (nextChord == c)
                                            {
                                                list.RemoveAt(count);//Eliminamos los acordes de la lista
                                            }
                                            list.Insert(count, "*{" + newChord + "!}");//* para eliminar un espacio que se crea en este caso
                                        }
                                        nextChord++;//vamos al siguiente acorde
                                        count++;//vamos al siguiente espacio del arreglo
                                        if (nextChord == chords.Length)
                                            break;
                                        if (chords[nextChord].Contains("/"))
                                        {//ACORDES PEGADOS CON BAJO
                                            //aqui termina 
                                            notes = hs.pullApartNoteWithBass(chords[nextChord]);
                                            upNotes = new string[2];
                                            int u = 0;
                                            foreach (string r in notes)
                                            {
                                                if (r.Contains("b"))
                                                {
                                                    upNotes[u] = scaleBemol.downTone(r);
                                                }
                                                else
                                                {
                                                    upNotes[u] = scaleSharp.downTone(r);
                                                }
                                                u++;
                                            }
                                            aux = chords[nextChord].Split('/');
                                            aux[0] = aux[0].Replace(notes[0], upNotes[0]);
                                            aux[1] = aux[1].Replace(notes[1], upNotes[1]);
                                            newChord = string.Join("/", aux);
                                        }
                                        else
                                        { //ACORDES PEGADOS SIN BAJO
                                            //subimos de tono el siguiente acorde
                                            string note = hs.pullApartNotes(chords[nextChord]);
                                            string upNote = "";
                                            if (note.Contains("b"))
                                            {
                                                upNote = scaleBemol.downTone(note);
                                            }
                                            else
                                            {
                                                upNote = scaleSharp.downTone(note);
                                            }
                                            newChord = chords[nextChord].Replace(note, upNote);

                                        }
                                    }
                                    lineReplace = list.ToArray();
                                }
                                else
                                {
                                    if (element.Equals("{" + chords[c] + "}"))
                                    {
                                        lineReplace[count] = "{" + newChord + "!}";//! marca de los que ya fueron cambiados
                                        break;
                                    }
                                }
                                count++;
                            }
                            line = String.Join(" ", lineReplace);
                            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                        }
                        else
                        {//PROCEDIMIENTO ACORDES SIN BAJO
                            string note = hs.pullApartNotes(chords[c]);//separamos notas
                            //subimos tono
                            String upNote;
                            if (note.Contains("b"))
                            {
                                upNote = scaleBemol.downTone(note);
                            }
                            else
                            {
                                upNote = scaleSharp.downTone(note);
                            }
                            string newChord = chords[c].Replace(note, upNote);
                            string[] lineReplace = line.Split(' ');
                            int count = 0;
                            foreach (string element in lineReplace)
                            {
                                if (hs.countChars(element, '{') > 1)//ACORDES PEGADOS CON BAJO
                                {
                                    string multichord = element.Replace('}', ',');
                                    string[] theChords = multichord.Split(',');
                                    //Variables necesarias
                                    int nextChord = c;//para recorrer acordes
                                    List<string> list = lineReplace.ToList<string>();
                                    int finalize = 0;
                                    foreach (string n in theChords)
                                    {
                                        finalize++;
                                        if (n.Equals("{" + chords[nextChord]))
                                        {
                                            if (nextChord == c)
                                            {
                                                list.RemoveAt(count);//Eliminamos los acordes de la lista
                                            }
                                            list.Insert(count, "*{" + newChord + "!}");//* para eliminar un espacio que se crea en este caso
                                            if (finalize == theChords.Length - 1)
                                                break;

                                        }
                                        nextChord++;//vamos al siguiente acorde
                                        count++;//vamos al siguiente espacio del arreglo
                                        if (nextChord == chords.Length)
                                            break;
                                        if (chords[nextChord].Contains("/"))
                                        {//ACORDES PEGADOS CON BAJO
                                            //aqui termina 
                                            string[] notes = hs.pullApartNoteWithBass(chords[nextChord]);
                                            string[] upNotes = new string[2];
                                            int u = 0;
                                            foreach (string r in notes)
                                            {
                                                if (r.Contains("b"))
                                                {
                                                    upNotes[u] = scaleBemol.downTone(r);
                                                }
                                                else
                                                {
                                                    upNotes[u] = scaleSharp.downTone(r);
                                                }
                                                u++;
                                            }
                                            string[] aux = chords[nextChord].Split('/');
                                            aux[0] = aux[0].Replace(notes[0], upNotes[0]);
                                            aux[1] = aux[1].Replace(notes[1], upNotes[1]);
                                            newChord = string.Join("/", aux);
                                        }
                                        else
                                        { //ACORDES PEGADOS SIN BAJO
                                            //subimos de tono el siguiente acorde
                                            note = hs.pullApartNotes(chords[nextChord]);
                                            upNote = "";
                                            if (note.Contains("b"))
                                            {
                                                upNote = scaleBemol.downTone(note);
                                            }
                                            else
                                            {
                                                upNote = scaleSharp.downTone(note);
                                            }
                                            newChord = chords[nextChord].Replace(note, upNote);
                                        }
                                    }
                                    lineReplace = list.ToArray();
                                    break;
                                }
                                else
                                {
                                    if (element.Equals("{" + chords[c] + "}"))
                                    {
                                        lineReplace[count] = "{" + newChord + "!}";//! marca de los que ya fueron cambiados
                                        break;
                                    }
                                }
                                count++;
                            }
                            //Juntamos array en una cadena
                            line = string.Join(" ", lineReplace);
                        }
                    }
                }
                newLine = "";
                int position = 0;//para saber si es el primer caracter de toda la linea
                bool firtTime = true;//para saber si es la primera vez que aparece * en esta linea
                foreach (char ch in line)
                {
                    if (ch.Equals('*'))
                    {
                        if (position != 0 && firtTime == false)
                        {//si "*" no esta en la primera posicion y no es el primero que aparece
                            newLine = newLine.Remove(newLine.Length - 1);
                        }
                        firtTime = false;
                        continue;
                    }
                    position++;
                    newLine = newLine + ch;
                }
                line = newLine;
                line = line.Replace("!", "");//quitamos la marca de todos los acordes de la linea
                //El if es para no agregar una linea adicional cada vez que se cambia tono
                if (i == lines.Length-1)//Si es la ultima linea el documento
                {
                    content = content + line;
                }
                else {
                    content = content + line + "\n";
                }
            }
            writeNewSong(content);
        }

        public void changeToSharp() {
            handleStrings hs = new handleStrings();
            string content = "";
            string[] lines = File.ReadAllLines(songFilePath);//leemos archivo

            for (int i = 0; i < lines.Length; i++)//leemos linea por linea(foreach no se puede :c)
            {
                string line = lines[i];
                if (line.Contains("{"))
                {
                    string[] chords = hs.pullApartChords(line);//separamos acordes
                    foreach (string chord in chords)
                    {
                        string note = hs.pullApartNotes(chord);//separamos notas
                        //cambiamos nota
                        String enharmonicNote;
                        if (note.Contains("b"))
                        {
                            enharmonicNote = scaleBemol.getEnharmonic(note, scaleSharp);
                        }
                        else
                        {
                            enharmonicNote = note;
                        }

                        string newChord = chord.Replace(note, enharmonicNote);
                        string[] lineReplace = line.Split(' ');
                        int count = 0;

                        foreach (string element in lineReplace)
                        {
                            if (element.Equals("{" + chord + "}"))
                            {
                                lineReplace[count] = "{" + newChord + "!}";
                                break;
                            }
                            count++;
                        }
                        line = String.Join(" ", lineReplace);
                    }
                }
                line = line.Replace("!", "");//quitamos la marca de todos los acordes de la linea
                //El if es para no agregar una linea adicional cada vez que se cambia tono
                if (i == lines.Length - 1)//Si es la ultima linea el documento
                {
                    content = content + line;
                }
                else
                {
                    content = content + line + "\n";
                }
            }
            writeNewSong(content);
        }

        public void changeToBemol() {
            handleStrings hs = new handleStrings();
            string content = "";
            string[] lines = File.ReadAllLines(songFilePath);//leemos archivo

            for (int i = 0; i < lines.Length; i++)//leemos linea por linea(foreach no se puede :c)
            {
                string line = lines[i];
                if (line.Contains("{"))
                {
                    string[] chords = hs.pullApartChords(line);//separamos acordes
                    foreach (string chord in chords)
                    {
                        string note = hs.pullApartNotes(chord);//separamos notas
                        //cambiamos nota
                        String enharmonicNote;
                        if (note.Contains("#"))
                        {
                            enharmonicNote = scaleSharp.getEnharmonic(note, scaleBemol);
                        }
                        else
                        {
                            enharmonicNote = note;
                        }

                        string newChord = chord.Replace(note, enharmonicNote);
                        string[] lineReplace = line.Split(' ');
                        int count = 0;

                        foreach (string element in lineReplace)
                        {
                            if (element.Equals("{" + chord + "}"))
                            {
                                lineReplace[count] = "{" + newChord + "!}";
                                break;
                            }
                            count++;
                        }
                        line = String.Join(" ", lineReplace);
                    }
                }
                line = line.Replace("!", "");//quitamos la marca de todos los acordes de la linea
                content = content + line + "\n";
            }
            writeNewSong(content);
        }

        private void writeNewSong(String content) {
            String[] lines = content.Split('\n');
            try
            { 
                StreamWriter sw = new StreamWriter(songFilePath);
                foreach (string line in lines)
                {
                    sw.WriteLine(line);
                }
                sw.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }

        private void initScales() {
            scaleSharp = new Scale();
            scaleBemol = new Scale();

            scaleSharp.addNote("C");
            scaleSharp.addNote("C#");
            scaleSharp.addNote("D");
            scaleSharp.addNote("D#");
            scaleSharp.addNote("E");
            scaleSharp.addNote("F");
            scaleSharp.addNote("F#");
            scaleSharp.addNote("G");
            scaleSharp.addNote("G#");
            scaleSharp.addNote("A");
            scaleSharp.addNote("A#");
            scaleSharp.addNote("B");

            scaleBemol.addNote("C");
            scaleBemol.addNote("Db");
            scaleBemol.addNote("D");
            scaleBemol.addNote("Eb");
            scaleBemol.addNote("E");
            scaleBemol.addNote("F");
            scaleBemol.addNote("Gb");
            scaleBemol.addNote("G");
            scaleBemol.addNote("Ab");
            scaleBemol.addNote("A");
            scaleBemol.addNote("Bb");
            scaleBemol.addNote("B");
        }
    }
}
