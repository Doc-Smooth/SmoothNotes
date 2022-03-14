using MvvmHelpers;
using SmoothNotes.Services.Storage;
using SmoothNotes.Views.Landing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace SmoothNotes.ViewModels
{
    public class ViewModelBase : BaseViewModel
    {
        internal async Task Logout()
        {
            if (await ValueParserService.Empty())
            {
                await Shell.Current.GoToAsync($"//{nameof(LandingPage)}");
                await Application.Current.MainPage.DisplayToastAsync("Logging out", 1000);

            }
            else
                await Application.Current.MainPage.DisplayToastAsync("Logging out Failed", 2000);
        }
    }
}
