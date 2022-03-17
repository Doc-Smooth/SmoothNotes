using SmoothNotes.Services.Storage;
using SmoothNotes.ViewModels;
using SmoothNotes.Views.Landing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmoothNotes.Views.Note
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NoteMenu : Popup
    {
        private Models.Note _note;
        public Models.Note Note
        {
            get
            {
                return _note;
            }
            set
            {
                _note = value;
                OnPropertyChanged();
            }
        }
        public NoteMenu()
        {
            InitializeComponent();
            BindingContext = this;
            Note = ValueParserService.note;
        }

        /// <summary>
        /// Click event handler for the delete btn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (!await ProfileService.Refresh())
                await Logout();

            var confirmation = await Application.Current.MainPage.DisplayAlert("Deleting " + Note.Name, "Are you sure you want to continue?", "Confirm", "Cancel");
            if (!confirmation)
                return;

            var result = await NoteService.Remove(Note.Id);
            if (result)
            {
                Dismiss("refresh");
                await Application.Current.MainPage.DisplayToastAsync("Note deleted", 1000);
            }
            else
                await Application.Current.MainPage.DisplayToastAsync("Something went wrong", 2000);
        }

        /// <summary>
        /// Function to handle logout
        /// </summary>
        /// <returns></returns>
        internal async Task Logout()
        {
            if (await ValueParserService.Empty())
            {
                await Application.Current.MainPage.DisplayToastAsync("Logging out", 1000);
                await Shell.Current.Navigation.PopToRootAsync();
                await Shell.Current.GoToAsync($"///{nameof(LandingPage)}");
                await Shell.Current.Navigation.PopToRootAsync();

            }
            else
                await Application.Current.MainPage.DisplayToastAsync("Logging out Failed", 2000);
        }
    }
}