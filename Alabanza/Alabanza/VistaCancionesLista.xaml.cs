using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alabanza.lists;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Alabanza
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VistaCancionesLista : ContentPage
    {
        listSongs lista;
        public ObservableCollection<string> Items { get; set; }
        public VistaCancionesLista(string listName)
        {
            InitializeComponent();
            Titulo.Text = listName;
            lista = new listSongs(listName);
            Items = new ObservableCollection<string>();
            string[] lists = lista.getContent();
            foreach (string l in lists)
            {
                Items.Add(l);
            }

            Canciones.ItemsSource = Items;
        }

        private void Canciones_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            this.Navigation.PushModalAsync(new VistaCancion(e.Item.ToString(),lista.getContent()));

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        private void agregarCancion(object sender, EventArgs e)
        {
            this.Navigation.PushModalAsync(new VistaListas());
        }

        private void EliminarMenuItem_Clicked(object sender, EventArgs e)
        {
            //ELIMINAMOS CANCION DEL DOCUMENTO
            MenuItem menuItem = sender as MenuItem;
            var contextItem = menuItem.BindingContext;
            string nameSong = contextItem.ToString();
            lista.deleteSongFromList(nameSong);

            //REFRESCAMOS LISTA
            Items = new ObservableCollection<string>();
            Canciones.ItemsSource = null;
            string[] lists = lista.getContent();
            foreach (string l in lists)
            {
                Items.Add(l);
            }
            Canciones.ItemsSource = Items;
        }

        private void subirNivel_Clicked(object sender, EventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            var contextItem = menuItem.BindingContext;
            string nameSong = contextItem.ToString();
            lista.subirCancion(nameSong);

            //REFRESCAMOS LISTA
            Items = new ObservableCollection<string>();
            Canciones.ItemsSource = null;
            string[] lists = lista.getContent();
            foreach (string l in lists)
            {
                Items.Add(l);
            }
            Canciones.ItemsSource = Items;
        }

        private void bajarNivel_Clicked(object sender, EventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            var contextItem = menuItem.BindingContext;
            string nameSong = contextItem.ToString();
            lista.bajarCancion(nameSong);

            //REFRESCAMOS LISTA
            Items = new ObservableCollection<string>();
            Canciones.ItemsSource = null;
            string[] lists = lista.getContent();
            foreach (string l in lists)
            {
                Items.Add(l);
            }
            Canciones.ItemsSource = Items;
        }
    }

}