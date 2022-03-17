using Newtonsoft.Json;
using SmoothNotes.Models;
using System;
using System.Collections.Generic;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmoothNotes.Views.Note
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty(nameof(SearchList), nameof(SearchList))]
    public partial class SearchResultPage : ContentPage
    {
        public List<SearchString> SearchStrings { get; set; }
        public string SearchList
        {
            set
            {
                BindList(value);
            }
        }

        /// <summary>
        /// Deserializes json and binds it
        /// </summary>
        /// <param name="list"></param>
        void BindList(string list)
        {
            try
            {
                SearchStrings = new List<SearchString>();
                SearchStrings = JsonConvert.DeserializeObject<List<SearchString>>(list);
                ListViewer.ItemsSource = SearchStrings;
            }
            catch (Exception e)
            {
                Application.Current.MainPage.DisplayToastAsync(e.Message, 2000);
            }
        }
        public SearchResultPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}