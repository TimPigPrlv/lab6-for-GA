using System;
using System.Diagnostics;
using MyDesktopApp.Core;

namespace MyDesktopApp.Core
{
    /// <summary>
    /// Класс <see cref="InputValidator"/> предназначен для валидации входных данных.
    /// </summary>
    public static class InputValidator
    {
        /// <summary>
        /// Метод для валидации положительного целого числа.
        /// </summary>
        /// <param name="input">Строка, представляющая число для валидации.</param>
        /// <param name="maxLimit">Максимально допустимое значение (по умолчанию 10).</param>
        /// <returns>Возвращает валидное положительное целое число.</returns>
        /// <exception cref="ArgumentException">Выбрасывается, если ввод не является положительным целым числом или превышает максимальный предел.</exception>
        public static int ValidatePositiveInteger(string input, int maxLimit = 10)
        {
            // Проверяем, может ли строка быть преобразована в целое число
            if (!int.TryParse(input, out int result) || result <= 0 || result > maxLimit)
            {
                // Если преобразование не удалось или число не в допустимом диапазоне, выбрасываем исключение
                throw new ArgumentException($"Введите положительное целое число (не более {maxLimit}).");
            }
            // Возвращаем валидное число
            return result;
        }

        /// <summary>
        /// Метод для проверки, что массив не пустой и не равен null.
        /// </summary>
        /// <param name="array">Целочисленный массив для проверки.</param>
        /// <exception cref="ArgumentException">Выбрасывается, если массив пустой или не определён.</exception>
        public static void ValidateArray(int[] array)
        {
            // Проверяем, что массив не равен null и имеет ненулевую длину
            if (array == null || array.Length == 0)
            {
                // Если массив пустой или не определён, выбрасываем исключение
                throw new ArgumentException("Ошибка! Массив пуст или не определён.");
            }
        }

        /// <summary>
        /// Метод для валидации ввода подтверждения от пользователя.
        /// </summary>
        /// <param name="input">Строка, представляющая ввод пользователя.</param>
        /// <returns>Возвращает true, если ввели 'н' (возврат) и false, если 'д' (выход).</returns>
        /// <exception cref="ArgumentException">Выбрасывается, если ввод некорректен.</exception>
        public static bool ValidateConfirmation(string input)
        {
            // Убираем пробелы и переводим строку в нижний регистр
            input = input.Trim().ToLower();

            // Используем оператор switch для проверки ввода
            return input switch
            {
                "д" => false, // Если введено 'д', возвращаем false (выход)
                "н" => true,  // Если введено 'н', возвращаем true (возврат)
                _ => throw new ArgumentException("Ошибка ввода. Введите 'д' для выхода или 'н' для возврата.") // Обработка некорректного ввода
            };
        }
    }

    /// <summary>
    /// Класс <see cref="ArraySortingService"/> предназначен для выполнения сортировки массивов.
    /// </summary>
    public class ArraySortingService
    {
        /// <summary>
        /// Метод для расчета времени сортировки и возвращает результаты.
        /// </summary>
        /// <param name="array">Целочисленный массив для сортировки.</param>
        /// <returns>Строка с результатами сортировки.</returns>
        public string CalculateSortingTime(int[] array)
        {
            // Выполняем пузырьковую сортировку и получаем отсортированный массив и время выполнения
            var (sortedBubble, bubbleTime) = BubbleSort(array);
            // Выполняем сортировку вставками и получаем отсортированный массив и время выполнения
            var (sortedInsertion, insertionTime) = InsertionSort(array);
            // Формируем строку с результатами
            return $"Пузырьковая сортировка: {bubbleTime} мс\n" +
                   $"Сортировка вставками: {insertionTime} мс\n" +
                   $"{(bubbleTime < insertionTime ? "Пузырьковая сортировка быстрее." : "Сортировка вставками быстрее.")}";
        }

        /// <summary>
        /// Метод для генерации случайного массива заданной длины.
        /// </summary>
        /// <param name="length">Длина массива.</param>
        /// <returns>Случайный целочисленный массив.</returns>
        public int[] GenerateRandomArray(int length)
        {
            Random random = new();
            int[] array = new int[length];
            // Заполняем массив случайными числами в диапазоне от -100 до 100
            for (int i = 0; i < length; i++)
            {
                array[i] = random.Next(-100, 100);
            }
            // Возвращаем сгенерированный массив
            return array;
        }

        /// <summary>
        /// Метод для копирования массива.
        /// </summary>
        /// <param name="source">Исходный массив для копирования.</param>
        /// <returns>Копия исходного массива.</returns>
        public int[] CopyArray(int[] source)
        {
            // Создаем новый массив той же длины, что и исходный
            int[] copy = new int[source.Length];
            // Копируем элементы из исходного массива в новый
            Array.Copy(source, copy, source.Length);
            // Возвращаем копию массива
            return copy;
        }

        /// <summary>
        /// Метод для измерения времени выполнения сортировки.
        /// </summary>
        /// <param name="sortingFunction">Функция сортировки, время выполнения которой нужно измерить.</param>
        /// <returns>Время выполнения в миллисекундах.</returns>
        private double MeasureExecutionTime(Action sortingFunction)
        {
            // Запускаем таймер
            Stopwatch stopwatch = Stopwatch.StartNew();
            // Вызываем функцию сортировки
            sortingFunction();
            // Останавливаем таймер
            stopwatch.Stop();
            // Возвращаем время выполнения в миллисекундах
            return stopwatch.Elapsed.TotalMilliseconds;
        }

        /// <summary>
        /// Метод для пузырьковой сортировки с возвращением отсортированного массива и временем выполнения.
        /// </summary>
        /// <param name="array">Целочисленный массив для сортировки.</param>
        /// <returns>Кортеж, содержащий отсортированный массив и время выполнения.</returns>
        public (int[], double) BubbleSort(int[] array)
        {
            // Копируем исходный массив для сортировки
            int[] data = CopyArray(array);
            // Измеряем время выполнения сортировки
            double time = MeasureExecutionTime(() =>
            {
                // Реализация пузырьковой сортировки
                for (int i = 0; i < data.Length - 1; i++)
                {
                    for (int j = 0; j < data.Length - i - 1; j++)
                    {
                        // Если текущий элемент больше следующего, меняем их местами
                        if (data[j] > data[j + 1])
                        {
                            int temp = data[j];
                            data[j] = data[j + 1];
                            data[j + 1] = temp;
                        }
                    }
                }
            });
            // Возвращаем отсортированный массив и время выполнения
            return (data, time);
        }

        /// <summary>
        /// Метод для сортировки вставками с возвращением отсортированного массива и временем выполнения.
        /// </summary>
        /// <param name="array">Целочисленный массив для сортировки.</param>
        /// <returns>Кортеж, содержащий отсортированный массив и время выполнения.</returns>
        public (int[], double) InsertionSort(int[] array)
        {
            // Копируем исходный массив для сортировки
            int[] data = CopyArray(array);
            // Измеряем время выполнения сортировки
            double time = MeasureExecutionTime(() =>
            {
                // Реализация сортировки вставками
                for (int i = 1; i < data.Length; i++)
                {
                    int key = data[i];
                    int j = i - 1;

                    // Сдвигаем элементы, которые больше ключа, на одну позицию вперед
                    while (j >= 0 && data[j] > key)
                    {
                        data[j + 1] = data[j];
                        j--;
                    }
                    // Вставляем ключ на его позицию
                    data[j + 1] = key;
                }
            });
            // Возвращаем отсортированный массив и время выполнения
            return (data, time);
        }
    }
}