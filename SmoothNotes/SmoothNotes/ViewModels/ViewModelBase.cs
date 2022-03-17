using MvvmHelpers;
using Plugin.Fingerprint;
using SmoothNotes.Services.Storage;
using SmoothNotes.Views.Folder;
using SmoothNotes.Views.Landing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace SmoothNotes.ViewModels
{
    /// <summary>
    /// Base for ViewModels
    /// </summary>
    public class ViewModelBase : BaseViewModel
    {

        /// <summary>
        /// Logout event
        /// </summary>
        /// <returns></returns>
        internal async Task Logout()
        {
            //Overrides all stored values and logs profile out.
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
