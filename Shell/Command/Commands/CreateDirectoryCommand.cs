namespace Shell.Command.Commands
{
    public class CreateDirectoryCommand(string host) : CommandTemplate(host)
    {
        public override string Description => "Create directory";

        public override async Task<HttpResponseMessage> GetResponse(string[] args)
        {
            var path = args[0];

            var data = new StringContent($"{{ \"Path\": \"{path}\"}}", System.Text.Encoding.UTF8, "application/json");

            using var client = new HttpClient();
            var response = await client.PostAsync($"{Api}/directory/", data);

            return response;
        }
    }
}
