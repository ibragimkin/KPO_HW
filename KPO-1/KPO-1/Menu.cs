namespace KPO_1
{
    internal class Menu
    {
        /// <summary>
        ///  Ищет длину самой длинной строки в массиве.
        /// </summary>
        /// <param name="strings">Массив строк для поиска.</param>
        /// <returns>Максимальная длина.</returns>
        private static int MaxLen(string[] strings)
        {
            int len = 0;
            foreach (string text in strings)
            {
                len = Math.Max(len, text.Length);
            }
            return len;
        }

        /// <summary>
        /// Строит консольное меню. Изначально курсор стоит на первом элементе.
        /// </summary>
        /// <param name="startMessage"> Начальное сообщение. Его невозможно выделить. </param>
        /// <param name="menuText">Массив, имеющий варианты меню.</param>
        /// <returns>Выбранный пользователем вариант.</returns>
        public static int ConsoleMenu(string startMessage, string[] menuText)
        {
            Console.Clear();
            if (menuText.Length == 0) 
            {
                Console.WriteLine("Пустой список.");
                Console.ReadKey();
                return 0;

            }
            Console.WriteLine(startMessage);
            int[] positions = { 0, 0 };
            // [0] - позиция пользователя.
            // [1] - позиция, откуда начинаются возможные варианты.
            positions[1] = Console.CursorTop;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("->" + menuText[0]);
            Console.ForegroundColor = ConsoleColor.White;
            for (int i = 1; i < menuText.Length; i++)
            {
                Console.WriteLine("  " + menuText[i]);
            }
            Console.CursorTop = positions[1];
            Console.CursorVisible = false;
            return UserNavigation(menuText, positions);
        }

        /// <summary>
        /// Обновляет позицию курсора. Прошлую позицию красит в стандартный цвет. Новую выбранную красит в зелёный.
        /// </summary>
        /// <param name="menuText">Массив, имеющий варианты меню. Нужен для обновления цветов и стрелки, указывающих на то, что выбран конкретный вариант.</param>
        /// <param name="positions">Массив, содержащий 0 элементом позицию курсора и 1 элементом первую позицию, где нет начального текста.</param>
        /// <param name="maxTextLen">Длина максимально длинного текста в menuText. Нужен для защиты от ошибки.</param>
        private static void PositionUpdate(string[] menuText, int[] positions, int maxTextLen)
        {
            try
            {
                Console.WindowWidth = Math.Max(Console.WindowWidth, maxTextLen);
            }
            catch { }
            // Защищает от возможного исключения при слишком узком окне. В trycatch на всякий случай, т.к. Visual Studio жалуется, что поддерживается только на Windows.
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write("  " + menuText[Console.CursorTop - positions[1]]);
            Console.SetCursorPosition(0, positions[0] + positions[1]);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("->" + menuText[positions[0]]);
            Console.ForegroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// Отвечает за навигацию по меню. Можно перемещаться, пока не будет нажат Enter.
        /// </summary>
        /// <param name="positions">Массив, содержащий 0 элементом позицию курсора и 1 элементом первую позицию, где нет начального текста.</param>
        /// <param name="menuText">Массив, имеющий варианты меню.</param>
        /// <returns>Индекс выбранного пользователем варианта.</returns>
        private static int UserNavigation(string[] menuText, int[] positions)
        {
            bool Entered = false;
            int maxTextLen = MaxLen(menuText) + 3;
            while (!Entered)
            {
                while (true)
                {
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.UpArrow or ConsoleKey.W:
                            if (positions[0] == 0)
                            {
                                positions[0] = menuText.Length - 1;
                                break;
                            }
                            positions[0]--;
                            break;
                        case ConsoleKey.DownArrow or ConsoleKey.S:
                            positions[0] = (positions[0] + 1) % menuText.Length;
                            break;
                        case ConsoleKey.Enter or ConsoleKey.Spacebar:
                            Entered = true;
                            break;
                    }
                    PositionUpdate(menuText, positions, maxTextLen);
                    break;
                }
            }
            Console.Clear();
            return positions[0];
        }
    }
}
