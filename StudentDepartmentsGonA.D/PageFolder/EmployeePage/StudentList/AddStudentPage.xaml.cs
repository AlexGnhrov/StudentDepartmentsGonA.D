using Microsoft.Win32;
using StudentDepartmentsGonA.D.ClassFolder;
using StudentDepartmentsGonA.D.DataFolder;
using StudentDepartmentsGonA.D.PageFolder.EmployeePage.StudentList.AdressPage;
using StudentDepartmentsGonA.D.WindowFolder.EmployeeFolder.AdditionalWIndows;
using StudentDepartmentsGonA.D.WindowFolder.EmployeeFolder.AdditionalWIndows.CityWindow;
using StudentDepartmentsGonA.D.WindowFolder.EmployeeFolder.AdditionalWIndows.GroupWindow;
using StudentDepartmentsGonA.D.WindowFolder.EmployeeFolder.AdditionalWIndows.PlaceOfIssuePassportWIndow;
using StudentDepartmentsGonA.D.WindowFolder.EmployeeFolder.AdditionalWIndows.StatusWIndow;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
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
using Group = StudentDepartmentsGonA.D.DataFolder.Group;

namespace StudentDepartmentsGonA.D.PageFolder.EmployeePage
{
    /// <summary>
    /// Логика взаимодействия для AddStudentPage.xaml
    /// </summary>
    public partial class AddStudentPage : Page
    {

        AddAdressPage addAdressPage;
        EditAdressPage editAdressPage;
        

