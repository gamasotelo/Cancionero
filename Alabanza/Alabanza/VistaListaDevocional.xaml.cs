using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alabanza.lists;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Alabanza
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VistaListaDevocional : ContentPage
    {
        public ObservableCollection<string> Items { get; set; }
        listManager lm = new listManager();

        public VistaListaDevocional()
        {
            InitializeComponent();
            Items = new ObservableCollection<string>();
            string[] lists = lm.getContent();
            foreach (string l in lists) {
                Items.Add(l);
            }
            NombresListas.ItemsSource = Items;
        }

        private async void AgregarLista(object sender, EventArgs e)
        {
            string name = await DisplayPromptAsync("Nueva Lista", "Ingrese nombre de la lista: ");
            if (!string.IsNullOrEmpty(name)) {
                if (lm.CreateList(name) == true)
                {
                    //Refrescamos lista de la pantalla
                    NombresListas.ItemsSource = null;
                    Items = new ObservableCollection<string>();
                    string[] lists = lm.getContent();
                    foreach (string l in lists)
                    {
                        Items.Add(l);
                    }
                    NombresListas.ItemsSource = Items;
                }
                else {
                    await DisplayAlert("ERROR", "Ya existe una lista con ese nombre", "aceptar");
                }
            }
        }

        private async void EliminarMenuItem_Clicked(object sender, EventArgs e)
        {
            var ans = await DisplayAlert("Atención!", "¿Desea eliminar lista?", "Si", "No");//retorna true o false
            if (ans == true)
            {
                //CONVERSION DE OBJETO
                // The sender is the menuItem
                MenuItem menuItem = sender as MenuItem;
                // Access the list item through the BindingContext
                var contextItem = menuItem.BindingContext;

                string nombre = contextItem.ToString();
                lm.deleteList(nombre);
                
                //Refrescamos la lista
                NombresListas.ItemsSource = null;
                Items = new ObservableCollection<string>();
                string[] lists = lm.getContent();
                foreach (string l in lists)
                {
                    Items.Add(l);
                }
                NombresListas.ItemsSource = Items;
            }
        }

        private void NombresListas_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;
            this.Navigation.PushModalAsync(new VistaCancionesLista(e.Item.ToString()));
            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }
}