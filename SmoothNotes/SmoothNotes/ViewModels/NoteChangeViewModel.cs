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
    /// <summary>
    /// Used by both new note and edit note pages
    /// </summary>
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
            if(ValueParserService.note == null)
                Note = new Note();
            else
                Note = ValueParserService.note;
            //Note.FolderId = Folder.Id;
            SaveNewCommand = new AsyncCommand(SaveNew);
            SaveCommand = new AsyncCommand(Save);
            DeleteCommand = new AsyncCommand(Delete);
        }

        /// <summary>
        /// Delete event handler
        /// </summary>
        /// <returns></returns>
        private async Task Delete()
        {
            if (!await ProfileService.Refresh())
                await Logout();


            var confirmation = await Application.Current.MainPage.DisplayAlert("Deleting " + ValueParserService.note.Name, "Are you sure you want to continue?", "Confirm", "Cancel");
            if (!confirmation)
                return;

            var result = await NoteService.Remove(ValueParserService.note.Id);
            if (result)
            {
                await Application.Current.MainPage.DisplayToastAsync("Note deleted", 1000);
                await Shell.Current.GoToAsync("..");
            }
            else
                await Application.Current.MainPage.DisplayToastAsync("Something went wrong", 2000);
        }

        /// <summary>
        /// Save note event handler
        /// </summary>
        /// <returns></returns>
        private async Task Save()
        {
            if (!await ProfileService.Refresh())
                await Logout();

            try
            {
                if (string.IsNullOrWhiteSpace(Note.Name))
                {
                    await Application.Current.MainPage.DisplayToastAsync("Error: Notation needs a Title", 1000);
                    return;
                }

                string key = await CryptographService.ReadPuKKey(ValueParserService.profile.PuK);
                string Text = "";
                if (!string.IsNullOrEmpty(Note.Text))
                {
                    Text = await CryptographService.RSAEncrypt(Note.Text, key, false);
                }
                Note.Text = Text;

                bool status = await NoteService.Edit(Note);
                if (status)
                {
                    await Application.Current.MainPage.DisplayToastAsync("Changes applied", 1000);
                    await Shell.Current.GoToAsync("..");
                }
                else
                    await Application.Current.MainPage.DisplayToastAsync("Something went wrong", 2000);
            }
            catch (Exception e)
            {
                await Shell.Current.GoToAsync("..");
                await Application.Current.MainPage.DisplayToastAsync(e.Message, 2000);
            }
        }


        /// <summary>
        /// Save new note event handler
        /// </summary>
        /// <returns></returns>
        private async Task SaveNew()
        {
            if (!await ProfileService.Refresh())
                await Logout();

            try
            {
                if (string.IsNullOrWhiteSpace(Note.Name))
                {
                    await Application.Current.MainPage.DisplayToastAsync("Error: Notation needs a Title", 1000);
                    return;
                }

                string key = await CryptographService.ReadPuKKey(ValueParserService.profile.PuK);
                string Text = "";
                if (!string.IsNullOrEmpty(Note.Text))
                {
                    Text = await CryptographService.RSAEncrypt(Note.Text, key, false);
                }
                Note note = new Note
                {
                    Id = "",
                    FolderId = ValueParserService.folder.Id,
                    Name = Note.Name,
                    Text = Text,
                    CrDate = DateTime.Now,
                    EdDate = DateTime.Now
                };

                bool status = await NoteService.Add(note);
                if (status)
                    await Application.Current.MainPage.DisplayToastAsync("Noted added", 1000);
                else
                    await Application.Current.MainPage.DisplayToastAsync("Something went wrong", 2000);
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception e)
            {
                await Shell.Current.GoToAsync("..");
                await Application.Current.MainPage.DisplayToastAsync(e.Message, 2000);
            }
        }
    }
}
