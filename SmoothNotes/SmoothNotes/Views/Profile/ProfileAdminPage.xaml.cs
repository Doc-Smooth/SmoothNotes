using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmoothNotes.Views.Profile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfileAdminPage : ContentPage
    {
        private bool biometric = false;

        public bool Biometric { get => biometric; set => biometric = value; }
        public ProfileAdminPage()
        {
            InitializeComponent();
        }

        private void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            biometric = e.Value;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Deleting Profile", "Warning:\nThis is an irreversible action.\nWould like to continue?", "Confirm", "Cancel");
        }
    }
}