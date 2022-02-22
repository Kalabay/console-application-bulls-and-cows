using System;
using System.IO;

namespace Bulls_and_Cows
{
    class Program
    {
        /// <summary>
        /// Метод для рандомной генерации числа от l до r
        /// </summary>
        /// <param name="l">левая граница</param>
        /// <param name="r">правая граница</param>
        /// <returns>рандомное число от l до r</returns>
        public static int Rand(int l, int r)
        {
            Random rand = new Random();
            //Возвращаем число от l до r
            return l + rand.Next() % (r - l + 1);
        }

        /// <summary>
        /// Метод для генерации n-значного числа в виде строки
        /// В нём нет одинаковых цифр
        /// </summary>
        /// <param name="n">количество цифр в числе</param>
        /// <returns>ответ игры в виде строки</returns>
        public static string NumberGenerator(int n)
        {
            if (n == 1)
            {
                return Rand(0, 9).ToString();
            }
            //Used - массив проверки на то, что цифра уже использована
            bool[] used = new bool[10];
            //Итогове n-значное число
            string num = "";
            //Первая цифра ответа, далее последняя цифра, добавленная к num
            int now = Rand(1, 9);
            num += (char)('0' + now);
            used[now] = true;
            //Количесвто оставшихся вариантов
            int remainingVariants = 9;
            //На каждой итерации добавляем по цифре к ответу 
            while (num.Length != n)
            {
                //Номер рандомной цифры из неиспользованных
                int variantNum = Rand(1, remainingVariants);
                for (now = 0; now < 10; ++now)
                {
                    //Проверяем, что цифра не использовалась
                    if (!used[now])
                    {
                        variantNum -= 1;
                    }
                    //Если номер стал = 0, то мы нашли цифру
                    if (variantNum == 0)
                    {
                        break;
                    }
                }
                num += (char)('0' + now);
                used[now] = true;
                remainingVariants -= 1;
            }
            return num;
        }

        /// <summary>
        /// Метод, который просто выводит текст Readme
        /// </summary>
        public static void Readme()
        {
            Console.WriteLine(File.ReadAllText("Readme.txt"));
        }

        /// <summary>
        /// Метод для получения количества быков и коров
        /// </summary>
        /// <param name="input">попытка пользователя</param>
        /// <param name="num">загаданное число</param>
        /// <param name="n">длина загаданного числа</param>
        public static void GetBullsAndCows(string input, string num, int n)
        {
            //Количесто быков, количество коров
            int bulls = 0, cows = 0;
            //Цикл по цифрам загаданного числа
            for (int i = 0; i < n; ++i)
            {
                //Цикл по цифрам числа игрока
                for (int j = 0; j < n; ++j)
                {
                    if (num[i] == input[j])
                    {
                        //Проверка на быка
                        if (i == j)
                        {
                            bulls += 1;
                        }
                        else
                        {
                            cows += 1;
                        }
                    }
                }
            }
            //Вывод быков и коров
            Console.WriteLine("Bulls: " + bulls + " Cows: " + cows);
        }

        /// <summary>
        /// Метод для проверки попытки пользователя и получения количества быков и коров
        /// </summary>
        /// <param name="input">попытка пользователя</param>
        /// <param name="num">загаданное число</param>
        /// <param name="n">длина загаданного числа</param>
        /// <returns>true если угадал, иначе false</returns>
        public static bool CheckNumber(string input, string num, int n)
        {
            //Проверка на совпадение с ответом
            if (num == input)
            {
                return true;
            }
            //Число игрока в типе long
            long userNum;
            //Проверка попытки пользователя на корректность
            if (!long.TryParse(input, out userNum) || ((n != 1 || userNum != 0) && userNum < (long)Math.Pow(10, n - 1))
                || userNum >= (long)Math.Pow(10, n) || input.Length != num.Length)
            {
                Console.WriteLine("Некорректный ввод, повторите попытку");
                return false;
            }
            //Массив для проверки повтора цифр в input
            bool[] check = new bool[10];
            //Заполняем этот массив
            for (int i = 0; i < n; ++i)
            {
                //Цифра на i позиции
                int now = input[i] - '0';
                //Проверка на повтор
                if (check[now])
                {
                    Console.WriteLine("Некорректный ввод, повторите попытку");
                    return false;
                }
                check[now] = true;
            }
            //выводим информацию о быках и коровах
            GetBullsAndCows(input, num, n);
            return false;
        }

