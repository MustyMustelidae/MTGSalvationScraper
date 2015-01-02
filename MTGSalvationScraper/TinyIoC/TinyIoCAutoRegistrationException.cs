using System;
using System.Collections.Generic;
using System.Linq;

namespace TinyIoC
{
    public class TinyIoCAutoRegistrationException : Exception
    {
        private const string ERROR_TEXT = "Duplicate implementation of type {0} found ({1}).";

        public TinyIoCAutoRegistrationException(Type registerType, IEnumerable<Type> types)
            : base(String.Format(ERROR_TEXT, registerType, GetTypesString(types)))
        {
        }

        public TinyIoCAutoRegistrationException(Type registerType, IEnumerable<Type> types, Exception innerException)
            : base(String.Format(ERROR_TEXT, registerType, GetTypesString(types)), innerException)
        {
        }

        private static string GetTypesString(IEnumerable<Type> types)
        {
            var typeNames = from type in types
                select type.FullName;

            return string.Join(",", typeNames.ToArray());
        }
    }
}