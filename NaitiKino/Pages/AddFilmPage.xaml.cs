using Microsoft.Win32;
using NaitiKino.Models;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для AddFilmPage.xaml
    /// </summary>
    public partial class AddFilmPage : Page
    {

        public byte[] imageData;
        public AddFilmPage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            using (var context = new NaitiKinoEntities())
            {
                GenreComboBox.ItemsSource = context.Genres.ToList();
                GenreComboBox.DisplayMemberPath = "Name";
                GenreComboBox.SelectedValuePath = "Id";
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new NaitiKinoEntities())
            {
                var newFilm = new Films
                {
                    Name = NameTextBox.Text,
                    Description = DescriptionTextBox.Text,
                    DurationInHours = int.TryParse(DurationTextBox.Text, out int duration) ? duration : (int?)null,
                    GenreId = (int?)GenreComboBox.SelectedValue,
                    Image = imageData
                };

                context.Films.Add(newFilm);
                context.SaveChanges();
            }

            MessageBox.Show("Фильм добавлен успешно", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BrowseButton_Click(Object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == true)
            {
                string ImagePath = openFileDialog.FileName;
                ImagePathTextBox.Text = ImagePath;

                imageData = File.ReadAllBytes(ImagePath);
            }
        }
    }
}
