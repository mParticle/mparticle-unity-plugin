using System;
using System.Collections.Generic;

public class MPEvent
{
    private string category;
    private long duration;
    private DateTime endTime;
    private Dictionary<string, string> info;
    private string name;
    private DateTime startTime;
    private MParticle.EventType eventType;
    
    /// <summary>
    /// Category is a string with a developer/company defined category of the event.
    /// </summary>
    public string Category
    {
        get
        {
            return category;
        }
        set
        {
            category = value;
        }
    }

    /// <summary>
    /// The duration, in milliseconds, of an event. This property can be set by a developer, or
    /// it can be calculated automatically by the mParticle SDK using the beginTiming/endTiming
    /// methods.
    /// </summary>
    public long Duration
    {
        get
        {
            return duration;
        }
        set
        {
            duration = value;
        }
    }

    /// <summary>
    /// If using the beginTiming/endTiming methods, this property contains the time the
    /// event ended. Otherwise it is nil.
    /// </summary>
    public DateTime EndTime
    {
        get
        {
            return endTime;
        }
        set
        {
            endTime = value;
        }
    }

    /// <summary>
    /// A dictionary containing further information about the event. The number of entries is 
    /// limited to 100 key value pairs. Keys must be strings (up to 255 characters) and values 
    /// can be strings (up to 255 characters), numbers, booleans, or dates
    /// </summary>
    public Dictionary<string, string> Info
    {
        get
        {
            return info;
        }
        set
        {
            info = value;
        }
    }

    /// <summary>
    /// The name of the event to be logged (required not nil). The event name must not contain
    /// more than 255 characters.
    /// </summary>
    public string Name
    {
        get
        {
            return name;
        }
        set
        {
            name = value;
        }
    }

    /// <summary>
    /// If using the beginTiming/endTiming methods, this property contains the time the
    /// event started. Otherwise it is nil.
    /// </summary>
    public DateTime StartTime
    {
        get
        {
            return startTime;
        }
        set
        {
            startTime = value;
        }
    }

    /// <summary>
    /// An enum value that indicates the type of event to be logged. If logging a screen event, this 
    /// property will be overridden to MPEventTypeNavigation. In all other cases the SDK will honor the type
    /// assigned to this property.
    /// @see MPEventType
    /// </summary>
    public MParticle.EventType EventType
    {
        get
        {
            return eventType;
        }
        set
        {
            eventType = value;
        }
    }

    /// <summary>
    /// MPEvent constructor
    /// </summary>
    /// <param name="eventName">The name of the event to be logged (required not null). The event name must not contain more than 255 characters.</param>
    /// <param name="eventType">An enum value that indicates the type of event to be logged.</param>
    /// <returns>An instance of MPEvent.</returns>
    public MPEvent (string eventName, MParticle.EventType eventType)
    {
        this.name = eventName;
        this.eventType = eventType;
        this.Duration = 0;
        this.StartTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        this.EndTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    }
}
