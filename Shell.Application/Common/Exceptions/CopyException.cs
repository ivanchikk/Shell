namespace Shell.Application.Common.Exceptions
{
    public class CopyException(string name, object key, Exception innerException)
        : Exception($"Entity \"{name}\" ({key}) can't be copied.", innerException);
}
