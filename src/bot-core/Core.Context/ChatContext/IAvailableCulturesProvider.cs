using System;
using System.Globalization;

namespace GarageGroup.Infra.Telegram.Bot;

public interface IAvailableCulturesProvider
{
    FlatArray<CultureInfo> AvailableCultures { get; }
}