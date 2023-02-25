using Microsoft.Extensions.Logging;

namespace Reagent.Messages;

/// <summary>
/// A <c>RegularEventMessage</c> is a message that is sent at a regular interval.
/// </summary>
public class RegularEventMessage : IMessage
{
    /// <summary>
    /// Create a new <c>RegularEventMessage</c>.
    /// </summary>
    /// <param name="agentGuid">The <c>Guid</c> of the <c>Agent</c>.</param>
    /// <param name="guid">The <c>Guid</c> of the <c>RegularEventMessage</c>.</param>
    /// <param name="logger">The logger.</param>
    private RegularEventMessage(Guid agentGuid, Guid guid, ILogger<RegularEventMessage> logger)
    {
        Guid = guid;
        Destination = agentGuid;
        _logger = logger;
    }

    // ReSharper disable once SuggestBaseTypeForParameterInConstructor
    /// <summary>
    /// Create a new <c>RegularEventMessage</c>.
    /// </summary>
    /// <param name="agent">The <c>Agent</c>.</param>
    /// <param name="guid">The <c>Guid</c> of the <c>RegularEventMessage</c>.</param>
    /// <param name="logger">The logger.</param>
    public RegularEventMessage(Agent.Agent agent, Guid guid, ILogger<RegularEventMessage> logger) : this(agent.Guid,
        guid, logger)
    {
    }

    /// <summary>
    /// Create a new <c>RegularEventMessage</c> with a random <c>Guid</c>.
    /// </summary>
    /// <param name="agent">The <c>Agent</c>.</param>
    /// <param name="logger">The logger.</param>
    public RegularEventMessage(Agent.Agent agent, ILogger<RegularEventMessage> logger) : this(agent, Guid.NewGuid(),
        logger)
    {
    }

    /// <summary>
    /// The <c>Guid</c> of the <c>RegularEventMessage</c>.
    /// </summary>
    public Guid Guid { get; }

    /// <summary>
    /// The logger.
    /// </summary>
    private readonly ILogger<RegularEventMessage> _logger;

    /// <summary>
    /// The <c>Guid</c> of the <c>Agent</c> that the message is sent to.
    /// </summary>
    public Guid Destination { get; }

    /// <summary>
    /// The <c>Guid</c> of the <c>Agent</c> that sent the message.
    /// </summary>
    public Guid Sender => Destination;

    /// <summary>
    /// Clone the <c>RegularEventMessage</c> with a new <c>Guid</c>.
    /// </summary>
    /// <param name="guid">The new <c>Guid</c>.</param>
    /// <returns>The cloned <c>RegularEventMessage</c>.</returns>
    protected virtual RegularEventMessage Clone(Guid guid)
    {
        _logger.LogTrace("Cloning {Message} with new Guid {Guid}", this, guid);
        return new RegularEventMessage(Destination, guid, _logger);
    }

    /// <summary>
    /// Clone the <c>RegularEventMessage</c> with a new random <c>Guid</c>.
    /// </summary>
    /// <returns>The cloned <c>RegularEventMessage</c>.</returns>
    protected virtual RegularEventMessage Clone() => Clone(Guid.NewGuid());

    /// <summary>
    /// Get a string representation of the <c>RegularEventMessage</c>.
    /// </summary>
    /// <returns>
    /// The string representation of the <c>RegularEventMessage</c>.
    /// </returns>
    public override string ToString() =>
        $"RegularEventMessage(Sender={Sender}, Destination={Destination}, Guid={Guid})";

    /// <summary>
    /// The logger for the static methods.
    /// </summary>
    public static ILogger<RegularEventMessage>? StaticLogger { get; set; } = null;

    /// <summary>
    /// Create and schedule messages like the prototype message.
    /// </summary>
    /// <param name="prototypeMessage">
    /// The prototype message. The <c>Guid</c> of the prototype message is ignored.
    /// </param>
    /// <param name="interval">
    /// The interval between the messages. The interval must be greater than zero.
    /// </param>
    /// <param name="startTime">
    /// The time at which the first message is sent. The start time must be before the end time.
    /// </param>
    /// <param name="endTime">
    /// The time at which the last message is sent (exclusive). The end time must be after the start time.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown if the start time is after the end time or the interval is zero.
    /// </exception>
    public static void CreateAndScheduleMessages(RegularEventMessage prototypeMessage, TimeSpan interval,
        DateTime startTime, DateTime endTime)
    {
        if (startTime > endTime)
        {
            throw new ArgumentException("The start time is after the end time", nameof(startTime));
        }

        if (interval.Equals(TimeSpan.Zero))
        {
            throw new ArgumentException("The interval is zero", nameof(interval));
        }
        
        if (interval < TimeSpan.Zero)
        {
            throw new ArgumentException("The interval is negative", nameof(interval));
        }

        StaticLogger!.LogDebug(
            "Creating and scheduling messages like {PrototypeMessage} from {StartTime} to {EndTime} with interval {Interval}",
            prototypeMessage, startTime, endTime, interval);

        var currentTime = startTime;
        while (currentTime < endTime)
        {
            var message = prototypeMessage.Clone();
            StaticLogger!.LogDebug("Scheduling message {Message} at time {Time}", message, currentTime);
            SimulationManager.SimulationManager.Instance!.ScheduleMessage(message, currentTime);
            currentTime += interval;
        }
    }
}