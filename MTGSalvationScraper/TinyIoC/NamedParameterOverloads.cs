using System.Collections.Generic;

namespace TinyIoC
{
    /// <summary>
    /// Name/Value pairs for specifying "user" parameters when resolving
    /// </summary>
    public sealed class NamedParameterOverloads : Dictionary<string, object>
    {
        public static NamedParameterOverloads FromIDictionary(IDictionary<string, object> data)
        {
            return data as NamedParameterOverloads ?? new NamedParameterOverloads(data);
        }

        public NamedParameterOverloads()
        {
        }

        public NamedParameterOverloads(IDictionary<string, object> data)
            : base(data)
        {
        }

        private static readonly NamedParameterOverloads _Default = new NamedParameterOverloads();

        public static NamedParameterOverloads Default
        {
            get
            {
                return _Default;
            }
        }
    }
}