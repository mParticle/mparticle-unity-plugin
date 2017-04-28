#if UNITY_IPHONE

using UnityEngine;
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

public class MParticleiOS : IMParticleSDK {
    /*
     Interface to native implementation 
     */

    // Properties
    [DllImport ("__Internal")]
    private static extern int _ConsoleLogging ();

    [DllImport ("__Internal")]
    private static extern void _SetConsoleLogging (int consoleLogging);

    [DllImport ("__Internal")]
    private static extern int _GetEnvironment();

    [DllImport ("__Internal")]
    private static extern bool _GetOptOut ();

    [DllImport ("__Internal")]
    private static extern void _SetOptOut (bool optOut);

    [DllImport ("__Internal")]
    private static extern double _GetSessionTimeout ();
    
    [DllImport ("__Internal")]
    private static extern void _SetSessionTimeout (double sessionTimeout);
    
    // Basic Tracking
    [DllImport ("__Internal")]
    private static extern void _LogEvent (string eventName, int eventType, string eventInfoJSON, double eventLength, string category);

    [DllImport ("__Internal")]
    private static extern void _LogScreen (string screenName, string eventInfoJSON);
    
    // Error, Exception, and Crash Handling
    [DllImport ("__Internal")]
    private static extern void _BeginUncaughtExceptionLogging ();

    [DllImport ("__Internal")]
    private static extern void _EndUncaughtExceptionLogging ();

    [DllImport ("__Internal")]
    private static extern void _LeaveBreadcrumb (string breadcrumbName, string eventInfoJSON);

    [DllImport ("__Internal")]
    private static extern void _LogError (string message, string eventInfoJSON);

    [DllImport ("__Internal")]
    private static extern void _LogException (string name, string message, string stackTrace);

    // eCommerce Transactions
    [DllImport ("__Internal")]
    private static extern void _LogTransaction (string productName, string affiliation, string sku, double unitPrice, int quantity, double revenueAmount, double taxAmount, double shippingAmount, string transactionId, string category, string currency);
    
    [DllImport ("__Internal")]
    private static extern void _LogLTVIncrease (double increaseAmount, string eventName, string eventInfoJSON);
    
    // Location
    [DllImport ("__Internal")]
    private static extern void _BeginLocationTracking (double accuracy, double distanceFilter);
    
    [DllImport ("__Internal")]
    private static extern void _EndLocationTracking ();
    
    // Network Performance Measurement
    [DllImport ("__Internal")]
    private static extern void _LogNetworkPerformance (string url, long startTime, string method, long length, long bytesSent, long bytesReceived);
    
    // Push Notifications
    [DllImport ("__Internal")]
    private static extern void _RegisterForPushNotificationWithTypes (uint pushNotificationTypes);

    [DllImport ("__Internal")]
    private static extern void _UnregisterForPushNotifications ();
    
    // Session management
    [DllImport ("__Internal")]
    private static extern void _BeginSession ();
    
    [DllImport ("__Internal")]
    private static extern void _EndSession ();
    
    [DllImport ("__Internal")]
    private static extern void _SetSessionAttribute (string key, string val);
    
    [DllImport ("__Internal")]
    private static extern void _Upload ();
    
    // User identity
    [DllImport ("__Internal")]
    private static extern void _SetUserAttribute (string key, string val);
    
    [DllImport ("__Internal")]
    private static extern void _SetUserIdentity (string identity, uint identityType);
    
    [DllImport ("__Internal")]
    private static extern void _SetUserTag (string tag);

    /*
     Private methods
     */
    private static string SerializeDictionary (Dictionary<string, string> dictionary)
    {
        if (dictionary == null)
        {
            return null;
        }

        string serializedString = "{";

        foreach (KeyValuePair<string, string> entry in dictionary)
        {
            serializedString += "\"" + entry.Key + "\":\"" + entry.Value + "\",";
        }

        if (serializedString.Length > 1)
        {
            serializedString = serializedString.Remove (serializedString.Length - 1);
        }

        serializedString += "}";

        return serializedString;
    }

    /* 
     Public interface for use inside C# / JS code 
     */
    
    // Properties
    public bool ConsoleLogging ()
    {
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            return false;
        }

