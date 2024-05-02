using System.Text.Json;
using System.Text.Json.Serialization;

namespace Core.CallMyTrade;

public static class Utils
{
    public static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions()
    {
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };
}