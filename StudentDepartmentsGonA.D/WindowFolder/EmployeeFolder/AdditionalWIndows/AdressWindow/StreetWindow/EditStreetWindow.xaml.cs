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

namespace StudentDepartmentsGonA.D.WindowFolder.EmployeeFolder.AdditionalWIndows.AdressWindow.StreetWindow
{
    /// <summary>
    /// Логика взаимодействия для EditStreetWindow.xaml
    /// </summary>
    public partial class EditStreetWindow : Window
    {

        Street street = new Street();
        string oldStreet;
        public EditStreetWindow(Street StreetID, bool EnableDelete)
        {
            DataContext = StreetID;

            InitializeComponent();

            DeleteBT.IsEnabled = EnableDelete;

            saveStreet();
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

        private void EditStreetBT_Click(object sender, RoutedEventArgs e)
        {
            var street = DBEntities.GetContext().Street.FirstOrDefault(u => u.NameStreet == StreetTB.Text);

            if (oldStreet != StreetTB.Text)
            {
                if (street != null)
                {
                    MBClass.Error("Такая улица уже существует");
                    StreetTB.Focus();
                    return;

                }
            }
            try
            {
                street = DBEntities.GetContext().Street.FirstOrDefault(u => u.StreetID == VariableClass.StreetID);

                street.NameStreet = StreetTB.Text;

                DBEntities.GetContext().SaveChanges();

                MBClass.Info("Улица успешно отредактирован!");

                oldStreet = StreetTB.Text;

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
                if (EditStreetBT.IsEnabled)
                {
                    EditStreetBT_Click(sender, e);
                }

            }
        }

        private void SteetTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(StreetTB.Text))
            {
                EditStreetBT.IsEnabled = false;
            }
            else
            {
                EditStreetBT.IsEnabled = true;
            }
        }

        private void DeleteBT_Click(object sender, RoutedEventArgs e)
        {
            var res = DBEntities.GetContext().Adress.FirstOrDefault(u => u.StreetID == VariableClass.StreetID);

            if (res != null)
            {
                MBClass.Error("Данная улица уже используется.\n" +
                    "Убедитесь, что эта улица отсутствует в списке студентов.");
                return;
            }

            if (MBClass.Question($"Вы действительно хотите удалить улицу?"))
            {
                DBEntities.GetContext().Street.Remove(DataContext as Street);

                DBEntities.GetContext().SaveChanges();

                MBClass.Info("Улица успешно удалёна!");

                Close();


            }
        }


        async Task saveStreet()
        {
            await Task.Delay(25);
            oldStreet = StreetTB.Text;
        }
    }
}

