using StudentDepartmentsGonA.D.ClassFolder;
using StudentDepartmentsGonA.D.DataFolder;
using StudentDepartmentsGonA.D.WindowFolder.EmployeeFolder.AdditionalWIndows;
using StudentDepartmentsGonA.D.WindowFolder.EmployeeFolder.AdditionalWIndows.AdressWindow.RegionWinow;
using StudentDepartmentsGonA.D.WindowFolder.EmployeeFolder.AdditionalWIndows.AdressWindow.StreetWindow;
using StudentDepartmentsGonA.D.WindowFolder.EmployeeFolder.AdditionalWIndows.CityWindow;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StudentDepartmentsGonA.D.PageFolder.EmployeePage.StudentList.AdressPage
{
    /// <summary>
    /// Логика взаимодействия для EditAdressPage.xaml
    /// </summary>
    public partial class EditAdressPage : Page
    {

        Adress adress = new Adress();
        bool DeleteButtonEnable;
        public EditAdressPage(Adress AdressID, bool DeleteButtonEnable)
        {
            this.DeleteButtonEnable = DeleteButtonEnable;

             DataContext = AdressID;

            InitializeComponent();

            DeleteBT.IsEnabled = DeleteButtonEnable;


            RegionCB.ItemsSource = DBEntities.GetContext().Region.ToList();
            CityCB.ItemsSource = DBEntities.GetContext().City.ToList();
            StreetCB.ItemsSource = DBEntities.GetContext().Street.ToList();
        }

        bool AdressWasIdited = false;

        private void BackBT_Click(object sender, RoutedEventArgs e)
        {
            if (AdressWasIdited)
            {
                VariableClass.EditAdreesIsUsing = false;
                VariableClass.AdreesWasEdit = true;
            }
            NavigationService.GoBack();
        }

        private void EditAdressBT_Click(object sender, RoutedEventArgs e)
        {

            try
            {

                adress = DBEntities.GetContext().Adress.FirstOrDefault(u => u.AdressID == VariableClass.AdressID);

                adress.IndexAdress = Int32.Parse(IndexAdressTB.Text);
                adress.RegionID = (int)RegionCB.SelectedValue;
                adress.CityID = (int)CityCB.SelectedValue;
                adress.StreetID = (int)StreetCB.SelectedValue;
                adress.House = Int32.Parse(HomeTB.Text);
                if (string.IsNullOrWhiteSpace(BuilidngTB.Text))
                {
                    adress.Building = null;
                }
                else
                {
                   adress.Building = Int32.Parse(BuilidngTB.Text);
                }
                adress.Appartament = Int32.Parse(AppartamentTB.Text);           

                DBEntities.GetContext().SaveChanges();

                MBClass.Info("Адресс упешно отредактирован!");

                AdressWasIdited = true;
            }
            catch (Exception ex)
            {

                MBClass.Error(ex);
            }

        }

        //---------------------------------------------------------------------------------------------------РЕГИОН----------------


        private void RegionCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnableButton();
        }

        private void AddRegionBT_Click(object sender, RoutedEventArgs e)
        {
            new AddRegionWindow().ShowDialog();

            if (VariableClass.RegionNewCreated == true)
            {
                RegionCB.ItemsSource = DBEntities.GetContext().Region.ToList().OrderBy(u => u.RegionID);

                RegionCB.SelectedIndex = RegionCB.Items.Count - 1;
                VariableClass.RegionNewCreated = false;

                RegionCB.ItemsSource = DBEntities.GetContext().Region.ToList();

            }
        }

        private void RegionCB_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (RegionCB.SelectedValue == null)
            {
                return;
            }
            else
            {

                Region region = RegionCB.SelectedItem as Region;
                VariableClass.RegionID = region.RegionID;

                new EditRegionWindow(RegionCB.SelectedItem as Region, DeleteButtonEnable).ShowDialog();

                RegionCB.ItemsSource = DBEntities.GetContext().Region.ToList();
                RegionCB.SelectedValue = VariableClass.RegionID;

            }
        }


        //---------------------------------------------------------------------------------------------------ГОРОД----------------

        private void CityCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnableButton();
        }

        private void AddCityBT_Click(object sender, RoutedEventArgs e)
        {
            new AddCityWindow().ShowDialog();

            if (VariableClass.CityNewCreated == true)
            {

                CityCB.ItemsSource = DBEntities.GetContext().City.ToList().OrderBy(u => u.CityID);

                CityCB.SelectedIndex = CityCB.Items.Count - 1;
                VariableClass.CityNewCreated = false;

                CityCB.ItemsSource = DBEntities.GetContext().City.ToList();
            }
        }

        private void CityCB_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (CityCB.SelectedValue == null)
            {
                return;
            }
            else
            {

                City city = CityCB.SelectedItem as City;
                VariableClass.CityID = city.CityID;

                new EditCityWindow(CityCB.SelectedItem as City, DeleteButtonEnable).ShowDialog();

                CityCB.ItemsSource = DBEntities.GetContext().City.ToList();
                CityCB.SelectedValue = VariableClass.CityID;

            }
        }

        //---------------------------------------------------------------------------------------------------УЛИЦА----------------

        private void StreetCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnableButton();
        }

        private void AddStreetBT_Click(object sender, RoutedEventArgs e)
        {
            new AddStreetWindow().ShowDialog();

            if (VariableClass.StreetNewCreated == true)
            {

                StreetCB.ItemsSource = DBEntities.GetContext().Street.ToList().OrderBy(u => u.StreetID);

                StreetCB.SelectedIndex = StreetCB.Items.Count - 1;
                VariableClass.StreetNewCreated = false;

                StreetCB.ItemsSource = DBEntities.GetContext().Street.ToList();
            }
        }

        private void StreetCB_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (StreetCB.SelectedValue == null)
            {
                return;
            }
            else
            {

                Street street = StreetCB.SelectedItem as Street;
                VariableClass.StreetID = street.StreetID;

                new EditStreetWindow(StreetCB.SelectedItem as Street, DeleteButtonEnable).ShowDialog();

                StreetCB.ItemsSource = DBEntities.GetContext().Street.ToList();
                StreetCB.SelectedValue = VariableClass.StreetID;

            }
        }


        //---------------------------------------------------------------------------------------------------ИНДЕКС----------------
        private void IndexAdressTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableButton();
        }

        private void IndexAdressTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            OnlyNums(sender, e);
        }

        //---------------------------------------------------------------------------------------------------ДОМ----------------

        private void HomeTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableButton();
        }

        private void HomeTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            OnlyNums(sender, e);
        }

        //---------------------------------------------------------------------------------------------------КОРПУС----------------


        private void BuilidngTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            OnlyNums(sender, e);
        }

        //---------------------------------------------------------------------------------------------------КВАРТИРА----------------

        private void AppartamentTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableButton();
        }

        private void AppartamentTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            OnlyNums(sender, e);
        }

        //-------------------------------------------------------------------------------------------------------------------



        private void OnlyNums(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }


        private void EnableButton()
        {
            if (RegionCB.SelectedValue == null ||
               CityCB.SelectedValue == null ||
               StreetCB.SelectedValue == null ||
               string.IsNullOrWhiteSpace(IndexAdressTB.Text) || IndexAdressTB.Text.Length < 6 ||
               string.IsNullOrWhiteSpace(HomeTB.Text) ||
               string.IsNullOrWhiteSpace(AppartamentTB.Text)
               )
            {
                EditAdressBT.IsEnabled = false;
            }
            else
            {
                EditAdressBT.IsEnabled = true;
            }
        }

        private void DeleteBT_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                var res = DBEntities.GetContext().Student.FirstOrDefault(u => u.PlaceOfRegistrationID == VariableClass.AdressID);

                if (res != null)
                {
                    MBClass.Error("Данный адресс уже используется.\n" +
                        "Убедитесь, что адресс отсутствует у студента");
                    return;
                }
                if (MBClass.Question($"Вы действительно хотите удалить адресс?"))
                {

                    DBEntities.GetContext().SaveChanges();

                    DBEntities.GetContext().Adress.Remove(DataContext as Adress);

                    DBEntities.GetContext().SaveChanges();

                    AdressWasIdited = true;

                    VariableClass.EditAdreesIsUsing = false;
                    VariableClass.AdreesWasEdit = true;

                    MBClass.Info("Адрес успешно удалён!");

                    NavigationService.GoBack();

                }

            }
            catch (Exception ex)
            {
                MBClass.Error(ex);
            }
        }
    }
}

