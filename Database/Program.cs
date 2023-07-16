using System.Reflection;
using System.Text;
using DbUp;
using DbUp.ScriptProviders;
using Database.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Database.CustomLogging;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting;

namespace Database
{
    public class Program
    {
        private static InMemoryUpgradeLog _inMemoryUpgradeLog = new InMemoryUpgradeLog();
        public static int Main(string[] args)
        {
            Console.WriteLine("DbUP - Database Migrations and deployments");

            IEnvironmentVariableProvider environmentVariableProvider = new EnvironmentVariableProvider();

            string? mysqlHostName = environmentVariableProvider.GetEnvironmentVariable("MYSQL_HOSTNAME");
            string? mysqlRootPassword = environmentVariableProvider.GetEnvironmentVariable("MYSQL_ROOT_PASSWORD");

            bool abort = false;

            if (string.IsNullOrEmpty(mysqlHostName))
            {
                Console.WriteLine("MYSQL_HOSTNAME environment variable not set");
                abort = true;
            }

            if (string.IsNullOrEmpty(mysqlRootPassword))
            {
                Console.WriteLine("MYSQL_ROOT_PASSWORD environment variable not set");
                abort = true;
            }

            if (abort)
            {
                Console.WriteLine("Aborting...");
                return 500;
            }

            Console.WriteLine("Host Name: {0}", mysqlHostName);
            Console.WriteLine("Password: {0}", mysqlRootPassword);

            var options = new FileSystemScriptOptions
            {
                // true = scan into subdirectories, false = top directory only
                IncludeSubDirectories = true,
                // Patterns to search the file system for. Set to "*.sql" by default.
                Extensions = new[] { "*.sql" },
                // Type of text encoding to use when reading the files. Defaults to "Encoding.UTF8".
                Encoding = Encoding.UTF8,
                // Pass each file path located to this function and filter based on the result
                //Filter = path => path.Contains("value")
            };



            var connectionString =
    args.FirstOrDefault()
    ?? string.Format("Server={0};User ID=root;Password={1};Database=davidtsimmons.com", mysqlHostName, mysqlRootPassword);

            var upgrader =
                DeployChanges.To
                    .MySqlDatabase(connectionString)
                    //.WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                    .WithScriptsFromFileSystem("Scripts", options)
                    .LogToConsole()
                    .LogTo(_inMemoryUpgradeLog)
                    .Build();

            var result = upgrader.PerformUpgrade();

            if (!result.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
                Console.ResetColor();
#if DEBUG
                Console.ReadLine();
#endif
                return -1;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success!");
            Console.WriteLine("Starting Success Listener");
            Console.ResetColor();

            new WebHostBuilder().UseKestrel().Configure(Configure).UseUrls(environmentVariableProvider.GetEnvironmentVariable("ASPNETCORE_URLS")).Build().Run();
            return 0;
        }

        static public void Configure(IApplicationBuilder app)
        {
            app.Run(async (context) =>
            {
                Console.WriteLine("UP Request Received: {0}", context.Request.Path);

                context.Response.StatusCode = 200;
                await context.Response.WriteAsync(JsonConvert.SerializeObject(_inMemoryUpgradeLog.GetLogEntries()));
            });
        }
    }
}
