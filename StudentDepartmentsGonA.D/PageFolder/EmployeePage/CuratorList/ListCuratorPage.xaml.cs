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

namespace StudentDepartmentsGonA.D.PageFolder.EmployeePage.CuratorList
{
    /// <summary>
    /// Логика взаимодействия для ListCuratorPage.xaml
    /// </summary>
    public partial class ListCuratorPage : Page
    {
        public ListCuratorPage()
        {
            InitializeComponent();
            ListCuratorLV.ItemsSource = DBEntities.GetContext().Curator.ToList().OrderBy(u => u.Surname);

        }

        private void AddCuratorBT_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddCuratorPage());
            VariableClass.AddAdressIiUsing = false;
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {


                ListCuratorLV.ItemsSource = DBEntities.GetContext().Curator.Where
                    (u => u.Surname.StartsWith(SearchTB.Text) ||
                    u.Name.StartsWith(SearchTB.Text) || u.Patronymic.StartsWith(SearchTB.Text) ||
                    u.Speciality.NameSpeciality.StartsWith(SearchTB.Text)).ToList();

            }
            catch (Exception ex)
            {
                MBClass.Error(ex);
            }
        }

        private void ListCuratorLV_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            selectItemCurator();
        }


        private void EditCuratorMI_Click(object sender, RoutedEventArgs e)
        {
            selectItemCurator();
        }

        private void selectItemCurator()
        {
            if (ListCuratorLV.SelectedItem == null)
            {
                return;
            }
            else
            {
                try
                {
                    Curator curator = ListCuratorLV.SelectedItem as Curator;

                    VariableClass.CuratorID = curator.CuratorID;

                    NavigationService.Navigate(new EditCuratorPage(ListCuratorLV.SelectedItem as Curator));
                }
                catch (Exception ex)
                {
                    MBClass.Error(ex);
                }
            }
        }


        private void DeleteCuratorMI_Click(object sender, RoutedEventArgs e)
        {
            if (ListCuratorLV.SelectedItem == null)
            {
                return;
            }
            else
            {
                try
                {
                    Curator curator = ListCuratorLV.SelectedItem as Curator;

                    if (MBClass.Question($"Удалить куратора с ФИО: \n" +
                        $"{curator.Surname} {curator.Name} {curator.Patronymic}?"))
                    {


                        DBEntities.GetContext().Curator.Remove(ListCuratorLV.SelectedItem as Curator);

                        DBEntities.GetContext().SaveChanges();

                        ListCuratorLV.ItemsSource = DBEntities.GetContext().Curator.ToList().OrderBy(u => u.Surname);


                    }
                }
                catch (Exception ex)
                {
                    MBClass.Error(ex);
                }
            }

        }

    }
}
