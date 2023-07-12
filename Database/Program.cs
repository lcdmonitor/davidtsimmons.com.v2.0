using System.Reflection;
using System.Text;
using DbUp;
using DbUp.ScriptProviders;

namespace Database
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Console.WriteLine("DbUP - Database Migrations and deployments");

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
    ?? "Server=127.0.0.1;User ID=root;Password=London123!;Database=davidtsimmons.com";

            var upgrader =
                DeployChanges.To
                    .MySqlDatabase(connectionString)
                    //.WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                    .WithScriptsFromFileSystem("Scripts", options)
                    .LogToConsole()
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
            Console.ResetColor();

            return 0;
        }
    }
}
