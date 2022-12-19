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

namespace StudentDepartmentsGonA.D.PageFolder.EmployeePage
{
    /// <summary>
    /// Логика взаимодействия для AddAdressPage.xaml
    /// </summary>
    public partial class AddAdressPage : Page
    {
        public AddAdressPage()
        {
            InitializeComponent();

            RegionCB.ItemsSource = DBEntities.GetContext().Region.ToList();
            CityCB.ItemsSource = DBEntities.GetContext().City.ToList();
            StreetCB.ItemsSource = DBEntities.GetContext().Street.ToList();
        }

        bool NewAdressAdded = false;

        private void BackBT_Click(object sender, RoutedEventArgs e)
        {
            if (NewAdressAdded)
            {
                VariableClass.AddAdressIiUsing = false;
                VariableClass.AdressNewCreated = true;
            }

            NavigationService.GoBack();
        }

        private void AddAdressBT_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (string.IsNullOrWhiteSpace(BuilidngTB.Text))
                {
                    DBEntities.GetContext().Adress.Add(new Adress
                    {


                        IndexAdress = Int32.Parse(IndexAdressTB.Text),
                        RegionID = (int)RegionCB.SelectedValue,
                        CityID = (int)CityCB.SelectedValue,
                        StreetID = (int)StreetCB.SelectedValue,
                        House = Int32.Parse(HomeTB.Text),
                        Building = null,
                        Appartament = Int32.Parse(AppartamentTB.Text)

                    });
                }
                else if (!string.IsNullOrWhiteSpace(BuilidngTB.Text))
                {
                    DBEntities.GetContext().Adress.Add(new Adress
                    {


                        IndexAdress = Int32.Parse(IndexAdressTB.Text),
                        RegionID = (int)RegionCB.SelectedValue,
                        CityID = (int)CityCB.SelectedValue,
                        StreetID = (int)StreetCB.SelectedValue,
                        House = Int32.Parse(HomeTB.Text),
                        Building = Int32.Parse(BuilidngTB.Text),
                        Appartament = Int32.Parse(AppartamentTB.Text)

                    });
                }

                DBEntities.GetContext().SaveChanges();

                MBClass.Info("Адресс упешно добавлен!");

                NewAdressAdded = true;
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
            Focus();
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

                new EditRegionWindow(RegionCB.SelectedItem as Region,true).ShowDialog();

                RegionCB.ItemsSource = DBEntities.GetContext().Region.ToList();
                RegionCB.SelectedValue = VariableClass.RegionID;
                Focus();
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
            Focus();
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

                new EditCityWindow(CityCB.SelectedItem as City,true).ShowDialog();

                CityCB.ItemsSource = DBEntities.GetContext().City.ToList();
                CityCB.SelectedValue = VariableClass.CityID;
                Focus();
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
            Focus();
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

                new EditStreetWindow(StreetCB.SelectedItem as Street,true).ShowDialog();

                StreetCB.ItemsSource = DBEntities.GetContext().Street.ToList();
                StreetCB.SelectedValue = VariableClass.StreetID;
                Focus();
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
            if(RegionCB.SelectedValue==null ||
               CityCB.SelectedValue == null ||
               StreetCB.SelectedValue == null ||
               string.IsNullOrWhiteSpace(IndexAdressTB.Text) || IndexAdressTB.Text.Length < 6 ||
               string.IsNullOrWhiteSpace(HomeTB.Text) ||
               string.IsNullOrWhiteSpace(AppartamentTB.Text)
               )
            {
                AddAdressBT.IsEnabled = false;
            }
            else
            {
                AddAdressBT.IsEnabled = true;
            }
        }

    }
}
