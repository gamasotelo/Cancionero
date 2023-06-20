using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlabanzaConsola.songs;
using Alabanza.songs;
using Xamarin.Essentials;
using System.IO;
using Xamarin.Forms;

namespace Alabanza
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            
        }

        private void abrirRepertorio(object sender, EventArgs e)
        {

             this.Navigation.PushModalAsync(new VistaListas());
        }


        private async void abrirImportar(object sender, EventArgs e)
        {
            try {
                var file = await FilePicker.PickAsync();
                if (file == null || !file.FileName.EndsWith(".txt")) {
                    return;
                }

                SongManager sm = new SongManager();
                sm.importSongs(file.FullPath);
                await DisplayAlert("Exito", "La lista se ha importado con exito", "Hecho");

                //EL CODIGO DE ABAJO SERVIRA PARA EXPORTAR
                /*string[] paths = {
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),"AtiElAlfaYLaOmega.txt"),
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),"DiosEstaAquí.txt"),
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),"QuieroLevantarMisManos.txt"),
                    //Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),"Renuevame.txt")
                };
                
                sm.exportSongs(paths);*/



            }
            catch (Exception extension) {
                await DisplayAlert("Error!", "Fue imposible leer el archivo :(", "Hecho");
                Console.WriteLine(extension.ToString());
            }
        }

        private void AbrirListas(object sender, EventArgs e)
        {
            this.Navigation.PushModalAsync(new VistaListaDevocional());
        }
    }
}
