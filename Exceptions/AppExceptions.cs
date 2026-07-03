namespace TravelApp.Infrastructure.Data.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
    public NotFoundException(string entity, object key) : base($"{entity} với id '{key}' không tồn tại.") { }
}
public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message) { }
}

public class UnauthorizedException : Exception
{
    public UnauthorizedException(string message = "Bạn không có quyền truy cập.") : base(message) { }
}
 
public class ConflictException : Exception
{
    public ConflictException(string message) : base(message) { }
}
