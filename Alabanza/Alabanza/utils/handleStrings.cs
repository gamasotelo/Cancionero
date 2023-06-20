using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace AlabanzaConsola.utils
{
    public class handleStrings
    {
        public string[] pullApartChords(string line)
        {
            line = removeWhiteSpaces(line);
            line = line.Replace("{","");
            line = line.Replace("}", ",");
            line = line.Substring(0, line.Length - 1);
            return line.Split(',');
        }

        public string[] pullApartNoteWithBass(string chord)
        {
            string[] parts = chord.Split('/');
            return new string[] { pullApartNotes(parts[0]), parts[1] };
        }

        public string pullApartNotes(string chord) {
            string alteration = chord;
            if (chord.Length > 1) {
                alteration = chord.Substring(1, 1);
            }

            if (alteration.Equals("#") || alteration.Equals("b")){
                return chord.Substring(0, 2);
            }
            else {
                return chord.Substring(0, 1);
            }
        }

        public int countChars(string line, char element)
        {
            int count = 0;
            foreach (char ch in line)
            {
                if (ch == element)
                {
                    count++;
                }
            }
            return count;
        }

        private string removeWhiteSpaces(string str)
        {
            return Regex.Replace(str, @"\s+", String.Empty);
        }
    }


}
