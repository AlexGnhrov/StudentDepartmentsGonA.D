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
using System.Windows.Shapes;

namespace StudentDepartmentsGonA.D.WindowFolder.EmployeeFolder.AdditionalWIndows.StatusWIndow
{
    /// <summary>
    /// Логика взаимодействия для EditStatusWIndow.xaml
    /// </summary>
    public partial class EditStatusWIndow : Window
    {
        Status status = new Status();
        string oldStatus;
        public EditStatusWIndow(Status status, bool EnableDelete)
        {
            DataContext = status;

            InitializeComponent();

            DeleteBT.IsEnabled = EnableDelete;

            saveStatus();

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

        private void EditStatusBT_Click(object sender, RoutedEventArgs e)
        {
            var status = DBEntities.GetContext().Status.FirstOrDefault(u => u.NameStatus == StatusTB.Text);

            if (oldStatus != StatusTB.Text)
            {
                if (status != null)
                {
                    MBClass.Error("Такой статус уже существует");
                    StatusTB.Focus();
                    return;

                }
            }
            try
            {
                status = DBEntities.GetContext().Status.FirstOrDefault(u => u.StatusID == VariableClass.StatusID);
                status.NameStatus = StatusTB.Text;

                DBEntities.GetContext().SaveChanges();

                MBClass.Info("Статус успешно отредактирован!");


                oldStatus = StatusTB.Text;
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
                if (EditStatusBT.IsEnabled)
                {
                    EditStatusBT_Click(sender, e);
                }

            }
        }

        private void CityTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(StatusTB.Text))
            {
                EditStatusBT.IsEnabled = false;
            }
            else
            {
                EditStatusBT.IsEnabled = true;
            }
        }

        private void DeleteBT_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var stat = DBEntities.GetContext().Student.FirstOrDefault(u => u.StatusID == VariableClass.StatusID);

                if (stat != null)
                {
                    MBClass.Error("Данный статус уже используется.\n" +
                        "Убедитесь, что этот статус отсутствует в списке студентов.");
                    return;
                }

                if (MBClass.Question($"Вы действительно хотите удалить статус?"))
                {
                    DBEntities.GetContext().Status.Remove(DataContext as Status);

                    DBEntities.GetContext().SaveChanges();

                    MBClass.Info("Статус успешно удалён!");
                    
                    Close();


                }
            }
            catch (Exception ex)
            {
                MBClass.Error(ex);
            }
        }


        async Task saveStatus()
        {
            await Task.Delay(25);
            oldStatus = StatusTB.Text;
        }
    }
}