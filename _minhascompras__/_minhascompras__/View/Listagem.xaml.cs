using _minhascompras__.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace _minhascompras__.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Listagem : ContentPage
    {

        ObservableCollection<Produto> lista_produtos = new ObservableCollection<Produto>();
        public Listagem()
        {
            InitializeComponent();
            
            lst_produtos.ItemsSource = lista_produtos;

        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            double soma = lista_produtos.Sum(i => i.PrecoTotal);

            DisplayAlert("Total: ", soma.ToString("C"), "OK");
        }

        private void ToolbarItem_Clicked_1(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Formulario());
        }

        protected override void OnAppearing()
        {
            if (lista_produtos.Count == 0)
            {
                Task.Run(async () =>
                {
                    List<Produto> temp = await App.Db.GellAll();

                    foreach (Produto p in temp)
                    {
                        lista_produtos.Add(p);
                    }
                });
            }
        }
        private async void MenuItem_Clicked(object sender, EventArgs e)
        {
            MenuItem disparador = sender as MenuItem;

            Produto produto_selecionado = (Produto)disparador.BindingContext;

            string mensagem = "Remover " + produto_selecionado.Nome + " da compra? ";

            bool confirmacao = await DisplayAlert("Tem Certeza?", mensagem, "Sim", "Não");

            if (confirmacao)
            {
                await App.Db.Delete(produto_selecionado.Id);
                lista_produtos.Remove(produto_selecionado);
            }
        }

        private void lst_produtos_Refreshing(object sender, EventArgs e)
        {
            lista_produtos.Clear();

            Task.Run(async () =>
            {
                List<Produto> temp = await App.Db.GellAll();

                foreach (Produto p in temp)
                {
                    lista_produtos.Add(p);
                }
            });

            ref_carregando.IsRefreshing = false;
        }


        private void txt_search_TextChanged(object sender, TextChangedEventArgs e)
        {
            string q = e.NewTextValue;

            lista_produtos.Clear();

            Task.Run(async () =>
            {
                List<Produto> temp = await App.Db.Search(q);

                foreach (Produto p in temp)
                {
                    lista_produtos.Add(p);
                }
            });
        }

        private void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Produto produto_selecionado = e.SelectedItem as Produto;

            Navigation.PushAsync(new Formulario
            {
                BindingContext = produto_selecionado
            });
        }
    }
}