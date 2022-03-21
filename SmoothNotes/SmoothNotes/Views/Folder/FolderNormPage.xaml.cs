using SmoothNotes.Services.Storage;
using SmoothNotes.ViewModels;
using SmoothNotes.Views.Landing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmoothNotes.Views.Folder
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FolderNormPage : ContentPage
    {
        public FolderNormPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Logout btn event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Logout_Clicked(object sender, EventArgs e)
        {
            var vm = (FolderViewModel)BindingContext;
            await vm.Logout();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                var vm = (FolderViewModel)BindingContext;
                await vm.RefreshCommand.ExecuteAsync();
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayToastAsync(e.Message, 2000);
            }
        }

        //protected override async void OnDisappearing()
        //{
        //    base.OnDisappearing();
        //    if (!await ProfileService.Refresh())
        //        await Logout();
        //}

        internal async Task Logout()
        {
            //Overrides all stored values and logs profile out.
            if (await ValueParserService.Empty())
            {
                await Application.Current.MainPage.DisplayToastAsync("Logging out", 1000);
                await Shell.Current.Navigation.PopToRootAsync();
                await Shell.Current.GoToAsync($"///{nameof(LandingPage)}");
                //await Shell.Current.Navigation.PopToRootAsync();

            }
            else
                await Application.Current.MainPage.DisplayToastAsync("Logging out Failed", 2000);
        }
    }
}