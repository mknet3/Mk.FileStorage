using System;
using System.Runtime.Serialization;

namespace Mk.FileStorage
{
    [Serializable]
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

        protected FileStorageException(string message)
            : base(message)
        {
        }

        protected FileStorageException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected FileStorageException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}