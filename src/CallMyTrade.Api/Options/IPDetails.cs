namespace CallMyTrade.Options;

internal sealed class IPDetails
{
    public string? Prefix { get; set; }
    public string? PrefixLength { get; set; }

    public override string ToString()
    {
        if (!string.IsNullOrWhiteSpace(Prefix) && !string.IsNullOrWhiteSpace(PrefixLength))
        {
            return Prefix + '/' + PrefixLength;
        }

        return string.Empty;
    }
}