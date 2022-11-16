using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace FileObcerver
{
    static class Obcerver
    {
        public static List<string> path = new List<string>(); //путь, пройденный в проводнике
        public static List<int> DriversInfo() //вывод всей инфы о дисках
        {
            int index = 3;
            List<int> drivers = new List<int>();
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            Console.SetCursorPosition(0, 3);
            foreach (DriveInfo d in allDrives)
            {
                Console.WriteLine("  Диск {0}", d.Name);
                Console.WriteLine("    Тип диска: {0}", d.DriveType);
                if (d.IsReady == true)
                {
                    Console.WriteLine("    Название диска: {0}", d.VolumeLabel);
                    Console.WriteLine("    Файловая система: {0}", d.DriveFormat);
                    Console.WriteLine("    Доступное пространство на диске: {0, 5} Гб",d.AvailableFreeSpace/1073741824);
                    Console.WriteLine("    Общее доступное пространство   : {0, 5} Гб", d.TotalFreeSpace/1073741824);
                    Console.WriteLine("    Все пространство на диске      : {0, 5} Гб ", d.TotalSize/1073741824);
                    drivers.Add(index);
                    index = index + 7; //Запись в лист координат для указателя
                }
                else
                {
                    drivers.Add(index);
                    index = index + 2; //Запись в лист координат для указателя
                }
            }
            return drivers;
        }

        public static void selectDrive(int index) //Добавление выбранного диска в путь
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            path.Insert(0,allDrives[index].Name);
        }

        public static List<int> obcerveDirectory() //Вывод содержимого выбранной папки
        {
            string[] allFiles = Directory.GetFileSystemEntries(Obcerver.path[Obcerver.path.Count - 1]);
            int i = 3;
            foreach (string file in allFiles)
            {
                string[] path = file.Split("/");
                Console.SetCursorPosition(3, i);
                Console.WriteLine(path[path.Length - 1]);
                i++;
            }

            List<int> positions = new List<int>(); //Список координат для указателя
            int j = 0;
            while (j<=allFiles.Length)
            {
                positions.Add(j+3);
                j++;
            }
            return positions;
        }

        public static void selectDirectory(int index) //Добавление выбранной папки в путь
        {
            string[] allFiles = Directory.GetFileSystemEntries(Obcerver.path[Obcerver.path.Count-1]);
            string file = allFiles[index];
            string[] path = file.Split("/");
            Obcerver.path.Add(path[path.Length - 1]);
        }

        public static void CreateDirectory(string name) //Создание папки
        {
            Directory.CreateDirectory(Obcerver.path[Obcerver.path.Count - 1]+name);
        }

        public static void Delete() //Удаление элемента
        {
            if (File.Exists(Obcerver.path[Obcerver.path.Count - 1])) //Если файл, то просто удаляем
            {
                File.Delete(Obcerver.path[Obcerver.path.Count - 1]);
            }
            else //Если папка, то удаляем все файлы, а только потом папку (нужно добавить функцию рекурсивного удаления папок)
            {
                string[] allFiles = Directory.GetFileSystemEntries(Obcerver.path[Obcerver.path.Count - 1]);
                
                foreach (string file in allFiles)
                {
                    if (File.Exists(file))
                    {
                         File.Delete(file);
                    }
                }
                Directory.Delete(Obcerver.path[Obcerver.path.Count - 1]);
                
            }
            Obcerver.path.RemoveAt(Obcerver.path.Count - 1);
        }

        public static void CreateFile(string name) //Создание файла
        {
            File.Create(Obcerver.path[Obcerver.path.Count - 1] + name);
        }
    }
}
