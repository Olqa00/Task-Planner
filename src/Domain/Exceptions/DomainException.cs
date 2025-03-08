namespace Planner.Domain.Exceptions;

internal abstract class DomainException : Exception
{
    public Guid Id { get; protected init; } = Guid.Empty;

    protected DomainException(string message) : base(message)
    {
    }
}
