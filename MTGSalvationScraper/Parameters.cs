using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTGSalvationScraper
{
    public class Parameters
    {
        private readonly Dictionary<string, string> _argumentAliases = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _namedArguments = new Dictionary<string, string>();
        private readonly List<string> _unnamedArguments = new List<string>();

        public Parameters(IEnumerable<string> rawArguments, char argumentTag = '-',
            char argumentSplitCharacter = '=')
        {
            Dictionary<string, string> namedArgs;
            List<string> unnamedArgs;
            ParseCommandLineArgs(rawArguments, out namedArgs, out unnamedArgs, argumentTag, argumentSplitCharacter);

            _namedArguments = namedArgs;
            _unnamedArguments = unnamedArgs;
        }

        public int UnnamedArgumentCount
        {
            get { return _unnamedArguments.Count; }
        }

        public int NamedArgumentCount
        {
            get { return _namedArguments.Count; }
        }

        public void AddArgumentAlias(string argument, string alias)
        {
            _argumentAliases.Add(argument, alias);
        }

        public string ResolveArgumentAlias(string argumentName)
        {
            return _argumentAliases.ContainsKey(argumentName)
                ? _argumentAliases[argumentName]
                : argumentName;
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            const string argFormat = "--{0}: {1}";

            stringBuilder.AppendLine("Named Args:");

            foreach (string argString in _namedArguments
                .Select(namedArg => string.Format(argFormat, namedArg.Key, namedArg.Value)))
            {
                stringBuilder.AppendLine(argString);
            }

            stringBuilder.AppendLine("Unnamed Args:");

            for (int argumentIndex = 0; argumentIndex < _unnamedArguments.Count; argumentIndex++)
            {
                string argString = string.Format(argFormat, argumentIndex, _unnamedArguments[argumentIndex]);
                stringBuilder.AppendLine(argString);
            }
            return stringBuilder.ToString();
        }

        public static void ParseCommandLineArgs(IEnumerable<string> rawArguments,
            out Dictionary<string, string> namedArgs, out List<string> unnamedArgs, char argumentTag = '-',
            char argumentSplitCharacter = '=')
        {
            namedArgs = new Dictionary<string, string>();
            unnamedArgs = new List<string>();

            foreach (string argument in rawArguments)
            {
                string[] trimSplitArg = argument
                    .Trim(argumentTag)
                    .Split(argumentSplitCharacter);

                if (trimSplitArg.Length == 2)
                {
                    string argKey = trimSplitArg[0];
                    string argValue = trimSplitArg[1];

                    namedArgs.Add(argKey, argValue);
                    continue;
                }
                unnamedArgs.Add(argument);
            }
        }

        public string GetNamedArgument(string argumentName)
        {
            return GetNamedArgument(argumentName, null, false);
        }

        public string GetNamedArgument(string argumentName, string defaultValue)
        {
            return GetNamedArgument(argumentName, defaultValue, true);
        }

        private string GetNamedArgument(string argumentName, string defaultValue, bool useDefault)
        {

            var resolvedArgumentName = ResolveArgumentAlias(argumentName);
            if (_namedArguments.ContainsKey(resolvedArgumentName))
            {
                return _namedArguments[resolvedArgumentName];
            }

            if (useDefault)
            {
                return defaultValue;
            }

            throw new ArgumentException("No matching argument value defined.");
        }

        public string GetUnnamedArgument(int argumentIndex, string defaultValue, bool useDefault)
        {
            if (argumentIndex <= _unnamedArguments.Count - 1)
            {
                return _unnamedArguments[argumentIndex];
            }

            if (useDefault)
            {
                return defaultValue;
            }
            string exceptionString = string.Format("Requested argument: {0}, but there are only {1} arguments",
                argumentIndex, _unnamedArguments.Count);
            throw new ArgumentOutOfRangeException("argumentIndex", argumentIndex, exceptionString);
        }

        public string GetUnnamedArgument(int argumentIndex)
        {
            return GetUnnamedArgument(argumentIndex, null, false);
        }

        public string GetUnnamedArgument(int argumentIndex, string defaultValue)
        {
            return GetUnnamedArgument(argumentIndex, defaultValue, false);
        }
        public bool TryGetNamedArgument(string argumentName)
        {

            var resolvedArgumentName = ResolveArgumentAlias(argumentName);
            return _namedArguments.ContainsKey(resolvedArgumentName);
        }
        public bool TryGetNamedArgument(string argumentName, out string argumentValue)
        {

            var resolvedArgumentName = ResolveArgumentAlias(argumentName);
            return _namedArguments.TryGetValue(resolvedArgumentName, out argumentValue);
        }

        public bool TryGetNamedArgument(string argumentName, string defaultValue, out string argumentValue)
        {
           var resolvedArgumentName = ResolveArgumentAlias(argumentName);
           bool valueDefined = _namedArguments.TryGetValue(resolvedArgumentName, out argumentValue);

           argumentValue = valueDefined ? resolvedArgumentName : defaultValue;

            return valueDefined;
        }

        public bool TryGetUnnamedArgument(int argumentIndex, out string argumentValue)
        {
            if (argumentIndex > _unnamedArguments.Count - 1)
            {
                argumentValue = null;
                return false;
            }
            argumentValue = _unnamedArguments[argumentIndex];
            return true;
        }

        public static Exception GenerateParameterException(string argumentName, int argumentIndex,
            string parameterPurpose)
        {
            string exceptionString =
                string.Format(
                    "The number of parameters passed is invalid. Named argument \"{0}\" or Unnamed argument #{1} must be defined. It {2}",
                    argumentName, argumentIndex, parameterPurpose);
            throw new ArgumentException(exceptionString);
        }
    }
}