using System;

namespace AsteriskMod.UnityUI
{
    public class UINotFoundException : Exception
    {
        public UINotFoundException() : base() { }
        public UINotFoundException(string message) : base(message) { }
        public UINotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class DuplicateUIException : Exception
    {
        public DuplicateUIException() : base() { }
        public DuplicateUIException(string message) : base(message) { }
        public DuplicateUIException(string message, Exception innerException) : base(message, innerException) { }
    }
}