        OpenFileDialog openFileDialog;
        public AddStudentPage()
        {
            InitializeComponent();


            VariableClass.AdressNewCreated = false;
            VariableClass.AddAdressIiUsing = false;

            PlaceIssueOfPassportCB.ItemsSource = DBEntities.GetContext().PlaceIssueOfPassport.ToList();
            PlaceOfBirthCB.ItemsSource = DBEntities.GetContext().City.ToList();
            PlaceOfRegistrationCB.ItemsSource = DBEntities.GetContext().Adress.ToList();
            GroupCB.ItemsSource = DBEntities.GetContext().Group.ToList();
            StatusCB.ItemsSource = DBEntities.GetContext().Status.ToList();


            PhoneNumTB.Text = "+7 ";
            PhoneNumTB.CaretIndex = 4;

        }

        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        string selectedFileName = "";


        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            AddPhoto();

        }
        private void AddStudentBT_Click(object sender, RoutedEventArgs e)
        {
            string[] SplitSNP = SNPstudentTB.Text.Split(' ');
            string[] SplitPassport = PassportNumTB.Text.Split(' ');

            string PassportSeriesNum = SplitPassport[0];
            int PassportNum = Convert.ToInt32(SplitPassport[1].ToString());

            var snil = DBEntities.GetContext().Student.FirstOrDefault(u => u.SNILS == snilsTB.Text);
            var inn = DBEntities.GetContext().Student.FirstOrDefault(u => u.INN == innTB.Text);
            var OMS = DBEntities.GetContext().Student.FirstOrDefault(u => u.OMS == omsTB.Text);

            if (snil != null)
            {
                MBClass.Error("Данный СНИЛС уже существует");
                snilsTB.Focus();
                return;
            }
            if (inn != null)
            {
                MBClass.Error("Данный ИИН уже существует");
                innTB.Focus();
                return;
            }
            if (OMS != null)
            {
                MBClass.Error("Данный ОМС уже существует");
                omsTB.Focus();
                return;
            }


            try
            {
                switch (SplitSNP.Length)
                {
                    case 2: // НЕ ПОЛНОЕ ФИО ---------------------------------------------
                        if (selectedFileName == "" )
                        {
                            DBEntities.GetContext().Student.Add(new Student()
                            {
                                Surname = SplitSNP[0].ToString(),
                                Name = SplitSNP[1].ToString(),
                                Patronymic = null,
                                PhoneNum = PhoneNumTB.Text,
                                Email = EmailTB.Text,
                                PassportSeries = PassportSeriesNum,
                                PassportNum = PassportNum,
                                INN = innTB.Text,
                                SNILS = snilsTB.Text,
                                OMS = omsTB.Text,
                                ReceiptDate = DateTime.Parse(DateReciptTB.Text),
                                ReleaseDate = (DateTime?)DateReleaseTB.SelectedDate,
                                DateOfBirth = DateTime.Parse(DateOfBirthTB.Text),
                                OrderStudent = OrderTB.Text,
                                PlaceOfBirthID = Convert.ToInt32(PlaceOfBirthCB.SelectedValue),
                                PlaceIssueOfPassportID = Convert.ToInt32(PlaceIssueOfPassportCB.SelectedValue),
                                PlaceOfRegistrationID = Convert.ToInt32(PlaceOfRegistrationCB.SelectedValue),
                                GroupID = (int?)GroupCB.SelectedValue,
                                StatusID = Convert.ToInt32(StatusCB.SelectedValue),
                                Image = null
                            });
                        }
                        else if (selectedFileName != "")
                        {
                            DBEntities.GetContext().Student.Add(new Student()
                            {
                                Surname = SplitSNP[0].ToString(),
                                Name = SplitSNP[1].ToString(),
                                Patronymic = null,
                                PhoneNum = PhoneNumTB.Text,
                                Email = EmailTB.Text,
                                PassportSeries = PassportSeriesNum,
                                PassportNum = PassportNum,
                                INN = innTB.Text,
                                SNILS = snilsTB.Text,
                                OMS = omsTB.Text,
                                ReceiptDate = DateTime.Parse(DateReciptTB.Text),
                                ReleaseDate = (DateTime?)DateReleaseTB.SelectedDate,
                                DateOfBirth = DateTime.Parse(DateOfBirthTB.Text),
                                OrderStudent = OrderTB.Text,
                                PlaceOfBirthID = Convert.ToInt32(PlaceOfBirthCB.SelectedValue),
                                PlaceIssueOfPassportID = Convert.ToInt32(PlaceIssueOfPassportCB.SelectedValue),
                                PlaceOfRegistrationID = Convert.ToInt32(PlaceOfRegistrationCB.SelectedValue),
                                GroupID = (int?)GroupCB.SelectedValue,
                                StatusID = Convert.ToInt32(StatusCB.SelectedValue),
                                Image = LoadAndReadImage.ConvertImageToByteArray(selectedFileName)
                            });
                        }
                        break;
                    case 3: // ПОЛНОЕ ФИО ---------------------------------------------
                        if (selectedFileName == "")
                        {
                            DBEntities.GetContext().Student.Add(new Student()
                            {
                                Surname = SplitSNP[0].ToString(),
                                Name = SplitSNP[1].ToString(),
                                Patronymic = SplitSNP[2].ToString(),
                                PhoneNum = PhoneNumTB.Text,
                                Email = EmailTB.Text,
                                PassportSeries = PassportSeriesNum,
                                PassportNum = PassportNum,
                                INN = innTB.Text,
                                SNILS = snilsTB.Text,
                                OMS = omsTB.Text,
                                ReceiptDate = DateTime.Parse(DateReciptTB.Text),
                                ReleaseDate = (DateTime?)DateReleaseTB.SelectedDate,
                                DateOfBirth = DateTime.Parse(DateOfBirthTB.Text),
                                OrderStudent = OrderTB.Text,
                                PlaceOfBirthID = Convert.ToInt32(PlaceOfBirthCB.SelectedValue),
                                PlaceIssueOfPassportID = Convert.ToInt32(PlaceIssueOfPassportCB.SelectedValue),
                                PlaceOfRegistrationID = Convert.ToInt32(PlaceOfRegistrationCB.SelectedValue),
                                GroupID = (int?)GroupCB.SelectedValue,
                                StatusID = Convert.ToInt32(StatusCB.SelectedValue),
                                Image = null
                            });
                        }
                        else if (selectedFileName != "")
                        {
                            DBEntities.GetContext().Student.Add(new Student()
                            {
                                Surname = SplitSNP[0].ToString(),
                                Name = SplitSNP[1].ToString(),
                                Patronymic = SplitSNP[2].ToString(),
                                PhoneNum = PhoneNumTB.Text,
                                Email = EmailTB.Text,
                                PassportSeries = PassportSeriesNum,
                                PassportNum = PassportNum,
                                INN = innTB.Text,
                                SNILS = snilsTB.Text,
                                OMS = omsTB.Text,
                                ReceiptDate = DateTime.Parse(DateReciptTB.Text),
                                ReleaseDate = (DateTime?)DateReleaseTB.SelectedDate,
                                DateOfBirth = DateTime.Parse(DateOfBirthTB.Text),
                                OrderStudent = OrderTB.Text,
                                PlaceOfBirthID = Convert.ToInt32(PlaceOfBirthCB.SelectedValue),
                                PlaceIssueOfPassportID = Convert.ToInt32(PlaceIssueOfPassportCB.SelectedValue),
                                PlaceOfRegistrationID = Convert.ToInt32(PlaceOfRegistrationCB.SelectedValue),
                                GroupID = Convert.ToInt32(GroupCB.SelectedValue),
                                StatusID = Convert.ToInt32(StatusCB.SelectedValue),
                                Image = LoadAndReadImage.ConvertImageToByteArray(selectedFileName)
                            });
                        }

                        break;
                }

                

                DBEntities.GetContext().SaveChanges();

                MBClass.Info("Студент успешно добавлен!");


            }
            catch (Exception ex)
            {
                throw;
                MBClass.Error(ex);
            }

        }

