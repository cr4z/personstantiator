using Personstantiator;
using Xunit;

namespace PersonstantiatorTests
{
    public class PersonstantiatorAppTests
    {
        [Fact]
        public void ExitCommand_ShouldStopApp()
        {
            // Arrange
            var app = new PersonstantiatorApp();

            // Act
            app.RunCommand("/exit");

            // Assert
            Assert.False(app.IsRunning);
        }

        [Fact]
        public void AddCommand_ShouldIncreasePeopleCount()
        {
            var app = new PersonstantiatorApp();

            app.RunCommand("/add Alice Hello");

            Assert.Equal(1, app.PeopleCount);
            Assert.Equal("Alice", app.People[0].Name);
            Assert.Equal("Hello", app.People[0].Catchphrase);
        }

        [Fact]
        public void SpeakCommand_ShouldPrintCatchphrase()
        {
            var app = new PersonstantiatorApp();
            app.RunCommand("/add Bob Yo");

            using var sw = new StringWriter();
            Console.SetOut(sw);

            app.RunCommand("/speak Bob");

            var output = sw.ToString().Trim();
            Assert.Equal("Yo", output);
        }

        [Fact]
        public void InvalidCommand_ShouldNotCrash()
        {
            var app = new PersonstantiatorApp();

            using var sw = new StringWriter();
            Console.SetOut(sw);

            app.RunCommand("/doesnotexist");

            var output = sw.ToString();
            Assert.Contains("Command does not exist", output);
        }
    }
}
