#if UNITY_IPHONE || UNITY_ANDROID
using UnityEngine;
using System.Collections.Generic;
using System;

public interface IMParticleSDK
{
    // Properties
    bool GetConsoleLogging ();
    void SetConsoleLogging (bool consoleLogging);
    MParticle.MPEnvironment GetEnvironment ();
    bool GetOptOut ();
    void SetOptOut (bool optOut);
    double GetSessionTimeout ();
    void SetSessionTimeout (double sessionTimeout);
    // Basic Tracking
    void LogEvent (MPEvent mpEvent);
    void LogEvent (string eventName, MParticle.EventType eventType, Dictionary<string, string> eventInfo, long startTime, long endTime, long duration, string category);
    void LogScreen (string screenName, Dictionary<string, string> eventInfo);
    // Error, Exception, and Crash Handling
    void BeginUncaughtExceptionLogging ();
    void EndUncaughtExceptionLogging ();
    void LeaveBreadcrumb (string breadcrumbName, Dictionary<string, string> eventInfo);
    void LogError (string message, Dictionary<string, string> eventInfo);
    void LogException (System.Exception exception, Dictionary<string, string> eventInfo, string message);
    void LogException (string condition, string stacktrace);
    // eCommerce Transactions
    void LogTransaction (MPProduct product);
    void LogLTVIncrease (decimal increaseAmount, string eventName, Dictionary<string, string> eventInfo);
    // Location
    void BeginLocationTracking (MParticle.LocationRange locationRange, long minTime, double minDistance);
    void EndLocationTracking ();
    // Network Performance Measurement
    void ExcludeUrlFromNetworkPerformanceMeasurement (string url);
    void LogNetworkPerformance (string url, long startTime, string method, long length, long bytesSent, long bytesReceived);
    // Push Notifications
    void DisablePushNotifications ();
    void EnablePushNotifications (string androidSenderId, uint iOSPushNotificationTypes);
    // Session management
    void BeginSession ();
    void EndSession ();
    void SetSessionAttribute (string key, string val);
    void Upload ();
    // User identity
    void SetUserAttribute (string key, string val);
    void SetUserIdentity (string identity, MParticle.UserIdentity identityType);
    void SetUserTag (string tag);
}

public class MParticle : MonoBehaviour, IMParticleSDK
{
    public enum MPEnvironment {AutoDetect = 0, Development, Production};

    public enum EventType {Unknown = 0, Navigation, Location, Search, Transaction, UserContent, UserPreference, Social, Other};

    public enum LocationRange {GPS = 1, Network = 500, Passive = 3000};

    public enum UserIdentity {Other = 0, CustomerId, Facebook, Twitter, Google, Microsoft, Yahoo, Email, Alias};

    private static MParticle instance;

    public static MParticle Instance
    {
        get { return instance ?? (instance = new GameObject("MParticle").AddComponent<MParticle>()); }
    }

    private IMParticleSDK mp;
    
    private IMParticleSDK mParticleInstance
    {
        get
        {
            if (mp == null)
            {
                #if UNITY_ANDROID
                    mp = new MParticleAndroid ();
                #elif UNITY_IPHONE
                    mp = new MParticleiOS ();
                #endif
            }
            return mp;
        }
    }

    void Awake ()
    {
        Application.RegisterLogCallback (HandleException);
    }

    private void HandleException (string condition, string stackTrace, LogType type)
    {
        if (type == LogType.Exception)
        {
            mParticleInstance.LogException(condition, stackTrace);
        }
    }

    //
    // Properties
    //

    /// <summary>
    /// Retrieves status of log output to the console. If true, it means logs will be output to the 
    /// console, if set to false the development logs will be suppressed. This property works in conjunction with 
    /// the environment property. If the environment is Production, consoleLogging will always be false, 
    /// regardless of the value you assign to it.
    /// </summary>
    /// <returns>The status of the log output to the console.</returns>
    public bool GetConsoleLogging ()
    {
        return mParticleInstance.GetConsoleLogging ();
    }

    /// <summary>
    /// Sets the status of log output to the console. If true, it means logs will be output to the 
    /// console, if set to false the development logs will be suppressed. This property works in conjunction with 
    /// the environment property. If the environment is Production, consoleLogging will always be false, 
    /// regardless of the value you assign to it.
    /// </summary>
    public void SetConsoleLogging (bool consoleLogging) 
    {
        mParticleInstance.SetConsoleLogging (consoleLogging);
    }

    /// <summary>
    /// Enables or disables log outputs to the console. If set to true development logs will be output to the 
    /// console, if set to false the development logs will be suppressed. This property works in conjunction with 
    /// the environment property. If the environment is Production, consoleLogging will always be false, 
    /// regardless of the value you assign to it.
    /// </summary>
    public bool ConsoleLogging
    {
        get { return mParticleInstance.GetConsoleLogging (); }
        set { mParticleInstance.SetConsoleLogging (value); }
    }
    
