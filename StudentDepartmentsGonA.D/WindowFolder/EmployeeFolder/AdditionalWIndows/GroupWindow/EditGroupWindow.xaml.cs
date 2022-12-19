using StudentDepartmentsGonA.D.ClassFolder;
using StudentDepartmentsGonA.D.DataFolder;
using StudentDepartmentsGonA.D.WindowFolder.EmployeeFolder.AdditionalWIndows.SpecialityWindow;
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
using System.Windows.Shapes;

namespace StudentDepartmentsGonA.D.WindowFolder.EmployeeFolder.AdditionalWIndows.GroupWindow
{
    /// <summary>
    /// Логика взаимодействия для EditGroupWindow.xaml
    /// </summary>
    public partial class EditGroupWindow : Window
    {
        Group group = new Group();

        string oldNameGroup;
        public EditGroupWindow(Group GroupID, bool EnableDelete)
        {
            InitializeComponent();

            DeleteBT.IsEnabled = EnableDelete;

            DataContext = GroupID;

            SpecialityCB.ItemsSource = DBEntities.GetContext().Speciality.ToList();
            CuratorCB.ItemsSource = DBEntities.GetContext().Curator.ToList().OrderBy(u => u.Surname);

            saveNameGroup();

        }

        bool BlockDragWindow = false;
        bool CloseBIsUsing = false;

        private void MainB_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!BlockDragWindow)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    DragMove();
                }
            }
        }


        private void EditGroupBT_Click(object sender, RoutedEventArgs e)
        {


            var namegroup = DBEntities.GetContext().Group.FirstOrDefault(u => u.NameGroup == NameGroupTB.Text);

            if (oldNameGroup != NameGroupTB.Text)
            { 
                if (namegroup != null)
                {
                    MBClass.Error("Такое название группы уже существует");
                    NameGroupTB.Focus();
                    return;

                }
            }
            try
            {
                group = DBEntities.GetContext().Group.FirstOrDefault(u => u.GroupID == VariableClass.GroupID);


                group.NameGroup = NameGroupTB.Text;
                group.CuratorID = (int?)CuratorCB.SelectedValue;
                group.SpecialityID = (int)SpecialityCB.SelectedValue;


                DBEntities.GetContext().SaveChanges();


                MBClass.Info("Группа успешно отредактирована!");
                VariableClass.GroupNewCreated = true;

                oldNameGroup = NameGroupTB.Text;

            }
            catch (Exception ex)
            {
                MBClass.Error(ex);
            }
        }


        private void BackBT_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        //------------------Кнопка закрытия приложения----------------------


        private async void CloseB_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CloseBIsUsing = true;

            await DisableDrag(e);

            CloseBIsUsing = false;

        }


        private void CloseB_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Close();

        }

        private async void Borders_MouseEnter(object sender, MouseEventArgs e)
        {
            await ChangeColorButton();
        }



        //-------Методы для обработки кнопок -------------------------------

        async Task ChangeColorButton()
        {

            while (CloseB.IsMouseOver)
            {

                if (BlockDragWindow)
                {
                    CloseB.Background = new SolidColorBrush(Color.FromRgb(255, 100, 100));
                }
                else
                {
                    CloseB.Background = new SolidColorBrush(Color.FromRgb(205, 50, 50));
                }

                CloseLB.Foreground = new SolidColorBrush(Colors.White);

                await Task.Delay(1);
            }
            CloseB.Background = null;


        }



        async Task DisableDrag(MouseButtonEventArgs e)
        {
            BlockDragWindow = true;

            while (e.LeftButton == MouseButtonState.Pressed)
            {
                await Task.Delay(1);
            }

            BlockDragWindow = false;

        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
            else if (e.Key == Key.Enter)
            {

                if (NameGroupTB.IsFocused)
                {
                    SpecialityCB.Focus();
                    SpecialityCB.IsDropDownOpen = true;

                }
                else if (SpecialityCB.IsFocused)
                {
                    SpecialityCB.Focus();
                    SpecialityCB.IsDropDownOpen = true;
                }
                else if (CuratorCB.IsFocused)
                {
                    if (EditGroupBT.IsEnabled)
                    {
                        EditGroupBT_Click(sender, e);
                    }
                }
            }
        }

        private void CuratorCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnableButton();
        }

        private void NameGroupTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableButton();
        }

        private void SpecialityCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnableButton();
            try
            {
                CuratorCB.ItemsSource = DBEntities.GetContext().Curator.Where(u => u.SpecialityID == (int?)SpecialityCB.SelectedValue).ToList();

                CuratorCB.IsEnabled = true;

            }
            catch (Exception ex)
            {
                MBClass.Error(ex);
            }
        }

        private void EnableButton()
        {
            if (string.IsNullOrWhiteSpace(NameGroupTB.Text) ||
                SpecialityCB.SelectedValue == null)
            {
                EditGroupBT.IsEnabled = false;
            }
            else
            {
                EditGroupBT.IsEnabled = true;
            }
        }


        private void AddSpecialityBT_Click(object sender, RoutedEventArgs e)
        {

            new AddSpecialityWindow().ShowDialog();


            if (VariableClass.SpecialityNewCreated == true)
            {
                SpecialityCB.ItemsSource = DBEntities.GetContext().Speciality.ToList().OrderBy(u => u.SpecialityID);

                SpecialityCB.SelectedIndex = SpecialityCB.Items.Count - 1;
                VariableClass.SpecialityNewCreated = false;

                SpecialityCB.ItemsSource = DBEntities.GetContext().Speciality.ToList();
                CuratorCB.IsEnabled = false;
            }
        }

        private void SpecialityCB_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SpecialityCB.SelectedValue == null)
            {
                return;
            }
            else
            {

                Speciality speciality = SpecialityCB.SelectedItem as Speciality;
                VariableClass.SpecialityID = speciality.SpecialityID;

                new EditSpecialityWindow(SpecialityCB.SelectedItem as Speciality,true).ShowDialog();

                SpecialityCB.ItemsSource = DBEntities.GetContext().Speciality.ToList();
                SpecialityCB.SelectedValue = VariableClass.SpecialityID;

            }
        }

        private void DeleteBT_Click(object sender, RoutedEventArgs e)
        {
            var res = DBEntities.GetContext().Student.FirstOrDefault(u => u.GroupID == VariableClass.GroupID);

            if (res != null)
            {
                MBClass.Error("Данная группа уже используется.\n" +
                    "Убедитесь, что этот группа отсутствует в списке студентов.");
                return;
            }

            if (MBClass.Question($"Вы действительно хотите удалить группу?"))
            {
                DBEntities.GetContext().Group.Remove(DataContext as Group);

                DBEntities.GetContext().SaveChanges();

                MBClass.Info("Группа успешно удалён!");

                Close();


            }
        }




       async Task saveNameGroup()
        {
            await Task.Delay(25);
            oldNameGroup = NameGroupTB.Text;
        }
    }
}
