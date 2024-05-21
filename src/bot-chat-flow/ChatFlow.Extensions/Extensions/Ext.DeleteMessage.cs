using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

partial class ChatFlowExtensions
{
    public static ChatFlow<T> DeleteMessages<T>(
        this ChatFlow<T> chatFlow,
        Func<IChatFlowContext<T>, FlatArray<int>> messageIdSetFactory)
    {
        ArgumentNullException.ThrowIfNull(chatFlow);
        ArgumentNullException.ThrowIfNull(messageIdSetFactory);

        return chatFlow.On(InnerInvokeAsync);

        Task InnerInvokeAsync(IChatFlowContext<T> context, CancellationToken cancellationToken)
        {
            var messageIdSet = messageIdSetFactory.Invoke(context);
            return context.Api.DeleteMessageSetAsync(messageIdSet, cancellationToken);
        }
    }
}