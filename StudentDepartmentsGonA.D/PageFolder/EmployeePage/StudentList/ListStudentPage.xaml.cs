using StudentDepartmentsGonA.D.ClassFolder;
using StudentDepartmentsGonA.D.DataFolder;
using StudentDepartmentsGonA.D.PageFolder.EmployeePage.StuentList;
using StudentDepartmentsGonA.D.WindowFolder.EmployeeFolder;
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

namespace StudentDepartmentsGonA.D.PageFolder.EmployeePage
{
    /// <summary>
    /// Логика взаимодействия для ListStudentPage.xaml
    /// </summary>
    public partial class ListStudentPage : Page
    {
        public ListStudentPage()
        {
            InitializeComponent();
            ListStudentLV.ItemsSource = DBEntities.GetContext().Student.ToList().OrderBy(u => u.GroupID);

        }

        private void AddStudentBT_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddStudentPage());
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {


                ListStudentLV.ItemsSource = DBEntities.GetContext().Student.Where
                    (u => u.Surname.StartsWith(SearchTB.Text) ||
                    u.Name.StartsWith(SearchTB.Text) || u.Patronymic.StartsWith(SearchTB.Text)
                    || u.Group.NameGroup.StartsWith(SearchTB.Text)).ToList();

            }
            catch (Exception ex)
            {
                MBClass.Error(ex);
            }
        }

        private void ListStudentLV_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ListStudentLV.SelectedItem == null)
            {
                return;
            }
            new StudentInfoWindow(ListStudentLV.SelectedItem as Student).ShowDialog();
        }

        private void InfoStudentMI_Click(object sender, RoutedEventArgs e)
        {
            if (ListStudentLV.SelectedItem == null)
            {
                return;
            }
            new StudentInfoWindow(ListStudentLV.SelectedItem as Student).ShowDialog();
        }

        private void EditStudentMI_Click(object sender, RoutedEventArgs e)
        {
            if (ListStudentLV.SelectedItem == null)
            {
                return;
            }
            else
            {
                try
                {
                    Student student = ListStudentLV.SelectedItem as Student;

                    VariableClass.StudentID = student.StudentID;

                    NavigationService.Navigate(new EditStudentPage(ListStudentLV.SelectedItem as Student));
                }
                catch (Exception ex)
                {
                    MBClass.Error(ex);
                }
            }
        }


        private void DeleteStudentMI_Click(object sender, RoutedEventArgs e)
        {

            if (ListStudentLV.SelectedItem == null)
            {
                return;
            }
            else
            {
                try
                {
                    Student student = ListStudentLV.SelectedItem as Student;

                    if (MBClass.Question($"Удалить студента с ФИО: \n" +
                        $"{student.Surname} {student.Name} {student.Patronymic}?"))
                    {
                        DBEntities.GetContext().Student.Remove(ListStudentLV.SelectedItem as Student);

                        DBEntities.GetContext().SaveChanges();
                        ListStudentLV.ItemsSource = DBEntities.GetContext().Student.ToList().OrderBy(u => u.GroupID);

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

