namespace Reagent.Messages;

/// <summary>
/// A <c>WakeUpMessage</c> is an <c>IMessage</c> that is sent to an <c>Agent</c> to wake it up at a specific time.
/// </summary>
public class WakeUpMessage : IMessage
{
    // ReSharper disable once SuggestBaseTypeForParameterInConstructor
    /// <summary>
    /// Create a new <c>WakeUpMessage</c> scheduling it.
    /// </summary>
    /// <param name="agent">The <c>Agent</c>.</param>
    /// <param name="wakeTime">The <c>DateTime</c> to wake up.</param>
    /// <param name="guid">The <c>Guid> of the <c>WakeUpMessage</c>.</param>
    public WakeUpMessage(Agent.Agent agent, DateTime wakeTime, Guid guid)
    {
        Guid = guid;
        _agentGuid = agent.Guid;
        WakeTime = wakeTime;

        SimulationManager.SimulationManager.Instance!.ScheduleMessage(this, wakeTime);
    }
    
    /// <summary>
    /// Create a new <c>WakeUpMessage</c> scheduling it with a randomly generated <c>Guid</c>.
    /// </summary>
    /// <param name="agent">The <c>Agent</c>.</param>
    /// <param name="wakeTime">The <c>DateTime</c> to wake up.</param>
    public WakeUpMessage(Agent.Agent agent, DateTime wakeTime) : this(agent, wakeTime, Guid.NewGuid())
    {
    }

    // ReSharper disable once MemberCanBeProtected.Global
    /// <summary>
    /// The <c>DateTime</c> to wake up.
    /// </summary>
    public virtual DateTime WakeTime { get; }

    /// <summary>
    /// The <c>Guid</c> of the <c>Agent</c> that the message is sent to.
    /// </summary>
    private readonly Guid _agentGuid;
    
    /// <summary>
    /// The <c>Guid</c> of the <c>WakeUpMessage</c>.
    /// </summary>
    public virtual Guid Guid { get; }

    /// <summary>
    /// The <c>Guid</c> of the <c>Agent</c> that the message is sent to.
    /// </summary>
    public virtual Guid Destination => _agentGuid;

    /// <summary>
    /// The <c>Guid</c> of the <c>Agent</c> that sent the message.
    /// </summary>
    public virtual Guid Sender => _agentGuid;
    
    /// <summary>
    /// Create a string representation of the <c>WakeUpMessage</c>.
    /// </summary>
    /// <returns>The string representation.</returns>
    public override string ToString()
    {
        return $"WakeUpMessage(Sender={Sender}, Destination={Destination}, Guid={Guid}, WakeTime={WakeTime})";
    }
}