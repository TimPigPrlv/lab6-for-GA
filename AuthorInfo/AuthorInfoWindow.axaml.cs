using Avalonia.Controls;
using Avalonia.Interactivity;
using MyDesktopApp.Core; 

namespace MyDesktopApp
{
    /// <summary>
    /// Класс <see cref="AuthorInfoWindow"/> отвечает за отображение информации об авторе в окне приложения.
    /// </summary>
    public partial class AuthorInfoWindow : Window
    {
        /// <summary>
        /// Конструктор класса <see cref="AuthorInfoWindow"/>.
        /// Инициализирует компоненты окна и отображает информацию об авторе в текстовом блоке.
        /// </summary>
        public AuthorInfoWindow()
        {
            InitializeComponent(); // Инициализация компонентов окна
            // Получение и отображение информации об авторе в текстовом блоке
            AuthorInfoTextBlock.Text = Author.ShowAuthorInfo();
        }

        /// <summary>
        /// Обработчик события нажатия кнопки "Закрыть".
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