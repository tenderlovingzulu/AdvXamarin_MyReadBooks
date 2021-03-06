﻿using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using MyReadBooks.Models;
using Prism.AppModel;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using SQLite;
using Xamarin.Forms;

namespace MyReadBooks.ViewModels
{
    public class BooksVM : IPageLifecycleAware
    {
        public ObservableCollection<Best_book> Books { get; set; }
        public ICommand NewBookCommand { get; set; }
        public ICommand BookDetailsCommand { get; set; }
        INavigationService _navigationService;
        IPageDialogService _dialogService;
        public BooksVM(INavigationService navigationService, IPageDialogService dialogService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            NewBookCommand = new DelegateCommand(NewBookAction);
            BookDetailsCommand = new DelegateCommand<object>(GoToDetails, CanGoToDetails);
            Books = new ObservableCollection<Best_book>();
            ReadSavedBooks();
        }

        bool CanGoToDetails(object arg)
        {
            return arg != null;
        }

        async void GoToDetails(object obj)
        {
            Best_book selectedBook = (obj as ListView).SelectedItem as Best_book;

            var parameter = new NavigationParameters();
            parameter.Add("selected_book", selectedBook);
            await _navigationService.NavigateAsync("BookDetailsPage", parameter);
        }

        private void ReadSavedBooks()
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabasePath))
            {
                conn.CreateTable<Best_book>();
                var books = conn.Table<Best_book>().ToList();
                Books.Clear();
                foreach (var book in books)
                {
                    Books.Add(book);
                }
            }
        }

        private async void NewBookAction()
        {
            await _dialogService.DisplayAlertAsync("Navigating", "Navigation in progress...", "Awesome!");
            await _navigationService.NavigateAsync("NewBookPage");
        }

        public void OnAppearing()
        {
            ReadSavedBooks();
        }

        public void OnDisappearing()
        {

        }
    }
}
