using UnityEngine;
using System.Collections.Generic;
using System;

public interface IMParticleSDK
{
    // Properties
    bool GetDebugMode ();
    void SetDebugMode (bool debugMode);
    bool GetSandboxMode ();
    void SetSandboxMode (bool sandboxMode);
    double GetSessionTimeout ();
    void SetSessionTimeout (double sessionTimeout);
    // Basic Tracking
    void LogEvent (string eventName, MParticle.EventType eventType, Dictionary<string, string> eventInfo, double eventLength, string category);
    void LogScreen (string screenName, Dictionary<string, string> eventInfo);
    // Error, Exception, and Crash Handling
    void BeginUncaughtExceptionLogging ();
    void EndUncaughtExceptionLogging ();
    void LeaveBreadcrumb (string breadcrumbName, Dictionary<string, string> eventInfo);
    void LogError (string message, Dictionary<string, string> eventInfo);
    void LogException (System.Exception exception, Dictionary<string, string> eventInfo, string message);
    // eCommerce Transactions
    void LogTransaction (MPProduct product);
    void LogLTVIncrease (decimal increaseAmount, string eventName, Dictionary<string, string> eventInfo);
    // network performance
    void ExcludeUrlFromNetworkPerformanceMeasurement(string url);
    void LogNetworkPerformance (string url, long startTime, string method, long length, long bytesSent, long bytesReceived);
    // Location
    void BeginLocationTracking (MParticle.LocationRange locationRange, long minTime, double minDistance);
    void EndLocationTracking ();
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

public class MParticle : MonoBehaviour, IMParticleSDK {
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
        if (instance != null && type == LogType.Exception)
        {
            this.LogError (condition + ": " + stackTrace);
        }
    }

    // Properties
    public bool GetDebugMode()
    {
		return mParticleInstance.GetDebugMode();
    }

	public void SetDebugMode(bool debug)
	{
		mParticleInstance.SetDebugMode (debug);
	}
	
    public bool GetSandboxMode()
    {
        return mParticleInstance.GetSandboxMode ();
	}
	
	public void SetSandboxMode(bool sandbox)
	{
		mParticleInstance.SetSandboxMode (sandbox);
	}

	public double GetSessionTimeout()
    {
		return mParticleInstance.GetSessionTimeout();
    }

	public void SetSessionTimeout(double timeout)
	{
		mParticleInstance.SetSessionTimeout(timeout);
	}
	
	// Basic Tracking
    public void LogEvent (string eventName, MParticle.EventType eventType)
    {
        mParticleInstance.LogEvent (eventName, eventType, null, 0, null);
    }

    public void LogEvent (string eventName, MParticle.EventType eventType, Dictionary<string, string> eventInfo, double eventLength)
    {
        mParticleInstance.LogEvent (eventName, eventType, eventInfo, eventLength, null);
    }

    public void LogEvent (string eventName, MParticle.EventType eventType, Dictionary<string, string> eventInfo)
    {
        mParticleInstance.LogEvent (eventName, eventType, eventInfo, 0, null);
    }

    public void LogEvent (string eventName, MParticle.EventType eventType, double eventLength)
    {
        mParticleInstance.LogEvent (eventName, eventType, null, eventLength, null);
    }

    public void LogEvent (string eventName, MParticle.EventType eventType, Dictionary<string, string> eventInfo, double eventLength, string category)
    {
        mParticleInstance.LogEvent (eventName, eventType, eventInfo, eventLength, category);
    }

    public void LogScreen (string screenName)
    {
        mParticleInstance.LogScreen (screenName, null);
    }

    public void LogScreen (string screenName, Dictionary<string, string> eventInfo)
    {
        mParticleInstance.LogScreen (screenName, eventInfo);
    }

    // Error, Exception, and Crash Handling
    public void BeginUncaughtExceptionLogging ()
    {
        mParticleInstance.BeginUncaughtExceptionLogging ();
    }

    public void EndUncaughtExceptionLogging ()
    {
        mParticleInstance.EndUncaughtExceptionLogging ();
    }

    public void LeaveBreadcrumb (string breadcrumbName)
    {
        mParticleInstance.LeaveBreadcrumb (breadcrumbName, null);
    }

    public void LeaveBreadcrumb (string breadcrumbName, Dictionary<string, string> eventInfo)
    {
        mParticleInstance.LeaveBreadcrumb (breadcrumbName, eventInfo);
    }

    public void LogError (string message)
    {
        mParticleInstance.LogError (message, null);
    }

    public void LogError (string message, Dictionary<string, string> eventInfo)
    {
        mParticleInstance.LogError (message, eventInfo);
    }

    public void LogException (Exception exception)
    {
        mParticleInstance.LogException (exception, null, null);
    }

    public void LogException (Exception exception, Dictionary<string, string> eventInfo)
    {
        mParticleInstance.LogException (exception, eventInfo, null);
    }

    public void LogException (System.Exception exception, Dictionary<string, string> eventInfo, string message)
    {
        mParticleInstance.LogException (exception, eventInfo, message);
    }

    // eCommerce Transactions
    public void LogTransaction (MPProduct product)
    {
        mParticleInstance.LogTransaction (product);
    }

    public void LogLTVIncrease (decimal increaseAmount, string eventName, Dictionary<string, string> eventInfo)
    {
        mParticleInstance.LogLTVIncrease (increaseAmount, eventName, eventInfo);
    }

    // Location
    public void BeginLocationTracking (MParticle.LocationRange locationRange, long minTime, double minDistance)
    {
        mParticleInstance.BeginLocationTracking (locationRange, minTime, minDistance);
    }

    public void EndLocationTracking ()
    {
        mParticleInstance.EndLocationTracking ();
    }

    // Push Notifications
    public void EnablePushNotifications (string senderId, uint pushNotificationTypes)
    {
        mParticleInstance.EnablePushNotifications (senderId, pushNotificationTypes);
    }

    // Session management
    public void BeginSession ()
    {
        mParticleInstance.BeginSession ();
    }

    public void EndSession ()
    {
        mParticleInstance.EndSession ();
    }

    public void SetSessionAttribute (string key, string val)
    {
        mParticleInstance.SetSessionAttribute (key, val);
    }

    public void Upload ()
    {
        mParticleInstance.Upload ();
    }

    // User identity
    public void SetUserAttribute (string key, string val)
    {
        mParticleInstance.SetUserAttribute (key, val);
    }

    public void SetUserIdentity (string identity, MParticle.UserIdentity identityType)
    {
        mParticleInstance.SetUserIdentity (identity, identityType);
    }

    public void SetUserTag (string tag)
    {
        mParticleInstance.SetUserTag (tag);
    }

    public void DisablePushNotifications()
    {
		mParticleInstance.DisablePushNotifications ();
	}

    public void ExcludeUrlFromNetworkPerformanceMeasurement(string url)
    {
        mParticleInstance.ExcludeUrlFromNetworkPerformanceMeasurement (url);
    }

    public void LogNetworkPerformance(string url, long startTime, string method, long length, long bytesSent, long bytesReceived)
    {
        mp.LogNetworkPerformance (url, startTime, method, length, bytesSent, bytesReceived);
    }
}
