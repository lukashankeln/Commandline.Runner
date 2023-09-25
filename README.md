# Commandline.Runner
Easy to use Package to help you start new Processes


# Examples

## Basic
This examples shows how to start a new Process for `filename` with the argument `--foo`

```csharp
var cmd = CommandlineFactory.Create("filename").AddArgument("--foo").Build();

var result = await cmd.RunAsync();
``` 