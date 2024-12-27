using System;
using System.Collections.Generic;

namespace MyDesktopApp.Core
{
    /// <summary>
    /// Сервис для управления игрой Тетрис.
    /// Обеспечивает логику игры, включая создание игрового поля, управление фигурами и подсчет очков.
    /// </summary>
    public class TetrisGameService
    {
        private readonly GameField _gameField; // Объект игрового поля, на котором происходит игра
        private readonly Random _random; // Объект для генерации случайных чисел (используется для создания фигур)

        /// <summary>
        /// Конструктор класса TetrisGameService.
        /// Инициализирует новое игровое поле с заданными размерами и создаёт экземпляр генератора случайных чисел.
        /// </summary>
        /// <param name="rows">Количество строк в игровом поле. По умолчанию 10.</param>
        /// <param name="columns">Количество столбцов в игровом поле. По умолчанию 10.</param>
        public TetrisGameService(int rows = 10, int columns = 10)
        {
            _gameField = new GameField(rows, columns); // Создание нового игрового поля с заданными размерами
            _random = new Random(); // Инициализация генератора случайных чисел
        }

        /// <summary>
        /// Запускает новую игру Тетрис.
        /// Проверяет наличие свободного места для новой фигуры и, если место доступно, создает новую фигуру.
        /// В противном случае возвращает состояние игры "Game Over".
        /// </summary>
        /// <returns>Список списков строк, представляющий текущее состояние игрового поля.</returns>
        public List<List<string>> StartGame()
        {
            // Проверка, есть ли свободное место для новой фигуры
            if (!_gameField.HasSpaceForNewFigure())
            {
                // Если места нет, возвращаем состояние "Game Over"
                return CreateGameOverGrid();
            }

            // Если место есть, создаем новую фигуру
            SpawnNewFigure();
            // Возвращаем текущее состояние игрового поля
            return _gameField.Render();
        }

        /// <summary>
        /// Перемещает текущую фигуру в зависимости от введенного направления.
        /// </summary>
        /// <param name="input">Строка, представляющая направление перемещения (A, D, S, W).</param>
        /// <returns>Список списков строк, представляющий текущее состояние игрового поля.</returns>
        public List<List<string>> MoveFigure(string input)
        {
            // Определение направления перемещения на основе ввода
            var direction = input.ToUpper() switch
            {
                "A" => Movement.Left,
                "D" => Movement.Right,
                "S" => Movement.Down,
                "W" => Movement.Rotate,
                _ => (Movement?)null
            };

            // Если направление задано, обрабатываем движение
            if (direction.HasValue)
            {
                _gameField.HandleMove(direction.Value); // Обработка движения фигуры

                // Проверка, может ли текущая фигура двигаться
                if (!_gameField.CanMoveCurrentFigure)
                {
                    // Создание новой фигуры, если текущая не может двигаться
                    var figure = TetrisFigureFactory.CreateRandomFigure(_random);
                    var gameState = _gameField.SpawnFigure(figure); // Попытка поместить новую фигуру на поле

                    // Если игра завершена, возвращаем состояние "Game Over"
                    if (gameState == GameState.GameOver)
                    {
                        return CreateGameOverGrid();
                    }
                }

                // Возвращаем текущее состояние игрового поля
                return _gameField.Render();
            }

            // Если направление не задано, просто возвращаем текущее состояние игрового поля
            return _gameField.Render();
        }

        /// <summary>
        /// Получает текущее количество очков игрока.
        /// </summary>
        /// <returns>Текущий счет игры.</returns>
        public int GetScore() => _gameField.Score;

        /// <summary>
        /// Проверяет, завершена ли игра.
        /// </summary>
        /// <returns>True, если игра завершена; в противном случае false.</returns>
        public bool IsGameOver() => !_gameField.HasSpaceForNewFigure();

        /// <summary>
        /// Создает игровое поле в состоянии "Game Over".
        /// </summary>
        /// <returns>Список списков строк, представляющий состояние "Game Over".</returns>
        private List<List<string>> CreateGameOverGrid()
        {
            var grid = new List<List<string>>();
            for (int i = 0; i < 10; i++)
            {
                var row = new List<string>();
                for (int j = 0; j < 10; j++)
                {
                    // Заполнение поля символами, где "X" обозначает окончание игры
                    row.Add(i == 4 && j >= 3 && j <= 6 ? "X" : "⬜");
                }
                grid.Add(row);
            }
            return grid;
        }

        /// <summary>
        /// Создает новую фигуру и добавляет ее на игровое поле.
        /// </summary>
        private void SpawnNewFigure()
        {
            var figure = TetrisFigureFactory.CreateRandomFigure(_random); // Создание новой случайной фигуры
            _gameField.SpawnFigure(figure); // Добавление фигуры на игровое поле
        }

