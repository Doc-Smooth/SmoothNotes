using SmoothNotes.Models;
using SmoothNotes.Services.Storage;
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
    public partial class RegistrationPage : ContentPage
    {

        public RegistrationPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Registration btn event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Registration_Clicked(object sender, EventArgs e)
        {
            // Adds fields to a new object
            var p = new Register();
            p.Id = "";
            p.Name = Username.Text;
            p.Pw = Password.Text;

            //Checks for different criteria, large, small letters, numbers and so on
            bool letterCh =
                p.Pw.Any(c => char.IsLetter(c));
            bool digitCh =
                p.Pw.Any(c => char.IsDigit(c));
            bool upperCh =
                p.Pw.Any(c => char.IsUpper(c));
            bool lowerCh =
                p.Pw.Any(c => char.IsLower(c));

            if (p.Pw.Length < 8 || !letterCh || !digitCh || !upperCh || !lowerCh)
            {
                await Application.Current.MainPage.DisplayToastAsync("Password needs to be more than 8 character,\ncontain small, large letters and numbers.", 3000);
                return;
            }

            var result = await ProfileService.Register(p);

            //Handles different return codes
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