    /// <summary>
    /// Gets the SDK running environment. The possible values are Development or Production.
    /// </summary>
    /// <returns>Whether the SDK is running in Development or Production mode.</returns>
    public MParticle.MPEnvironment GetEnvironment ()
    {
        return mParticleInstance.GetEnvironment ();
    }

    /// <summary>
    /// Environment property for the mParticle SDK.
    /// The running environment determines key logging and debugging behaviors in the SDK.
    /// </summary>
    /// <returns>Whether the SDK is running in Development or Production mode.</returns>
    public MParticle.MPEnvironment Environment
    {
        get { return mParticleInstance.GetEnvironment (); }
    }

    /// <summary>
    /// Gets the opt-out status for the application; true indicates opting-out of event tracking.
    /// </summary>
    /// <returns>Whether the app has opted-out of event tracking.</returns>
    public bool GetOptOut ()
    {
        return mParticleInstance.GetOptOut ();
    }

    /// <summary>
    /// Sets the opt-out status for the application. Set it to true to opt-out of event tracking. Default value is false.
    /// </summary>
    /// <param name="optOut">The opt-out status.</param>
    public void SetOptOut (bool optOut)
    {
        mParticleInstance.SetOptOut (optOut);
    }

    /// <summary>
    /// Opt-out status property for the application; true indicates opting-out of event tracking.
    /// </summary>
    /// <returns>Whether the app has opted-out of event tracking.</returns>
    public bool OptOut
    {
        get { return mParticleInstance.GetOptOut (); }
        set { mParticleInstance.SetOptOut (value); }
    }

    /// <summary>
    /// Gets the the amount of time, in seconds, for an inactive session to time out.
    /// </summary>
    /// <returns>Session timeout in seconds.</returns>
    public double GetSessionTimeout ()
    {
        return mParticleInstance.GetSessionTimeout ();
    }

    /// <summary>
    /// Sets the the amount of time, in seconds, for an inactive session to time out.
    /// </summary>
    /// <param name="sessionTimeout">The amount of time, in seconds.</param>
    public void SetSessionTimeout (double sessionTimeout)
    {
        mParticleInstance.SetSessionTimeout (sessionTimeout);
    }
    
    /// <summary>
    /// Session timeout property. Gets or sets the the amount of time, in seconds, for an inactive session to time out.
    /// </summary>
    /// <returns>Session timeout in seconds.</returns>
    public double SessionTimeout
    {
        get { return mParticleInstance.GetSessionTimeout (); }
        set { mParticleInstance.SetSessionTimeout (value); }
    }

    //
    // Basic Tracking
    //

    /// <summary>
    /// Logs an event. This is one of the most fundamental method of the SDK. Developers define all the characteristics
    /// of an event (name, type, attributes, etc) in an instance of MPEvent and pass that instance to this method to 
    /// log its data to the mParticle SDK.
    /// </summary>
    /// <param name="mpEvent">An instance of MPEvent</param>
    public void LogEvent (MPEvent mpEvent)
    {
        mParticleInstance.LogEvent (mpEvent);
    }

    /// <summary>
    /// Logs an event. The eventInfo is limited to 100 key value pairs. 
    /// The eventName and strings in eventInfo cannot contain more than 255 characters.
    /// </summary>
    /// <param name="eventName">The name of the event to be tracked (required not null)</param>
    /// <param name="eventType">An enum value that indicates the type of event that is to be tracked.</param>
    /// <param name="eventInfo">A dictionary containing further information about the event.</param>
    public void LogEvent (string eventName, MParticle.EventType eventType, Dictionary<string, string> eventInfo)
    {
        mParticleInstance.LogEvent (eventName, eventType, eventInfo, 0, 0, 0, null);
    }

    /// <summary>
    /// Logs an event. The eventInfo is limited to 100 key value pairs. 
    /// The eventName and strings in eventInfo cannot contain more than 255 characters.
    /// </summary>
    /// <param name="eventName">The name of the event to be tracked (required not null)</param>
    /// <param name="eventType">An enum value that indicates the type of event that is to be tracked.</param>
    /// <param name="eventInfo">A dictionary containing further information about the event.</param>
    /// <param name="startTime">The time the event started, if not applicable, pass 0 (zero)</param>
    /// <param name="endTime">The time the event ended, if not applicable, pass 0 (zero)</param>
    /// <param name="duration">The duration of the event, if not applicable, pass 0 (zero)</param>
    /// <param name="category">Category is a string with a developer/company defined category of the event. If not applicable, pass null.</param>
    public void LogEvent (string eventName, MParticle.EventType eventType, Dictionary<string, string> eventInfo, long startTime, long endTime, long duration, string category)
    {
        mParticleInstance.LogEvent (eventName, eventType, eventInfo, startTime, endTime, duration, category);
    }

