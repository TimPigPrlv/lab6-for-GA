using Avalonia;
using System;

namespace MyDesktopApp
{
    /// <summary>
    /// Основной класс приложения, содержащий метод Main для запуска приложения Avalonia.
    /// Этот класс является точкой входа в приложение и отвечает за его инициализацию.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Основной метод, который запускает приложение.
        /// Метод помечен атрибутом [STAThread], что указывает на использование однопоточной модели 
        /// для работы с интерфейсом пользователя, что необходимо для приложений Windows.
        /// </summary>
        /// <param name="args">Аргументы командной строки, переданные приложению.</param>
        [STAThread]
        public static void Main(string[] args) 
            => BuildAvaloniaApp() // Вызов метода для конфигурации приложения
                .StartWithClassicDesktopLifetime(args); // Запуск приложения с классическим жизненным циклом рабочего стола

        
        /// <summary>
        /// Метод для настройки и конфигурации приложения Avalonia.
        /// Используется для определения платформы, настройки шрифтов и логирования.
        /// </summary>
        /// <returns>Объект AppBuilder, который позволяет настраивать приложение.</returns>
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>() // Конфигурация приложения с использованием класса App
                .UsePlatformDetect() // Определяет платформу (Windows, Linux, macOS) и применяет соответствующие настройки
                .WithInterFont() // Устанавливает шрифт Inter для всего приложения
                .LogToTrace(); // Включает логирование в трассировку для отладки и мониторинга
    }
}