namespace Efficy.Domain.Exceptions;

public class NotFoundException: Exception
{
    public NotFoundException(string key) : base($"Object was not found. Key: {key}")
    {
    }
}