        private void BackBT_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ListStudentPage());

            VariableClass.AdressNewCreated = true;
            VariableClass.AddAdressIiUsing = false;

            VariableClass.AdreesWasEdit = true;
            VariableClass.EditAdreesIsUsing = false;
        }

        private void SNPstudentTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            string[] SplitSNP = SNPstudentTB.Text.Split(' ');
            int strlen = SNPstudentTB.Text.Length;

            if (SplitSNP.Length > 3)
            {
                SNPstudentTB.Text = SNPstudentTB.Text.Remove(strlen - 1);
                SNPstudentTB.CaretIndex = strlen;
            }
            EnableButton();
  

        }


        //----------------ТЕЛЕФОН --------------------------------------------------
        private void PhoneNum_TextChanged(object sender, TextChangedEventArgs e)
        {

            TextBox PhoneNumText = PhoneNumTB;

            try
            {
                if (!Keyboard.IsKeyDown(Key.Back))
                {
                    switch (PhoneNumText.Text.Length)
                    {
                        case 7:
                            PhoneNumText.Text = PhoneNumText.Text.Insert(6, "-");
                            PhoneNumText.CaretIndex = 8;
                            break;
                        case 11:
                            PhoneNumText.Text = PhoneNumText.Text.Insert(10, "-");
                            PhoneNumText.CaretIndex = 12;
                            break;
                        case 14:
                            PhoneNumText.Text = PhoneNumText.Text.Insert(13, "-");
                            PhoneNumText.CaretIndex = 15;
                            break;

                        default:
                            break;
                    }
                }

                else
                {
                    if (PhoneNumText.Text.Length < 4)
                    {
                        PhoneNumText.Text = "+7 ";
                        PhoneNumText.CaretIndex = 4;
                    }
                    switch (PhoneNumText.Text.Length)
                    {
                        case 7:
                            PhoneNumText.Text =
                                PhoneNumText.Text.Remove(PhoneNumText.Text.LastIndexOf("-"));
                            PhoneNumText.CaretIndex = 6;
                            break;
                        case 11:
                            PhoneNumText.Text =
                                PhoneNumText.Text.Remove(PhoneNumText.Text.LastIndexOf("-"));
                            PhoneNumText.CaretIndex = 10;
                            break;
                        case 14:
                            PhoneNumText.Text =
                                PhoneNumText.Text.Remove(PhoneNumText.Text.LastIndexOf("-"));
                            PhoneNumText.CaretIndex = 13;
                            break;

                        default:
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

        private void PhoneNumTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            OnlyNums(sender, e);
        }

        //-----------------------------------------------------------------------------------------

        private void Email_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableButton();
        }

        //-----------------ПАСПОРТ ТЕКСТ БОКС--------------------------------------------------

        private void PassportNum_TextChanged(object sender, TextChangedEventArgs e)
        {

            TextBox PassportText = PassportNumTB;


            try
            {
                if (!Keyboard.IsKeyDown(Key.Back))
                {
                    switch (PassportText.Text.Length)
                    {
                        case 3:
                            PassportText.Text = PassportText.Text.Insert(2, "-");
                            PassportText.CaretIndex = 4;
                            break;
                        case 6:
                            PassportText.Text = PassportText.Text.Insert(5, " ");
                            PassportText.CaretIndex = 7;
                            break;
                    }
                }

                else
                {
                    switch (PassportText.Text.Length)
                    {
                        case 4:
                            PassportText.Text =
                                PassportText.Text.Remove(PassportText.Text.LastIndexOf("-"));
                            PassportText.CaretIndex = 2;
                            break;
                        case 6:
                            PassportText.Text =
                                PassportText.Text.Remove(PassportText.Text.LastIndexOf(" "));
                            PassportText.CaretIndex = 4;
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

        private void PassportNumTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            OnlyNums(sender, e);
        }

        //-----------------ИИН ТЕКСТ БОКС--------------------------------------------------

        private void innTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableButton();

        }

        private void innTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            OnlyNums(sender, e);
        }

        //----------------СНИЛС ТЕКСБОКС--------------------------------------------------

        private void snilsTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox SnilsText = snilsTB;


            try
            {
                if (!Keyboard.IsKeyDown(Key.Back))
                {
                    switch (SnilsText.Text.Length)
                    {
                        case 4:
                            SnilsText.Text = SnilsText.Text.Insert(3, "-");
                            SnilsText.CaretIndex = 5;
                            break;
                        case 8:
                            SnilsText.Text = SnilsText.Text.Insert(7, "-");
                            SnilsText.CaretIndex = 9;
                            break;
                        case 12:
                            SnilsText.Text = SnilsText.Text.Insert(11, " ");
                            SnilsText.CaretIndex = 13;
                            break;
                    }
                }

                else
                {
                    switch (SnilsText.Text.Length)
                    {
                        case 4:
                            SnilsText.Text =
                                SnilsText.Text.Remove(SnilsText.Text.LastIndexOf("-"));
                            SnilsText.CaretIndex = 3;
                            break;
                        case 8:
                            SnilsText.Text =
                                SnilsText.Text.Remove(SnilsText.Text.LastIndexOf("-"));
                            SnilsText.CaretIndex = 7;
                            break;
                        case 12:
                            SnilsText.Text =
                                SnilsText.Text.Remove(SnilsText.Text.LastIndexOf(" "));
                            SnilsText.CaretIndex = 11;
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

        private void snilsTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            OnlyNums(sender, e);
        }

        //----------------ОМС ТЕКСБОКС--------------------------------------------------

        private void omsTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableButton();
        }

        private void omsTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            OnlyNums(sender, e);
        }

        //------------------------------------------------------------------------
        private void DateReciptTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableButton();
        }

        private void DateReciptTB_TextChanged(object sender, SelectionChangedEventArgs e)
        {
            EnableButton();
        }

        private void PlaceIssueOfPassportCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnableButton();
        }

        private void PlaceOfBirthCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnableButton();
        }

        private void PlaceOfRegistrationCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            EnableButton();
        }

        private void StatusCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnableButton();
        }


        //------Кем выдан паспорт---------------------------------
        private void AddPlaceOfIssuePassportTB_Click(object sender, RoutedEventArgs e)
        {
            new AddPlaceOfIssuePassportWindow().ShowDialog();

            if (VariableClass.pioPassportNewCreated == true)
            {

                PlaceIssueOfPassportCB.ItemsSource = DBEntities.GetContext().PlaceIssueOfPassport.ToList();

                PlaceIssueOfPassportCB.SelectedIndex = PlaceIssueOfPassportCB.Items.Count-1;

                VariableClass.pioPassportNewCreated = false;
            }
            Focus();
        }

        private void PlaceIssueOfPassportCB_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(PlaceIssueOfPassportCB.SelectedValue == null)
            {
                return;
            }
            else          
            {

                PlaceIssueOfPassport pioPassport = PlaceIssueOfPassportCB.SelectedItem as PlaceIssueOfPassport;
                VariableClass.pioPassoprtID = pioPassport.PlaceIssueOfPassportID;

                new EditPlaceOfIssuePassportWindow(PlaceIssueOfPassportCB.SelectedItem as PlaceIssueOfPassport,true).ShowDialog();

                PlaceIssueOfPassportCB.ItemsSource = DBEntities.GetContext().PlaceIssueOfPassport.ToList();
                PlaceIssueOfPassportCB.SelectedValue = VariableClass.pioPassoprtID;
                Focus();
            }
        }

        //------Место рождения----------------------------------
        private void AddPlaceOfBirthBT_Click(object sender, RoutedEventArgs e)
        {
            new AddCityWindow().ShowDialog();

            if (VariableClass.CityNewCreated == true)
            {

                PlaceOfBirthCB.ItemsSource = DBEntities.GetContext().City.ToList().OrderBy(u => u.CityID);

                PlaceOfBirthCB.SelectedIndex = PlaceOfBirthCB.Items.Count-1;
                VariableClass.CityNewCreated = false;

                PlaceOfBirthCB.ItemsSource = DBEntities.GetContext().City.ToList();
            }
            Focus();
        }

        private void PlaceOfBirthCB_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (PlaceOfBirthCB.SelectedValue == null)
            {
                return;
            }
            else
            {



                City city = PlaceOfBirthCB.SelectedItem as City;
                VariableClass.CityID = city.CityID;

                new EditCityWindow(PlaceOfBirthCB.SelectedItem as City,true).ShowDialog();

                PlaceOfBirthCB.ItemsSource = DBEntities.GetContext().City.ToList();
                PlaceOfBirthCB.SelectedValue = VariableClass.CityID;

                Focus();
            }
        }

        //------Место регистрации----------------------------------

        private async void AddPlaceOfRegisrtation_Click(object sender, RoutedEventArgs e)
        {

            if (VariableClass.AddAdressIiUsing == false)
            {
                addAdressPage = new AddAdressPage();
                NavigationService.Navigate(addAdressPage);

                UntileCreated();
                VariableClass.AddAdressIiUsing = true;

            }
            else
            {
                NavigationService.Navigate(addAdressPage);
            }
        }
        private async void PlaceOfRegistrationCB_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (PlaceOfRegistrationCB.SelectedValue == null)
            {
                return;
            }
            else
            {
                if (VariableClass.EditAdreesIsUsing == false)
                {

                    Adress adress = PlaceOfRegistrationCB.SelectedItem as Adress;
                    VariableClass.AdressID = adress.AdressID;

                    UntilEdited();
                    NavigationService.Navigate(editAdressPage = new EditAdressPage(PlaceOfRegistrationCB.SelectedItem as Adress,true));

                }
                else
                {
                    NavigationService.Navigate(editAdressPage);
                }

                Focus();
            }

        }

        //------Группа----------------------------------------------

      
        private void AddGroupBT_Click(object sender, RoutedEventArgs e)
        {

            new AddGroupWindow().ShowDialog();


            if (VariableClass.GroupNewCreated == true)
            {
                GroupCB.ItemsSource = DBEntities.GetContext().Group.ToList().OrderBy(u => u.GroupID);
                GroupCB.SelectedIndex = GroupCB.Items.Count - 1;

                VariableClass.GroupNewCreated = false;

                GroupCB.ItemsSource = DBEntities.GetContext().Group.ToList();
            }
            Focus();

        }

        private void GroupCB_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (GroupCB.SelectedValue == null)
            {
                return;
            }
            else
            {

                Group group = GroupCB.SelectedItem as Group;
                VariableClass.GroupID = group.GroupID;

                new EditGroupWindow(GroupCB.SelectedItem as Group,true).ShowDialog();

                GroupCB.ItemsSource = DBEntities.GetContext().Group.ToList();
                GroupCB.SelectedValue = VariableClass.GroupID;
                Focus();
            }
        }

        //------Cтатус----------------------------------------------

        private void AddStatusCB_Click(object sender, RoutedEventArgs e)
        {
            new AddStatusWindow().ShowDialog();

            if (VariableClass.StatusNewCreated == true)
            {
                StatusCB.ItemsSource = DBEntities.GetContext().Status.ToList().OrderBy(u => u.StatusID);

                StatusCB.SelectedIndex = StatusCB.Items.Count - 1;
                VariableClass.StatusNewCreated = false;

                StatusCB.ItemsSource = DBEntities.GetContext().Status.ToList();
            }
            Focus();
        }

        private void StatusCB_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (StatusCB.SelectedValue == null)
            {
                return;
            }
            else
            {

                Status status = StatusCB.SelectedItem as Status;
                VariableClass.StatusID = status.StatusID;

                new EditStatusWIndow(StatusCB.SelectedItem as Status,true).ShowDialog();

                StatusCB.ItemsSource = DBEntities.GetContext().Status.ToList();
                StatusCB.SelectedValue = VariableClass.StatusID;

            }
            Focus();
        }





        async Task UntileCreated()
        {

            while (true)
            {
                if (VariableClass.AdressNewCreated == true)
                {

                    PlaceOfRegistrationCB.ItemsSource = DBEntities.GetContext().Adress.ToList();

                    PlaceOfRegistrationCB.SelectedIndex = PlaceOfRegistrationCB.Items.Count-1;

                    VariableClass.AdressNewCreated = false;


                    break;
                }

                await Task.Delay(1);
            }

        }

        async Task UntilEdited()
        {

            while (true)
            {
                if (VariableClass.AdreesWasEdit == true)
                {

                    PlaceOfRegistrationCB.ItemsSource = DBEntities.GetContext().Adress.ToList();

                    PlaceOfRegistrationCB.SelectedValue = VariableClass.AdressID;

                    VariableClass.AdreesWasEdit = false;


                    break;
                }

                await Task.Delay(1);
            }
        }







        private void OnlyNums(object sender, TextCompositionEventArgs e)
        {

            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }


        private void EnableButton()
        {
            string[] SplitSNP = SNPstudentTB.Text.Split(' ');

            if (string.IsNullOrWhiteSpace(PhoneNumTB.Text) ||
                string.IsNullOrWhiteSpace(PhoneNumTB.Text) ||
                string.IsNullOrWhiteSpace(EmailTB.Text) ||
                string.IsNullOrWhiteSpace(PassportNumTB.Text) ||
                string.IsNullOrWhiteSpace(innTB.Text) ||
                string.IsNullOrWhiteSpace(snilsTB.Text) ||
                string.IsNullOrWhiteSpace(omsTB.Text) ||
                DateReciptTB.SelectedDate == null ||
                DateOfBirthTB.SelectedDate == null ||
                PlaceIssueOfPassportCB.SelectedValue == null ||
                PlaceOfBirthCB.SelectedValue == null ||
                PlaceOfRegistrationCB.SelectedValue == null ||
                StatusCB.SelectedValue == null ||
                (SplitSNP.Length == 1 ||
                 SplitSNP.Length == 2 && SplitSNP[1] == "" ||
                 SplitSNP.Length == 3 && SplitSNP[2] == ""))

            {
                AddStudentBT.IsEnabled = false;

            }
            else
            {


                if (PhoneNumTB.Text.Length == 16 &
                    PassportNumTB.Text.Length == 12 &
                    innTB.Text.Length == 12 &
                    snilsTB.Text.Length == 14 &
                    omsTB.Text.Length == 19)
                {
                    AddStudentBT.IsEnabled = true;

                }
                else
                {
                    AddStudentBT.IsEnabled = false;
                }

            }

        }

        private void AddPhoto()
        {
            openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png *.jpeg)|*.png;*.jpeg";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);


            Student student = new Student();
            if (openFileDialog.ShowDialog() == true)
            {
                selectedFileName = openFileDialog.FileName;
                student.Image = LoadAndReadImage.ConvertImageToByteArray(selectedFileName);
                PhotoIB.ImageSource = LoadAndReadImage.ConvertByteArrayImage(student.Image);

            }
        }

        private void DateOfBirthTB_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            EnableButton();
        }
    }
}