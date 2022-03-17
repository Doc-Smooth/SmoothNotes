using MvvmHelpers;
using MvvmHelpers.Commands;
using SmoothNotes.Models;
using SmoothNotes.Services.Storage;
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
    public class NoteViewModel : ViewModelBase
    {
        public ObservableRangeCollection<Note> Notes { get; set; }
        public AsyncCommand AddCommand { get; }
        public AsyncCommand RefreshCommand { get; }
        public AsyncCommand<Note> SelectedCommand { get; }
        public AsyncCommand<Note> MenuCommand { get; }

        public NoteViewModel()
        {
            Title = "Note Overview";
            Notes = new ObservableRangeCollection<Note>();

            AddCommand = new AsyncCommand(Add);
            RefreshCommand = new AsyncCommand(Refresh);
            SelectedCommand = new AsyncCommand<Note>(Selected);
            MenuCommand = new AsyncCommand<Note>(Menu);
        }

        /// <summary>
        /// Menu event handler
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private async Task Menu(Note arg)
        {
            var nav = App.Current.MainPage.Navigation;
            ValueParserService.note = arg;
            var done = await NavigationExtensions.ShowPopupAsync(nav, new NoteMenu());
            if (Convert.ToString(done) == "refresh")
                await Refresh();
        }

        /// <summary>
        /// Selected/edit event handler
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private async Task Selected(Note arg)
        {
            ValueParserService.note = arg;
            await Shell.Current.GoToAsync($"{nameof(NoteEditPage)}");

        }

        /// <summary>
        /// Refresh list event handler
        /// </summary>
        /// <returns></returns>
        private async Task Refresh()
        {
            IsBusy = true;
            Notes.Clear();
            try
            {
                bool result = await NoteService.Gather();
                if (result)
                {
                    var ns = ValueParserService.folder.Notes;

                    ns = ns.OrderBy(f => f.Name).ToList();
                    foreach (var item in ns)
                    {

                        // Have to check for doubles otherwise it will add all items twice on a fresh reload. Don't know why?
                        var check = true;
                        foreach (var y in Notes)
                        {
                            if (y.Id == item.Id)
                            {
                                check = false;
                            }
                        }
                        if (check)
                        {
                            Notes.Add(item);
                        }
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
        /// Add event handler
        /// </summary>
        /// <returns></returns>
        private async Task Add()
        {
            ValueParserService.note = null;
            var route = $"{nameof(NoteCreatePage)}";
            await Shell.Current.GoToAsync(route);
        }
    }
}
