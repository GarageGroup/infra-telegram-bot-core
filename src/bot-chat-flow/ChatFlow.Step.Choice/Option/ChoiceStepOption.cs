using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed class ChoiceStepOption<T, TValue>
{
    private readonly Func<ChoiceStepRequest, CancellationToken, ValueTask<Result<ChoiceStepSet<TValue>, ChatRepeatState>>> choiceSetFactory;

    private readonly Func<ChoiceStepItem<TValue>, ChatMessageSendRequest> resultMessageFactory;

    private readonly Func<ChoiceStepItem<TValue>, T> selectedItemMapper;

    public ChoiceStepOption(
        Func<ChoiceStepRequest, CancellationToken, ValueTask<Result<ChoiceStepSet<TValue>, ChatRepeatState>>> choiceSetFactory,
        Func<ChoiceStepItem<TValue>, ChatMessageSendRequest> resultMessageFactory,
        Func<ChoiceStepItem<TValue>, T> selectedItemMapper)
    {
        this.choiceSetFactory = choiceSetFactory;
        this.resultMessageFactory = resultMessageFactory;
        this.selectedItemMapper = selectedItemMapper;
    }

    public ChoiceStepOption(
        ChoiceStepSet<TValue> choiceSet,
        Func<ChoiceStepItem<TValue>, ChatMessageSendRequest> resultMessageFactory,
        Func<ChoiceStepItem<TValue>, T> selectedItemMapper)
    {
        choiceSetFactory = InnerGetChoiceSetValueTask;
        this.resultMessageFactory = resultMessageFactory;
        this.selectedItemMapper = selectedItemMapper;

        ValueTask<Result<ChoiceStepSet<TValue>, ChatRepeatState>> InnerGetChoiceSetValueTask(
            ChoiceStepRequest _, CancellationToken token)
            =>
            new(choiceSet);
    }

    public ValueTask<Result<ChoiceStepSet<TValue>, ChatRepeatState>> GetChoicesAsync(
        ChoiceStepRequest request, CancellationToken cancellationToken)
        =>
        choiceSetFactory.Invoke(request, cancellationToken);

    public ChatMessageSendRequest CreateResultMessage(ChoiceStepItem<TValue> selectedItem)
        =>
        resultMessageFactory.Invoke(selectedItem);

    public T MapFlowState(ChoiceStepItem<TValue> selectedItem)
        =>
        selectedItemMapper.Invoke(selectedItem);
}