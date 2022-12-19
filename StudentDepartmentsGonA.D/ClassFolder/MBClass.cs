using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace StudentDepartmentsGonA.D.ClassFolder
{
    class MBClass
    {

        public static void Error(string text)
        {
            MessageBox.Show(text,"Ошибка",MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static void Error(Exception ex)
        {
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static void Info(string text)
        {
            MessageBox.Show(text, "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static bool Question(string text)
        {
          return MessageBoxResult.Yes ==
                MessageBox.Show(text, "Информация", MessageBoxButton.YesNo, MessageBoxImage.Information);
        }

        public static void Exit()
        {
            if (Question("Вы действительно хотите выйти?"))
            {
                App.Current.Shutdown();
            }
        }
    }
}
