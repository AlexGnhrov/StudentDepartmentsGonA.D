using StudentDepartmentsGonA.D.ClassFolder;
using StudentDepartmentsGonA.D.DataFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace StudentDepartmentsGonA.D.WindowFolder.EmployeeFolder.AdditionalWIndows
{
    /// <summary>
    /// Логика взаимодействия для AddPlaceOfIssuePassportWindow.xaml
    /// </summary>
    public partial class AddPlaceOfIssuePassportWindow : Window
    {
        public AddPlaceOfIssuePassportWindow()
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


        private void AddPlacePassportBT_Click(object sender, RoutedEventArgs e)
        {

            var CosePaspprot = DBEntities.GetContext().PlaceIssueOfPassport.FirstOrDefault(u => u.DepartmentСode == DepartamentCodeTB.Text);
            var DepPassport = DBEntities.GetContext().PlaceIssueOfPassport.FirstOrDefault(u => u.NamePlace == NamePlaceTB.Text);

            if (CosePaspprot != null)
            {
                MBClass.Error("Такой код департамента уже существует");
                DepartamentCodeTB.Focus();
                return;

            }
            if (DepPassport != null)
            {
                MBClass.Error("Такой департамент уже существует");
                NamePlaceTB.Focus();
                return;

            }
            try
            {
                DBEntities.GetContext().PlaceIssueOfPassport.Add(new PlaceIssueOfPassport()
                {
                    DepartmentСode = DepartamentCodeTB.Text,
                    NamePlace = NamePlaceTB.Text,
                });

                DBEntities.GetContext().SaveChanges();


                MBClass.Info("Место успешно добавлено!");
                VariableClass.pioPassportNewCreated = true;
            }
            catch(Exception ex)
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
                if (AddPlacePassportBT.IsEnabled)
                {
                    AddPlacePassportBT_Click(sender, e);
                }
                else if (DepartamentCodeTB.IsFocused)
                {
                   NamePlaceTB.Focus();
                }
            }
        }

        private void DepartamentCodeTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);

        }

        private void DepartamentCodeTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox DepCode = DepartamentCodeTB;


            try
            {
                if (!Keyboard.IsKeyDown(Key.Back))
                {
                    switch (DepCode.Text.Length)
                    {
                        case 3:
                            DepCode.Text = DepCode.Text.Insert(3, "-");
                            DepCode.CaretIndex = 5;
                            break;
                    }
                }

                else
                {
                    switch (DepCode.Text.Length)
                    {
                        case 4:
                            DepCode.Text =
                                DepCode.Text.Remove(DepCode.Text.LastIndexOf("-"));
                            DepCode.CaretIndex = 3;
                            break;
                    }
                }
            }
            catch (Exception)
            {
                return;
            }

            EnableButton();
        }

        private void NamePlaceTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableButton();
        }

        private void EnableButton()
        {
            if (string.IsNullOrWhiteSpace(DepartamentCodeTB.Text) ||
                DepartamentCodeTB.Text.Length < 7||
                string.IsNullOrWhiteSpace(NamePlaceTB.Text))
            {
                AddPlacePassportBT.IsEnabled = false;
            }
            else
            {
                AddPlacePassportBT.IsEnabled = true;
            }
        }
    }
}
