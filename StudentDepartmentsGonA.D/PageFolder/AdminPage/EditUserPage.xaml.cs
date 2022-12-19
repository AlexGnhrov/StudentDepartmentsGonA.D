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
    /// Логика взаимодействия для EditUserPage.xaml
    /// </summary>
    public partial class EditUserPage : Page
    {
        User User = new User();

        string oldLogin;

        public EditUserPage(User UserID)
        {
            InitializeComponent();

            DataContext = UserID;

            RoleCB.ItemsSource = DBEntities.GetContext().Role.ToList();

            SaveLogin();

        }

        private void BackBT_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ListUserPage());
        }

        private void EditUserBT_Click(object sender, RoutedEventArgs e)
        {

            if (oldLogin != LoginTB.Text)
            {
                var user = DBEntities.GetContext().User.FirstOrDefault(u => u.Login == LoginTB.Text);

                if (user != null)
                {
                    MBClass.Error("Такой логин уже существует");
                    LoginTB.Focus();
                    return;

                }
            }

            try
            {
                User = DBEntities.GetContext().User.FirstOrDefault(u => u.UserID == VariableClass.UserID);

                User.Login = LoginTB.Text;
                User.Password = PasswordTB.Text;
                User.RoleID = Int32.Parse(RoleCB.SelectedValue.ToString());


                DBEntities.GetContext().SaveChanges();

                MBClass.Info("Пользователь успешно отредактирован!");

                oldLogin = LoginTB.Text;
            }
            catch (Exception ex)
            {
                MBClass.Error(ex);
            }
        }

        private void Page_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (EditUserBT.IsEnabled)
                {
                    EditUserBT_Click(sender, e);
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
                EditUserBT.IsEnabled = false;
            }
            else
            {
                EditUserBT.IsEnabled = true;
            }
        }

        private void DeleteBT_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (MBClass.Question("Вы действительно хотите удалить пользователя?"))
                {
                    DBEntities.GetContext().User.Remove(DataContext as User);

                    DBEntities.GetContext().SaveChanges();

                    MBClass.Info("Пользователь успешно удалён!");

                    NavigationService.Navigate(new ListUserPage());
                }
            }
            catch (Exception ex)
            {
                MBClass.Error(ex);
            }
        }


        async Task SaveLogin()
        {
            await Task.Delay(25);
            oldLogin = LoginTB.Text;
        }
    }
}
