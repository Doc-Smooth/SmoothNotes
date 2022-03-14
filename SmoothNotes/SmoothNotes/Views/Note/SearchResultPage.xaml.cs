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
            //try
            //{
            //    var r = await LifeCycleService.StillAlive();
            //    if (!r)
            //        await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");

            //}
            //catch (Exception e)
            //{
            //    await Application.Current.MainPage.DisplayToastAsync(e.Message, 2000);
            //    await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            //}
        }
    }
}