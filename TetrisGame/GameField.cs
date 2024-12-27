using System;
using System.Collections.Generic;
using MyDesktopApp.Core;

namespace MyDesktopApp
{

    /// <summary>
    /// Represents the Tetris game field, including the logic for figure movement,
    /// collision detection, and scoring.
    /// </summary>
    public class GameField
    {
        private readonly int[,] _matrix;
        private readonly int _rows;
        private readonly int _columns;
        private TetrisFigure? _activeFigure;
        private Position? _activePosition;

        /// <summary>
        /// Gets the current score of the game.
        /// </summary>
        public int Score { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameField"/> class.
        /// </summary>
        /// <param name="rows">The number of rows in the game field.</param>
        /// <param name="columns">The number of columns in the game field.</param>
        public GameField(int rows, int columns)
        {
            _rows = rows;
            _columns = columns;
            _matrix = new int[rows, columns];
        }

        /// <summary>
        /// Determines if there is space available to spawn a new Tetris figure.
        /// </summary>
        /// <returns><see langword="true"/> if there is space; otherwise, <see langword="false"/>.</returns>
        public bool HasSpaceForNewFigure()
        {
            for (int col = 0; col < _columns; col++)
            {
                if (_matrix[0, col] != 0)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Spawns a new Tetris figure at the top of the game field.
        /// </summary>
        /// <param name="figure">The Tetris figure to spawn.</param>
        /// <exception cref="InvalidOperationException">Thrown if there is no space to spawn the figure.</exception>
        public GameState SpawnFigure(TetrisFigure figure)
        {
            Console.WriteLine("Spawning new figure");
            _activeFigure = figure;
            _activePosition = new Position(0, (_columns - figure.Width) / 2);

           if (CalculateCollision(_activePosition, figure) != CollistionType.None)
            {
                Console.WriteLine("No space available for spawning a new figure. Game Over.");
                return GameState.GameOver;
            }

            return GameState.Success;
        }

        /// <summary>
        /// Gets a value indicating whether the current figure can still move.
        /// </summary>
        public bool CanMoveCurrentFigure => _activeFigure != null && _activePosition != null;


        /// <summary>
        /// Handles movement of the active Tetris figure.
        /// </summary>
        /// <param name="direction">The direction in which to move the figure.</param>
        public void HandleMove(Movement direction)
        {
            if (_activeFigure == null || _activePosition == null)
            {
                return;
            }

            Console.WriteLine($"Current Position: {_activePosition.Row}, {_activePosition.Column}");

            Position newPosition = direction switch
            {
                Movement.Left => _activePosition with { Column = _activePosition.Column - 1 },
                Movement.Right => _activePosition with { Column = _activePosition.Column + 1 },
                Movement.Down => _activePosition with { Row = _activePosition.Row + 1 },
                _ => _activePosition
            };

            Console.WriteLine($"New Position: {newPosition.Row}, {newPosition.Column}");

            if (direction == Movement.Rotate)
            {
                var rotatedFigure = _activeFigure.Rotate();
                if (CalculateCollision(_activePosition, rotatedFigure) == CollistionType.None)
                {
                    _activeFigure = rotatedFigure;
                }
                else
                {
                    Console.WriteLine("Cannot rotate the figure, it will overlap with existing blocks.");
                }
            }
            else
            {
                if (CalculateCollision(newPosition, _activeFigure) == CollistionType.None)
                {
                    _activePosition = newPosition;
                }
                else
                {
                    var collision = CalculateCollision(newPosition, _activeFigure);
                    if (collision == CollistionType.Left || collision == CollistionType.Right)
                    {
                        Console.WriteLine($"Cannot move {direction}, moving it down instead.");
                        HandleMove(Movement.Down);
                    }
                    else
                    {
                        if (direction == Movement.Down)
                        {
                            Console.WriteLine("Cannot move down, placing the figure on the field.");
                            PlaceFigure(_activePosition, _activeFigure);
                            FixCurrentFigure();
                        }
                    }
                }
            }
        }
        private void FixCurrentFigure()
        {
            _activeFigure = null;
            _activePosition = null;
            ClearFullRows();
        }

        private void ClearFullRows()
        {
            for (int row = 0; row < _rows; row++)
            {
                if (IsRowFull(row))
                {
                    ClearRow(row);
                    DropRowsAbove(row);
                    Score += 10;
                }
            }
        }

        private bool IsRowFull(int row)
        {
            for (int col = 0; col < _columns; col++)
            {
                if (_matrix[row, col] == 0)
                    return false;
            }
            return true;
        }

        private void ClearRow(int row)
        {
            for (int col = 0; col < _columns; col++)
                _matrix[row, col] = 0;
        }

        private void DropRowsAbove(int row)
        {
            for (int i = row; i > 0; i--)
            {
                for (int col = 0; col < _columns; col++)
                {
                    _matrix[i, col] = _matrix[i - 1, col];
                }
            }
            for (int col = 0; col < _columns; col++)
            {
                _matrix[0, col] = 0;
            }
        }

        private CollistionType CalculateCollision(Position position, TetrisFigure figure)
        {
            for (int i = 0; i < figure.Height; i++)
            {
                for (int j = 0; j < figure.Width; j++)
                {
                    if (figure.Shape[i, j] == 1)
                    {
                        int row = position.Row + i;
                        int col = position.Column + j;
                        if (row < 0)
                        {
                            return CollistionType.Bottom;
                        }
                        if (row >= _rows)
                        {
                            return CollistionType.Block;
                        }
                        if (col < 0)
                        {
                            return CollistionType.Left;
                        }
                        if (col >= _columns)
                        {
                            return CollistionType.Right;
                        }
                        if (_matrix[row, col] == 1)
                        {
                            return CollistionType.Block;
                        }
                    }
                }
            }
            return CollistionType.None;
        }

        private enum CollistionType
        {
            None,
            Left,
            Right,
            Bottom,
            Block
        }

        private void PlaceFigure(Position position, TetrisFigure figure)
        {
            for (int i = 0; i < figure.Height; i++)
            {
                for (int j = 0; j < figure.Width; j++)
                {
                    if (figure.Shape[i, j] == 1)
                    {
                        _matrix[position.Row + i, position.Column + j] = 1;
                    }
                }
            }
        }

        private void ClearActiveFigure()
        {
            if (_activeFigure == null || _activePosition == null)
                return;

            for (int i = 0; i < _activeFigure.Height; i++)
            {
                for (int j = 0; j < _activeFigure.Width; j++)
                {
                    if (_activeFigure.Shape[i, j] == 1)
                    {
                        _matrix[_activePosition.Row + i, _activePosition.Column + j] = 0;
                    }
                }
            }
        }

        /// <summary>
        /// Renders the game field and the current Tetris figure to the console.
        /// </summary>
        public List<List<string>> Render()
        {
            var grid = new List<List<string>>();

            for (int i = 0; i < _rows; i++)
            {
                var row = new List<string>();
                for (int j = 0; j < _columns; j++)
                {
                    
                    bool isActivePart = _activeFigure != null && _activePosition != null &&
                                        IsPartOfActiveFigure(i, j, _activePosition, _activeFigure);

                    if (_matrix[i, j] == 1 || isActivePart)
                    {
                        row.Add("🟦");
                    }
                    else
                    {
                        row.Add("⬜");
                    }
                }
                grid.Add(row);
            }

            return grid;
        }
                
        private bool IsPartOfActiveFigure(int row, int col, Position position, TetrisFigure figure)
        {
            for (int i = 0; i < figure.Height; i++)
            {
                for (int j = 0; j < figure.Width; j++)
                {
                    if (figure.Shape[i, j] == 1 &&
                        position.Row + i == row &&
                        position.Column + j == col)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }

    /// <summary>
    /// Represents a position on the game field.
    /// </summary>
    /// <param name="Row">The row index.</param>
    /// <param name="Column">The column index.</param>
    public record Position(int Row, int Column);
}