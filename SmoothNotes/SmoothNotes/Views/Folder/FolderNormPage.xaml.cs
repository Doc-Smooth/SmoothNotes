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

namespace SmoothNotes.Views.Folder
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FolderNormPage : ContentPage
    {
        public FolderNormPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Logout btn event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Logout_Clicked(object sender, EventArgs e)
        {
            var vm = (FolderViewModel)BindingContext;
            await vm.Logout();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                var vm = (FolderViewModel)BindingContext;
                await vm.RefreshCommand.ExecuteAsync();
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayToastAsync(e.Message, 2000);
            }
        }
    }
}