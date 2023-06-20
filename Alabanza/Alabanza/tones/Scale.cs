using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlabanzaConsola.tones
{
    public class Scale
    {
        Note head;
        Note tail;
        public int size;

        public Scale() {
            this.head = null;
            this.tail = null;
            this.size = 0;
        }

        public void addNote(string data) {
            Note newNote = new Note(data, this.head, this.tail);

            if (this.tail != null)
            {
                newNote.prev = this.tail;
                this.tail.next = newNote;
                this.tail = newNote;
                this.head.prev = this.tail;
            }
            else
            {
                this.tail = newNote;
                this.head = newNote;
            }
            this.size++;
        }

        public string print()
        {
            Note current = this.head;
            string result = "";
            int count = 1;
            while (count <= 12)
            {
                result += current.data + " <-> ";
                current = current.next;
                count++;
            }
            return result += " X ";
        }

        public string upTone(string tone)
        {
            Note current = this.head;
            Note previus;
            int count = 1;
            while (count <= 12)
            {
                previus = current;
                current = current.next;
                String currentTone = current.data;
                if (tone == currentTone)
                {
                    previus = current;
                    current = current.next;
                    return current.data;
                }
            }
            return "this shouldn't execute";
        }

        public string downTone(string tone)
        {
            Note current = this.head;
            Note previus;
            int count = 12;
            while (count <= 12)
            {
                previus = current;
                current = current.next;
                string currentTone = current.data;
                if (tone == currentTone)
                {
                    return current.prev.data;
                }
            }
            return "this shouldn't execute";
        }

        public string getEnharmonic(string note, Scale scale){
            Note current = this.head;
            Note previus;

            Note scaleCurrent = scale.head;
            Note scalePrevius;

            int count = 1;
            string enharmonic = "";
            while (count <= 12)
            {
                previus = current;
                current = current.next;

                scalePrevius = scaleCurrent;
                scaleCurrent = scaleCurrent.next;

                String currentTone = current.data;
                if (note == currentTone)
                {
                    enharmonic = scaleCurrent.data;
                    return enharmonic;
                }
            }
            return "this shouln't execute";
        }




    }
}
