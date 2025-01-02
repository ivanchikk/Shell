namespace Shell.Command.Commands
{
    public class HelpCommand : ICommand
    {
        public Task Execute(string[] args)
        {
            throw new NotImplementedException();
        }

        public string Description => "Show commands list";
    }
}
