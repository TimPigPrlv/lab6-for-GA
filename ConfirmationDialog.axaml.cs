using Avalonia.Controls;
using Avalonia.Interactivity;

namespace MyDesktopApp
{
    /// <summary>
    /// Класс для диалогового окна подтверждения, наследующий от <see cref="Window"/>.
    /// Используется для запроса подтверждения у пользователя.
    /// </summary>
    public partial class ConfirmationDialog : Window
    {
        /// <summary>
        /// Результат выбора пользователя в диалоговом окне.
        /// Возвращает true, если пользователь выбрал "Да", иначе false.
        /// </summary>
        public bool Result { get; private set; }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ConfirmationDialog"/>.
        /// </summary>
        public ConfirmationDialog()
        {
            InitializeComponent(); // Инициализация компонентов окна
        }

        /// <summary>
        /// Обработчик события нажатия кнопки "Да".
        /// Устанавливает результат в true и закрывает диалоговое окно.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Данные события.</param>
        private void YesButton_Click(object? sender, RoutedEventArgs e)
        {
            Result = true; // Устанавливаем результат в true
            Close(); // Закрываем диалоговое окно
        }

        /// <summary>
        /// Обработчик события нажатия кнопки "Нет".
        /// Устанавливает результат в false и закрывает диалоговое окно.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Данные события.</param>
        private void NoButton_Click(object? sender, RoutedEventArgs e)
        {
            Result = false; // Устанавливаем результат в false
            Close(); // Закрываем диалоговое окно
        }
    }
}