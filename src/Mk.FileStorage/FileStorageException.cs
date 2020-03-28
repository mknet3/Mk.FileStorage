using System;

namespace Mk.FileStorage
{
    public class FileStorageException : Exception
    {
        public FileStorageErrorCode ErrorCode { get; }

        public FileStorageException(string message, FileStorageErrorCode errorCode)
            : base(message)
        {
            ErrorCode = errorCode;
        }

        public FileStorageException(string message, FileStorageErrorCode errorCode, Exception innerException)
            : base(message, innerException)
        {
            ErrorCode = errorCode;
        }
    }
}