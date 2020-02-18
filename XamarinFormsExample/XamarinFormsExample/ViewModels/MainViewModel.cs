using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinFormsExample.Models;
using XamarinFormsExample.Services;

namespace XamarinFormsExample.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public ObservableCollection<Personagem> Personagens { get; }
        public Command<Personagem> ExibirDetalhePersonagemCommand { get; }

        private MarvelApiService _marvelApiService;

        public MainViewModel()
        {
            Titulo = "Heróis Marvel";
            Personagens = new ObservableCollection<Personagem>();
            ExibirDetalhePersonagemCommand = new Command<Personagem>(ExecuteExibirDetalhePersonagemCommand);
            _marvelApiService = new MarvelApiService();
        }

        private async void ExecuteExibirDetalhePersonagemCommand(Personagem personagem)
        {
            await Navigation.PushAsync<DetalhesViewModel>(false, personagem);
        }

        public override async Task LoadAsync()
        {
            Ocupado = true;

            try
            {
                var personagens = await _marvelApiService.GetPersonagensAsync();
                Personagens.Clear();

                foreach(var person in personagens)
                {
                    Personagens.Add(person);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Erro", ex.Message);
            }
            finally
            {
                Ocupado = false;
            }
        }
    }
}
