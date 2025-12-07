using System.Collections.Immutable;
using System.Net;
using System.Text.Json.Serialization;

namespace WT.Customers.Portal.BuildingBlocks.Types;

public class OperationResult<TData>
{
    public string RequestId { get; set; }
    public bool Succeeded { get; set; }
    public bool IsFile { get; set; }
    public int StatusCode { get; set; }
    public TData? Data { get; set; }
    public ImmutableList<ResultMessage> Messages { get; set; } = Enumerable.Empty<ResultMessage>().ToImmutableList();

    public static OperationResult<TData> Success(TData data) => SuccessResult(data, (int)HttpStatusCode.OK,
        new ResultMessage("Success", ResultSeverity.Information));

    public static OperationResult<FileOutputDto> File(FileOutputDto data) => new()
    {
        Data = data,
        StatusCode = (int)HttpStatusCode.Created,
        Succeeded = true,
        IsFile = true
    };

    public static OperationResult<TData> Success(TData data, params ResultMessage[] messages) =>
        SuccessResult(data, (int)HttpStatusCode.OK, messages);

    public static OperationResult<TData> Created(TData data) => SuccessResult(data, (int)HttpStatusCode.Created,
        new ResultMessage("ResourceCreated", ResultSeverity.Information));

    public static OperationResult<TData> NotFound() => NotFound("NotFound");
    public static OperationResult<TData> BadRequest() => BadRequest("BadRequest");

    public static OperationResult<TData> NotFound(params string[] messages) => FailResult((int)HttpStatusCode.NotFound,
        messages.Select(m => new ResultMessage(m, ResultSeverity.Error)).ToArray());

    public static OperationResult<TData> BadRequest(params string[] messages) => FailResult(
        (int)HttpStatusCode.BadRequest, messages.Select(m => new ResultMessage(m, ResultSeverity.Error)).ToArray());

    public static OperationResult<TData> Conflict(params string[] messages) => FailResult((int)HttpStatusCode.Conflict,
        messages.Select(m => new ResultMessage(m, ResultSeverity.Error)).ToArray());

    public static OperationResult<TData> ServerError() => ServerError(Array.Empty<string>());

    public static OperationResult<TData> ServerError(params string[] messages) => FailResult(
        (int)HttpStatusCode.InternalServerError,
        messages.Select(m => new ResultMessage(m, ResultSeverity.Error)).ToArray());

    public static OperationResult<TData> SuccessResult(TData data, int statusCode, params ResultMessage[] messages) =>
        new OperationResult<TData>
        {
            Data = data,
            StatusCode = statusCode,
            Succeeded = true,
            Messages = messages != null
                ? messages.ToImmutableList()
                : Enumerable.Empty<ResultMessage>().ToImmutableList()
        };

    public static OperationResult<TData> FailResult(int statusCode, params ResultMessage[] messages) =>
        new OperationResult<TData>
        {
            StatusCode = statusCode,
            Messages = messages != null
                ? messages.ToImmutableList()
                : Enumerable.Empty<ResultMessage>().ToImmutableList()
        };
}

public class ResultMessage
{
    public ResultMessage(string message, ResultSeverity severity)
    {
        Message = message ?? throw new ArgumentNullException(nameof(message));
        Severity = severity;
    }

    public string Message { get; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ResultSeverity Severity { get; }
}

public enum ResultSeverity
{
    Information = 0,
    Warning = 1,
    Error = 2
}