        return _ConsoleLogging ();
    }

    public void SetConsoleLogging (bool consoleLogging)
    {
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            return;
        }

        _SetConsoleLogging (consoleLogging);        
    }

    public MParticle.MPEnvironment GetEnvironment ()
    {
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            return MParticle.MPEnvironment.Development;
        }

		return (MParticle.MPEnvironment) Enum.Parse(typeof(MParticle.MPEnvironment),_GetEnvironment ().ToString());
    }

    public bool GetOptOut ()
    {
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            return false;
        }

        return _GetOptOut ();
    }

    public void SetOptOut (bool optOut)
    {
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            return;
        }

        _SetOptOut (optOut);        
    }

    public double GetSessionTimeout ()
    {
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            return 600;
        }
        
        return _GetSessionTimeout ();
    }

    public void SetSessionTimeout (double sessionTimeout)
    {
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            return;
        }
        
        _SetSessionTimeout (sessionTimeout);
    }

    // Basic Tracking
    public void LogEvent (string eventName, MParticle.EventType eventType, Dictionary<string, string> eventInfo, long eventLength, string category)
    {
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            return;
        }
        
        string eventInfoJSON = SerializeDictionary (eventInfo);

        _LogEvent (eventName, (int)eventType, eventInfoJSON, (double)eventLength, category);
    }

    public void LogScreen (string screenName, Dictionary<string, string> eventInfo)
    {
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            return;
        }
        
        string eventInfoJSON = SerializeDictionary (eventInfo);

        _LogScreen (screenName, eventInfoJSON);
    }
    
    // Error, Exception, and Crash Handling
    public void BeginUncaughtExceptionLogging()
    {
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            return;
        }
        
        _BeginUncaughtExceptionLogging ();
    }

    public void EndUncaughtExceptionLogging ()
    {
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            return;
        }

        _EndUncaughtExceptionLogging ();
    }

    public void LeaveBreadcrumb (string breadcrumbName, Dictionary<string, string> eventInfo)
    {
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            return;
        }
        
        string eventInfoJSON = SerializeDictionary (eventInfo);

        _LeaveBreadcrumb (breadcrumbName, eventInfoJSON);
    }
    
    public void LogError (string message, Dictionary<string, string> eventInfo)
    {
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            return;
        }
        
        string eventInfoJSON = SerializeDictionary (eventInfo);

        _LogError (message, eventInfoJSON);
    }

	public void LogException (string condition, string stacktrace)
	{
		if (Application.platform == RuntimePlatform.OSXEditor)
		{
			return;
		}
		
		_LogException (condition, condition, stacktrace);
	}

    public void LogException (System.Exception exception, Dictionary<string, string> eventInfo, string message)
    {
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            return;
        }
        
        _LogException (exception.ToString (), exception.Message, exception.StackTrace);
    }

    // eCommerce Transactions
    public void LogTransaction (MPProduct product)
    {
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            return;
        }

        _LogTransaction (product.ProductName,
                         product.Affiliation,
                         product.ProductSku,
                         product.UnitPrice,
                         product.Quantity,
                         product.TotalRevenue,
                         product.TaxAmount,
                         product.ShippingAmount,
                         product.TransactionId,
                         product.ProductCategory,
                         product.CurrencyCode);
    }

    public void LogLTVIncrease (decimal increaseAmount, string eventName, Dictionary<string, string> eventInfo)
    {
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            return;
        }

        string eventInfoJSON = SerializeDictionary (eventInfo);

        _LogLTVIncrease ((double) increaseAmount, eventName, eventInfoJSON);
    }

    // Location
    public void BeginLocationTracking (MParticle.LocationRange locationRange, long minTime, double minDistance)
    {
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            return;
        }

        _BeginLocationTracking ((double)locationRange, minDistance);
    }
    
    public void EndLocationTracking ()
    {
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            return;
        }

        _EndLocationTracking ();
    }
    
    // Network Performance Measurement
    public void ExcludeUrlFromNetworkPerformanceMeasurement (string url)
    {
    }

    public void LogNetworkPerformance (string url, long startTime, string method, long length, long bytesSent, long bytesReceived)
    {
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            return;
        }

        _LogNetworkPerformance (url, startTime, method, length, bytesSent, bytesReceived);
    }

    // Push Notifications
    public void EnablePushNotifications (string androidSenderId, uint iOSPushNotificationTypes)
    {
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            return;
        }

        _RegisterForPushNotificationWithTypes (iOSPushNotificationTypes);
    }

    public void DisablePushNotifications ()
    {
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            return;
        }

        _UnregisterForPushNotifications ();
    }
    
    // Session management
    public void BeginSession ()
    {
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            return;
        }

        _BeginSession ();
    }
    
    public void EndSession ()
    {
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            return;
        }

        _EndSession ();
    }
    
    public void SetSessionAttribute (string key, string val)
    {
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            return;
        }

        _SetSessionAttribute (key, val);
    }
    
    public void Upload ()
    {
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            return;
        }

        _Upload ();
    }
    
    // User identity
    public void SetUserAttribute (string key, string val)
    {
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            return;
        }

        _SetUserAttribute (key, val);
    }
    
    public void SetUserIdentity (string identity, MParticle.UserIdentity identityType)
    {
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            return;
        }

        _SetUserIdentity (identity, (uint)identityType);
    }
    
    public void SetUserTag (string tag)
    {
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            return;
        }

        _SetUserTag (tag);
    }
}

#endif