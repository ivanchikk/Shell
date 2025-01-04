namespace Shell.Command.Commands
{
    public class DeleteFileCommand(string host) : CommandTemplate(host)
    {
        public override string Description => "Delete file";

        public override async Task<HttpResponseMessage> GetResponse(string[] args)
        {
            var path = args[0];

            using var client = new HttpClient();
            var response = await client.DeleteAsync($"{Api}/file/{Uri.EscapeDataString(path)}");

            return response;
        }

        public override string GetFormatPattern(string replacement = @"\\")
        {
            return @"\";
        }
    }
}
