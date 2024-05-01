using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

public readonly record struct BotMaskPosition
{
    [JsonConstructor]
    public BotMaskPosition(BotMaskPositionPoint point, float xShift, float yShift, float scale)
    {
        Point = point;
        XShift = xShift;
        YShift = yShift;
        Scale = scale;
    }

    public BotMaskPositionPoint Point { get; }

    public float XShift { get; }

    public float YShift { get; }

    public float Scale { get; }
}