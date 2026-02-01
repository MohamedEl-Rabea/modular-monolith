namespace WT.B2C.API.BuildingBlocks.Types;

public sealed record OperationResult<T>(T? Value, bool Success, string ErrorMessage)
{
    public static OperationResult<T> Ok() =>
        new(default, true, string.Empty);

    public static OperationResult<T> Ok(T value) =>
        new(value, true, string.Empty);

    public static OperationResult<T> Fail(string error) =>
        new(default, false, error);
}