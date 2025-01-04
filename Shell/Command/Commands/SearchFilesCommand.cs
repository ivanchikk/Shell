using System.Text.Json;
using File = Shell.Models.File;

namespace Shell.Command.Commands
{
    public class SearchFilesCommand(string host) : CommandTemplate(host)
    {
        public override string Description => "Search files by name and creationDate";
        public override async Task<HttpResponseMessage> GetResponse(string[] args)
        {
            var path = args[0];
            var name = args[1];
            var type = string.Empty;
            if (args.Length > 2)
                type = args[2];

            using var client = new HttpClient();
            var response = await client.GetAsync($"{Api}/file/search?path={Uri.EscapeDataString(path)}&name={Uri.EscapeDataString(name)}&creationDate={Uri.EscapeDataString(type)}");

            return response;
        }

        public override void ProcessResponse(string content)
        {
            JsonSerializer.Deserialize<List<File>>(content, Options)
                ?.ForEach(Console.WriteLine);
        }

        public override string GetFormatPattern(string replacement = @"\\")
        {
            return @"\";
        }

        public override void ValidateArguments(string[] args, int number = 1)
        {
            base.ValidateArguments(args, 2);
        }
    }
}
