namespace FoodShareNet.Application.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(String name, object key) : base($"Entity '{name}' ({key}) was not found")
    {
        
    }

    public NotFoundException(string message) : base(message)
    {
        
    }
    
}