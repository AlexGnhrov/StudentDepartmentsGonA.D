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

namespace StudentDepartmentsGonA.D.WindowFolder.EmployeeFolder.AdditionalWIndows.SpecialityWindow
{
    /// <summary>
    /// Логика взаимодействия для AddSpecialityWindow.xaml
    /// </summary>
    public partial class AddSpecialityWindow : Window
    {
        public AddSpecialityWindow()
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


        private void AddSpecialityBT_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DBEntities.GetContext().Speciality.Add(new Speciality()
                {
                    SpecialityCode = SpecialityCodeTB.Text,
                    NameSpeciality = NameSpecialityTB.Text,
                    TermOFTraining = TermOFTrainingTB.Text
                });

                DBEntities.GetContext().SaveChanges();


                MBClass.Info("Место успешно добавлено!");
                VariableClass.SpecialityNewCreated = true;
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
                if (AddSpecialityBT.IsEnabled)
                {
                     AddSpecialityBT_Click(sender, e);
                }
                else if (SpecialityCodeTB.IsFocused)
                {
                    NameSpecialityTB.Focus();
                }
                else if (NameSpecialityTB.IsFocused)
                {
                    TermOFTrainingTB.Focus();
                }
            }
        }



        private void SpecialityCodeTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableButton();
        }

        private void NameSpecialityTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableButton();
        }

        private void TermOFTrainingTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableButton();
        }

        private void EnableButton()
        {
            if (string.IsNullOrWhiteSpace(SpecialityCodeTB.Text) || SpecialityCodeTB.Text.Length != 8 ||
                string.IsNullOrWhiteSpace(NameSpecialityTB.Text) ||
                string.IsNullOrWhiteSpace(TermOFTrainingTB.Text))
            {
                AddSpecialityBT.IsEnabled = false;
            }
            else
            {
                AddSpecialityBT.IsEnabled = true;
            }
        }

    }
}
