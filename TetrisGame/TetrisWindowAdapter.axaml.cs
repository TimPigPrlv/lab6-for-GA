using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using MyDesktopApp.Core;
using System.Collections.Generic;
using System;

namespace MyDesktopApp
{
    /// <summary>
    /// Представляет основной адаптер окна для игры Тетрис.
    /// Обрабатывает ввод пользователя, рендеринг игры и управление состоянием игры.
    /// </summary>
    public partial class TetrisWindowAdapter : Window
    {
        private readonly TetrisGameService _tetrisGameService;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="TetrisWindowAdapter"/>.
        /// Настраивает игровой сервис и запускает игру.
        /// </summary>
        public TetrisWindowAdapter()
        {
            InitializeComponent();
            _tetrisGameService = new TetrisGameService(10, 10); // Инициализация игрового сервиса с сеткой 10x10

            // Запуск игры и рендеринг начального игрового поля
            RenderGameField(_tetrisGameService.StartGame());
        }

        /// <summary>
        /// Обрабатывает события нажатия кнопок для перемещения фигуры Тетриса.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Данные события.</param>
        private void OnMoveClick(object? sender, RoutedEventArgs e)
        {
            // Проверяем, является ли источник кнопкой и имеет ли она допустимый тег направления
            if (sender is Button button && button.Tag is string direction)
            {
                HandleMove(direction); // Обрабатываем перемещение в зависимости от нажатой кнопки
            }
        }

        /// <summary>
        /// Обрабатывает события нажатия клавиш для перемещения фигуры Тетриса.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Данные события, содержащие информацию о нажатой клавише.</param>
        private void OnKeyDown(object? sender, KeyEventArgs e)
        {
            Console.WriteLine($"Key Pressed: {e.Key}");
            // Привязываем нажатия клавиш к строкам направления
            string? direction = e.Key switch
            {
                Key.Left or Key.A => "A", 
                Key.Right or Key.D => "D",
                Key.Down or Key.S => "S",
                Key.Up or Key.W => "W", 
                Key.Space or Key.Enter => "S", // Пробел и Enter для движения вниз
                _ => null
            };

            // Если направление определено, обрабатываем перемещение
            if (direction != null)
            {
                HandleMove(direction);
            }
        }

        /// <summary>
        /// Обрабатывает перемещение фигуры Тетриса в зависимости от указанного направления.
        /// Обновляет игровое поле и проверяет условия окончания игры.
        /// </summary>
        /// <param name="direction">Направление для перемещения фигуры.</param>
        private void HandleMove(string direction)
        {
            // Выполняем перемещение и получаем обновленное игровое поле
            var result = _tetrisGameService.MoveFigure(direction);
            RenderGameField(result); // Рендерим обновленное игровое поле

            // Проверяем, закончилась ли игра, и обрабатываем конец игры
            if (_tetrisGameService.IsGameOver())
            {
                ResultLabel.Text = "Game Over!"; // Выводим сообщение об окончании игры
                DisableControls(); // Отключаем элементы управления
            }
        }

        /// <summary>
        /// Обрабатывает событие нажатия кнопки закрытия для выхода из приложения.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Данные события.</param>
        private void OnCloseClick(object? sender, RoutedEventArgs e)
        {
            Close(); // Закрываем окно игры
        }

        /// <summary>
        /// Рендерит игровое поле на основе текущего состояния игры.
        /// Обновляет визуальное представление сетки Тетриса и счета.
        /// </summary>
        /// <param name="grid">Текущее состояние игровой сетки.</param>
        private void RenderGameField(List<List<string>> grid)
        {
            GameFieldGrid.Children.Clear(); // Очищаем текущее игровое поле

            // Заполняем игровое поле текущим состоянием сетки
            foreach (var row in grid)
            {
                foreach (var cell in row)
                {
                    var block = new TextBlock
                    {
                        Text = cell,
                        FontSize = 24,
                        HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                        VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center
                    };
                    GameFieldGrid.Children.Add(block); // Добавляем каждый блок в игровое поле
                }
            }

            // Обновляем метку счета
            ScoreLabel.Text = $"Score: {_tetrisGameService.GetScore()}";
        }

        /// <summary>
        /// Отключает элементы управления, когда игра окончена.
        /// </summary>
        private void DisableControls()
        {
            MoveLeftButton.IsEnabled = false;
            MoveRightButton.IsEnabled = false;
            MoveDownButton.IsEnabled = false;
            RotateButton.IsEnabled = false;
        }
    }
}