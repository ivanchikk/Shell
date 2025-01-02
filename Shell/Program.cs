using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;
using Shell.Command;

namespace Shell
{
    public class Program
    {
        private static string _api = "http://localhost:1212/" + Suffix;
        private const string Suffix = "api";
        private static string _currentDrive = "C:\\";
        private static readonly IEnumerable<DriveInfo> Drives = DriveInfo.GetDrives();

        private static async Task Main()
        {
            Console.Title = "Shell";
            Console.CursorVisible = false;

            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
            using var appSettings = JsonDocument.Parse(await File.ReadAllTextAsync(filePath));

            var isHostPresent = appSettings.RootElement.TryGetProperty("host", out var host);
            if (isHostPresent && !string.IsNullOrEmpty(host.GetString()))
            {
                _api = host.GetString()!;
                _api += _api[^1..].Equals("/") ? Suffix : "/" + Suffix;
            }

            Console.Write("Waiting for the server to start");
            while (!await IsServerAvailable())
            {
                Console.Write(".");
                await Task.Delay(300);
                Console.Write(".");
                await Task.Delay(300);
                Console.Write(".");
                Console.Write("\b\b\b");
                await Task.Delay(300);
                Console.Write("   \b\b\b");
                await Task.Delay(300);
            }
            Console.Clear();
            Console.CursorVisible = true;

            var commandRegistry = new CommandRegistry(_api);

            while (true)
            {
                Console.Write($"{_currentDrive}> ");
                var input = Console.ReadLine()?.Trim();
                if (string.IsNullOrWhiteSpace(input)) continue;

                var formatedInput = Regex.Replace(input, @"\\+", @"\\");

                var args = Regex.Matches(formatedInput, @"(?:[^\s""]|""[^""]*"")+")
                    .Select(m => m.Value.Trim('"'))
                    .ToArray();

                var commandKey = args[0];
                var commandArgs = args[1..];

                if (commandKey == "exit") break;

                switch (commandKey)
                {
                    case "clear":
                        Console.Clear();
                        continue;
                    case "help":
                        foreach (var key in commandRegistry.GetCommandsHelp())
                            Console.WriteLine($"Command: {key.CommandKey,-10}Description: {key.Description}");
                        continue;
                    case "cd" when commandArgs.Length > 0:
                        var driveName = Regex.Replace(commandArgs[0], @"\\+", @"\");

                        if (Drives.Any(d => d.Name.Equals(driveName)))
                            _currentDrive = driveName;
                        else
                            Console.WriteLine($"Invalid drive: {driveName}. Available drives: {string.Join(", ", Drives)}");
                        continue;
                    case "cd" when commandArgs.Length == 0:
                        Console.WriteLine("1 argument required!");
                        continue;
                    case "ls":
                        commandKey = "lsdrive";
                        commandArgs = [_currentDrive];
                        break;
                }

                try
                {
                    var command = commandRegistry.GetCommand(commandKey);
                    if (command != null)
                        await command.Execute(commandArgs);
                    else
                        Console.WriteLine($"Unknown command: {commandKey}. Try 'help'");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        private static async Task<bool> IsServerAvailable()
        {
            using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(1) };
            try
            {
                var response = await client.GetAsync(_api);
                return response.StatusCode == HttpStatusCode.OK;
            }
            catch
            {
                return false;
            }
        }
    }
}
