using System.Threading;

namespace Infrastructure.Application
{
    /// <summary>
    /// Represents a life cycle manager that provides access to cancellation tokens.
    /// </summary>
    public interface ILifeCycleManager
    {
        /// <summary>
        /// Gets the CancellationTokenSource associated with this life cycle manager.
        /// </summary>
        CancellationTokenSource Source { get; }

        /// <summary>
        /// Gets the CancellationToken associated with this life cycle manager.
        /// </summary>
        CancellationToken Token { get; }
    }
}
