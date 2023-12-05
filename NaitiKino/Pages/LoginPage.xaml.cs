using NaitiKino.Models;
using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text;
            string password = PasswordBox.Password;

            using (var context = new NaitiKinoEntities())
            {
                var user = context.Users.FirstOrDefault(u => u.Login == login && u.Password == password);
                
                if (user != null)
                {
                    MessageBox.Show("Вы успешно вошли!");
                    NavigationService.Navigate(new ManageFilmPage());
                }
                else
                {
                    MessageBox.Show("Что-то пошло не так!");
                    return;
                }
            }

        }
    }
}
