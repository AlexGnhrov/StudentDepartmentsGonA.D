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

namespace StudentDepartmentsGonA.D.PageFolder.EmployeePage.StuentList
{
    /// <summary>
    /// Логика взаимодействия для EditStudentPage.xaml
    /// </summary>
    public partial class EditStudentPage : Page
    {
        AddAdressPage addAdressPage;
        EditAdressPage editAdressPage;


        OpenFileDialog openFileDialog;

        Student student = new Student();

        bool IntilizationEnd = false;
        string oldINN, oldSNIL, oldOMS;
        public EditStudentPage(Student student)
        {


            InitializeComponent();

            DataContext = student;

            PlaceIssueOfPassportCB.ItemsSource = DBEntities.GetContext().PlaceIssueOfPassport.ToList();
            PlaceOfBirthCB.ItemsSource = DBEntities.GetContext().City.ToList();
            PlaceOfRegistrationCB.ItemsSource = DBEntities.GetContext().Adress.ToList();
            GroupCB.ItemsSource = DBEntities.GetContext().Group.ToList();
            StatusCB.ItemsSource = DBEntities.GetContext().Status.ToList();

            PhotoIB.ImageSource = LoadAndReadImage.ConvertByteArrayImage(student.Image);

            VariableClass.AdressNewCreated = false;
            VariableClass.AddAdressIiUsing = false;

            saveInn();
            saveSnil();
            saveOms();

            correctSNP();

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
        private void EditStudentBT_Click(object sender, RoutedEventArgs e)
        {


            var snil = DBEntities.GetContext().Student.FirstOrDefault(u => u.SNILS == snilsTB.Text);
            var inn = DBEntities.GetContext().Student.FirstOrDefault(u => u.INN == innTB.Text);
            var OMS = DBEntities.GetContext().Student.FirstOrDefault(u => u.OMS == omsTB.Text);

            if (oldSNIL != snilsTB.Text)
            {
                if (snil != null)
                {
                    MBClass.Error("Данный СНИЛС уже существует");
                    snilsTB.Focus();
                    return;
                }
            }
            if (oldINN != innTB.Text)
            {
                if (inn != null)
                {
                    MBClass.Error("Данный ИИН уже существует");
                    innTB.Focus();
                    return;
                }
            }
            if (oldOMS != omsTB.Text)
            {
                if (OMS != null)
                {
                    MBClass.Error("Данный ОМС уже существует");
                    omsTB.Focus();
                    return;
                }
            }
            try
            {
                string[] SplitSNP = SNPstudentTB.Text.Split(' ');
                string[] SplitPassport = PassportNumTB.Text.Split(' ');

                string PassportSeriesNum = SplitPassport[0];
                int PassportNum = Convert.ToInt32(SplitPassport[1].ToString());



                student = DBEntities.GetContext().Student.FirstOrDefault(u => u.StudentID == VariableClass.StudentID);

                student.Surname = SplitSNP[0].ToString();
                student.Name = SplitSNP[1].ToString();
                if (SplitSNP.Length == 2)
                {
                    student.Patronymic = null;
                }
                else
                {
                    student.Patronymic = SplitSNP[2].ToString();
                }
                student.PhoneNum = PhoneNumTB.Text;
                student.Email = EmailTB.Text;
                student.PassportSeries = PassportSeriesNum;
                student.PassportNum = PassportNum;
                student.INN = innTB.Text;
                student.SNILS = snilsTB.Text;
                student.OMS = omsTB.Text;
                student.ReceiptDate = DateTime.Parse(DateReciptTB.Text);
                student.ReleaseDate = (DateTime?)DateReleaseTB.SelectedDate;
                student.DateOfBirth = DateTime.Parse(DateOfBirthTB.Text);
                student.OrderStudent = OrderTB.Text;
                student.PlaceOfBirthID = Convert.ToInt32(PlaceOfBirthCB.SelectedValue);
                student.PlaceIssueOfPassportID = Convert.ToInt32(PlaceIssueOfPassportCB.SelectedValue);
                student.PlaceOfRegistrationID = Convert.ToInt32(PlaceOfRegistrationCB.SelectedValue);
                student.GroupID = (int?)GroupCB.SelectedValue;
                student.StatusID = Convert.ToInt32(StatusCB.SelectedValue);
                if (selectedFileName != "")
                {
                    student.Image = LoadAndReadImage.ConvertImageToByteArray(selectedFileName);
                }

                DBEntities.GetContext().SaveChanges();

                MBClass.Info("Студент успешно отредактирован!");

                oldOMS = omsTB.Text;
                oldSNIL = snilsTB.Text;
                oldINN = innTB.Text;
            }
            catch (Exception ex)
            {

                MBClass.Error(ex);
            }

        }


        private void DeleteBT_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (MBClass.Question("Вы действительно хотите удалить студента?"))
                {
                    DBEntities.GetContext().Student.Remove(DataContext as Student);

                    DBEntities.GetContext().SaveChanges();

                    MBClass.Info("Студент успешно удалён!");

                    NavigationService.Navigate(new ListStudentPage());
                }
            }
            catch (Exception ex)
            {
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

        private void DateOfBirthTB_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
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

                PlaceIssueOfPassportCB.SelectedIndex = PlaceIssueOfPassportCB.Items.Count - 1;

                VariableClass.pioPassportNewCreated = false;
            }
        }

