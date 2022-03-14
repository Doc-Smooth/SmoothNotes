using SmoothNotes.ViewModels;
using SmoothNotes.Views.Landing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmoothNotes.Views.Note
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotePage : ContentPage
    {
        public NotePage()
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

            try
            {
                var vm = (NoteViewModel)BindingContext;
                //if (vm.Notes.Count == 0)
                await vm.RefreshCommand.ExecuteAsync();
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayToastAsync(e.Message, 2000);
            }
        }
    }
}