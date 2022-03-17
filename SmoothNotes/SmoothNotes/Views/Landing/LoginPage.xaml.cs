using SmoothNotes.Models;
using SmoothNotes.Services.Storage;
using SmoothNotes.Views.Folder;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmoothNotes.Views.Landing
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private bool biometric = false;
        public bool Biometric { get => biometric; set => biometric = value; }


        public LoginPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Checks if there is any existing biometrics registered
        /// </summary>
        private async void BioChecker()
        {
            var bios = await BiometricService.BiometricStatus();
            if (bios.Count > 0)
                Biometric = true;
        }


        /// <summary>
        /// Login btn event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Login_Clicked(object sender, EventArgs e)
        {
            var login = new ProfileDTO();
            login.username = Username.Text;
            login.password = Password.Text;
            ValueParserService.profileDTO = login;
            if (await ProfileService.Login(ValueParserService.profileDTO))
            {
                var p = await ProfileService.GetProfile(Username.Text);
                if (p != null)
                {
                    string secret = "";
                    ValueParserService.profile = p;
                    secret = await SecureStorage.GetAsync(p.Name);
                    if(secret == null)
                        await SecureStorage.SetAsync(p.Name, Password.Text);

                    await Application.Current.MainPage.DisplayToastAsync(p.Name + " Logging in", 500);
                    await Shell.Current.GoToAsync($"///{nameof(FolderNormPage)}");
                }
                else
                    await Application.Current.MainPage.DisplayToastAsync("Failed to login", 1000);
            }
            else
                await Application.Current.MainPage.DisplayToastAsync("Failed to login", 1000);
        }

        /// <summary>
        /// Registration btn event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Registration_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"{nameof(RegistrationPage)}");
        }

        /// <summary>
        /// Biometric login btn event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Biometric_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"{nameof(BiometricMenu)}");
        }


        protected override async void OnAppearing()
        {
            base.OnAppearing();

            BioChecker();
            if(Biometric)
                bioBtn.IsVisible = true;
            else
                bioBtn.IsVisible = false;
        }
    }
}
