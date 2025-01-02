namespace Shell.Application.Common.Exceptions
{
    public class DuplicateException(string name, object key)
        : Exception($"Entity \"{name}\" ({key}) already exists.");
}
