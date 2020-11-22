using System;

namespace Mk.FileStorage
{
    public readonly struct FileStorageErrorCode
    {
        private readonly string _value;

        public FileStorageErrorCode(string value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public static FileStorageErrorCode FileNotFound { get; } = new FileStorageErrorCode(nameof(FileNotFound));

        public static FileStorageErrorCode FileAlreadyExists { get; } = new FileStorageErrorCode(nameof(FileAlreadyExists));

        public static bool operator ==(FileStorageErrorCode left, FileStorageErrorCode right) => left.Equals(right);

        public static bool operator !=(FileStorageErrorCode left, FileStorageErrorCode right) => !left.Equals(right);

        public static implicit operator FileStorageErrorCode(string value) => new FileStorageErrorCode(value);

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public override bool Equals(object? obj) => obj is FileStorageErrorCode other && Equals(other);

        public bool Equals(FileStorageErrorCode other) => string.Equals(_value, other._value, System.StringComparison.Ordinal);

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public override int GetHashCode() => _value?.GetHashCode() ?? 0;

        public override string ToString() => _value;
    }
}