using System;
using System.Collections.Generic;
using System.Text; 



namespace MyDesktopApp
{
    /// <summary>
    /// Класс <see cref="Author"/> предназначен для отображения информации об авторе приложения.
    /// </summary>
    public class Author
    {
        /// <summary>
        /// Метод <see cref="ShowAuthorInfo"/> возвращает информацию об авторе в виде строки.
        /// </summary>
        /// <returns>Строка с информацией об авторе.</returns>
        public static string ShowAuthorInfo()
        {
            // Создаем экземпляр StringBuilder для построения строки
            StringBuilder sb = new StringBuilder();
            
            // Добавляем заголовок информации об авторе
            sb.AppendLine("--- Информация об авторе ---");
            
            // Добавляем имя автора
            sb.AppendLine("Автор: Тимофей Сергеевич Привалов");
            
            // Добавляем информацию о группе автора
            sb.AppendLine("Студент группы 6101-090301D");
            
            // Возвращаем собранную строку с информацией
            return sb.ToString();
        }
    }
}