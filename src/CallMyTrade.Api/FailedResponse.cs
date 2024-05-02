namespace CallMyTrade;

public sealed record FailedResponse
{
    public List<ValidationError>? ValidationErrors { get; init; }
    
    public string? ExceptionMessage { get; init; }
    
    public string? ExceptionStackTrace { get; init; }
}

public sealed record ValidationError
{
    public string? ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }
} 