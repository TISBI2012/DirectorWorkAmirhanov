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
    /// Логика взаимодействия для RegistryPage.xaml
    /// </summary>
    public partial class RegistryPage : Page
    {
        public byte[] imageData;

        public RegistryPage()
        {
            InitializeComponent();
        }

        private void RegistryButton_Click(object sender, RoutedEventArgs e)
        {
            string login = Login.Text;
            string password = Password.Password;
            string firstname = Firstname.Text;
            string secondname = Secondname.Text;
            string patronymic = Patronymic.Text;
            string nickname = Nickname.Text;
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(firstname) || string.IsNullOrWhiteSpace(secondname) ||
                string.IsNullOrWhiteSpace(nickname))
            {
                MessageBox.Show("Все поля должны быть заполнены.");
                return;
            }

            if (login.Length < 5)
            {
                MessageBox.Show("Логин должен быть не менее 5 символов.");
                return;
            }

            if (password.Length < 8)
            {
                MessageBox.Show("Пароль должен быть не менее 8 символов.");
                return;
            }

            if (!password.Any(char.IsUpper) || !password.Any(char.IsLower) ||
                !password.Any(char.IsDigit) || !password.Any(ch => !char.IsLetterOrDigit(ch)))
            {
                MessageBox.Show("Пароль должен содержать заглавные и строчные буквы, цифры и специальные символы.");
                return;
            }

            var newUser = new Users
            {
                Login = login,
                Password = password,
                Firstname = firstname,
                Secondname = secondname,
                Patronymic = patronymic,
                Nickname = nickname,
                Avatar = imageData
                
            };

            using (var context = new NaitiKinoEntities())
            {
                if (context.Users.Any(u => u.Login == login))
                {
                    MessageBox.Show("Пользователь с таким логином уже существует.");
                    return;
                }

                context.Users.Add(newUser);
                context.SaveChanges();
            }

            Login.Text = "";
            Password.Password = "";
            Firstname.Text = "";
            Secondname.Text = "";
            Patronymic.Text = "";
            Nickname.Text = "";

            MessageBox.Show("Регистрация успешна!");
            NavigationService.Navigate(new ManageFilmPage());
        }


        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LoginPage());
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
