using Reagent.Messages;
using Reagent.Properties;

namespace Reagent.Agent;

/// <summary>
/// Class <c>Agent</c> represents an agent in the simulation.
/// </summary>
public abstract class Agent : IGuidD
{
    /// <summary>
    /// The <c>Guid</c> which uniquely identifies the <c>Agent</c>.
    /// </summary>
    public Guid Guid { get; }

    // ReSharper disable once MemberCanBePrivate.Global
    /// <summary>
    /// Create a new <c>Agent</c>.
    /// </summary>
    /// <param name="guid">The <c>Guid</c> of the <c>Agent</c></param>
    protected Agent(Guid guid)
    {
        Guid = guid;
    }

    /// <summary>
    /// Create a new <c>Agent</c> with a randomly generated <c>Guid</c>.
    /// </summary>
    protected Agent() : this(Guid.NewGuid())
    {
    }

    /// <summary>
    /// Handle a message.
    /// </summary>
    /// <param name="message">The message.</param>
    public abstract void HandleMessage(IMessage message);

    /// <summary>
    /// Create a string representation of the <c>Agent</c>.
    /// </summary>
    /// <returns>The string representation</returns>
    public override string ToString()
    {
        return $"Agent(Guid={Guid})";
    }
}