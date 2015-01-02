using System;

namespace TinyIoC
{
    public class TinyIoCWeakReferenceException : Exception
    {
        private const string ERROR_TEXT = "Unable to instantiate {0} - referenced object has been reclaimed";

        public TinyIoCWeakReferenceException(Type type)
            : base(String.Format(ERROR_TEXT, type.FullName))
        {
        }

        public TinyIoCWeakReferenceException(Type type, Exception innerException)
            : base(String.Format(ERROR_TEXT, type.FullName), innerException)
        {
        }
    }
}