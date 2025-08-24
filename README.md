# Personstantiator

A lightweight C# console app with a generic command registry that applies reflection, custom attributes, and delegate-based command execution.

It's built to show how a **generic command registry** can be wired up cleanly with **reflection and attributes**, while keeping the app code simple and testable through modular .NET architecture.

## Overview

Personstantiator is a small sandbox project where you can:

- Create people with names and catchphrases  
- List existing people  
- Make them "speak"  
- Update or clear the roster  
- Exit gracefully  
- Get contextual help on available commands  


## Commands

| Command | Description |
|---------|-------------|
| `/add <name> <catchphrase>` | Add a new person |
| `/read` | List all people |
| `/speak <name>` | Make a person speak |
| `/setcatchphrase <name> <catchphrase>` | Update a catchphrase |
| `/clear` | Clear the list of people |
| `/exit` | Exit the program |
| `/help` | Show available commands and descriptions |

## Project Structure

```
Personstantiator.sln
├── Personstantiator/         # Core console app
└── Personstantiator.Tests/   # xUnit test suite
```

- **PersonstantiatorApp** — application state and entry point  
- **CommandRegistry<TOptions>** — generic registry for mapping commands  
- **CommandRegistryOptions** — defines command signatures and metadata via `[Comment]` attributes  
- **Unit tests** — cover app behavior, registry error handling, and help text  

## Running

From the solution root:

```bash
dotnet build
dotnet run --project Personstantiator
```

## Testing

Run the xUnit tests using the CLI, as demonstrated below, or Visual Studio UI.

```bash
dotnet test
```
