using System;
using System.Globalization;

namespace GarageGroup.Infra.Telegram.Bot;

partial class ContextExtensions
{
    internal static FlatArray<CultureInfo> GetAvailableCultures(this IChatContext context)
        =>
        context is not IAvailableCulturesProvider culturesProvider ? default : culturesProvider.AvailableCultures;
}