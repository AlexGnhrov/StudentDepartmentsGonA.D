using StudentDepartmentsGonA.D.ClassFolder;
using StudentDepartmentsGonA.D.DataFolder;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
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

namespace StudentDepartmentsGonA.D.PageFolder.AdminPage
{
    /// <summary>
    /// Логика взаимодействия для AddUserPage.xaml
    /// </summary>
    public partial class AddUserPage : Page
    {


        public AddUserPage()
        {
            InitializeComponent();
            RoleCB.ItemsSource = DBEntities.GetContext().Role.ToList();
        }

        private void BackBT_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ListUserPage());
        }

        private void AddUserBT_Click(object sender, RoutedEventArgs e)
        {
            var user = DBEntities.GetContext().User.FirstOrDefault(u => u.Login == LoginTB.Text);

            if(user != null)
            {
                MBClass.Error("Такой логин уже существует");
                LoginTB.Focus();
                return;

            }
            try
            {
                DBEntities.GetContext().User.Add(new User()
                {
                    Login = LoginTB.Text,
                    Password = PasswordTB.Text,
                    RoleID = Convert.ToInt32(RoleCB.SelectedValue)
                });
                DBEntities.GetContext().SaveChanges();
                MBClass.Info("Пользовательно успешно добавлен!");
            }
            catch(Exception ex)
            {
                MBClass.Error(ex);
            }
        }

        private void Page_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (AddUserBT.IsEnabled)
                {
                    AddUserBT_Click(sender, e);
                }
                else if (LoginTB.IsFocused)
                {
                    PasswordTB.Focus();
                }
                else if (PasswordTB.IsFocused)
                {
                    RoleCB.Focus();
                    RoleCB.IsDropDownOpen = true;
                }
            }
        }

        private void LoginTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableButton();
        }

        private void PasswordTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableButton();
        }

        private void RoleCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnableButton();
        }

        private void EnableButton()
        {
            if (string.IsNullOrWhiteSpace(LoginTB.Text) ||
                string.IsNullOrWhiteSpace(PasswordTB.Text) ||
                RoleCB.SelectedValue == null)
            {
                AddUserBT.IsEnabled = false;
            }
            else
            {
                AddUserBT.IsEnabled = true;
            }
        }
    }
}
