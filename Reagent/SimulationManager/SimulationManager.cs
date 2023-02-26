using Microsoft.Extensions.Logging;
using Reagent.Messages;

namespace Reagent.SimulationManager;

/// <summary>
/// The <c>SimulationManager</c> is responsible for managing the simulation.
/// </summary>
public class SimulationManager
{
    /// <summary>
    /// The time at which the simulation starts.
    /// </summary>
    public virtual DateTime StartTime { get; }

    /// <summary>
    /// The time at which the simulation ends.
    /// </summary>
    public virtual DateTime EndTime { get; }

    /// <summary>
    /// The current time of the simulation.
    /// </summary>
    private DateTime _currentTime;

    /// <summary>
    /// The current time of the simulation.
    /// </summary>
    public virtual DateTime CurrentTime
    {
        get => _currentTime;
        protected set => _currentTime = value;
    }

    /// <summary>
    /// The logger for the <c>SimulationManager</c>.
    /// </summary>
    private readonly ILogger<SimulationManager> _logger;

    /// <summary>
    /// The queue of messages to be sent.
    /// </summary>
    protected virtual SortedDictionary<DateTime, Queue<IMessage>> MessageQueue { get; set; } = new();

    /// <summary>
    /// The agents in the simulation.
    /// </summary>
    protected virtual IDictionary<Guid, Agent.Agent> Agents { get; set; } = new Dictionary<Guid, Agent.Agent>();

    /// <summary>
    /// Creates a new <c>SimulationManager</c>.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="startTime">The start time of the simulation.</param>
    /// <param name="endTime">The end time of the simulation.</param>
    public SimulationManager(ILogger<SimulationManager> logger, DateTime startTime, DateTime endTime)
    {
        _logger = logger;
        StartTime = startTime;
        EndTime = endTime;
        _currentTime = startTime;
    }

    /// <summary>
    /// Schedules a message to be sent at a specific time.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="time">The time.</param>
    /// <exception cref="ArgumentOutOfRangeException">If the time is not between start time and end time or is before the current time.</exception>
    public virtual void ScheduleMessage(IMessage message, DateTime time)
    {
        CheckTimeIsValid(time);


        if (MessageQueue.TryGetValue(time, out var messages))
        {
            _logger.LogDebug("Adding message {Message} to existing queue", message);
        }
        else
        {
            _logger.LogDebug("Adding message {Message} to a new queue", message);
            messages = new Queue<IMessage>();
            MessageQueue[time] = messages;
        }

        MessageQueue[time].Enqueue(message);
    }

    /// <summary>
    /// Checks if a time is valid.
    /// </summary>
    /// <param name="time">The time</param>
    /// <exception cref="ArgumentOutOfRangeException">If the time is not between start time and end time or is before the current time.</exception>
    protected virtual void CheckTimeIsValid(DateTime time)
    {
        if (time < StartTime)
        {
            throw new ArgumentOutOfRangeException(nameof(time), time,
                $"The time is before the start time: {StartTime}");
        }

        if (time > EndTime)
        {
            throw new ArgumentOutOfRangeException(nameof(time), time, $"The time is after the end time: ${EndTime}");
        }

        if (time < CurrentTime)
        {
            throw new ArgumentOutOfRangeException(nameof(time), time,
                $"The time is before the current time: ${CurrentTime}");
        }
    }

    /// <summary>
    /// Adds an agent to the simulation.
    /// </summary>
    /// <param name="agent">The agent.</param>
    public virtual void AddAgent(Agent.Agent agent)
    {
        _logger.LogDebug("Adding agent {Agent} to the simulation", agent);
        Agents[agent.Guid] = agent;
    }

    /// <summary>
    /// Runs the simulation for a specific time.
    /// </summary>
    /// <param name="time">The time.</param>
    /// <exception cref="InvalidOperationException">If a message is sent to an unknown agent.</exception>
    /// <exception cref="ArgumentOutOfRangeException">If the time is not between start time and end time or is before the current time.</exception>
    protected virtual void RunForTime(DateTime time)
    {
        _logger.LogInformation("Running simulation for {Time}", CurrentTime);
        CheckTimeIsValid(time);

        while (MessageQueue[time].Any())
        {
            var message = MessageQueue[time].Dequeue();
            if (Agents.TryGetValue(message.Destination, out var agent))
            {
                _logger.LogDebug("Sending message {Message} to destination {Destination}", message, agent);
            }
            else
            {
                throw new InvalidOperationException(
                    $"Trying to send message {message} to unknown agent with Guid: {message.Destination}");
            }
        }

        MessageQueue.Remove(time);
        _logger.LogTrace("Removed queue for time {Time}", time);
    }

    /// <summary>
    /// Send a message immediately by adding it to the back of the <c>MessageQueue</c> at the current time.
    /// </summary>
    /// <param name="message">The message.</param>
    public virtual void SendMessageNow(IMessage message)
    {
        _logger.LogDebug("Adding message {Message} to queue with current time {CurrentTime}", message, CurrentTime);
        if (!MessageQueue.TryGetValue(CurrentTime, out var queue))
        {
            queue = new Queue<IMessage>();
            MessageQueue[CurrentTime] = queue;
        }

        queue.Enqueue(message);
    }

    /// <summary>
    /// Run the simulation between the start and end time.
    /// </summary>
    /// <exception cref="InvalidOperationException">If a message is sent to an unknown agent.</exception>
    /// <exception cref="ArgumentOutOfRangeException">If the time is not between start time and end time or is before the current time.</exception>
    public virtual void Run()
    {
        _logger.LogInformation("Starting simulation at time {CurrentTime}", CurrentTime);
        while (CurrentTime < EndTime && MessageQueue.Any())
        {
            var nextTime = MessageQueue.First().Key;
            CurrentTime = nextTime;
            RunForTime(nextTime);
        }

        _logger.LogInformation("Simulation finished at time {CurrentTime}", CurrentTime);
    }

    /// <summary>
    /// Get a string representation of the <c>SimulationManager</c>.
    /// </summary>
    /// <returns>The string representation.</returns>
    public override string ToString()
    {
        return $"SimulationManager(StartTime={StartTime}, EndTime={EndTime}, CurrentTime={CurrentTime})";
    }
}