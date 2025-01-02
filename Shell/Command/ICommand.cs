namespace Shell.Command
{
    public interface ICommand
    {
        Task Execute(string[] args);
        string Description { get; }
    }
}
