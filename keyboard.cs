using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileObcerver
{
    internal class keyboard
    {
        public delegate void KeyHandler(ConsoleKey consoleKey); //Создание делегата и подвязка события
        public event KeyHandler OnChange;
        public void keyControl()
        {
            ConsoleKey consoleKey;

            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                consoleKey = keyInfo.Key;

                OnChange?.Invoke(consoleKey); //Событие на каждое нажатие кнопки
            }
        }
    }
}
