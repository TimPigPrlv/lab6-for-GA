using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace MyDesktopApp
{
    /// <summary>
    /// Основной класс приложения, наследующий от <see cref="Application"/>.
    /// Управляет инициализацией приложения и настройкой главного окна.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Метод инициализации приложения.
        /// Загружает XAML-разметку для приложения.
        /// </summary>
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this); // Загружаем разметку XAML
        }

        /// <summary>
        /// Метод, вызываемый после завершения инициализации фреймворка.
        /// Устанавливает главное окно приложения.
        /// </summary>
        public override void OnFrameworkInitializationCompleted()
        {
            // Проверяем, что приложение использует классический стиль для настольного приложения
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // Устанавливаем главное окно приложения
                desktop.MainWindow = new MainWindow();
            }

            // Вызываем базовую реализацию для завершения инициализации
            base.OnFrameworkInitializationCompleted();
        }
    }
}