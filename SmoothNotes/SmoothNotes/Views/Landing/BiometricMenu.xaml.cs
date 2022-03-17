using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;
using SmoothNotes.Models;
using SmoothNotes.Services.Storage;
using SmoothNotes.ViewModels;
using SmoothNotes.Views.Folder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmoothNotes.Views.Landing
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BiometricMenu : ContentPage
    {
        public List<string> profiles { get; set; }
        public BiometricMenu()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Biometric identification
            var request = new AuthenticationRequestConfiguration("Authentication", "Authenticate Identity")
            {
                CancelTitle = "Cancel",
                FallbackTitle = "Password/PIN",
                AllowAlternativeAuthentication = true,
                ConfirmationRequired = false
            };
            var result = await CrossFingerprint.Current.AuthenticateAsync(request);
            if (result.Authenticated)
            {
                profiles = await BiometricService.BiometricStatus();
                profiles = profiles.OrderBy(f => f).ToList();
                ListView.ItemsSource = profiles;
                //var vm = (BiometricViewModel)BindingContext;
                //await vm.GetProfilesCommand.ExecuteAsync();
            }
            else
            {
                await Shell.Current.GoToAsync($"..");
            }
        }

        /// <summary>
        /// Item tab event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ProfileDTO pf = new ProfileDTO();
            pf.username = (string)e.Item;
            pf.password = await SecureStorage.GetAsync(pf.username);
            ValueParserService.profileDTO = pf;

            if (await ProfileService.Login(ValueParserService.profileDTO))
            {
                var p = await ProfileService.GetProfile(pf.username);
                if (p != null)
                {
                    ValueParserService.profile = p;
                    await Shell.Current.GoToAsync($"///{nameof(FolderNormPage)}");
                }
            }
        }
    }
}