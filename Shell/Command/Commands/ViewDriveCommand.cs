using System.Text.Json;
using Shell.Models;

namespace Shell.Command.Commands
{
    public class ViewDriveCommand(string host) : CommandTemplate(host)
    {
        public override string Description => "View drive";

        public override async Task<HttpResponseMessage> GetResponse(string[] args)
        {
            var name = args[0];

            using var client = new HttpClient();
            var response = await client.GetAsync($"{Api}/drive/get-content?name={Uri.EscapeDataString(name)}");

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
