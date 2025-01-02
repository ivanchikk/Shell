namespace Shell.Application.Common.Exceptions
{
    public class CreateException(string name, object key, Exception innerException)
        : Exception($"Entity \"{name}\" ({key}) can't be created.", innerException);
}