    /// <summary>
    /// Logs a screen.
    /// </summary>
    /// <param name="screenName">The name of the screen to be tracked (required not null)</param>
    public void LogScreen (string screenName)
    {
        mParticleInstance.LogScreen (screenName, null);
    }

    /// <summary>
    /// Logs a screen with a screen name and an attributes dictionary.
    /// </summary>
    /// <param name="screenName">The name of the screen to be tracked (required not null)</param>
    /// <param name="eventInfo">A dictionary containing further information about the screen.</param>
    public void LogScreen (string screenName, Dictionary<string, string> eventInfo)
    {
        mParticleInstance.LogScreen (screenName, eventInfo);
    }

    //
    // Error, Exception, and Crash Handling
    //

    /// <summary>
    /// Enables mParticle exception handling to automatically log events on uncaught exceptions.
    /// </summary>
    public void BeginUncaughtExceptionLogging ()
    {
        mParticleInstance.BeginUncaughtExceptionLogging ();
    }

    /// <summary>
    /// Disables mParticle exception handling.
    /// </summary>
    public void EndUncaughtExceptionLogging ()
    {
        mParticleInstance.EndUncaughtExceptionLogging ();
    }

    /// <summary>
    /// Leaves a breadcrumb. Breadcrumbs are send together with crash reports to help with debugging.
    /// </summary>
    /// <param name="breadcrumbName">The name of the breadcrumb (required not null)</param>
    public void LeaveBreadcrumb (string breadcrumbName)
    {
        mParticleInstance.LeaveBreadcrumb (breadcrumbName, null);
    }

    /// <summary>
    /// Leaves a breadcrumb. Breadcrumbs are send together with crash reports to help with debugging.
    /// </summary>
    /// <param name="breadcrumbName">The name of the breadcrumb (required not null)</param>
    /// <param name="eventInfo">A dictionary containing further information about the breadcrumb.</param>
    public void LeaveBreadcrumb (string breadcrumbName, Dictionary<string, string> eventInfo)
    {
        mParticleInstance.LeaveBreadcrumb (breadcrumbName, eventInfo);
    }

    /// <summary>
    /// Logs an error with a message.
    /// </summary>
    /// <param name="message">The name of the error event (required not null)</param>
    public void LogError (string message)
    {
        mParticleInstance.LogError (message, null);
    }

    /// <summary>
    /// Logs an error with a message and an attributes dictionary.
    /// </summary>
    /// <param name="message">The name of the error event (required not null)</param>
    /// <param name="eventInfo">A dictionary containing further information about the error.</param>
    public void LogError (string message, Dictionary<string, string> eventInfo)
    {
        mParticleInstance.LogError (message, eventInfo);
    }

    /// <summary>
    /// Logs an exception.
    /// </summary>
    /// <param name="condition">The exception which occured.</param>
    /// <param name="stacktrace">The stacktrace which occured.</param>
    public void LogException (String condition, String stacktrace)
    {
        mParticleInstance.LogException (condition, stacktrace);
    }
    
    /// <summary>
    /// Logs an exception.
    /// </summary>
    /// <param name="exception">The exception which occured.</param>
    public void LogException (Exception exception)
    {
        mParticleInstance.LogException (exception, null, null);
    }

    /// <summary>
    /// Logs an exception with an attributes dictionary.
    /// </summary>
    /// <param name="exception">The exception which occured.</param>
    /// <param name="eventInfo">A dictionary containing further information about the exception.</param>
    public void LogException (Exception exception, Dictionary<string, string> eventInfo)
    {
        mParticleInstance.LogException (exception, eventInfo, null);
    }

    /// <summary>
    /// Logs an exception with a message and an attributes dictionary.
    /// </summary>
    /// <param name="exception">The exception which occured.</param>
    /// <param name="eventInfo">A dictionary containing further information about the exception.</param>
    /// <param name="message">A message or the class name of the topmost view controller.</param>
    public void LogException (System.Exception exception, Dictionary<string, string> eventInfo, string message)
    {
        mParticleInstance.LogException (exception, eventInfo, message);
    }

    //
    // eCommerce Transactions
    //

    /// <summary>
    /// Logs an e-commerce transaction with an MPProduct.
    /// <seealso cref="MPProduct"/>
    /// </summary>
    /// <param name="product">An instance of MPProduct.</param>
    public void LogTransaction (MPProduct product)
    {
        mParticleInstance.LogTransaction (product);
    }

