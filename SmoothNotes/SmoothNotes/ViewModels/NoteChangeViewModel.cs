using MvvmHelpers.Commands;
using SmoothNotes.Models;
using SmoothNotes.Services.Storage;
using SmoothNotes.Views.Note;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace SmoothNotes.ViewModels
{
    public class NoteChangeViewModel : ViewModelBase
    {
        private Note note;
        private Folder folder;
        private Profile profile;

        public string Title { get; set; }
        public Note Note { get => note; set => note = value; }
        public Folder Folder { get => folder; set => folder = value; }
        public Profile Profile { get => profile; set => profile = value; }

        public AsyncCommand SaveNewCommand { get; }
        public AsyncCommand SaveCommand { get; }
        public AsyncCommand DeleteCommand { get; }

        public NoteChangeViewModel()
        {
            Title = $"Adding new note";
            //Profile = SelectedService.profile;
            Folder = ValueParserService.folder;
            if(ValueParserService.note.Id == null)
                Note = new Note();
            else
                Note = ValueParserService.note;
            //Note.FolderId = Folder.Id;
            SaveNewCommand = new AsyncCommand(SaveNew);
            SaveCommand = new AsyncCommand(Save);
            DeleteCommand = new AsyncCommand(Delete);
        }

        private async Task Delete()
        {
            await Shell.Current.GoToAsync("..");
            await Application.Current.MainPage.DisplayToastAsync("Note was deleted", 2000);
        }

        private async Task Save()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Note.Name))
                {
                    await Application.Current.MainPage.DisplayToastAsync("Error: Notation needs a Title", 1000);
                    return;
                }

                //await NoteService.Add(FolderId, title, text);
                ValueParserService.note = new Note();
                await Shell.Current.GoToAsync("..");
                //var r = await LifeCycleService.StillAlive();
                //if (!r)
                //{
                //    await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                //    return;
                //}
                //else
                //{
                //}
            }
            catch (Exception e)
            {
                await Shell.Current.GoToAsync("..");
                await Application.Current.MainPage.DisplayToastAsync(e.Message, 2000);
            }
        }

        private async Task SaveNew()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Note.Name))
                {
                    await Application.Current.MainPage.DisplayToastAsync("Error: Notation needs a Title", 1000);
                    return;
                }

                //await NoteService.Add(FolderId, title, text);
                await Shell.Current.GoToAsync("..");
                //var r = await LifeCycleService.StillAlive();
                //if (!r)
                //{
                //    await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                //    return;
                //}
                //else
                //{
                //}
            }
            catch (Exception e)
            {
                await Shell.Current.GoToAsync("..");
                await Application.Current.MainPage.DisplayToastAsync(e.Message, 2000);
            }
        }
    }
}
