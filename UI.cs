using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileObcerver
{
    internal class UI
    {
        private int index = 0; //Номер положения указателя
        private int selectedIndex = 0; //Номер выбранного элемента
        List<int> positions = new List<int> {3}; //Список положений указателя
        static private int step = 0; //Колличество шагов, пройденных в проводнике
        private string top = ""; //Название выбранного элемента


        keyboard keyboard = new keyboard(); //Создается экземпляр модели клавиатуры
        
        public string[] description= new string[5]{"F1 - создать файл", "F2 - Создать папку", "F3 - Удалить", "Enter - выбрать", "Space - перейти"};
        //И описание фунционала меню
        public void drawGrid() //Отрисовка сетки интерфейса
        {
            Console.Clear();
            Console.CursorVisible = false;
            Console.SetCursorPosition(0, 1);
            for(int i = 0; i < Console.BufferWidth; i++)
            {
                Console.Write("-");
            }
            for (int i = 2; i < 17 ; i++)
            {
                Console.SetCursorPosition(Console.BufferWidth / 4 * 3, i);
                Console.Write("|");
            }
            Console.SetCursorPosition(Console.BufferWidth / 4 * 3+1, 7);
            for (int i = Console.BufferWidth / 4 * 3 +1; i < Console.BufferWidth; i++)
            {
                Console.Write("-");
            }
            Console.SetCursorPosition(Console.BufferWidth / 4 * 3 + 1, 2);
            for(int i =0; i<description.Length; i++)
            {
                Console.SetCursorPosition(Console.BufferWidth / 4 * 3 + 1, 2+i);
                Console.WriteLine(description[i]);
            }
            Arrow(0); //Запуск указателя
        }

        public void menu()
        {
            keyboard.OnChange += Detector; //Подписываемся на событие с нажатием клавиши
            new Thread(keyboard.keyControl).Start(); //Запускаем уловитель события в поток
            drawGrid(); //Вывод интерфейса
            positions = Obcerver.DriversInfo(); //Выводим диски (по умолчанию)
        }

        public void Detector(ConsoleKey consoleKey) //Обработчик нажатий
        {

            switch (consoleKey)
            {
                case ConsoleKey.UpArrow://Перемещение указателя на 1 вверх
                    Arrow(-1);
                    break;
                case ConsoleKey.DownArrow://Перемещение указателя на 1 вниз
                    Arrow(1);
                    break;
                case ConsoleKey.Enter: //Выбор элемента
                    selectedIndex = index;
                    if (step == 0) //Если шаг 0, значит работаем с дисками
                    {
                        Obcerver.selectDrive(selectedIndex);
                    }
                    else //работаем с папками
                    {
                        Obcerver.selectDirectory(selectedIndex);
                    }
                    Console.SetCursorPosition(Console.BufferWidth / 2 - top.Length / 2, 0);
                    for (int i = 0; i <= top.Length + 1; i++)
                    {
                        Console.Write(" ");
                    }
                    top = Obcerver.path[step];
                    Top(top); //Вывод выбранного
                    break;
                case ConsoleKey.Spacebar: //Переход к элементу
                    if (File.Exists(Obcerver.path[Obcerver.path.Count - 1])) //Если файл, то запуск через PowerShell
                    {
                        Process.Start(new ProcessStartInfo { FileName = Obcerver.path[Obcerver.path.Count - 1], UseShellExecute = true });
                    }
                    else //Если папка
                    {
                        step++; //Пройден новый шаг
                        index = 0; //Указатель в положение по умолчанию
                        drawGrid(); 
                        positions = Obcerver.obcerveDirectory(); //Вывод содержимого выбранной папки
                        
                    }
                    break;

                case ConsoleKey.Escape:
                    index = 0;
                    step--; //Откатываем шаг
                    
                    drawGrid();
                    if (step < 0)
                    {
                        Process.GetCurrentProcess().Kill();
                    }
                    else if(step == 0)
                    {
                        positions = Obcerver.DriversInfo();
                        
                    }
                    else
                    {
                        Obcerver.path.RemoveAt(Obcerver.path.Count - 1);
                        drawGrid();
                        positions = Obcerver.obcerveDirectory();
                    }
                    break;
                case ConsoleKey.F2:
                    Console.SetCursorPosition(Console.BufferWidth / 4 * 3 + 2, 9); 
                    string name = Console.ReadLine();//Считываем название папки
                    Obcerver.CreateDirectory(name);
                    drawGrid();//Обновление меню
                    break;
                case ConsoleKey.F3:
                    Obcerver.Delete();
                    drawGrid();//Обновление меню
                    positions = Obcerver.obcerveDirectory();
                    break;
                case ConsoleKey.F1:
                    Console.SetCursorPosition(Console.BufferWidth / 4 * 3 + 2, 9);
                    name = Console.ReadLine();//Считываем название файла
                    Obcerver.CreateFile(name);
                    drawGrid();//Обновляем меню
                    positions = Obcerver.obcerveDirectory();
                    break;

            }

        }

        private void Arrow(int direction)
        {
            Console.SetCursorPosition(1, positions[index]);
            Console.Write(" "); //удаляем старый индикатор
            index = index + direction;
            if(index<positions.Count && index >= 0) //Проверяем новый на выход за предел значений
            {
                Console.SetCursorPosition(1, positions[index]);
                Console.Write(">"); //рисуем
            }
            else
            {
                index = 0; //индикатор в положение по умолчанию
            }
        }

        private void Top(string top) //Вывод выбранного элемента
        {
            Console.SetCursorPosition(Console.BufferWidth / 2 - top.Length / 2, 0);
            Console.Write(top);
        }

    }
}
