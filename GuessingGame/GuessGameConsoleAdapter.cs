using System.Diagnostics.Contracts;
using System;
using MyDesktopApp.Core;

namespace MyDesktopApp.ConsoleAdapter
{
    /// <summary>
    /// Класс <see cref="ConsoleGuessingGame"/> реализует консольную версию игры "Угадай результат".
    /// Позволяет пользователю угадывать результат вычисления двух чисел.
    /// </summary>
    public class ConsoleGuessingGame
    {
        /// <summary>
        /// Служба для обработки логики игры.
        /// </summary>
        private readonly GuessingGameService _gameService;

        /// <summary>
        /// Конструктор класса <see cref="ConsoleGuessingGame"/>.
        /// Инициализирует службу игры.
        /// </summary>
        public ConsoleGuessingGame()
        {
            // Инициализация службы игры
            _gameService = new GuessingGameService();
        }

        /// <summary>
        /// Метод для запуска игры.
        /// Запрашивает у пользователя числа A и B, затем позволяет угадывать результат вычисления.
        /// </summary>
        public void Play()
        {
            Console.WriteLine("--- Угадай результат ---"); // Заголовок игры

            // Чтение чисел A и B от пользователя
            double numberA = ReadDoubleInput("Введите число A:");
            double numberB = ReadDoubleInput("Введите число B:");

            // Вычисление правильного ответа
            double correctAnswer = _gameService.ComputeAnswer(numberA, numberB);

            // Получение максимального количества попыток
            int maxAttempts;
            do
            {
                maxAttempts = _gameService.GetMaxAttempts();
                if (maxAttempts < 1)
                {
                    Console.WriteLine("Количество попыток должно быть положительным. Пожалуйста, введите корректное значение.");
                    // Здесь можно добавить логику для установки нового значения, если это необходимо
                }
            } while (maxAttempts < 1);

            bool isCorrect = false; // Флаг для отслеживания правильного ответа

            // Информирование игрока о количестве попыток
            Console.WriteLine($"Угадайте результат вычисления. У вас есть {maxAttempts} попытки.");

            // Цикл для обработки попыток угадывания
            for (int attempt = 1; attempt <= maxAttempts && !isCorrect; attempt++)
            {
                // Чтение предположения пользователя
                double userGuess = ReadDoubleInput($"Попытка {attempt}: Введите ваш ответ:");

                // Проверка, является ли предположение пользователя правильным
                var (isCorrectResult, message) = _gameService.IsCorrectGuess(userGuess);
                Console.WriteLine(message); // Вывод сообщения о результате проверки

                // Если предположение правильное
                if (isCorrectResult)
                {
                    Console.WriteLine($"Поздравляем! Вы угадали правильный ответ: {correctAnswer}");
                    isCorrect = true; // Установка флага правильного ответа
                }
                else
                {
                    // Если предположение неверное, информируем пользователя о количестве оставшихся попыток
                    Console.WriteLine($"Неверно. Осталось попыток: {maxAttempts - attempt}");
                }
            }

            // Если игрок не угадал за отведенное количество попыток
            if (!isCorrect)
            {
                Console.WriteLine($"Вы проиграли! Правильный ответ: {correctAnswer}");
            }
        }

        /// <summary>
        /// Метод для чтения числа с плавающей точкой от пользователя.
        /// Запрашивает ввод и повторяет запрос до тех пор, пока не будет введено корректное число.
        /// </summary>
        /// <param name="prompt">Подсказка для пользователя.</param>
        /// <returns>Корректно введенное число с плавающей точкой.</returns>
        private double ReadDoubleInput(string prompt)
        {
            double result = 0.0; // Переменная для хранения результата
            bool valid = false; // Флаг для определения корректности ввода

            // Цикл до тех пор, пока пользователь не введет корректное число
            while (!valid)
            {
                Console.WriteLine(prompt); // Вывод подсказки
                string input = Console.ReadLine()?.Trim() ?? ""; // Чтение ввода пользователя

                try
                {
                    // Попытка преобразовать ввод в число с плавающей точкой
                    result = double.Parse(input);
                    valid = true; // Установка флага в true, если ввод корректный
                }
                catch
                {
                    // Обработка ошибок преобразования
                    Console.WriteLine("Ошибка! Введите число с плавающей точкой.");
                }
            }

            return result; // Возвращаем корректно введенное число
        }
    }
}