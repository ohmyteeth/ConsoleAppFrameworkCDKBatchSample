global using ConsoleAppFramework;
using Batch.Filters;
using Microsoft.Extensions.Hosting;

[assembly: ConsoleAppFrameworkGeneratorOptions(DisableNamingConversion = true)]

var builder = Host.CreateApplicationBuilder();
var app = builder.ToConsoleAppBuilder();

app.UseFilter<BatchFilter>();

app.Run(args);

