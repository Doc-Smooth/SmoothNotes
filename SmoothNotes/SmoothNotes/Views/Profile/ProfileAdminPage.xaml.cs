using MvvmHelpers;
using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;
using SmoothNotes.Services.Storage;
using SmoothNotes.Views.Landing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmoothNotes.Views.Profile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfileAdminPage : ContentPage
    {
        private bool biometric = false;

        public bool Biometric 
        { 
            get
            {
                return biometric;
            }
            set
            {
                biometric = value;
                if(value)
                    bio_switch.IsToggled = true;
                else
                    bio_switch.IsToggled = false;
            }
        }
        public ProfileAdminPage()
        {
            InitializeComponent();
        }

        private void Switch_BindingContextChanged(object sender, EventArgs e)
        {
            if (!(sender is Switch s)) return;

            s.Toggled += Switch_Toggled;
        }

        private async void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            if (!await ProfileService.Refresh())
                await Logout();

            if (e.Value)
            {
                var request = new AuthenticationRequestConfiguration("Activate Biometrics", "Scan Finger");
                var result = await CrossFingerprint.Current.AuthenticateAsync(request);
                if (result.Authenticated)
                {
                    await BiometricService.BioStatusChange(ValueParserService.profile.Name);
                    Biometric = true;
                }
                else
                    Biometric = false;
            }
            else
            {
                await BiometricService.BioStatusChange(ValueParserService.profile.Name);
                Biometric = false;
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (!await ProfileService.Refresh())
                await Logout();

            var status = await DisplayAlert("Deleting Profile", "Warning:\nThis is an irreversible action.\nWould you like to continue?", "Confirm", "Cancel");
            var result = false;
            if (status)
            {
                result = await ProfileService.Delete(ValueParserService.profile.Id);
                if (result)
                {
                    await Application.Current.MainPage.DisplayToastAsync("Profile Deleted", 1000);
                    await Logout();
                }
                else
                    await Application.Current.MainPage.DisplayToastAsync("Something went wrong", 2000);
            }
            else
                return;
        }

        protected override async void OnAppearing()
        {
            if (!await ProfileService.Refresh())
                await Logout();

            base.OnAppearing();

            //Sets biometric option to match saved value. Removes event then sets the bool and adds event back in.
            var bios = await BiometricService.BiometricStatus();
            bio_switch.Toggled -= Switch_Toggled;
            Biometric = false;
            foreach (var item in bios)
            {
                if (item == ValueParserService.profile.Name)
                    Biometric = true;
            }
            bio_switch.Toggled += Switch_Toggled;
        }

        /// <summary>
        /// Logout btn event handler
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