        /// <summary>
        /// Метод для обработки попыток игрока угадать число 
        /// </summary>
        /// <param name="num">загаданное число</param>
        /// <param name="n">длина загаданного числа</param>
        public static void UserAttempts(string num, int n)
        {
            //Информация от игрока
            string input;
            //Цикл пока игра не кончится
            while (true)
            {
                Console.WriteLine("Введите число");
                //Считываем информацию от игрока
                input = Console.ReadLine();
                //Проверка на конец игры
                if (input == "Stop")
                {
                    return;
                }
                //Сравниваем попытку игрока с ответом
                if (CheckNumber(input, num, n))
                {
                    Console.WriteLine("Поздравляю вы победили!!!");
                    return;
                }
            }
        }

        /// <summary>
        /// Метод отвечающий за одну игру
        /// </summary>
        public static void Game()
        {
            Console.WriteLine("Игра началась!");
            Console.WriteLine("Выберете уровень сложности вводом числа n от 1 до 10");
            //Считываем информацию от игрока
            string input = Console.ReadLine();
            //Уровень сложности
            int n;
            //Проверяем, что входные данные корректны
            while (!int.TryParse(input, out n) || n <= 0 || n > 10)
            {
                //Проверка на конец игры
                if (input == "Stop")
                {
                    return;
                }
                Console.WriteLine("Некорректный ввод, повторите попытку");
                //Считываем информацию от игрока
                input = Console.ReadLine();
            }
            //Генерируем ответ
            string num = NumberGenerator(n);
            Console.WriteLine("Вам загадано число, его количество цифр равняется " + n);
            Console.WriteLine("Попытайтесь его угадать");
            Console.WriteLine("Введённое вами число, если оно не равно 0, должно не содержать ведущих 0");
            Console.WriteLine("Все цифры должны быть различны, число должно быть >= 0");
            Console.WriteLine("И введённая вами попытка должна содержать ровно");
            Console.WriteLine("Столько символов, сколько содержит загаданное число");
            //Обрабатываем числа пользователя
            UserAttempts(num, n);
        }

        /// <summary>
        /// Обработка старта игры
        /// </summary>
        /// <returns>выбор пользователя в начале игры (Start или Stop)</returns>
        public static string Start()
        {
            //Информация от игрока
            string input;
            //Цикл для обработки первых запросов
            do
            {
                //Считываем информацию от игрока
                input = Console.ReadLine();
                if (input == "Readme")
                {
                    //Выводим текст инструкции
                    Readme();
                    Console.WriteLine("Введите Start, чтобы начать. Введите Stop, чтобы закончить");
                    Console.WriteLine("Введите Readme, чтобы опять посмотреть инструкцию");
                }
                else if (input != "Start" && input != "Stop")
                {
                    Console.WriteLine("Некорректный ввод, повторите попытку");
                }
                //Проверка на конец/начало игры
            } while (input != "Start" && input != "Stop");
            return input;
        }

        /// <summary>
        /// Быки и коровы
        /// </summary>
        public static void Main()
        {
            Console.WriteLine("Добро пожаловть в игру Быки и коровы!!!");
            Console.WriteLine("Чтобы начать игру напишите Start, чтобы закончить игру напишите Stop");
            Console.WriteLine("Если вы хотите посмотреть интсрукцию, то можете:");
            Console.WriteLine("Открыть файл Readme или написать Readme, но только до начала игры");
            //Старт игры
            string input = Start();
            //Цикл для повторения игр 
            while (input != "Stop")
            {
                //Проверка на некорректность
                if (input != "Start")
                {
                    Console.WriteLine("Некорректный ввод, повторите попытку");
                }
                else
                {
                    //Запуск одной игры
                    Game();
                    Console.WriteLine("Игра закончилась");
                    Console.WriteLine("Введите Start, чтобы начать новую игру");
                    Console.WriteLine("Введите Stop, чтобы перестать играть");
                }
                //Считываем информацию от игрока
                input = Console.ReadLine();
            }
        }
    }
}