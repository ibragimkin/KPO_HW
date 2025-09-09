using System;
using System.Text.RegularExpressions;

namespace FileStoringService.Domain.ValueObjects
{
    /// <summary>
    /// Объект-значение для хранения хеша файла.
    /// </summary>
    public sealed class FileHash : IEquatable<FileHash>
    {
        private static readonly Regex HexRegex = new Regex("^[a-f0-9]+$", RegexOptions.Compiled);
        private readonly string _value;

        /// <summary>
        /// Значение хеша в виде шестнадцатеричной строки (строчные буквы).
        /// </summary>
        public string Value => _value;

        private FileHash(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Hash не может быть пустым.", nameof(value));

            var normalized = value.Trim().ToLowerInvariant();
            if (!HexRegex.IsMatch(normalized))
                throw new ArgumentException("Hash должен быть шестнадцатеричной строкой.", nameof(value));

            _value = normalized;
        }

        /// <summary>
        /// Создать экземпляр FileHash из строки.
        /// </summary>
        /// <param name="hexString">Шестнадцатеричная строка хеша.</param>
        /// <returns>Новый экземпляр FileHash.</returns>
        public static FileHash FromString(string hexString) =>
            new FileHash(hexString);

        public override bool Equals(object? obj) =>
            Equals(obj as FileHash);

        public bool Equals(FileHash? other) =>
            other is not null && _value == other._value;

        public override int GetHashCode() =>
            _value.GetHashCode(StringComparison.Ordinal);

        public override string ToString() =>
            _value;

        public static bool operator ==(FileHash left, FileHash right) =>
            left?.Equals(right) ?? right is null;

        public static bool operator !=(FileHash left, FileHash right) =>
            !(left == right);
    }
}