    /// <summary>
    /// Increases the LTV (LifeTime Value) amount of a user.
    /// </summary>
    /// <param name="increaseAmount">The amount to be added to LTV.</param>
    /// <param name="eventName">The name of the event (Optional). If not applicable, pass null.</param>
    /// <param name="eventInfo">A dictionary containing further information about the LTV.</param>
    public void LogLTVIncrease (decimal increaseAmount, string eventName, Dictionary<string, string> eventInfo)
    {
        mParticleInstance.LogLTVIncrease (increaseAmount, eventName, eventInfo);
    }

    //
    // Location
    //

    /// <summary>
    /// Begins geographic location tracking.
    /// </summary>
    /// <param name="locationRange">An enum specifying the desired accuracy.</param>
    /// <param name="minTime">Minimum interval to update location (Android only).</param>
    /// <param name="minDistance">Minimum distance change to update location.</param>
    public void BeginLocationTracking (MParticle.LocationRange locationRange, long minTime, double minDistance)
    {
        mParticleInstance.BeginLocationTracking (locationRange, minTime, minDistance);
    }

    /// <summary>
    /// Ends geographic location tracking.
    /// </summary>
    public void EndLocationTracking ()
    {
        mParticleInstance.EndLocationTracking ();
    }

    //
    // Network Performance Measurement
    //

    public void ExcludeUrlFromNetworkPerformanceMeasurement (string url)
    {
        mParticleInstance.ExcludeUrlFromNetworkPerformanceMeasurement (url);
    }

    /// <summary>
    /// Allows you to log a network performance measurement independently from the mParticle SDK automatic measurement.
    /// </summary>
    /// <param name="url">The absolute URL string being measured.</param>
    /// <param name="startTime">The time when the network communication started, measured in milliseconds, since Unix Epoch Time.</param>
    /// <param name="method">The http method used in the network communication (e.g. GET, POST).</param>
    /// <param name="length">The number of milliseconds it took for the network communication took to complete.</param>
    /// <param name="bytesSent">The number of bytes sent.</param>
    /// <param name="bytesReceived">The number of bytes received.</param>
    public void LogNetworkPerformance(string url, long startTime, string method, long length, long bytesSent, long bytesReceived)
    {
        mParticleInstance.LogNetworkPerformance (url, startTime, method, length, bytesSent, bytesReceived);
    }

    //
    // Push Notifications
    //

    public void EnablePushNotifications (string senderId, uint pushNotificationTypes)
    {
        mParticleInstance.EnablePushNotifications (senderId, pushNotificationTypes);
    }

    public void DisablePushNotifications()
    {
        mParticleInstance.DisablePushNotifications ();
    }
    
    //
    // Session management
    //

    /// <summary>
    /// Begins a new user session. It will end the current session, if one is active.
    /// </summary>
    public void BeginSession ()
    {
        mParticleInstance.BeginSession ();
    }

    /// <summary>
    /// Ends the current session.
    /// </summary>
    public void EndSession ()
    {
        mParticleInstance.EndSession ();
    }

    /// <summary>
    /// Sets a single session attribute. The property will be combined with any existing attributes.
    /// There is a 100 count limit to existing session attributes.
    /// </summary>
    /// <param name="key">The attribute key.</param>
    /// <param name="val">The attribute value.</param>
    public void SetSessionAttribute (string key, string val)
    {
        mParticleInstance.SetSessionAttribute (key, val);
    }

    /// <summary>
    /// Force uploads queued messages to mParticle.
    /// </summary>
    public void Upload ()
    {
        mParticleInstance.Upload ();
    }

    //
    // User identity
    //

    /// <summary>
    /// Sets a single user attribute. The property will be combined with any existing attributes.
    /// There is a 100 count limit to user attributes.
    /// </summary>
    /// <param name="key">The attribute key.</param>
    /// <param name="val">The attribute value.</param>
    public void SetUserAttribute (string key, string val)
    {
        mParticleInstance.SetUserAttribute (key, val);
    }

    /// <summary>
    /// Sets User/Customer Identity.
    /// </summary>
    /// <param name="identity">A string representing the user identity.</param>
    /// <param name="identityType">An enum with the user identity type.</param>
    public void SetUserIdentity (string identity, MParticle.UserIdentity identityType)
    {
        mParticleInstance.SetUserIdentity (identity, identityType);
    }

    /// <summary>
    /// Sets a single user tag or attribute. The tag will be combined with any existing attributes.
    /// There is a 100 count limit to user attributes.
    /// </summary>
    /// <param name="tag">The user tag/attribute.</param>
    public void SetUserTag (string tag)
    {
        mParticleInstance.SetUserTag (tag);
    }
}
#endif