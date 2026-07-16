namespace CodeYield.Mediator
{
    /// <summary>
    /// Marker interface for a command that does not return a value.
    /// </summary>
    public interface ICommand { }

    /// <summary>
    /// Represents a command that returns a response of type <typeparamref name="TResponse"/>.
    /// </summary>
    /// <typeparam name="TResponse">The type of data returned after executing the command.</typeparam>
    public interface ICommand<TResponse> { }
}
