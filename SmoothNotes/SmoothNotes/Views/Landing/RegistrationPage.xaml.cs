using SmoothNotes.Models;
using SmoothNotes.Services.Storage;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmoothNotes.Views.Landing
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistrationPage : ContentPage
    {

        public RegistrationPage()
        {
            InitializeComponent();
        }

        private async void Registration_Clicked(object sender, EventArgs e)
        {
            var p = new ProfileDTO();
            p.username = Username.Text;
            p.password = Password.Text;

            bool letterCh =
                p.password.Any(c => char.IsLetter(c));
            bool digitCh =
                p.password.Any(c => char.IsDigit(c));
            bool upperCh =
                p.password.Any(c => char.IsUpper(c));
            bool lowerCh =
                p.password.Any(c => char.IsLower(c));

            if (p.password.Length < 8 || !letterCh || !digitCh || !upperCh || !lowerCh)
            {
                await Application.Current.MainPage.DisplayToastAsync("Password needs to be more than 8 character,\ncontain small, large letters and numbers.", 3000);
                return;
            }

            var result = await ProfileService.Register(p);

            if(result == "1")
            {
                await Application.Current.MainPage.DisplayToastAsync("Username already taken.", 3000);
                return;
            }else if (result.Contains("Error##"))
            {
                await Application.Current.MainPage.DisplayToastAsync(result, 3000);
                return;
            }
            else
            {
                await Shell.Current.GoToAsync($"..");
                await Application.Current.MainPage.DisplayToastAsync("Profile Created", 1000);
            }
        }
    }
}
