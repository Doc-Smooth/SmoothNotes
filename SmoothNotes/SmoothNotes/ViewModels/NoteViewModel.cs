using MvvmHelpers;
using MvvmHelpers.Commands;
using SmoothNotes.Models;
using SmoothNotes.Services.Storage;
using SmoothNotes.Views.Note;
using System;
using System.Collections.Generic;
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
        public AsyncCommand<Note> RemoveCommand { get; }
        public AsyncCommand<Note> EditCommand { get; }
        public AsyncCommand<Note> FavoriteCommand { get; }
        public AsyncCommand RefreshCommand { get; }
        public AsyncCommand<Note> SelectedCommand { get; }
        public AsyncCommand<Note> MenuCommand { get; }

        public NoteViewModel()
        {
            Title = "Note Overview";
            Notes = new ObservableRangeCollection<Note>();

            AddCommand = new AsyncCommand(Add);
            RemoveCommand = new AsyncCommand<Note>(Remove);
            EditCommand = new AsyncCommand<Note>(Edit);
            FavoriteCommand = new AsyncCommand<Note>(Favorite);
            RefreshCommand = new AsyncCommand(Refresh);
            SelectedCommand = new AsyncCommand<Note>(Selected);
            MenuCommand = new AsyncCommand<Note>(Menu);
        }

        private Task Menu(Note arg)
        {
            var nav = App.Current.MainPage.Navigation;
            ValueParserService.note = arg;
            NavigationExtensions.ShowPopup(nav, new NoteMenu());
            return Task.CompletedTask;
        }

        private async Task Selected(Note arg)
        {
            ValueParserService.note = arg;
            await Shell.Current.GoToAsync($"{nameof(NoteEditPage)}");

        }

        private async Task Refresh()
        {
            IsBusy = true;
            Notes.Clear();
            try
            {
                List<Note> notes = new List<Note>()
                {
                    new Note()
                    {
                        Name = "Note 1",
                        Text = "Note 1 Text"
                    },
                    new Note()
                    {
                        Name = "Note 2",
                        Text = "Note 2 Text"
                    },
                    new Note()
                    {
                        Name = "Note 3",
                        Text = "Note 3 Text"
                    }
                };

                foreach (var item in notes)
                {
                    Notes.Add(item);
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

        private Task Favorite(Note arg)
        {
            throw new NotImplementedException();
        }

        private async Task Edit(Note arg)
        {
            ValueParserService.note = arg;
            await Shell.Current.GoToAsync($"{nameof(NoteEditPage)}");
        }

        private Task Remove(Note arg)
        {
            throw new NotImplementedException();
        }

        private async Task Add()
        {
            //var stack = Application.Current.MainPage.Navigation.NavigationStack;
            //await Application.Current.MainPage.DisplayToastAsync(stack, 2000);
            var route = $"{nameof(NoteCreatePage)}";
            await Shell.Current.GoToAsync(route);
        }
    }
}
