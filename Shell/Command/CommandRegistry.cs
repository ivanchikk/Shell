using Shell.Command.Commands;

namespace Shell.Command
{
    public class CommandRegistry(string host = "https://localhost:1212/api")
    {
        private readonly Dictionary<string, ICommand> _commands = new()
        {
            {"help", new HelpCommand()},
            {"clear", new ClearConsoleCommand()},
            {"cd", new ChangeDriveCommand()},
            {"ls", new ViewCurrentDriveCommand()},
            {"find", new SearchFilesCommand(host)},
            {"lsdrive", new ViewDriveCommand(host) },
            {"lsdir", new ViewDirectoryCommand(host) },
            {"mkfile", new CreateFileCommand(host) },
            {"mkdir", new CreateDirectoryCommand(host) },
            {"cpfile", new CopyFileCommand(host) },
            {"cpdir", new CopyDirectoryCommand(host) },
            {"mvfile", new MoveFileCommand(host) },
            {"mvdir", new MoveDirectoryCommand(host) },
            {"rmfile", new DeleteFileCommand(host) },
            {"rmdir", new DeleteDirectoryCommand(host) },
        };

        public ICommand? GetCommand(string key) =>
            _commands.GetValueOrDefault(key);

        public IEnumerable<(string CommandKey, string Description)> GetCommandsHelp() =>
            _commands.Select(c => (c.Key, c.Value.Description));
    }
}