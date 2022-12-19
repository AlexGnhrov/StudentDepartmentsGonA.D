using StudentDepartmentsGonA.D.ClassFolder;
using StudentDepartmentsGonA.D.PageFolder.EmployeePage;
using StudentDepartmentsGonA.D.PageFolder.EmployeePage.CuratorList;
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

namespace StudentDepartmentsGonA.D.WindowFolder.EmployeeFolder
{
    /// <summary>
    /// Логика взаимодействия для EmployeeWindow.xaml
    /// </summary>
    public partial class EmployeeWindow : Window
    {
        public EmployeeWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new ListStudentPage());

        }

        bool BlockDragWindow = false;

        bool CloseBIsUsing = false;
        bool HideBIsUsing = false;
        bool ResizeIsUsing = false;

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
            if (!HideBIsUsing && !ResizeIsUsing)
            {
                MBClass.Exit();
            }
        }

        private void Borders_MouseEnter(object sender, MouseEventArgs e)
        {
            ChangeColorButton();
        }


        //------------------Кнопка расширения приложения----------------------

        private async void ResizeB_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ResizeIsUsing = true;

            await DisableDrag(e);

            ResizeIsUsing = false;
        }

        private void ResizeB_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!HideBIsUsing && !CloseBIsUsing)
            {
                if (WindowState == WindowState.Normal)
                {
                    WindowState = WindowState.Maximized;
                }
                else
                {
                    WindowState = WindowState.Normal;
                }
            }
        }

        private void ResizeB_MouseEnter(object sender, MouseEventArgs e)
        {
            ChangeColorButton();
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
            if (!CloseBIsUsing && !ResizeIsUsing)
            {
                WindowState = WindowState.Minimized;
            }
        }

        private void HideB_MouseEnter(object sender, MouseEventArgs e)
        {
            ChangeColorButton();
        }




        //-------Методы для обработки цвета кнопок -------------------------------

        async Task ChangeColorButton()
        {
            if (!HideBIsUsing && !ResizeIsUsing)
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

            if (!CloseBIsUsing && !ResizeIsUsing)
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

            if (!HideBIsUsing && !CloseBIsUsing)
            {
                while (ResizeB.IsMouseOver)
                {
                    if (BlockDragWindow)
                    {
                        ResizeB.Background = new SolidColorBrush(Color.FromRgb(150, 150, 150));
                    }
                    else
                    {
                        ResizeB.Background = new SolidColorBrush(Color.FromRgb(211, 211, 211));
                    }
                    await Task.Delay(1);
                }
                ResizeB.Background = null;
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

        private void CuratorListBT_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ListCuratorPage());
            VariableClass.AdressNewCreated = true;
            VariableClass.AddAdressIiUsing = false;

            VariableClass.AdreesWasEdit = true;
            VariableClass.EditAdreesIsUsing = false;
        }

        private void StudentListBT_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ListStudentPage());
            VariableClass.AdressNewCreated = true;
            VariableClass.AddAdressIiUsing = false;

            VariableClass.AdreesWasEdit = true;
            VariableClass.EditAdreesIsUsing = false;
        }

        private void ExitBT_Click(object sender, RoutedEventArgs e)
        {
            if (MBClass.Question("Вы действительно хотите выйти из учётной записи?"))
            {
                new AuthorizationWindow().Show();
                Close();
            }
        }


        //==============Методы для Ёлки==============================================

        int ClickCount = 0;

        bool isClicked = false;
        bool ResetClickInit = false;

        bool ElkaisOn = false;
        private async void LogoIM_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isClicked = true;
            ClickCount++;

            if (!ResetClickInit)   ResetClick();

            if (ClickCount == 7)
            {
                if (!ElkaisOn)
                {
                    ElkaisOn = true;
                    MainFrame.Visibility = Visibility.Hidden;
                    MainLB.Visibility = Visibility.Hidden;

                    await NYEvent();

                    MainFrame.Visibility = Visibility.Visible;
                    MainLB.Visibility = Visibility.Visible;
                }
            }
        }


        async Task ResetClick()
        {
            while (isClicked)
            {
                isClicked = false;
                await Task.Delay(3000);
            }
            ClickCount = 0;
            ResetClickInit = false;
        }



        int FlakeIsOver = 0;
        int AmountFlake = 75;

        async Task NYEvent()
        {
            Random r = new Random();

            int FlakeY;
            int FlakeX;

            for (int i = 0; i <= 551; i++)
            {
                ElkaIM.Height = i++;

                await Task.Delay(1);
            }

            Image image;
            BitmapImage bi3 = new BitmapImage();

            bi3.BeginInit();
            bi3.UriSource = new Uri("/ResourceFolder/ImageFolder/Snowflake.png", UriKind.Relative);
            bi3.EndInit();

            for (int i = 1; i <= AmountFlake; i++)
            {
                image = new Image();

                CreateSnowFlake(image, bi3);


                FlakeY = 700;
                FlakeX = r.Next(0, 908);

                image.Margin = new Thickness(FlakeX, 0, 0, FlakeY);

                SnowFlakeFalling(image, FlakeX, FlakeY);

                await Task.Delay(r.Next(100, 401));
            }

            while (FlakeIsOver != AmountFlake)
            {
                await Task.Delay(1);
            }

            FlakeIsOver = 0;

            int LabelMarginX = 0;
            int LabelMarginY = -120;
            int Rotation = 0;

            while (true)
            {
                if (LabelMarginY <= 590) LabelMarginY += 4;
                else if (LabelMarginY > 590) LabelMarginY = 590;

                if (LabelMarginY % 10 == 0) LabelMarginX += 3;

                if (LabelMarginX <= 310) LabelMarginX++;
                else if (LabelMarginX > 310) LabelMarginX = 310;

                Rotation += 10;
                if (Rotation > 360) Rotation = 0;



                hpnLB.RenderTransform = new RotateTransform(Rotation);
                hpnLB.Margin = new Thickness(LabelMarginX, 0, 0, LabelMarginY);

                if (LabelMarginY == 590 && LabelMarginX == 310 && Rotation == 0)
                {
                    break;
                }

                await Task.Delay(1);
            }


            await Task.Delay(7000);

            hpnLB.Margin = new Thickness(0, 0, 0, -120);
            Gridder.Children.RemoveRange(11, AmountFlake);

            for (int i = 551; i >= 0; i--)
            {
                ElkaIM.Height = i--;

                await Task.Delay(1);
            }


            await Task.Delay(100);



            ElkaisOn = false;
        }

        async Task SnowFlakeFalling(Image imageS, int FlakeX, int FlakeY)
        {
            int RotationSnowFlakes = 0;

            Random r = new Random();
            while (true)
            {

                if (r.Next(0, 2) == 1)
                {
                    FlakeX += r.Next(1, 4);
                }
                else
                {
                    FlakeX -= r.Next(1, 4);
                }
                FlakeY -= r.Next(5, 10);
                imageS.Margin = new Thickness(FlakeX, 0, 0, FlakeY);
                imageS.RenderTransform = new RotateTransform(RotationSnowFlakes);



                if (FlakeY <= -50)
                {
                    FlakeIsOver++;
                    break;
                }
                else if (FlakeY < 120 && FlakeX > 320 && FlakeX < 610)
                {

                    if (r.Next(1, 11) == r.Next(1, 11))
                    {
                        FlakeIsOver++;
                        break;
                    }

                }
                else if (FlakeY < 250 && FlakeX > 340 && FlakeX < 540)
                {

                    if (r.Next(1, 13) == r.Next(1, 13))
                    {
                        FlakeIsOver++;
                        break;
                    }

                }
                else if (FlakeY < 448 && FlakeX > 420 && FlakeX < 490)
                {

                    if (r.Next(1, 16) == r.Next(1, 16))
                    {
                        FlakeIsOver++;
                        break;
                    }

                }

                RotationSnowFlakes += 2;
                await Task.Delay(100);
            }
        }


        void CreateSnowFlake(Image image, BitmapImage bitmapImage)
        {

            image.Stretch = Stretch.Fill;
            image.Source = bitmapImage;

            image.Height = 40;
            image.Width = 40;

            image.HorizontalAlignment = HorizontalAlignment.Left;
            image.VerticalAlignment = VerticalAlignment.Bottom;

            Gridder.Children.Add(image);

            Grid.SetRow(image, 1);
            Grid.SetColumn(image, 1);
            Grid.SetRowSpan(image, 3);
        }

    }
}