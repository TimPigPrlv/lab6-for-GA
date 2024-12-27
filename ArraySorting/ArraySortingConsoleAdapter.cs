using System;                     
using System.Diagnostics;         
using System.Collections.Generic; 
using MyDesktopApp.Core;

namespace MyDesktopApp.ConsoleAdapter
{
    /// <summary>
    /// Класс <see cref="ArraySortingConsoleAdapter"/> отвечает за взаимодействие с пользователем через консоль.
    /// Он предоставляет интерфейс для ввода данных, генерации случайных массивов и сравнения алгоритмов сортировки.
    /// </summary>
    public class ArraySortingConsoleAdapter
    {
        /// <summary>
        /// Поле для хранения экземпляра сервиса сортировки массивов.
        /// </summary>
        private readonly ArraySortingService _sortingService;

        /// <summary>
        /// Конструктор класса <see cref="ArraySortingConsoleAdapter"/>, инициализирующий сервис сортировки.
        /// </summary>
        public ArraySortingConsoleAdapter()
        {
            // Создаем новый экземпляр <see cref="ArraySortingService"/> для работы с массивами.
            _sortingService = new ArraySortingService();
        }

        /// <summary>
        /// Метод, запускающий основную логику приложения.
        /// Запрашивает у пользователя размер массива, генерирует случайный массив,
        /// выполняет сортировку с использованием различных алгоритмов и выводит результаты.
        /// </summary>
        public void Run()
        {
            // Запрашиваем у пользователя размер массива.
            Console.Write("Введите размер массива: ");
            // Читаем ввод пользователя и преобразуем его в целое число; если ввод пустой, по умолчанию используем 10.
            int length = int.Parse(Console.ReadLine() ?? "10");
            // Генерируем случайный массив заданного размера.
            int[] array = _sortingService.GenerateRandomArray(length);

            // Выводим исходный массив в консоль.
            Console.WriteLine("Исходный массив:");
            // Преобразуем массив в строку и выводим его.
            Console.WriteLine(string.Join(", ", array));

            // Информируем пользователя о том, что будет производиться сравнение сортировок.
            Console.WriteLine("Сравниваем пузырьковую сортировку и сортировку вставками...");

            // Выполняем пузырьковую сортировку и получаем отсортированный массив и время выполнения.
            var (bubbleSorted, bubbleTime) = _sortingService.BubbleSort(array);
            // Выполняем сортировку вставками и получаем отсортированный массив и время выполнения.
            var (insertionSorted, insertionTime) = _sortingService.InsertionSort(array);

            // Выводим время выполнения пузырьковой сортировки.
            Console.WriteLine($"Время пузырьковой сортировки: {bubbleTime} мс");
            // Выводим время выполнения сортировки вставками.
            Console.WriteLine($"Время сортировки вставками: {insertionTime} мс");

            // Сравниваем время выполнения двух сортировок и выводим, какая из них быстрее.
            if (bubbleTime < insertionTime)
                Console.WriteLine("Пузырьковая сортировка быстрее.");
            else
                Console.WriteLine("Сортировка вставками быстрее.");
        }
    }
}