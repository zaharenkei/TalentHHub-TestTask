namespace TalentHHub_TestTask.Handlers
{
    public interface IHandler<TRequest, TResponse>
    {
        ValueTask<TResponse> Handle (TRequest request, CancellationToken token);
    }
}
