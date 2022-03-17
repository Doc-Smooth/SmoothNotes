using MvvmHelpers;
using MvvmHelpers.Commands;
using SmoothNotes.Models;
using SmoothNotes.Services.Storage;
using SmoothNotes.Views.Folder;
using SmoothNotes.Views.Landing;
using SmoothNotes.Views.Note;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace SmoothNotes.ViewModels
{
    /// <summary>
    /// Used by both FolderNorm and FolderFav Pages
    /// </summary>
    public class FolderViewModel : ViewModelBase
    {
        public ObservableRangeCollection<Folder> Folders { get; set; }
        public ObservableRangeCollection<Folder> FoldersFav { get; set; }
        public AsyncCommand AddCommand { get; }
        public AsyncCommand RefreshCommand { get; }
        public AsyncCommand<Folder> SelectedCommand { get; }
        public AsyncCommand<Folder> MenuCommand { get; }

        public FolderViewModel()
        {
            Title = "Folder Overview";
            Folders = new ObservableRangeCollection<Folder>();
            FoldersFav = new ObservableRangeCollection<Folder>();
            foreach (var item in ValueParserService.profile.folders)
            {
                Folders.Add(item);
                if (item.Fav)
                    FoldersFav.Add(item);
            }

            AddCommand = new AsyncCommand(Add);
            RefreshCommand = new AsyncCommand(Refresh);
            SelectedCommand = new AsyncCommand<Folder>(Selected);
            MenuCommand = new AsyncCommand<Folder>(Menu);
        }

        /// <summary>
        /// Menu event handler
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private async Task Menu(Folder arg)
        {
            var nav = App.Current.MainPage.Navigation;
            ValueParserService.folder = arg;
            var done = await NavigationExtensions.ShowPopupAsync(nav, new FolderMenu());
            if(Convert.ToString(done) == "refresh")
                await Refresh();
        }

        /// <summary>
        /// Select/Edit event handler
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private async Task Selected(Folder arg)
        {
            ValueParserService.folder = arg;
            await Shell.Current.GoToAsync($"{nameof(NotePage)}");
        }

        /// <summary>
        /// Refresh list event handler
        /// </summary>
        /// <returns></returns>
        private async Task Refresh()
        {
            IsBusy = true;
            Folders.Clear();
            FoldersFav.Clear();

            if (!await ProfileService.Refresh())
                await Logout();

            List<Folder> folders = new List<Folder>();
            folders = await FolderService.GetProfileFolders();
            if(folders == null)
                folders = new List<Folder>();
            try
            {
                folders = folders.OrderBy(f => f.Name).ToList();
                foreach (var item in folders)
                {

                    // Have to check for doubles otherwise it will add all items twice on a fresh reload. Don't know why?
                    var check = true;
                    foreach (var y in Folders)
                    {
                        if (y.Id == item.Id)
                        {
                            check = false;
                        }
                    }
                    if (check)
                    {
                        Folders.Add(item);
                        if (item.Fav)
                            FoldersFav.Add(item);
                    }
                }
                IsBusy = false;
                await Application.Current.MainPage.DisplayToastAsync("Reloaded", 1000);
            }
            catch (Exception e)
            {
                IsBusy = false;
                await Application.Current.MainPage.DisplayToastAsync(e.Message, 2000);
            }
        }

        /// <summary>
        /// Add folder event handler
        /// </summary>
        /// <returns></returns>
        private async Task Add()
        {
            try
            {
                if (!await ProfileService.Refresh())
                    await Logout();

                var Title = await App.Current.MainPage.DisplayPromptAsync("Title", "Folder title", "OK", "Cancel");
                if (string.IsNullOrWhiteSpace(Title))
                {
                    await Application.Current.MainPage.DisplayToastAsync("Folders need a title", 1000);
                    return;
                }
                //await FolderService.Add(SelectedService.profile.Id, Title);
                if(await FolderService.Add(Title))
                {
                    await Application.Current.MainPage.DisplayToastAsync("Folder created", 1000);
                    await Refresh();
                }
                else
                    await Application.Current.MainPage.DisplayToastAsync("Something went wrong", 1000);
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayToastAsync(e.Message, 2000);
            }
        }
    }
}
