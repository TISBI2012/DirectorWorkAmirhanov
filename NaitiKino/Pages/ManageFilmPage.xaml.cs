using NaitiKino.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NaitiKino.Pages
{
    /// <summary>
    /// Логика взаимодействия для ManageFilmPage.xaml
    /// </summary>
    public partial class ManageFilmPage : Page
    {
        public List<Genres> Genres { get; set; }
        public ObservableCollection<Films> FilmsCollection { get; set; }

        public ManageFilmPage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            using (var context = new NaitiKinoEntities())
            {
                Genres = context.Genres.ToList();
                FilmsCollection = new ObservableCollection<Films>(context.Films.Include("Genres").ToList());
                FilmsDataGrid.ItemsSource = FilmsCollection;
                GenreSortComboBox.ItemsSource = Genres;
                GenreSortComboBox.DisplayMemberPath = "Name";
                GenreSortComboBox.SelectedValuePath = "Id";
            }
        }

        private void GenreSortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GenreSortComboBox.SelectedValue != null)
            {
                int genreId = (int)GenreSortComboBox.SelectedValue;
                using (var context = new NaitiKinoEntities())
                {
                    FilmsCollection = new ObservableCollection<Films>(context.Films.Where(f => f.GenreId == genreId).Include("Genres").ToList());
                    FilmsDataGrid.ItemsSource = FilmsCollection;
                }
            }
        }
        
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddFilmPage());
        }

        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedFilm = FilmsDataGrid.SelectedItem as Films;

            if (selectedFilm != null)
            {
                using (var context = new NaitiKinoEntities())
                {
                    context.Entry(selectedFilm).State = EntityState.Deleted;
                    context.SaveChanges();
                }
                LoadData();
            }
        }
        private void FilmsDataGrid_EditClick(object sender, DataGridCellEditEndingEventArgs e)
        {
            var editedFilm = e.Row.Item as Films;
            if (editedFilm != null)
            {
                using (var context = new NaitiKinoEntities())
                {
                    context.Entry(editedFilm).State = EntityState.Modified;
                    MessageBox.Show("Данные успешно обновлены", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                    context.SaveChanges();
                }
            }
        }
    }
}
