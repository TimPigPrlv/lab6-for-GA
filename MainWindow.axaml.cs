using Avalonia.Controls;// Не забудьте добавить необходимые пространства имен
using Avalonia.Interactivity;
using Avalonia.Controls.ApplicationLifetimes;
using System.Threading.Tasks;
using Avalonia;



namespace MyDesktopApp
{
    /// <summary>
    /// Основное окно приложения, наследующее от <see cref="Window"/>.
    /// Содержит методы для обработки событий пользовательского интерфейса.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="MainWindow"/>.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent(); // Инициализация компонентов окна
        }

        /// <summary>
        /// Обработчик события нажатия кнопки "Показать автора".
        /// Открывает окно с информацией об авторе.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Данные события.</param>
        private void OnShowAuthorClick(object? sender, RoutedEventArgs e)
        {
            var authorWindow = new AuthorInfoWindow(); // Создаем экземпляр окна информации об авторе
            authorWindow.ShowDialog(this); // Показываем окно как диалог
        }

        /// <summary>
        /// Обработчик события нажатия кнопки "Начать игру".
        /// Открывает окно игры "Угадай число".
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Данные события.</param>
        private void OnOpenGuessingGameClick(object? sender, RoutedEventArgs e)
        {
            var guessingGame = new GuessingGameFormsAdapter(); // Создаем экземпляр адаптера игры
            guessingGame.ShowDialog(this); // Показываем окно как диалог
        }

        /// <summary>
        /// Обработчик события нажатия кнопки "Сортировка".
        /// Открывает окно сортировки массива.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Данные события.</param>
        private void OnOpenSortingClick(object? sender, RoutedEventArgs e)
        {
            var sortingWindow = new ArraySortingWindowAdapter(); // Создаем экземпляр адаптера окна сортировки
            sortingWindow.Show(); // Показываем окно
        }

        /// <summary>
        /// Обработчик события нажатия кнопки "Тетрис".
        /// Открывает окно игры "Тетрис".
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Данные события.</param>
        private void OnOpenTetrisClick(object? sender, RoutedEventArgs e)
        {
            var tetrisWindow = new TetrisWindowAdapter(); // Создаем экземпляр адаптера окна Тетриса
            tetrisWindow.Show(); // Показываем окно
        }

        /// <summary>
        /// Обработчик события нажатия кнопки "Закрыть".
        /// Открывает диалог подтверждения перед закрытием приложения.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Данные события.</param>
        private async void OnCloseClick(object? sender, RoutedEventArgs e)
        {
            var confirmationDialog = new ConfirmationDialog(); // Создаем экземпляр диалогового окна подтверждения
            await confirmationDialog.ShowDialog(this); // Показываем окно как диалог

            // Проверяем, был ли выбран вариант "Да"
            if (confirmationDialog.Result)
            {
                // Закрываем приложение, если пользователь подтвердил
                if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
                {
                    desktopLifetime.Shutdown(); // Завершаем работу приложения
                }
            }
        }
    }
}