using Newtonsoft.Json;
using SmoothNotes.Models;
using SmoothNotes.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmoothNotes.Views.Note
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NoteEditPage : ContentPage
    {
        public int lineNum { get; set; }
        public List<SearchString> results { get; set; }
        public string line { get; set; }
        public NoteEditPage()
        {
            InitializeComponent();
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            if(await DisplayAlert("Delete", "Are you sure about this?", "Accept", "Cancel"))
            {
                var vm = (NoteChangeViewModel)BindingContext;
                await vm.DeleteCommand.ExecuteAsync();
            }
        }

        void OnSearchButtonClicked(object sender, EventArgs e)
        {
            SearchFunc(TextField.Text, SearchField.Text);
        }
        void SearchFunc(string sub, string searchWord)
        {
            lineNum = 0;
            line = "";
            using (StringReader reader = new StringReader(sub))
            {
                results = new List<SearchString>();

                while ((line = reader.ReadLine()) != null)
                {
                    lineNum++;
                    try
                    {
                        if (line.ToLower().Contains(searchWord.ToLower()))
                        {
                            results.Add(new SearchString { LineNum = lineNum, Line = line });
                        }
                    }
                    catch (Exception e)
                    {
                        Application.Current.MainPage.DisplayToastAsync(e.Message, 2000);
                    }
                }
            }
            var route = $"{nameof(SearchResultPage)}?SearchList={JsonConvert.SerializeObject(results)}";
            Shell.Current.GoToAsync(route);
        }
    }
}