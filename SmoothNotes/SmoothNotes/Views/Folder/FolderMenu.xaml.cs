using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SmoothNotes.Models;
using SmoothNotes.Services.Storage;
using SmoothNotes.ViewModels;
using Xamarin.CommunityToolkit.Extensions;
using SmoothNotes.Views.Landing;

namespace SmoothNotes.Views.Folder
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FolderMenu : Popup
    {
        private Models.Folder _folder;
        public Models.Folder Folder 
        { 
            get
            {
                return _folder;
            }
            set
            {
                _folder = value;
                OnPropertyChanged();
            }
        }

        public FolderMenu()
        {
            InitializeComponent();
            BindingContext = this;
            Folder = ValueParserService.folder;
        }

        /// <summary>
        /// Edit btn event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Edit_Clicked(object sender, EventArgs e)
        {
            if (!await ProfileService.Refresh())
                await Logout();

            string oTitle = ValueParserService.folder.Name;
            var Title = await App.Current.MainPage.DisplayPromptAsync("Title", ValueParserService.folder.Name, "OK", "Cancel");
            if (oTitle != Title && !string.IsNullOrEmpty(Title))
            {
                ValueParserService.folder.Name = Title;
                if (await FolderService.Edit(ValueParserService.folder))
                {
                    await Application.Current.MainPage.DisplayToastAsync("Folder changed", 1000);
                    Dismiss("refresh");
                }
                else
                    await Application.Current.MainPage.DisplayToastAsync("Something went wrong", 1000);
            }
            else if (string.IsNullOrEmpty(Title))
                await Application.Current.MainPage.DisplayToastAsync("Title can't be empty or spaces", 1000);
        }

        /// <summary>
        /// Favorite btn event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Fav_Clicked(object sender, EventArgs e)
        {
            if (!await ProfileService.Refresh())
                await Logout();

            ValueParserService.folder.Fav = !ValueParserService.folder.Fav;
            if(await FolderService.Edit(ValueParserService.folder)){
                await Application.Current.MainPage.DisplayToastAsync("Folder favorite status changed", 1000);
                Dismiss("refresh");
            }
            else
                await Application.Current.MainPage.DisplayToastAsync("Something went wrong", 1000);
        }

        /// <summary>
        /// Delete btn event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Delete_Clicked(object sender, EventArgs e)
        {
            if (!await ProfileService.Refresh())
                await Logout();

            var result = await Application.Current.MainPage.DisplayAlert("Deleting " + ValueParserService.folder.Name, "Are you sure you want to continue?", "Confirm", "Cancel");
            if(!result)
            {
                await Application.Current.MainPage.DisplayToastAsync("Process canceled", 1000);
            }
            else
            {
                if (await FolderService.Delete(ValueParserService.folder.Id))
                {
                    Dismiss("refresh");
                    await Application.Current.MainPage.DisplayToastAsync("Folder deleted", 1000);
                }
                else
                    await Application.Current.MainPage.DisplayToastAsync("Something went wrong", 1000);
            }

        }

        /// <summary>
        /// Logout event handler
        /// </summary>
        /// <returns></returns>
        internal async Task Logout()
        {
            if (await ValueParserService.Empty())
            {
                await Application.Current.MainPage.DisplayToastAsync("Logging out", 500);
                await Shell.Current.Navigation.PopToRootAsync();
                await Shell.Current.GoToAsync($"///{nameof(LandingPage)}");
                await Shell.Current.Navigation.PopToRootAsync();


            }
            else
                await Application.Current.MainPage.DisplayToastAsync("Logging out Failed", 2000);
        }
    }
}