        private void PlaceIssueOfPassportCB_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (PlaceIssueOfPassportCB.SelectedValue == null)
            {
                return;
            }
            else
            {

                PlaceIssueOfPassport pioPassport = PlaceIssueOfPassportCB.SelectedItem as PlaceIssueOfPassport;
                VariableClass.pioPassoprtID = pioPassport.PlaceIssueOfPassportID;

                new EditPlaceOfIssuePassportWindow(PlaceIssueOfPassportCB.SelectedItem as PlaceIssueOfPassport, false).ShowDialog();

                PlaceIssueOfPassportCB.ItemsSource = DBEntities.GetContext().PlaceIssueOfPassport.ToList();
                PlaceIssueOfPassportCB.SelectedValue = VariableClass.pioPassoprtID;

            }
        }

        //------Место рождения----------------------------------
        private void AddPlaceOfBirthBT_Click(object sender, RoutedEventArgs e)
        {
            new AddCityWindow().ShowDialog();

            if (VariableClass.CityNewCreated == true)
            {

                PlaceOfBirthCB.ItemsSource = DBEntities.GetContext().City.ToList().OrderBy(u => u.CityID);

                PlaceOfBirthCB.SelectedIndex = PlaceOfBirthCB.Items.Count - 1;
                VariableClass.CityNewCreated = false;

                PlaceOfBirthCB.ItemsSource = DBEntities.GetContext().City.ToList();
            }
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

                new EditCityWindow(PlaceOfBirthCB.SelectedItem as City, false).ShowDialog();

                PlaceOfBirthCB.ItemsSource = DBEntities.GetContext().City.ToList();
                PlaceOfBirthCB.SelectedValue = VariableClass.CityID;

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
                    NavigationService.Navigate(editAdressPage = new EditAdressPage(PlaceOfRegistrationCB.SelectedItem as Adress, false));

                }
                else
                {
                    NavigationService.Navigate(editAdressPage);
                }


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

                new EditGroupWindow(GroupCB.SelectedItem as Group, false).ShowDialog();

                GroupCB.ItemsSource = DBEntities.GetContext().Group.ToList();
                GroupCB.SelectedValue = VariableClass.GroupID;

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

                new EditStatusWIndow(StatusCB.SelectedItem as Status, false).ShowDialog();

                StatusCB.ItemsSource = DBEntities.GetContext().Status.ToList();
                StatusCB.SelectedValue = VariableClass.StatusID;

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
                EditStudentBT.IsEnabled = false;

            }
            else
            {


                if (PhoneNumTB.Text.Length == 16 &
                    PassportNumTB.Text.Length == 12 &
                    innTB.Text.Length == 12 &
                    snilsTB.Text.Length == 14 &
                    omsTB.Text.Length == 19)
                {
                    EditStudentBT.IsEnabled = true;

                }
                else
                {
                    EditStudentBT.IsEnabled = false;
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








        async Task UntileCreated()
        {

            while (true)
            {
                if (VariableClass.AdressNewCreated == true)
                {

                    PlaceOfRegistrationCB.ItemsSource = DBEntities.GetContext().Adress.ToList();

                    PlaceOfRegistrationCB.SelectedIndex = PlaceOfRegistrationCB.Items.Count - 1;

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


        async Task saveInn()
        {
            await Task.Delay(25);
            oldINN = innTB.Text;
        }
        async Task saveSnil()
        {
            await Task.Delay(25);
            oldSNIL = snilsTB.Text;
        }


        async Task saveOms()
        {
            await Task.Delay(25);
            oldOMS = omsTB.Text;
        }

        async Task correctSNP()
        {
            await Task.Delay(25);
            string[] SplitSNP = SNPstudentTB.Text.Split(' ');
            if(SplitSNP.Length > 2 && SplitSNP[2] == "")
            {
                SNPstudentTB.Text = SplitSNP[0] + " " + SplitSNP[1];
                    
            }
        }
    }
    
}
