using SmoothNotes.ViewModels;
using SmoothNotes.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using SmoothNotes.Views.Landing;
using SmoothNotes.Views.Folder;
using SmoothNotes.Views.Note;
using SmoothNotes.Views.Profile;
using System.Linq;
using SmoothNotes.Services.Storage;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;

namespace SmoothNotes
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();

            /// NOTE: Routes not directly available in the shell. Meaning routes that are not absolute routes.
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(RegistrationPage), typeof(RegistrationPage));
            Routing.RegisterRoute(nameof(BiometricMenu), typeof(BiometricMenu));

            Routing.RegisterRoute(nameof(FolderMenu), typeof(FolderMenu));

            Routing.RegisterRoute(nameof(NotePage), typeof(NotePage));
            Routing.RegisterRoute(nameof(NoteEditPage), typeof(NoteEditPage));
            Routing.RegisterRoute(nameof(NoteCreatePage), typeof(NoteCreatePage));
            Routing.RegisterRoute(nameof(SearchResultPage), typeof(SearchResultPage));
        }

        //private async void OnMenuItemClicked(object sender, EventArgs e)
        //{
        //    await Shell.Current.GoToAsync("//LoginPage");
        //}
    }
}
