using System;

namespace MyDesktopApp.Core
{
    /// <summary>
    /// Класс <see cref="GuessingGameService"/> содержит логику игры "Угадай результат".
    /// Отвечает за управление состоянием игры, вычисление правильного ответа и проверку предположений пользователя.
    /// </summary>
    public class GuessingGameService
    {
        private double _correctAnswer; // Переменная для хранения правильного ответа
        private int _maxAttempts; // Максимальное количество попыток

        /// <summary>
        /// Свойство, представляющее формулу, используемую в игре.
        /// </summary>
        public string Formula => "5 * π * log(B) / (sin(A) + 1)";
        
        /// <summary>
        /// Свойство, указывающее количество оставшихся попыток.
        /// </summary>
        public int AttemptsLeft { get; private set; }

        /// <summary>
        /// Конструктор класса <see cref="GuessingGameService"/>.
        /// Сбрасывает состояние игры при инициализации.
        /// </summary>
        public GuessingGameService()
        {
            ResetGame(); // Сброс игры при инициализации
        }

        /// <summary>
        /// Метод для сброса состояния игры.
        /// Устанавливает количество оставшихся попыток в максимальное значение и сбрасывает правильный ответ.
        /// </summary>
        public void ResetGame()
        {
            AttemptsLeft = _maxAttempts; // Установка оставшихся попыток в максимальное значение
            _correctAnswer = 0; // Сброс правильного ответа
        }

        /// <summary>
        /// Метод для начала новой игры.
        /// Устанавливает максимальное количество попыток и вычисляет правильный ответ на основе входных данных.
        /// </summary>
        /// <param name="maxAttempts">Максимальное количество попыток.</param>
        /// <param name="a">Первое число для вычисления.</param>
        /// <param name="b">Второе число для вычисления.</param>
        /// <exception cref="ArgumentException">Выбрасывается, если количество попыток отрицательное.</exception>
        public void StartGame(int maxAttempts, double a, double b)
        {
            if (maxAttempts < 1) // Проверка на положительность
            {
                throw new ArgumentException("Количество попыток должно быть положительным."); // Выбрасываем исключение
            }

            this._maxAttempts = maxAttempts; // Установка максимального количества попыток
            _correctAnswer = ComputeAnswer(a, b); // Вычисление правильного ответа на основе входных данных
            AttemptsLeft = _maxAttempts; // Обновление оставшихся попыток
        }

        /// <summary>
        /// Метод для проверки предположения пользователя.
        /// </summary>
        /// <param name="guess">Предположение пользователя.</param>
        /// <returns>Кортеж с результатом проверки: является ли предположение правильным и сообщение.</returns>
        public (bool IsCorrect, string Message) IsCorrectGuess(double guess)
        {
            // Проверка, остались ли попытки
            if (AttemptsLeft <= 0)
            {
                return (false, "Попытки закончились!"); // Возвращаем сообщение об окончании попыток
            }

            // Проверка, является ли предположение пользователя правильным
            if (Math.Abs(guess - _correctAnswer) < 0.01) // Используем допустимую погрешность
            {
                return (true, $"Поздравляем! Ответ: {_correctAnswer:F2}"); // Возвращаем сообщение о правильном ответе
            }

            AttemptsLeft--; // Уменьшаем количество оставшихся попыток
            string message = AttemptsLeft > 0
                ? $"Неверно! Осталось попыток: {AttemptsLeft}" // Сообщение об ошибочном предположении
                : $"Игра окончена. Правильный ответ: {_correctAnswer}"; // Сообщение об окончании игры

            return (false, message); // Возвращаем результат проверки
        }

        /// <summary>
        /// Метод для вычисления правильного ответа.
        /// </summary>
        /// <param name="a">Первое число для вычисления.</param>
        /// <param name="b">Второе число для вычисления.</param>
        /// <returns>Правильный ответ, вычисленный по заданной формуле.</returns>
        public double ComputeAnswer(double a, double b)
        {
            // Вычисляем ответ по заданной формуле
            return Math.PI * 5 * Math.Log(b) / (Math.Sin(a) + 1);
        }

        /// <summary>
        /// Метод для получения максимального количества попыток.
        /// </summary>
        /// <returns>Максимальное количество попыток.</returns>
        public int GetMaxAttempts() => _maxAttempts; // Возвращаем значение максимального количества попыток
    }
}