using System;
using System.Collections.Generic;
using System.IO;

namespace Domain.TemplateMethod
{
    /// <summary>
    /// Абстрактный класс, реализующий паттерн "Шаблонный метод" для импорта данных.
    /// Алгоритм импорта включает в себя чтение файла, парсинг, валидацию и сохранение данных.
    /// </summary>
    /// <typeparam name="T">Тип объекта, который импортируется.</typeparam>
    public abstract class ImportTemplate<T>
    {
        /// <summary>
        /// Шаблонный метод для импорта данных из файла.
        /// </summary>
        /// <param name="filePath">Путь к файлу импорта.</param>
        public void ImportData(string filePath)
        {
            string rawData = ReadFile(filePath);

            List<T> data = ParseData(rawData);

            ValidateData(data);

            SaveData(data);
        }

        /// <summary>
        /// Читает данные из файла.
        /// Можно переопределить, если необходимо добавить дополнительную логику чтения.
        /// </summary>
        /// <param name="filePath">Путь к файлу.</param>
        /// <returns>Сырые данные из файла в виде строки.</returns>
        protected virtual string ReadFile(string filePath)
        {
            return File.ReadAllText(filePath);
        }

        /// <summary>
        /// Абстрактный метод парсинга данных.
        /// Конкретная реализация зависит от формата файла (JSON, CSV, YAML).
        /// </summary>
        /// <param name="rawData">Сырые данные из файла.</param>
        /// <returns>Список объектов типа T.</returns>
        protected abstract List<T> ParseData(string rawData);

        /// <summary>
        /// Абстрактный метод валидации данных.
        /// Проверяет корректность полученных объектов.
        /// </summary>
        /// <param name="data">Список объектов, полученных после парсинга.</param>
        protected abstract void ValidateData(List<T> data);

        /// <summary>
        /// Абстрактный метод сохранения данных.
        /// Здесь можно реализовать сохранение в репозиторий или базу данных.
        /// </summary>
        /// <param name="data">Список валидированных объектов.</param>
        protected abstract void SaveData(List<T> data);
    }
}
