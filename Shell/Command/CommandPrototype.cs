using Shell.Models;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Shell.Command
{
    public abstract class CommandPrototype(string host) : ICommand
    {
        protected readonly string Api = host;
        protected static readonly JsonSerializerOptions Options = new()
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        public async Task Execute(string[] args)
        {
            ValidateArguments(args);

            var formatedArgs = args.Select(arg =>
                Regex.Replace(arg, @"\\+", GetFormatPattern())).ToArray();

            var response = await GetResponse(formatedArgs);
            await HandleResponse(response);
        }

        public abstract string Description { get; }

        public abstract Task<HttpResponseMessage> GetResponse(string[] args);

        public virtual string GetFormatPattern(string replacement = @"\\")
        {
            return replacement;
        }

        public virtual void ValidateArguments(string[] args, int number = 1)
        {
            if (number == 1 && args.Length < 1)
                throw new Exception($"1 argument required!");
            if (args.Length < number)
                throw new Exception($"{number} arguments required!");
        }

        public virtual async Task HandleResponse(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                ProcessResponse(content);
            }
            else
            {
                var contentError = JsonSerializer.Deserialize<Response>(content, Options);
                Console.WriteLine(contentError?.Error);
            }
        }

        public virtual void ProcessResponse(string content) { }
    }
}