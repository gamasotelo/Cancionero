using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlabanzaConsola.tones
{
    public class Note
    {
        public string data;
        public Note next;
        public Note prev;

        public Note(string data, Note next, Note prev) {
            this.data = data;
            this.next = next;
            this.prev = prev;
        }
    }
}
