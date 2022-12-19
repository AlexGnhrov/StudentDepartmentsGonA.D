using Microsoft.Win32;
using StudentDepartmentsGonA.D.ClassFolder;
using StudentDepartmentsGonA.D.DataFolder;
using StudentDepartmentsGonA.D.WindowFolder.EmployeeFolder.AdditionalWIndows.SpecialityWindow;
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
using Group = StudentDepartmentsGonA.D.DataFolder.Group;

namespace StudentDepartmentsGonA.D.PageFolder.EmployeePage.CuratorList
{
    /// <summary>
    /// Логика взаимодействия для EditCuratorPage.xaml
    /// </summary>
    public partial class EditCuratorPage : Page
    {
        OpenFileDialog openFileDialog;
        Curator curator = new Curator();

        public EditCuratorPage(Curator curator)
        {
            InitializeComponent();

            DataContext = curator;

            SpecialityCB.ItemsSource = DBEntities.GetContext().Speciality.ToList();

            PhotoIB.ImageSource = LoadAndReadImage.ConvertByteArrayImage(curator.Image);
            




        }

        string selectedFileName = "";

        private void EditCuratorBT_Click(object sender, RoutedEventArgs e)
        {
            string[] SplitSNP = SNPCuratorTB.Text.Split(' ');
            try
            {


                curator = DBEntities.GetContext().Curator.FirstOrDefault(u => u.CuratorID == VariableClass.CuratorID);

                curator.Surname = SplitSNP[0];
                curator.Name = SplitSNP[1];
                if (SplitSNP.Length == 2)
                {
                    curator.Patronymic = null;
                }
                else if (SplitSNP.Length == 3)
                {
                    curator.Patronymic = SplitSNP[2];
                }
                curator.Patronymic = null;
                curator.PhoneNum = PhoneNumTB.Text;
                curator.Email = EmailTB.Text;
                curator.SpecialityID = (int)SpecialityCB.SelectedValue;
                if (selectedFileName != "")
                {
                    curator.Image = LoadAndReadImage.ConvertImageToByteArray(selectedFileName);
                }



                DBEntities.GetContext().SaveChanges();

                MBClass.Info("Куратор успешно отредактирован!");
            }
            catch (Exception ex)
            {

                MBClass.Error(ex);
            }

        }

        private void BackBT_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ListCuratorPage());
        }

        private void Page_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {

                if (SNPCuratorTB.IsFocused)
                {
                    PhoneNumTB.Focus();
                }
                else if (PhoneNumTB.IsFocused)
                {
                    EmailTB.Focus();
                }
                else if (EmailTB.IsFocused)
                {
                    SpecialityCB.Focus();
                    SpecialityCB.IsDropDownOpen = true;
                }
                else if (SpecialityCB.IsFocused)
                {
                    if (EditCuratorBT.IsEnabled)
                    {
                        EditCuratorBT_Click(sender, e);
                    }
                }
            }
        }

        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            AddPhoto();
        }



        private void PhoneNumTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

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

        private void EmailTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableButton();
        }

        private void SpecialityCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnableButton();
        }

        private void AddSpecialityBT_Click(object sender, RoutedEventArgs e)
        {

            new AddSpecialityWindow().ShowDialog();


            if (VariableClass.SpecialityNewCreated == true)
            {
                SpecialityCB.ItemsSource = DBEntities.GetContext().Speciality.ToList().OrderBy(u => u.SpecialityID);

                SpecialityCB.SelectedIndex = SpecialityCB.Items.Count - 1;
                VariableClass.SpecialityNewCreated = false;

                SpecialityCB.ItemsSource = DBEntities.GetContext().Speciality.ToList();
            }
            Focus();
        }

        private void SpecialityCB_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SpecialityCB.SelectedValue == null)
            {
                return;
            }
            else
            {

                Speciality speciality = SpecialityCB.SelectedItem as Speciality;
                VariableClass.SpecialityID = speciality.SpecialityID;

                new EditSpecialityWindow(SpecialityCB.SelectedItem as Speciality,false).ShowDialog();

                SpecialityCB.ItemsSource = DBEntities.GetContext().Speciality.ToList();
                SpecialityCB.SelectedValue = VariableClass.SpecialityID;
                Focus();
            }
        }

        private void EnableButton()
        {
            string[] SplitSNP = SNPCuratorTB.Text.Split(' ');
            if (string.IsNullOrWhiteSpace(SNPCuratorTB.Text) ||
                string.IsNullOrWhiteSpace(PhoneNumTB.Text) ||
                SpecialityCB.SelectedValue == null ||
                 (SplitSNP.Length == 1 ||
                 SplitSNP.Length == 2 && SplitSNP[1] == "" ||
                 SplitSNP.Length == 3 && SplitSNP[2] == ""))
            {
                EditCuratorBT.IsEnabled = false;
            }
            else
            {
                EditCuratorBT.IsEnabled = true;
            }
        }


        private void AddPhoto()
        {
            openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png *.jpeg)|*.png;*.jpeg";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);


            Curator curator = new Curator();
            if (openFileDialog.ShowDialog() == true)
            {
                selectedFileName = openFileDialog.FileName;
                curator.Image = LoadAndReadImage.ConvertImageToByteArray(selectedFileName);
                PhotoIB.ImageSource = LoadAndReadImage.ConvertByteArrayImage(curator.Image);

            }
        }

        private void SNPCuratorTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            string[] SplitSNP = SNPCuratorTB.Text.Split(' ');
            int strlen = SNPCuratorTB.Text.Length;

            if (SplitSNP.Length > 3)
            {
                SNPCuratorTB.Text = SNPCuratorTB.Text.Remove(strlen - 1);
                SNPCuratorTB.CaretIndex = strlen;
            }
            EnableButton();
        }

        private void DeleteBT_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                if (MBClass.Question($"Вы действительно хотите удалить куратора?"))
                {

                    DBEntities.GetContext().SaveChanges();

                    DBEntities.GetContext().Curator.Remove(DataContext as Curator);

                    DBEntities.GetContext().SaveChanges();

                    NavigationService.Navigate(new ListCuratorPage());

                }
            }
            catch (Exception ex)
            {
                MBClass.Error(ex);
            }


        }



    }
}
