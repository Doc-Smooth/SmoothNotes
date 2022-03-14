using SmoothNotes.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmoothNotes.Views.Note
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NoteCreatePage : ContentPage
    {
        public NoteCreatePage()
        {
            InitializeComponent();
            //var vm = (NoteChangeViewModel)BindingContext;
        }
    }
}