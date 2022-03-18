using MvvmHelpers;
using MvvmHelpers.Commands;
using SmoothNotes.Models;
using SmoothNotes.Services.Storage;
using SmoothNotes.Views.Folder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SmoothNotes.ViewModels
{
    public class BiometricViewModel : ViewModelBase
    {
        public ObservableRangeCollection<ProfileDTO> Profiles { get; set; }
        public AsyncCommand<ProfileDTO> LoginCommand { get; }
        public AsyncCommand GetProfilesCommand { get; }
        public AsyncCommand<ProfileDTO> SelectCommand { get; }

        public BiometricViewModel()
        {
            Profiles = new ObservableRangeCollection<ProfileDTO>();

            LoginCommand = new AsyncCommand<ProfileDTO>(Login);
            GetProfilesCommand = new AsyncCommand(GetProfiles);
            SelectCommand = new AsyncCommand<ProfileDTO>(Select);
        }

        /// <summary>
        /// Select event handler
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private async Task Select(ProfileDTO arg)
        {
            ValueParserService.profileDTO = arg;
        }

        /// <summary>
        /// Login event handler
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private async Task Login(ProfileDTO arg)
        {
            arg.password = await SecureStorage.GetAsync(arg.username);
            ValueParserService.profileDTO = arg;

            if (await ProfileService.Login(ValueParserService.profileDTO))
            {
                var p = await ProfileService.GetProfile(arg.username);
                if (p != null)
                {
                    ValueParserService.profile = p;
                    await Shell.Current.GoToAsync($"//{nameof(FolderNormPage)}");
                }
            }
        }

        /// <summary>
        /// Get all profiles that are regisered with biometric
        /// </summary>
        /// <returns>list of ProfileDTO</returns>
        public async Task GetProfiles()
        {
            List<string> p = await BiometricService.BiometricStatus();
            p = p.OrderBy(f => f).ToList();
            foreach (var item in p)
            {
                Profiles.Add(new ProfileDTO { username = item });
            }
        }
    }
}
