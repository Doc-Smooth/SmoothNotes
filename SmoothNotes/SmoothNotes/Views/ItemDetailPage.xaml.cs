using SmoothNotes.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace SmoothNotes.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}