        /// <summary>
        /// Форматирует игровое поле для вывода в консоль.
        /// </summary>
        /// <param name="grid">Состояние игрового поля для форматирования.</param>
        /// <returns>Строка, представляющая отформатированное состояние игрового поля.</returns>
        public string RenderToConsole(List<List<string>> grid)
        {
            var result = string.Empty;
            foreach (var row in grid)
            {
                result += string.Join("", row) + "\n"; // Объединение строк для вывода
            }
            return result;
        }
    }



    /// <summary>
    /// Перечисление для направления движения фигур в игре Тетрис.
    /// </summary>
    public enum Movement 
    { 
        Left,   // Движение влево
        Right,  // Движение вправо
        Down,   // Движение вниз
        Rotate  // Поворот фигуры
    }

    /// <summary>
    /// Запись, представляющая позицию фигуры на игровом поле.
    /// </summary>
    /// <param name="Row">Строка (Y) позиции.</param>
    /// <param name="Column">Столбец (X) позиции.</param>
    public record Position(int Row, int Column);

    /// <summary>
    /// Перечисление для типов столкновения фигур.
    /// </summary>
    public enum CollisionType 
    { 
        None,   // Нет столкновения
        Left,   // Столкновение с левой стороной
        Right,  // Столкновение с правой стороной
        Bottom,  // Столкновение с нижней стороной
        Block   // Столкновение с другой фигурой
    }

    /// <summary>
    /// Запись, представляющая фигуру Тетриса.
    /// </summary>
    /// <param name="Shape">Двумерный массив, представляющий форму фигуры.</param>
    /// <param name="Type">Тип фигуры.</param>
    public record TetrisFigure(int[,] Shape, ShapeType Type)
    {
        /// <summary>
        /// Ширина фигуры.
        /// </summary>
        public int Width => Shape.GetLength(1);

        /// <summary>
        /// Высота фигуры.
        /// </summary>
        public int Height => Shape.GetLength(0);

        /// <summary>
        /// Поворачивает фигуру на 90 градусов по часовой стрелке.
        /// </summary>
        /// <returns>Новая фигура, представляющая повёрнутую фигуру.</returns>
        public TetrisFigure Rotate()
        {
            int newHeight = Width; // Новая высота после поворота
            int newWidth = Height; // Новая ширина после поворота
            var rotatedShape = new int[newHeight, newWidth];

            // Поворот фигуры
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    rotatedShape[j, newWidth - i - 1] = Shape[i, j];
                }
            }
            return new TetrisFigure(rotatedShape, Type); // Возвращаем новую фигуру
        }
    }

    /// <summary>
    /// Фабрика для создания фигур Тетриса.
    /// </summary>
    public static class TetrisFigureFactory
    {
        // Словарь, хранящий формы фигур по их типам
        private static readonly Dictionary<ShapeType, int[,]> Shapes = new()
        {
            { ShapeType.Square, new[,] { { 1, 1 }, { 1, 1 } } }, // Квадрат
            { ShapeType.Line, new[,] { { 1, 1, 1, 1 } } }, // Линия
            { ShapeType.L, new[,] { { 1, 1 }, { 1, 0 }, { 1, 0 } } }, // Фигура L
            { ShapeType.T, new[,] { { 1, 1, 1 }, { 0, 1, 0 } } }, // Фигура T
            { ShapeType.J, new[,] { { 1, 1 }, { 0, 1 }, { 0, 1 } } }, // Фигура J
            { ShapeType.Z, new[,] { { 0, 1, 1 }, { 1, 1, 0 } } }, // Фигура Z
            { ShapeType.S, new[,] { { 1, 1, 0 }, { 0, 1, 1 } } } // Фигура S
        };

        /// <summary>
        /// Создает фигуру заданного типа.
        /// </summary>
        /// <param name="type">Тип фигуры.</param>
        /// <returns>Созданная фигура.</returns>
        public static TetrisFigure CreateFigure(ShapeType type) => new(Shapes[type], type);

        /// <summary>
        /// Создает случайную фигуру из доступных типов.
        /// </summary>
        /// <param name="random">Генератор случайных чисел.</param>
        /// <returns>Случайно созданная фигура.</returns>
        public static TetrisFigure CreateRandomFigure(Random random)
        {
            var types = Enum.GetValues<ShapeType>(); // Получение всех доступных типов фигур
            return CreateFigure(types[random.Next(types.Length)]); // Возврат случайной фигуры
        }
    }

    /// <summary>
    /// Перечисление для типов фигур Тетриса.
    /// </summary>
    public enum ShapeType
    {
        Square, // Квадрат
        Line,   // Линия
        L,      // Фигура L
        T,      // Фигура T
        J,      // Фигура J
        Z,      // Фигура Z
        S       // Фигура S
    }

    /// <summary>
    /// Перечисление для состояний игры.
    /// </summary>
    public enum GameState// enum Перечисления позволяют создавать набор именованных констант, что делает код более читабельным и понятным.
    {    
        Success,  // Игра завершена успешно
        GameOver, // Игра завершена (проигрыш)
        Paused    // Игра на паузе
    }
}