using Batch.Filters;
using System.Reflection;

var builder = ConsoleApp.CreateBuilder(args, (context, options) => {
    options.GlobalFilters = [new BatchFilter()];
    options.NameConverter = s => s; // 変換しない
});

var app = builder.Build();
app.AddAllCommandType(Assembly.GetExecutingAssembly());

app.Run();
