namespace Shell.Command.Commands
{
    public class DeleteDirectoryCommand(string host) : CommandTemplate(host)
    {
        public override string Description => "Delete directory";

        public override async Task<HttpResponseMessage> GetResponse(string[] args)
        {
            var path = args[0];

            using var client = new HttpClient();
            var response = await client.DeleteAsync($"{Api}/directory/{Uri.EscapeDataString(path)}");

            return response;
        }

        public override string GetFormatPattern(string replacement = @"\\")
        {
            return @"\";
        }
    }
}
