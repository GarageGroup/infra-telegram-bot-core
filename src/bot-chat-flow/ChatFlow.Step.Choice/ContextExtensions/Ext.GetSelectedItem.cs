using System;
using System.Text.Json;

namespace GarageGroup.Infra.Telegram.Bot;

partial class ChatFlowContextExtensions
{
    internal static ChoiceStepItem<TValue>? GetSelectedItem<TValue>(this IChatFlowContextBase context, ChoiceStepState state)
    {
        if (string.IsNullOrEmpty(context.Update.CallbackQuery?.Data))
        {
            return null;
        }

        foreach (var item in state.Items)
        {
            if (string.Equals(item.Id, context.Update.CallbackQuery.Data) is false)
            {
                continue;
            }

            return new(
                id: item.Id.OrEmpty(),
                title: item.Title.OrEmpty(),
                value: item.Value.Deserialize<TValue>(SerializerOptions)!);
        }

        return null;
    }
}