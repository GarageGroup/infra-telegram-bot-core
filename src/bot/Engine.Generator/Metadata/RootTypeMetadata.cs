namespace GarageGroup.Infra.Telegram.Bot;

internal sealed record class RootTypeMetadata
{
    public RootTypeMetadata(string typeName, string @namespace)
    {
        TypeName = typeName ?? string.Empty;
        Namespace = @namespace ?? string.Empty;
    }

    public string TypeName { get; }

    public string Namespace { get; }
}