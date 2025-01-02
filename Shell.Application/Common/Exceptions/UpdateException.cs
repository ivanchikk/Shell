namespace Shell.Application.Common.Exceptions
{
    public class UpdateException(string name, object key, Exception innerException)
        : Exception($"Entity \"{name}\" ({key}) can't be updated.", innerException);
}
