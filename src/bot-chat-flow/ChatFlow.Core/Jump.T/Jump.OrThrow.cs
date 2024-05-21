using System;
using static System.FormattableString;

namespace GarageGroup.Infra.Telegram.Bot;

partial record struct ChatFlowJump<T>
{
    public T NextStateOrThrow()
        =>
        InnerNextStateOrThrow(
            CreateTagMustBeNextException);

    public T NextStateOrThrow(Func<Exception> exceptionFactory)
        =>
        InnerNextStateOrThrow(
            exceptionFactory ?? throw new ArgumentNullException(nameof(exceptionFactory)));

    public T RestartStateOrThrow()
        =>
        InnerRestartStateOrThrow(
            CreateTagMustBeRestartException);

    public T RestartStateOrThrow(Func<Exception> exceptionFactory)
        =>
        InnerRestartStateOrThrow(
            exceptionFactory ?? throw new ArgumentNullException(nameof(exceptionFactory)));

    public ChatBreakState BreakStateOrThrow()
        =>
        InnerBreakStateOrThrow(
            CreateTagMustBeBreakException);

    public ChatBreakState BreakStateOrThrow(Func<Exception> exceptionFactory)
        =>
        InnerBreakStateOrThrow(
            exceptionFactory ?? throw new ArgumentNullException(nameof(exceptionFactory)));

    public ChatRepeatState RepeatStateOrThrow()
        =>
        InnerRepeatStateOrThrow(
            CreateTagMustBeRepeatException);

    public ChatRepeatState RepeatStateOrThrow(Func<Exception> exceptionFactory)
        =>
        InnerRepeatStateOrThrow(
            exceptionFactory ?? throw new ArgumentNullException(nameof(exceptionFactory)));

    private T InnerNextStateOrThrow(Func<Exception> exceptionFactory)
        =>
        Tag is ChatFlowJumpTag.Next ? nextState : throw exceptionFactory.Invoke();

    private T InnerRestartStateOrThrow(Func<Exception> exceptionFactory)
        =>
        Tag is ChatFlowJumpTag.Restart ? nextState : throw exceptionFactory.Invoke();

    private ChatBreakState InnerBreakStateOrThrow(Func<Exception> exceptionFactory)
        =>
        Tag is ChatFlowJumpTag.Break ? breakState : throw exceptionFactory.Invoke();

    private ChatRepeatState InnerRepeatStateOrThrow(Func<Exception> exceptionFactory)
        =>
        Tag is ChatFlowJumpTag.Repeat ? repeatState : throw exceptionFactory.Invoke();

    private InvalidOperationException CreateTagMustBeNextException()
        =>
        CreateUnexpectedTagException(ChatFlowJumpTag.Next);

    private InvalidOperationException CreateTagMustBeRestartException()
        =>
        CreateUnexpectedTagException(ChatFlowJumpTag.Restart);

    private InvalidOperationException CreateTagMustBeBreakException()
        =>
        CreateUnexpectedTagException(ChatFlowJumpTag.Break);

    private InvalidOperationException CreateTagMustBeRepeatException()
        =>
        CreateUnexpectedTagException(ChatFlowJumpTag.Repeat);

    private InvalidOperationException CreateUnexpectedTagException(ChatFlowJumpTag expectedTag)
        =>
        new(Invariant($"The step result tag must be {expectedTag} but it is {Tag}."));
}