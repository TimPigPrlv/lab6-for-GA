using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using MyDesktopApp.Core;

namespace MyDesktopApp
{
    /// <summary>
    /// Класс <see cref="GuessingGameFormsAdapter"/> представляет адаптер для игры "Угадай результат" в форме окна.
    /// Позволяет пользователю играть в игру через графический интерфейс.
    /// </summary>
    public partial class GuessingGameFormsAdapter : Window
    {
        /// <summary>
        /// Служба для обработки логики игры.
        /// </summary>
        private readonly GuessingGameService _gameService;

        /// <summary>
        /// Конструктор класса <see cref="GuessingGameFormsAdapter"/>.
        /// Инициализирует компоненты окна и службу игры.
        /// </summary>
        public GuessingGameFormsAdapter()
        {
            InitializeComponent(); // Инициализация компонентов окна
            _gameService = new GuessingGameService(); // Инициализация службы игры
            // Установка текста метки с формулой игры
            FormulaLabel.Text = $"Формула: {_gameService.Formula}";
        }

        /// <summary>
        /// Метод обрабатывает событие нажатия кнопки "Начать".
        /// Запускает игру с заданными пользователем параметрами.
        /// </summary>
        /// <param name="sender">Объект-отправитель события.</param>
        /// <param name="e">Данные события.</param>
        private void OnStartClick(object? sender, RoutedEventArgs e)
        {
            try
            {
                // Чтение количества попыток из текстового поля, с использованием значения по умолчанию
                int attemptsNum = int.Parse(NumberOfAttemptsTextInput.Text ?? "3");
                // Чтение чисел A и B из текстовых полей, с использованием значения по умолчанию
                double numberA = double.Parse(InputA.Text ?? "0");
                double numberB = double.Parse(InputB.Text ?? "0");

                // Запуск игры с использованием службы
                _gameService.StartGame(attemptsNum, numberA, numberB);

                // Обновление пользовательского интерфейса с информацией о начале игры
                ResultLabel.Text = $"Игра началась! У вас {_gameService.GetMaxAttempts()} попытки.";
                GuessInput.Text = ""; // Очистка поля ввода для предположения
                GuessInput.IsEnabled = true; // Включение поля ввода
            }
            catch
            {
                // Обработка ошибок ввода данных
                ResultLabel.Text = "Ошибка ввода данных!";
            }
        }

        /// <summary>
        /// Метод обрабатывает событие нажатия кнопки "Проверить".
        /// Проверяет предположение пользователя и обновляет интерфейс с результатом.
        /// </summary>
        /// <param name="sender">Объект-отправитель события.</param>
        /// <param name="e">Данные события.</param>
        private void OnCheckClick(object? sender, RoutedEventArgs e)
        {
            try
            {
                // Чтение предположения пользователя из текстового поля
                double guess = double.Parse(GuessInput.Text ?? "0");

                // Проверка, является ли предположение правильным
                var (isCorrect, message) = _gameService.IsCorrectGuess(guess);
                ResultLabel.Text = message; // Вывод сообщения о результате проверки

                // Отключение ввода, если игра закончилась
                if (_gameService.AttemptsLeft == 0 || isCorrect)
                {
                    GuessInput.IsEnabled = false; // Отключение поля ввода
                }
            }
            catch
            {
                // Обработка ошибок, если пользователь ввел некорректные данные
                ResultLabel.Text = "Ошибка! Введите число.";
            }
        }

        /// <summary>
        /// Метод обрабатывает событие нажатия кнопки "Закрыть".
        /// Закрывает текущее окно.
        /// </summary>
        /// <param name="sender">Объект-отправитель события.</param>
        /// <param name="e">Данные события.</param>
        private void OnCloseClick(object? sender, RoutedEventArgs e)
        {
            Close(); // Закрытие окна
        }
    }
}