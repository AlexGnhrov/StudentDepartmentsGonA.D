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
    /// Логика взаимодействия для AddStatusWindow.xaml
    /// </summary>
    public partial class AddStatusWindow : Window
    {
        public AddStatusWindow()
        {
            InitializeComponent();
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

        private void AddStatusBT_Click(object sender, RoutedEventArgs e)
        {
            var status = DBEntities.GetContext().Status.FirstOrDefault(u => u.NameStatus == StatusTB.Text);


            if (status != null)
            {
                MBClass.Error("Такой статус уже существует");
                StatusTB.Focus();
                return;

            }
            
            try
            {
                DBEntities.GetContext().Status.Add(new Status
                {
                    NameStatus = StatusTB.Text
                });

                DBEntities.GetContext().SaveChanges();

                MBClass.Info("Статус успешно добавлен!");

                VariableClass.StatusNewCreated = true;

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
                if (AddStatusBT.IsEnabled)
                {
                    AddStatusBT_Click(sender, e);
                }

            }
        }

        private void CityTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(StatusTB.Text))
            {
                AddStatusBT.IsEnabled = false;
            }
            else
            {
                AddStatusBT.IsEnabled = true;
            }
        }
    }
}