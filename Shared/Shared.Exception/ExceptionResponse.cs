using System.Net;

namespace Shared.Exception;

public record ExceptionResponse(object Response, HttpStatusCode StatusCode);