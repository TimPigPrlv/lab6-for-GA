using System;
namespace MyDesktopApp 

{
    /// <summary>
    /// Provides methods to validate and process user input for Tetris game controls.
    /// </summary>
    public static class TetrisInputValidator
    {
        /// <summary>
        /// Validates if the input corresponds to valid Tetris game controls.
        /// Valid controls are: A (Move Left), D (Move Right), S (Move Down), W (Rotate), and Space (Fast Drop).
        /// </summary>
        /// <param name="input">The user input string to validate.</param>
        /// <returns><see langword="true"/> if the input is a valid control; otherwise, <see langword="false"/>.</returns>
        public static bool ValidateControlInput(string input)
        {
            return input is "A" or "S" or "D" or "W" or " ";
        }

        /// <summary>
        /// Prompts the user until valid Tetris game control input is provided.
        /// </summary>
        /// <returns>A validated string representing a valid control.</returns>
        public static string GetValidatedControlInput()
        {
            while (true)
            {
                Console.Write("Enter your move (A/D/S/W/Space): ");
                var key = Console.ReadKey(intercept: true);
                Console.WriteLine();
                
                string input = key.KeyChar.ToString().ToUpper();
                if (key.Key == ConsoleKey.Spacebar)
                {
                    input = " ";
                }

                if (ValidateControlInput(input))
                {
                    return input;
                }

                Console.WriteLine("Invalid input. Valid inputs are A, D, S, W, or Space.");
            }
        }
    }
}
