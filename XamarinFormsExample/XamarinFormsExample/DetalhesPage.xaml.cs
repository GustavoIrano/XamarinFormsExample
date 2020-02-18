using System.ComponentModel;
using Xamarin.Forms;
using XamarinFormsExample.ViewModels;

namespace XamarinFormsExample
{
    [DesignTimeVisible(false)]
    public partial class DetalhesPage : ContentPage
    {
        private DetalhesViewModel viewModel => BindingContext as DetalhesViewModel;

        public DetalhesPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await viewModel.LoadAsync();
        }
    }
}