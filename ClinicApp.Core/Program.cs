using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using IHost host = Host.CreateDefaultBuilder(args).Build();

//// Ask the service provider for the configuration abstraction.
//IConfiguration config = host.Services.GetRequiredService<IConfiguration>();

//// Get values from the config given their key and their target type.
//string keyThreeNestedValue = config.GetValue<string>("ApiSettings:ConnectionStrings");

//// Write the values to the console.
//// Console.WriteLine($"KeyThree:Message = {keyThreeNestedValue}");

//// Application code which might rely on the config could start here.

await host.RunAsync();