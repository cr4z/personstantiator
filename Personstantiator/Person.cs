
namespace Personstantiator
{
    public class Person(string name, string catchphrase)
    {
        public readonly string Name = name;
        public string Catchphrase { get; set; } = catchphrase;
    }
}
