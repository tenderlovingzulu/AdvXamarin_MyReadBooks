﻿using System;
using System.ComponentModel;
using System.Windows.Input;
using MyReadBooks.Models;
using Prism.Commands;
using Prism.Navigation;
using SQLite;

namespace MyReadBooks.ViewModels
{
    public class BookDetailsVM : INavigatedAware, INotifyPropertyChanged
    {
        private string bookName;
        public string BookName
        {
            get { return bookName; }
            set
            {
                bookName = value;
                OnPropertyChanged("BookName");
            }
        }

        private string bookAuthor;
        public string BookAuthor
        {
            get { return bookAuthor; }
            set
            {
                bookAuthor = value;
                OnPropertyChanged("BookAuthor");
            }
        }

        private string bookImageUrl;
        public string BookImageUrl
        {
            get { return bookImageUrl; }
            set
            {
                bookImageUrl = value;
                OnPropertyChanged("BookImageUrl");
            }
        }

        public ICommand DeleteCommand { get; set; }

        Best_book selectedBook;

        public BookDetailsVM()
        {
            DeleteCommand = new DelegateCommand(DeleteBook);
        }

        void DeleteBook()
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabasePath))
            {
                conn.CreateTable<Best_book>();
                int booksDeleted = conn.Delete(selectedBook);
                if (booksDeleted >= 1)
                {
                    App.Current.MainPage.DisplayAlert("Success", "Book deleted", "Ok");
                }
                else
                {
                    App.Current.MainPage.DisplayAlert("Failure", "An error ocurred while deleting the book, please try again.", "Ok");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            selectedBook = parameters["selected_book"] as Best_book;
            BookName = selectedBook.Title;
            BookAuthor = selectedBook.Author_Name;
            BookImageUrl = selectedBook.Image_url;
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
