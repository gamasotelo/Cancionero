using System;
using System.IO;
using Xamarin.Forms;
using AlabanzaConsola.songs;
using Xamarin.Essentials;
using Xamarin.Forms.Xaml;
using System.Diagnostics;
using System.Drawing;

namespace Alabanza
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VistaCancion : ContentPage
    {
        string songName;
        string[] content;
        Song song;
        string[] songsList;
        public VistaCancion(string songName, string[] songsList)
        {
            InitializeComponent();
            
            //Sin esta linea no se detectan los cambios de orientación de la pantalla
            DeviceDisplay.MainDisplayInfoChanged += OnMainDisplayInfoChanged;

            this.songsList = songsList;
            this.songName = songName;
            nameSong.Text = songName;   
            string path = getPath(songName);
            song = new Song(path);
            content = song.print();

            getMaxChars();

            if (getDisplayOrientation().Equals("vertical"))
            {
                printOnScreen();
            } else {
                printOnScreenTablet();
            }
        }


        private string getDisplayOrientation() {
            var displayInformation = DeviceDisplay.MainDisplayInfo;
            // Verificar la orientación actual del dispositivo
            var orientation = displayInformation.Orientation;
            // Portrait = vertical : Landscape = horizontal
            string result = (orientation == Xamarin.Essentials.DisplayOrientation.Portrait) ? "vertical" : "horizontal";
            return result;
        }


        void OnMainDisplayInfoChanged(object sender, DisplayInfoChangedEventArgs e)
        {
            //ESTE METODO DETECTA SI LA PANTALLA CAMBIO DE ORIENTACIÓN
            // Process changes
            var displayInfo = e.DisplayInfo;

            cancion.Text = "";
            content = song.print();
            if (getDisplayOrientation().Equals("vertical"))
            {
                printOnScreen();
            }
            else
            {
                printOnScreenTablet();
            }
            
        }



        //PARA TABLETA

        public void printOnScreenTablet()
        {
            string songString = "";

            foreach (string line in content)
            {
                if (line != null)
                {
                    string linea = line;
                    if (line.Contains("{"))
                    {
                        linea = line.Replace('{', ' ');
                        linea = linea.Replace('}', ' ');
                    }
                    if (songString.Equals(""))
                    {
                        songString = linea + "\n";
                    }
                    else
                    {
                        songString = songString + linea + "\n";
                    }
                }
            }
            cancion.Text = songString;
        }

        //PARA CELULAR

        public void printOnScreen() {
            string songString = "";
            //titulo.Text = songName;
            int max = getMaxChar();
            int i = 0;

            while(i<content.Length-1){
                if (content[i].Contains("{"))
                {
                    content[i] = content[i].Replace("{", " ");
                    content[i] = content[i].Replace("}", " ");
                    string[] words = content[i + 1].Split(' ');//obtenemos parte de la letra y la dividimos por palabras
                    int lastWord = getLastWord(words, max);//obtenemos ultima palabra dentro del limite de caracteres admitido

                    if ((content[i + 1].Length + 1) != lastWord)//si la linea de la letra no cabe
                    { //El +1 es por el espacio que agrega el algoritmo en lasword
                        string line2 = content[i + 1].Substring(0, lastWord);
                        string line4 = content[i + 1].Substring(lastWord);//Hasta aqui termina acomodo de letra
                        //Comienza acomodo de acordes
                        string line1;
                        string line3;
                        if (content[i].Length <= lastWord)
                        {//si la linea de acordes es mas pequeña que la de la letra
                            line1 = content[i];
                            songString = songString + line1 + "\n" + line2 + "\n" + line4 + "\n";
                        }
                        else
                        {

                            if (!content[i].Substring(lastWord - 1, 1).Equals(" "))//si el lugar de corte no es un espacio
                            {
                                int back = 2;
                                while (true)
                                {
                                    if (content[i].Substring(lastWord - back, 1).Equals(" ") || content[i].Substring(lastWord - back, 1).Equals("{"))
                                    {
                                        line1 = content[i].Substring(0, (lastWord - back) + 1);
                                        line3 = content[i].Substring((lastWord - back) + 1);
                                        for (int m = 0; m < back; m++)
                                        {
                                            line4 = " " + line4;
                                        }
                                        songString = songString + line1 + "\n" + line2 + "\n" + line3 + "\n" + line4 + "\n";
                                        break;
                                    }
                                    back++;
                                }
                            }
                            else
                            {
                                line1 = content[i].Substring(0, lastWord);
                                line3 = content[i].Substring(lastWord);
                                songString = songString + line1 + "\n" + line2 + "\n" + line3 + "\n" + line4 + "\n";
                            }
                        }
                        i += 2;
                        continue;
                    }else {//SI LA LINEA DE LA LETRA SI CABE

                        string line2;
                        string line4;
                        //Comienza acomodo de acordes
                        string line1;
                        string line3;

                        line2 = content[i+1];

                        if (content[i].Length > lastWord)//SI LA LINEA DE LOS ACORDES ES MAS GRADE QUE LA DE LA LETRA
                        {
                            if (content[i].Length > max)
                            {
                                line1 = content[i].Substring(0, lastWord);
                                line3 = content[i].Substring(lastWord);
                                songString = songString + line1 + "\n" + line2 + "\n" + line3 + "\n";
                                i += 2;
                                continue;
                            }
                            else {
                                line1 = content[i];
                                songString = songString + line1 + "\n" + line2 + "\n";
                                i += 2;
                                continue;
                            }
                            
                        }

                        line1 = content[i];
                        songString = songString + line1 + "\n" + line2 + "\n";
                        i +=2;
                        continue;
                    }
                }
                    songString = songString + content[i] + "\n";
                    i++;
                              
            }
            //AQUI SE REALIZA LA IMPRESION
            cancion.Text = songString;
            Debug.WriteLine(songString);
        }

        private int getLastWord(string[] words, int max) {
            int count = 0;
            foreach (string word in words) {
                int lastCount = count;
                count = count + word.Length + 1;//lo que habia en cuenta + la palabra + el espacio en blanco
                if (count>max) {
                    return lastCount;
                }
            }
            return count;
        }

        private void goNextSong() {
            string aux = getNextSong();
            if(aux != null)
            {
                this.songName = aux;
                string path = getPath(songName);
                nameSong.Text = songName;
                song = new Song(path);
                content = song.print();
                printOnScreen();
            }  
        }

        private string getNextSong() {
            for (int i = 0; i<songsList.Length;i++) {
                if (songsList[i].Equals(songName)) {
                    if (i != songsList.Length - 1) {
                        return songsList[i + 1];
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            return null;
        }

        private void goPreviousSong()
        {
            string aux = getPreviousSong();
            if (aux != null)
            {
                this.songName = aux;
                string path = getPath(songName);
                nameSong.Text = songName;
                song = new Song(path);
                content = song.print();
                printOnScreen();
            }
        }

        private string getPreviousSong()
        {
            for (int i = 0; i < songsList.Length; i++)
            {
                if (songsList[i].Equals(songName))
                {
                    if (i!=0)
                    {
                        return songsList[i-1];
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            return null;
        }

        private void getMaxChars() {
            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
            var densidad = mainDisplayInfo.Density;
            var ResoluciónPantallaWidth = mainDisplayInfo.Width;
            var longitudFuente = 18;

            Debug.WriteLine(densidad / ResoluciónPantallaWidth * longitudFuente);
        }

        private int getMaxChar(){

            if (cancion.FontSize == 16) {
                return 32;
            }
            return 32;
        }

        private void subirTono(object sender, EventArgs e)
        {
            song.upHalfTone();
            content = song.print();
            printOnScreen();
        }

        private void bajarTono(object sender, EventArgs e)
        {
            song.downHalfTone();
            content = song.print();
            printOnScreen();
        }

        private string getPath(string songName) {
            string path = songName.Replace(" ", "");
            path = path + ".txt";
            path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), path);
            return path;
        }

        private async void SwipeGestureRecognizer_Swiped(object sender, SwipedEventArgs e)
        {
            Console.WriteLine("LA DIRECCION FUE " + e.Direction);
            switch (e.Direction)
            {
                case SwipeDirection.Left:
                    goNextSong();
                    break;
                case SwipeDirection.Right:
                    goPreviousSong();
                    break;
            }
        }
    }
}