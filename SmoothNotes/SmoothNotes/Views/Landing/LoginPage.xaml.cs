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
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void Login_Clicked(object sender, EventArgs e)
        {
            var login = new ProfileDTO();
            login.username = Username.Text;
            login.password = Password.Text;
            ValueParserService.profileDTO = login;
            if (await ProfileService.Login(ValueParserService.profileDTO))
            {
                var p = await ProfileService.GetProfile(Username.Text);
                if(p != null)
                {
                    string secret = "";
                    ValueParserService.profile = p;
                    try
                    {
                        secret = await SecureStorage.GetAsync(p.Name);
                    }
                    catch (Exception)
                    {
                        await SecureStorage.SetAsync(p.Name, Password.Text);
                    }
                    await Application.Current.MainPage.DisplayToastAsync(p.Name + " Logging in", 500);
                    await Shell.Current.GoToAsync($"//{nameof(FolderNormPage)}");
                } 
                else
                    await Application.Current.MainPage.DisplayToastAsync("Failed to login", 1000);
            }
            else
                await Application.Current.MainPage.DisplayToastAsync("Failed to login", 1000);
        }

        private async void Registration_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"{nameof(RegistrationPage)}");
        }
    }
}
