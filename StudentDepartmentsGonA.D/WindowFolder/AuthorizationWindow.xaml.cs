using StudentDepartmentsGonA.D.ClassFolder;
using StudentDepartmentsGonA.D.DataFolder;
using StudentDepartmentsGonA.D.WindowFolder.AdminFolder;
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
using System.Windows.Shapes;

namespace StudentDepartmentsGonA.D.WindowFolder
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        public AuthorizationWindow()
        {
            InitializeComponent();
        }

        bool BlockDragWindow = false;

        bool CloseBIsUsing = false;
        bool HideBIsUsing = false;

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

        //------------------Кнопка закрытия приложения----------------------


        private async void CloseB_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CloseBIsUsing = true;

            await DisableDrag(e);

            CloseBIsUsing = false;

        }


        private void CloseB_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!HideBIsUsing)
            {
                Close();
            }
        }

        private async void Borders_MouseEnter(object sender, MouseEventArgs e)
        {
            await ChangeColorButton();
        }

        //------------------Кнопка скрытия приложения----------------------


        private async void HideB_MouseDown(object sender, MouseButtonEventArgs e)
        {
            HideBIsUsing = true;

            await DisableDrag(e);

            HideBIsUsing = false;
        }

        private void HideB_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!CloseBIsUsing)
            {
                WindowState = WindowState.Minimized;
            }
        }

        private async void HideB_MouseEnter(object sender, MouseEventArgs e)
        {
            await ChangeColorButton();
        }


        private void LoginBT_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var user = DBEntities.GetContext().User.FirstOrDefault(u => u.Login == LogintTB.Text);

                if (user == null)
                {
                    MBClass.Error("Введён не верный логин");
                    LogintTB.Focus();
                    return;

                }
                if (user.Password != PasswordPB.Password)
                {
                    MBClass.Error("Введён не верный паролль");
                    PasswordPB.Focus();
                    return;

                }
                else
                {
                    switch (user.RoleID)
                    {
                        case 1:
                            new AdministrationWindow().Show();
                            Close();
                            break;
                        case 2:
                            new EmployeeWindow().Show();
                            Close();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MBClass.Error(ex);
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (LoginBT.IsEnabled)
                {
                    LoginBT_Click(sender, e);
                }
                else if (LogintTB.IsFocused)
                {
                    PasswordPB.Focus();
                }
            }
        }

        private void LogintTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableButton();
        }

        private void PasswordPB_PasswordChanged(object sender, RoutedEventArgs e)
        {
            EnableButton();
        }


        private void EnableButton()
        {
            if (string.IsNullOrWhiteSpace(PasswordPB.Password) ||
                string.IsNullOrWhiteSpace(LogintTB.Text))
            {
                LoginBT.IsEnabled = false;
            }
            else
            {
                LoginBT.IsEnabled = true;
            }
        }



        //-------Методы для обработки кнопок -------------------------------

        async Task ChangeColorButton()
        {
            if (!HideBIsUsing)
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
                CloseLB.Foreground = new SolidColorBrush(Colors.Black);
            }

            if (!CloseBIsUsing)
            {
                while (HideB.IsMouseOver)
                {
                    if (BlockDragWindow)
                    {
                        HideB.Background = new SolidColorBrush(Color.FromRgb(150, 150, 150));
                    }
                    else
                    {
                        HideB.Background = new SolidColorBrush(Color.FromRgb(211, 211, 211));
                    }
                    await Task.Delay(1);
                }
                HideB.Background = null;
            }


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
    }
}