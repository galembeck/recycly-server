namespace Domain.Utils.Constants;

public sealed record EmailServiceSettings
{
    public string ApiToken { get; init; }
    public string FromEmail { get; init; }
    public string FromName { get; init; }
}
