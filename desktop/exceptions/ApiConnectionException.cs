using System;
using System.Runtime.Serialization;

namespace desktop
{
    [Serializable]
    internal class ApiConnectionException : Exception
    {
        public ApiConnectionException() { }

        public ApiConnectionException(string? message)
            : base(message) { }

        public ApiConnectionException(string? message, Exception? innerException)
            : base(message, innerException) { }

        protected ApiConnectionException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
