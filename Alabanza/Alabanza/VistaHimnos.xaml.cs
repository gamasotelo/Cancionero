using Alabanza.songs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Alabanza.lists;

namespace Alabanza
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class VistaHimnos : ContentPage
    {
        public ObservableCollection<string> Items { get; set; }
        SongList songList;
        public VistaHimnos()
        {
            InitializeComponent();
            songList = new SongList("hymn.txt");
            Items = new ObservableCollection<string>();
            string[] songs = songList.readSongs();
            Console.WriteLine(songs.Length);
            foreach (string song in songs)
            {
                if (!song.Equals(""))
                {
                    Items.Add(song);
                }

            }
            string[] ordenar = Items.ToArray();
            Array.Sort(ordenar);
            ListaHimnos.ItemsSource = ordenar;
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            this.Navigation.PushModalAsync(new VistaCancion(e.Item.ToString(),songList.readSongs()));

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            ListaHimnos.ItemsSource = Items.Where(s => s.ToLower().Contains(e.NewTextValue.ToLower()));
        }

        private async void EliminarMenuItem_Clicked(object sender, EventArgs e)
        {
            var ans = await DisplayAlert("Atención!", "¿Desea eliminar canción?", "Si", "No");//retorna true o false
            if (ans == true)
            {
                //CONVERSION DE OBJETO
                // The sender is the menuItem
                MenuItem menuItem = sender as MenuItem;
                // Access the list item through the BindingContext
                var contextItem = menuItem.BindingContext;

                SongManager sm = new SongManager();
                string nombre = contextItem.ToString();
                sm.deleteSong(nombre, "hymn.txt");

                //Refrescamos la lista
                ListaHimnos.ItemsSource = null;
                SongList songList = new SongList("hymn.txt");
                string[] songs = songList.readSongs();
                Items = new ObservableCollection<string>();
                foreach (string song in songs)
                {
                    if (!song.Equals(""))
                    {
                        Items.Add(song);
                    }
                }
                string[] ordenar = Items.ToArray();
                Array.Sort(ordenar);
                ListaHimnos.ItemsSource = ordenar;
            }
        }

        private async void AgregarItemLista_Clicked(object sender, EventArgs e)
        {
            //obtenemos cancion
            MenuItem menuItem = sender as MenuItem;
            var contextItem = menuItem.BindingContext;
            string newSong = contextItem.ToString();

            //Obtenemos listas para mostrar
            listSongs list = new listSongs("listNames.txt");
            string[] listContent = list.getContent();

            try
            {
                //Obtenemos lista a ingresar
                string selected = await DisplayActionSheet("¿Agregar a que lista?", "cancelar", null, listContent);
                if (!selected.Equals("cancelar"))
                {
                    //ingresamos cancion
                    Console.WriteLine("EL NOMBRE DE LISTA ENVIADO ES: " + selected);
                    listSongs listNewSong = new listSongs(selected);
                    listNewSong.addSongToList(newSong);
                }
            }
            catch (Exception exc)
            {

            }
        }
    }
}