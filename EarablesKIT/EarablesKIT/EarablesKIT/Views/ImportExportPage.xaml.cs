﻿using EarablesKIT.Models;
using EarablesKIT.Resources;
using EarablesKIT.ViewModels;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using System;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EarablesKIT.Views
{
    /// <summary>
    /// Codebehind for class <see cref="ImportExportPage"/>
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImportExportPage : ContentPage
    {
        private ImportExportViewModel _viewModel;
        private IExceptionHandler _exceptionHandler;

        /// <summary>
        /// Command DeleteDataCommand is called when the DeleteData command is pressed. Calls method DeleteEntries
        /// </summary>
        public ICommand DeleteDataCommand => new Command(DeleteEntries);

        /// <summary>
        /// Constructor for class ImportExportPage
        /// </summary>
        public ImportExportPage()
        {
            InitializeComponent();
            _exceptionHandler =
                (IExceptionHandler)ServiceManager.ServiceProvider.GetService(typeof(IExceptionHandler));
            BindingContext = _viewModel = new ImportExportViewModel();
        }

        private async void ImportButton_Clicked(object sender, EventArgs e)
        {
            FileData filedata;
            try
            {
                filedata = await CrossFilePicker.Current.PickFile();
            }
            catch (Exception)
            {
                _exceptionHandler.HandleException(new Exception(AppResources.ImportExportFileError));
                return;
            }
            _viewModel.ImportCommand.Execute(filedata);
        }

        private void DeleteEntries()
        {
            _viewModel.DeleteCommand.Execute(this);
        }
    }
}