namespace CodeYield.Mediator
{
    /// <summary>
    /// Handles a command that does not return a value.
    /// </summary>
    /// <typeparam name="TCommand">The command type.</typeparam>
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        /// <summary>Executes the command.</summary>
        Task HandleAsync(TCommand command, CancellationToken ct = default);
    }

    /// <summary>
    /// Handles a command and returns a response.
    /// </summary>
    /// <typeparam name="TCommand">The command type.</typeparam>
    /// <typeparam name="TResponse">The response type.</typeparam>
    public interface ICommandHandler<in TCommand, TResponse> where TCommand : ICommand<TResponse>
    {
        /// <summary>Executes the command and returns the result.</summary>
        Task<TResponse> HandleAsync(TCommand command, CancellationToken ct = default);
    }
}
