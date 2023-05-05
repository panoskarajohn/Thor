namespace Shared.Exception;

public interface IExceptionCompositionRoot
{
    ExceptionResponse Map(System.Exception exception);
}