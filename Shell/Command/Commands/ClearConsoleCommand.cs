namespace Shell.Command.Commands
{
    public class ClearConsoleCommand : ICommand
    {
        public Task Execute(string[] args)
        {
            throw new NotImplementedException();
        }

        public string Description => "Clear console";
    }
}
