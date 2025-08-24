using Personstantiator;

Console.WriteLine("Welcome to Personstantiator.");

var app = new PersonstantiatorApp();

while (app.IsRunning)
{
    string input = Console.ReadLine() ?? "";
    app.RunCommand(input);
}