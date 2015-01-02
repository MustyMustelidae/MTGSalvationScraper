using System;

namespace TinyIoC
{
    public class TinyIoCRegistrationTypeException : Exception
    {
        private const string REGISTER_ERROR_TEXT = "Cannot register type {0} - abstract classes or interfaces are not valid implementation types for {1}.";

        public TinyIoCRegistrationTypeException(Type type, string factory)
            : base(String.Format(REGISTER_ERROR_TEXT, type.FullName, factory))
        {
        }

        public TinyIoCRegistrationTypeException(Type type, string factory, Exception innerException)
            : base(String.Format(REGISTER_ERROR_TEXT, type.FullName, factory), innerException)
        {
        }
    }
}