using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace StudentDepartmentsGonA.D.ClassFolder
{
    class LoadAndReadImage
    {
        /// <summary>
        /// Метод для конвертирования из БД в Image на окне
        /// </summary>
        /// <param name="array">поле из БД</param>
        /// <returns>Изображение</returns>
        public static BitmapImage ConvertByteArrayImage(byte[] array)
        {
            //если байтовый массив не пустой
            if (array != null)
            {
                //записываем палученные данные в массив
                //для дальнейшего преобразования
                using (var ms = new MemoryStream(array, 0, array.Length))
                {
                    //инициализируем новый экземпляр класса BitmapImage
                    //который существует для поддержки синтаксиса xaml
                    //и предоставляет дополнительные свойства для загрузки
                    //битовой карты
                    var image = new BitmapImage();

                    //создаем сигнал о начале инициализации объекта BitmapImage
                    image.BeginInit();

                    //получем опцию для использования данных
                    //BitmapCacheOption.OnLoad кэширует все изображения
                    //из хранилища памяти
                    image.CacheOption = BitmapCacheOption.OnLoad;

                    //получаем исходный поток BitmapImage
                    image.StreamSource = ms;

                    //создаем сигнал о завершении инициализации объекта
                    //BitmapImage
                    image.EndInit();

                    return image;
                }
            }
            return null;
        }

        /// <summary>
        /// Метод для конвертирования из Image в массив байтов для БД
        /// </summary>
        /// <param name="fileName">Строка из диалогового окна</param>
        /// <returns>Массив байтов</returns>
        public static byte[] ConvertImageToByteArray(string fileName)
        {
            Bitmap bitmap = new Bitmap(fileName);
            ImageFormat imageFormat = bitmap.RawFormat;

            var imageToConvert = Image.FromFile(fileName);

            using (var ms = new MemoryStream())
            {
                imageToConvert.Save(ms, imageFormat);
                return ms.ToArray();
            }

        }
    }
}
