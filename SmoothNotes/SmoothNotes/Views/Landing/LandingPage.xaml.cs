using SmoothNotes.Services.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmoothNotes.Views.Landing
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LandingPage : ContentPage
    {
        public LandingPage()
        {
            InitializeComponent();

            //Makes sure securestore value is initialized.
            BiometricService.FirstTimeSetup();
        }

        /// <summary>
        /// Image btn event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ImageButton_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"{nameof(LoginPage)}");
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            //await Shell.Current.Navigation.PopToRootAsync();
        }
    }
}