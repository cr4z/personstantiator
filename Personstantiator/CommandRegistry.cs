using System.Linq.Expressions;
using System.Reflection;

namespace Personstantiator
{
    public class CommandRegistry<TOptions>(TOptions options, Dictionary<string, Expression<Func<TOptions, Delegate>>> commands) where TOptions : class
    {
        private readonly Dictionary<string, Expression<Func<TOptions, Delegate>>> _commands = commands;
        private readonly TOptions _options = options;

        public IEnumerable<string> GetHelp()
        {
            foreach (var kv in _commands)
            {
                if (kv.Value.Body is MemberExpression memberExpr &&
                    memberExpr.Member is PropertyInfo propInfo)
                {
                    var comment = propInfo.GetCustomAttribute<CommentAttribute>()?.Text ?? "(no description)";
                    yield return $"{kv.Key} - {comment}";
                }
            }
        }

        public class CommandResult
        {
            public bool IsError { get; }
            public string? ErrorMessage { get; }

            private CommandResult(bool isError, string? errorMessage)
            {
                IsError = isError;
                ErrorMessage = errorMessage;
            }

            public static CommandResult Success() => new(false, null);
            public static CommandResult Error(string message) => new(true, message);
        }

        public CommandResult RunActionForInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return CommandResult.Error("Please enter a command.");

            var parts = input.Split(' ');
            var command = parts[0];
            var args = parts.Skip(1).ToArray();

            if (!_commands.TryGetValue(command, out var expr))
                return CommandResult.Error("Command does not exist.");

            var del = expr.Compile()(_options);

            try
            {
                del.DynamicInvoke(args);
                return CommandResult.Success();
            }
            catch (TargetInvocationException tie) when (tie.InnerException is not null)
            {
                return CommandResult.Error(tie.InnerException.Message);
            }
            catch (Exception ex)
            {
                return CommandResult.Error(ex.Message);
            }
        }
    }
}
