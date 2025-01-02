namespace Shell.Application.Common.Exceptions
{
    public class DeleteException(string name, object key, Exception innerException)
        : Exception($"Entity \"{name}\" ({key}) can't be deleted.", innerException);
}
