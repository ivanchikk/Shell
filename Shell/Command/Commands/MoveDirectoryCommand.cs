﻿namespace Shell.Command.Commands
{
    public class MoveDirectoryCommand(string host) : CommandPrototype(host)
    {
        public override string Description => "Move(update) directory";

        public override async Task<HttpResponseMessage> GetResponse(string[] args)
        {
            var path = args[0];
            var newPath = args[1];

            var data = new StringContent($"{{ \"Path\": \"{path}\", \"newPath\": \"{newPath}\" }}", System.Text.Encoding.UTF8, "application/json");

            using var client = new HttpClient();
            var response = await client.PutAsync($"{Api}/directory/", data);

            return response;
        }

        public override void ValidateArguments(string[] args, int number = 1)
        {
            base.ValidateArguments(args, 2);
        }
    }
}