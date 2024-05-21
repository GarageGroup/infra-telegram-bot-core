using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

partial class ChatFlowContextExtensions
{
    internal static async ValueTask<ChatFlowJump<T>> ShowChoiceSetAsync<T, TValue>(
        this IChatFlowContextBase context,
        ChoiceStepState state,
        ChoiceStepOption<T, TValue> option,
        CancellationToken cancellationToken)
    {
        var request = new ChoiceStepRequest
        {
            Text = state.MessageIds.IsEmpty ? default : context.Update.Message?.Text
        };

        var typingTask = context.Api.SendChatActionAsync(BotChatAction.Typing, cancellationToken);
        var choiceSetTask = option.GetChoicesAsync(request, cancellationToken).AsTask();

        await Task.WhenAll(typingTask, choiceSetTask).ConfigureAwait(false);

        var result = await choiceSetTask.Result.MapSuccessAsync(InnerRepeatWithChoiceSetAsync).ConfigureAwait(false);
        return result.Fold(ChatFlowJump.Repeat<T>, ChatFlowJump.Repeat<T>);

        async Task<ChatRepeatState> InnerRepeatWithChoiceSetAsync(ChoiceStepSet<TValue> choiceStepSet)
        {
            var message = await context.SendChoiceSetAsync(choiceStepSet, cancellationToken).ConfigureAwait(false);

            var nextState = state.MakeNextState(message, choiceStepSet);
            context.StepState.Set(nextState);

            return default;
        }
    }

    private static Task<BotMessage> SendChoiceSetAsync<TValue>(
        this IChatFlowContextBase context, ChoiceStepSet<TValue> choiceStepSet, CancellationToken cancellationToken)
    {
        var request = new ChatMessageSendRequest(choiceStepSet.ChoiceText)
        {
            ReplyMarkup = choiceStepSet.Items.IsEmpty ? null : new BotInlineKeyboardMarkup
            {
                InlineKeyboard = choiceStepSet.Items.Map(CreateInlineButtonRow)
            }
        };

        return context.Api.SendMessageAsync(request, cancellationToken);

        static FlatArray<BotInlineKeyboardButton> CreateInlineButtonRow(ChoiceStepItem<TValue> item)
            =>
            [
                new(item.Title)
                {
                    CallbackData = item.Id
                }
            ];
    }

    private static ChoiceStepState MakeNextState<TValue>(
        this ChoiceStepState state, BotMessage message, ChoiceStepSet<TValue> choiceStepSet)
    {
        if (choiceStepSet.Items.IsEmpty)
        {
            return state with
            {
                MessageIds = state.MessageIds.Concat(message.MessageId)
            };
        }

        var items = state.Items.AsEnumerable().ToDictionary(GetId);

        foreach (var item in choiceStepSet.Items)
        {
            items[item.Id] = new()
            {
                Id = item.Id,
                Title = item.Title,
                Value = JsonSerializer.SerializeToElement(item.Value, SerializerOptions)
            };
        }

        return state with
        {
            MessageIds = state.MessageIds.Concat(message.MessageId),
            Items = items.Values.ToFlatArray()
        };

        static string GetId(ChoiceStepStateItem item)
            =>
            item.Id.OrEmpty();
    }
}