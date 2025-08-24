using System.Linq.Expressions;

namespace Personstantiator
{
    public class PersonstantiatorApp
    {
        private readonly List<Person> people = new();
        private bool isRunning = true;
        private readonly CommandRegistry<CommandRegistryOptions> registry;
        public class CommandRegistryOptions
        {
            [Comment("Exit the program")]
            public required Action Exit { get; set; }

            [Comment("Add a new person: <name> <catchphrase>")]
            public required Action<string, string> AddPerson { get; set; }

            [Comment("List all people")]
            public required Action ReadPeople { get; set; }

            [Comment("Make the person speak: <name>")]
            public required Action<string> Speak { get; set; }

            [Comment("Clear the list of people")]
            public required Action Clear { get; set; }

            [Comment("Set a person's catchphrase: <name> <catchphrase>")]
            public required Action<string, string> SetCatchphrase { get; set; }
        }


        public PersonstantiatorApp()
        {
            registry = new CommandRegistry<CommandRegistryOptions>(
                new CommandRegistryOptions
                {
                    Exit = () => isRunning = false,
                    AddPerson = (name, catchphrase) => people.Add(new Person(name, catchphrase)),
                    ReadPeople = () =>
                    {
                        if (people.Count < 1) throw new Exception("There are currently no instantiated people!");
                        Console.WriteLine(string.Join(", ", people.Select(p => p.Name)));
                    },
                    Speak = name => Console.WriteLine(people.Find(p => p.Name == name)!.Catchphrase),
                    Clear = () => people.Clear(),
                    SetCatchphrase = (name, catchphrase) => people.Find(p => p.Name == name)!.Catchphrase = catchphrase
                },
                new Dictionary<string, Expression<Func<CommandRegistryOptions, Delegate>>>
                {
                    ["/exit"] = o => o.Exit,
                    ["/add"] = o => o.AddPerson,
                    ["/read"] = o => o.ReadPeople,
                    ["/speak"] = o => o.Speak,
                    ["/clear"] = o => o.Clear,
                    ["/setcatchphrase"] = o => o.SetCatchphrase,
                    ["/help"] = _ => new Action(() => Console.WriteLine(string.Join(Environment.NewLine, registry!.GetHelp())))
                });
        }

        public void RunCommand(string input)
        {
            var result = registry.RunActionForInput(input);

            if (result.IsError)
            {
                Console.WriteLine(result.ErrorMessage);
            }
        }

        public bool IsRunning => isRunning;
        public int PeopleCount => people.Count;
        public IReadOnlyList<Person> People => people.AsReadOnly();
    }
}
