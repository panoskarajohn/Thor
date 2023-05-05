namespace Shared.Exception;

public interface IExceptionToResponseMapper
{
    ExceptionResponse Map(System.Exception exception);
}