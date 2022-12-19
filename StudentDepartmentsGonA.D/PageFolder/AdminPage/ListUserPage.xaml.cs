using StudentDepartmentsGonA.D.ClassFolder;
using StudentDepartmentsGonA.D.DataFolder;
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

namespace StudentDepartmentsGonA.D.PageFolder.AdminPage
{
    /// <summary>
    /// Логика взаимодействия для ListUserPage.xaml
    /// </summary>
    public partial class ListUserPage : Page
    {
        public ListUserPage()
        {
            InitializeComponent();
            ListUserLB.ItemsSource = DBEntities.GetContext().User.ToList();


        }

        private void AddStudentBT_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddUserPage());

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                ListUserLB.ItemsSource = DBEntities.GetContext().User.Where
                    (u => u.Login.StartsWith(SearchTB.Text) ||
                     u.Password.StartsWith(SearchTB.Text)).ToList();


            }
            catch (Exception ex)
            {
                MBClass.Error(ex);
            }
        }

        private void DeleteMi_Click(object sender, RoutedEventArgs e)
        {
            if (ListUserLB.SelectedItem == null)
            {
                return;
            }
            else
            {
                try
                {
                    User user = ListUserLB.SelectedItem as User;

                    if (MBClass.Question($"Удалить пользователя с логином " +
                        $"{user.Login}"))
                    {
                        DBEntities.GetContext().User.Remove(ListUserLB.SelectedItem as User);

                        DBEntities.GetContext().SaveChanges();

                        ListUserLB.ItemsSource = DBEntities.GetContext().User.ToList();
                    }
                }
                catch (Exception ex)
                {
                    MBClass.Error(ex);
                }
            }
        }

        private void EditUserMI_Click(object sender, RoutedEventArgs e)
        {
            ListUserLB_SelectItem();
        }

        private void ListUserLB_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListUserLB_SelectItem();
        }

        private void ListUserLB_SelectItem()
        {
            try
            {
                if (ListUserLB.SelectedItem == null)
                {
                    return;
                }
                else
                {
                    User user = ListUserLB.SelectedItem as User;
                    VariableClass.UserID = user.UserID;
                    NavigationService.Navigate(new EditUserPage(ListUserLB.SelectedItem as User));
                }
            }
            catch (Exception ex)
            {
                MBClass.Error(ex);
            }
        }
    }
}
