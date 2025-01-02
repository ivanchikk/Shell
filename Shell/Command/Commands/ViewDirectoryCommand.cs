using Shell.Models;
using System.Text.Json;

namespace Shell.Command.Commands
{
    public class ViewDirectoryCommand(string host) : CommandPrototype(host)
    {
        public override string Description => "View directory";

        public override async Task<HttpResponseMessage> GetResponse(string[] args)
        {
            var path = args[0];

            using var client = new HttpClient();
            var response = await client.GetAsync($"{Api}/directory/get-content?path={Uri.EscapeDataString(path)}");

            return response;
        }

        public override void ProcessResponse(string content)
        {
            JsonSerializer.Deserialize<List<Item>>(content, Options)
                ?.ForEach(Console.WriteLine);
        }

        public override string GetFormatPattern(string replacement = @"\\")
        {
            return @"\";
        }
    }
}
