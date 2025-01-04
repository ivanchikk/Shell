namespace Shell.Command.Commands
{
    public class CopyDirectoryCommand(string host) : CommandTemplate(host)
    {
        public override string Description => "Copy directory";

        public override async Task<HttpResponseMessage> GetResponse(string[] args)
        {
            var path = args[0];
            var newPath = args[1];

            var data = new StringContent($"{{ \"Path\": \"{path}\", \"newPath\": \"{newPath}\" }}", System.Text.Encoding.UTF8, "application/json");

            using var client = new HttpClient();
            var response = await client.PostAsync($"{Api}/directory/copy", data);

            return response;
        }

        public override void ValidateArguments(string[] args, int number = 1)
        {
            base.ValidateArguments(args, 2);
        }
    }
}
