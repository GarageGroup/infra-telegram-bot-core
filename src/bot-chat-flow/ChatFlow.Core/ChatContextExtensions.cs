using System;

namespace GarageGroup.Infra.Telegram.Bot;

public static class ChatContextExtensions
{
    public static ChatFlow<TFlow> StartChatFlow<TFlow>(this IChatContext context, string chatFlowId, Func<TFlow> initialFactory)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(initialFactory);

        return new(chatFlowId.OrEmpty(), context, initialFactory);
    }

    public static ChatFlow<TFlow> StartChatFlow<TIn, TOut, TFlow>(this ChatCommandRequest<TIn, TOut> request, Func<TIn, TFlow> initialFactory)
        where TIn : IChatCommandIn<TOut>
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(initialFactory);

        return new(TIn.Type, request.Context, InnerInit);

        TFlow InnerInit()
            =>
            initialFactory.Invoke(request.Value);
    }
}