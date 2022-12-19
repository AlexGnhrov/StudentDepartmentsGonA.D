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

namespace StudentDepartmentsGonA.D.WindowFolder.EmployeeFolder.AdditionalWIndows.CityWindow
{
    /// <summary>
    /// Логика взаимодействия для EditCityWindow.xaml
    /// </summary>
    public partial class EditCityWindow : Window
    {
        City city = new City();
        string oldCity;
        public EditCityWindow(City CityID, bool EnableDelete)
        {
            DataContext = CityID;

         
            InitializeComponent();

            DeleteBT.IsEnabled = EnableDelete;

            SaveCity();
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

        private void EditCityBT_Click(object sender, RoutedEventArgs e)
        {

            if (oldCity != CityTB.Text)
            {
                var city = DBEntities.GetContext().City.FirstOrDefault(u => u.NameCity == CityTB.Text);

                if (city != null)
                {
                    MBClass.Error("Такой город уже существует");
                    CityTB.Focus();
                    return;

                }
            }


            try
            {
                city = DBEntities.GetContext().City.FirstOrDefault(u => u.CityID == VariableClass.CityID);

                city.NameCity = CityTB.Text;

                DBEntities.GetContext().SaveChanges();

                MBClass.Info("Город успешно отредактирован!");

                oldCity = CityTB.Text;

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
                if (EditCityBT.IsEnabled)
                {
                    EditCityBT_Click(sender, e);
                }

            }
        }

        private void CityTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CityTB.Text))
            {
                EditCityBT.IsEnabled = false;
            }
            else
            {
                EditCityBT.IsEnabled = true;
            }
        }

        private void DeleteBT_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var res = DBEntities.GetContext().Student.FirstOrDefault(u => u.PlaceOfBirthID == VariableClass.CityID);
                var res2 = DBEntities.GetContext().Adress.FirstOrDefault(u => u.CityID == VariableClass.CityID);
                if (res != null || res2 != null)
                {
                    MBClass.Error("Данный город уже используется.\n" +
                        "Убедитесь, что этот город отсутствует в списке студентов или адрессе.");
                    return;
                }

                if (MBClass.Question($"Вы действительно хотите удалить город?"))
                {
                    DBEntities.GetContext().City.Remove(DataContext as City);

                    DBEntities.GetContext().SaveChanges();

                    MBClass.Info("Город успешно удалён!");

                    Close();


                }
            }
            catch (Exception ex)
            {

                MBClass.Error(ex);
            }

        }

        async Task SaveCity()
        {
            await Task.Delay(25);
            oldCity = CityTB.Text;
        }
    }
}

