﻿using EarablesKIT.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EarablesKIT.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DataOverviewPage : ContentPage
    {
        private DataOverviewViewModel _viewModel;

        /// <summary>
        /// Default constructor for page DataOverview
        /// </summary>
        public DataOverviewPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new DataOverviewViewModel();
            this.Appearing += _viewModel.OnAppearing;
        }

       
    }
}