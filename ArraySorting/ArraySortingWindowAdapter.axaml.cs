using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Linq;
using MyDesktopApp.Core;

namespace MyDesktopApp
{
    /// <summary>
    /// Класс <see cref="ArraySortingWindowAdapter"/> отвечает за взаимодействие с окном сортировки массивов.
    /// Он предоставляет функциональность для создания, сортировки и анализа массивов целых чисел.
    /// </summary>
    public partial class ArraySortingWindowAdapter : Window
    {
        /// <summary>
        /// Сервис для сортировки массивов.
        /// </summary>
        private readonly ArraySortingService _sortingService;

        /// <summary>
        /// Массив целых чисел, инициализированный как пустой.
        /// </summary>
        private int[] _array = Array.Empty<int>();

        /// <summary>
        /// Конструктор класса <see cref="ArraySortingWindowAdapter"/>.
        /// Инициализирует компоненты окна и создает экземпляр сервиса сортировки.
        /// </summary>
        public ArraySortingWindowAdapter()
        {
            InitializeComponent(); // Инициализация компонентов окна
            _sortingService = new ArraySortingService(); // Создание экземпляра сервиса сортировки
        }

        /// <summary>
        /// Обработчик события нажатия кнопки для создания массива с заданным размером.
        /// </summary>
        /// <param name="sender">Объект-отправитель события.</param>
        /// <param name="e">Данные события.</param>
        private void OnCreateArrayClick(object? sender, RoutedEventArgs e)
        {
            try
            {
                // Валидация длины массива, вводимой пользователем
                int length = InputValidator.ValidatePositiveInteger(ArrayLengthInput.Text ?? string.Empty, 10);
                // Генерация случайного массива заданной длины
                _array = _sortingService.GenerateRandomArray(length);
                // Отображение массива на экране
                ShowArray();
            }
            catch (ArgumentException ex)
            {
                // Отображение сообщения об ошибке, если валидация не прошла
                ResultLabel.Text = $"Ошибка: {ex.Message}";
            }
        }

        /// <summary>
        /// Обработчик события нажатия кнопки для создания случайного массива фиксированной длины (10).
        /// </summary>
        /// <param name="sender">Объект-отправитель события.</param>
        /// <param name="e">Данные события.</param>
        private void OnRandomArrayClick(object? sender, RoutedEventArgs e)
        {
            // Генерация случайного массива длиной 10
            _array = _sortingService.GenerateRandomArray(10);
            // Отображение массива на экране
            ShowArray();
        }

        /// <summary>
        /// Обработчик события нажатия кнопки для сортировки массива.
        /// </summary>
        /// <param name="sender">Объект-отправитель события.</param>
        /// <param name="e">Данные события.</param>
        private void OnSortClick(object? sender, RoutedEventArgs e)
        {
            try
            {
                // Выполнение сортировки вставками и получение отсортированного массива и времени сортировки
                var (sortedArray, time) = _sortingService.InsertionSort(_array);
                _array = sortedArray; // Обновление текущего массива на отсортированный
                // Отображение отсортированного массива на экране
                ShowArray();

                // Отображение времени сортировки
                ResultLabel.Text = _sortingService.CalculateSortingTime(_array);
            }
            catch (ArgumentException ex)
            {
                // Отображение сообщения об ошибке, если валидация не прошла
                ResultLabel.Text = $"Ошибка: {ex.Message}";
            }
        }

        /// <summary>
        /// Обработчик события нажатия кнопки для нахождения минимального и максимального значений в массиве.
        /// </summary>
        /// <param name="sender">Объект-отправитель события.</param>
        /// <param name="e">Данные события.</param>
        private void OnMinMaxClick(object? sender, RoutedEventArgs e)
        {
            try
            {
                // Валидация массива перед выполнением операций
                InputValidator.ValidateArray(_array);
                // Нахождение минимального и максимального значений
                int min = _array.Min();
                int max = _array.Max();

                // Отображение минимального и максимального значений
                ResultLabel.Text = $"Минимум: {min}, Максимум: {max}";
                // Отображение массива с выделением минимального и максимального значений
                ShowArray(highlightMin: min, highlightMax: max);
            }
            catch (ArgumentException ex)
            {
                // Отображение сообщения об ошибке, если валидация не прошла
                ResultLabel.Text = $"Ошибка: {ex.Message}";
            }
        }

        /// <summary>
        /// Обработчик события нажатия кнопки для вычисления среднего значения элементов массива.
        /// </summary>
        /// <param name="sender">Объект-отправитель события.</param>
        /// <param name="e">Данные события.</param>
        private void OnAverageClick(object? sender, RoutedEventArgs e)
        {
            try
            {
                // Валидация массива перед выполнением операций
                InputValidator.ValidateArray(_array);
                // Вычисление среднего значения
                double average = _array.Average();
                // Отображение среднего значения с двумя знаками после запятой
                ResultLabel.Text = $"Среднее значение: {average:F2}";
            }
            catch (ArgumentException ex)
            {
                // Отображение сообщения об ошибке, если валидация не прошла
                ResultLabel.Text = $"Ошибка: {ex.Message}";
            }
        }

        /// <summary>
        /// Обработчик события нажатия кнопки для закрытия окна.
        /// </summary>
        /// <param name="sender">Объект-отправитель события.</param>
        /// <param name="e">Данные события.</param>
        private void OnCloseClick(object? sender, RoutedEventArgs e)
        {
            Close(); // Закрытие текущего окна
        }

        /// <summary>
        /// Метод для отображения массива в элементе управления ListBox.
        /// </summary>
        /// <param name="highlightMin">Значение для выделения минимального элемента (по умолчанию null).</param>
        /// <param name="highlightMax">Значение для выделения максимального элемента (по умолчанию null).</param>
        private void ShowArray(int? highlightMin = null, int? highlightMax = null)
        {
            ArrayListBox.Items.Clear(); // Очистка предыдущих элементов в ListBox
            for (int i = 0; i < _array.Length; i++)
            {
                string value = _array[i].ToString();
                // Если значение минимальное, выделяем его квадратными скобками
                if (highlightMin.HasValue && _array[i] == highlightMin.Value)
                {
                    value = $"[{value}]";
                }
                // Если значение максимальное, выделяем его круглыми скобками
                else if (highlightMax.HasValue && _array[i] == highlightMax.Value)
                {
                    value = $"({value})";
                }
                // Добавляем значение в ListBox
                ArrayListBox.Items.Add(value);
            }
        }
    }
}