using System.Linq.Expressions;
using Personstantiator;

namespace CommandRegistryTests
{
    public class CommandRegistryTests
    {
        private CommandRegistry<PersonstantiatorApp.CommandRegistryOptions> MakeRegistry()
        {
            var opts = new PersonstantiatorApp.CommandRegistryOptions
            {
                Exit = () => { },
                AddPerson = (n, c) => { },
                ReadPeople = () => { },
                Speak = n => { },
                Clear = () => { },
                SetCatchphrase = (n, c) => { }
            };

            var commands = new Dictionary<string, Expression<Func<PersonstantiatorApp.CommandRegistryOptions, Delegate>>>
            {
                ["/exit"] = o => o.Exit,
                ["/add"] = o => o.AddPerson,
                ["/read"] = o => o.ReadPeople,
                ["/speak"] = o => o.Speak,
                ["/clear"] = o => o.Clear,
                ["/setcatchphrase"] = o => o.SetCatchphrase,
            };

            return new CommandRegistry<PersonstantiatorApp.CommandRegistryOptions>(opts, commands);
        }

        [Fact]
        public void RunActionForInput_ShouldReturnError_WhenInputIsEmpty()
        {
            var registry = MakeRegistry();

            var result = registry.RunActionForInput("");

            Assert.True(result.IsError);
            Assert.Contains("Please enter", result.ErrorMessage);
        }

        [Fact]
        public void RunActionForInput_ShouldReturnError_WhenCommandNotFound()
        {
            var registry = MakeRegistry();

            var result = registry.RunActionForInput("/notacommand");

            Assert.True(result.IsError);
            Assert.Contains("does not exist", result.ErrorMessage);
        }

        [Fact]
        public void RunActionForInput_ShouldReturnError_WhenDelegateThrows()
        {
            var opts = new PersonstantiatorApp.CommandRegistryOptions
            {
                Exit = () => throw new InvalidOperationException("boom"),
                AddPerson = (n, c) => { },
                ReadPeople = () => { },
                Speak = n => { },
                Clear = () => { },
                SetCatchphrase = (n, c) => { }
            };

            var commands = new Dictionary<string, Expression<Func<PersonstantiatorApp.CommandRegistryOptions, Delegate>>>
            {
                ["/exit"] = o => o.Exit
            };

            var registry = new CommandRegistry<PersonstantiatorApp.CommandRegistryOptions>(opts, commands);

            var result = registry.RunActionForInput("/exit");

            Assert.True(result.IsError);
            Assert.Contains("boom", result.ErrorMessage);
        }

        [Fact]
        public void GetHelp_ShouldReturnComments()
        {
            var registry = MakeRegistry();

            var help = registry.GetHelp().ToList();

            Assert.Contains(help, h => h.Contains("/exit") && h.Contains("Exit the program"));
            Assert.Contains(help, h => h.Contains("/add") && h.Contains("Add a new person"));
        }
    }
}
