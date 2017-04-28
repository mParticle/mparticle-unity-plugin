using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Collections.Generic;

public class MParticleiOS : InterfaceMParticleSDK {
    /*
     Interface to native implementation 
     */
    [DllImport ("__Internal")]
    private static extern bool _GetDebugMode ();
    
    [DllImport ("__Internal")]
    private static extern void _SetDebugMode (bool debugMode);
    
    [DllImport ("__Internal")]
    private static extern bool _GetSandboxMode ();
    
    [DllImport ("__Internal")]
    private static extern void _SetSandboxMode (bool sandboxMode);
    
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
    private static extern void _EndUncaughtExceptionLoggin ();

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
    
    // Push Notifications
    [DllImport ("__Internal")]
    private static extern void _RegisterForPushNotificationWithTypes (uint pushNotificationTypes);
    
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
        string serializedString = "{";

        foreach (KeyValuePair<string, string> entry in dictionary)
        {
            string key = WWW.EscapeURL (entry.Key);
            string val = WWW.EscapeURL (entry.Value);

            serializedString += key + ":" + val + ",";
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
    bool GetDebugMode ()
    {
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            return false;
        }

        return _GetDebugMode ();
    }

    void SetDebugMode (bool debugMode)
    {
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            return;
        }

        _SetDebugMode (debugMode);
    }

    bool GetSandboxMode ()
    {
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            return false;
        }
        
        return _GetSandboxMode ();
    }

    void SetSandboxMode (bool sandboxMode)
    {
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            return;
        }
        
        _SetSandboxMode (sandboxMode);
    }

    double GetSessionTimeout ()
    {
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            return 600;
        }
        
        return _GetSessionTimeout ();
    }

    void SetSessionTimeout (double sessionTimeout)
    {
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            return;
        }
        
        _SetSessionTimeout (sessionTimeout);
    }

    // Basic Tracking
    public void LogEvent (string eventName, MParticle.EventType eventType, Dictionary<string, string> eventInfo, double eventLength, string category)
    {
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            return;
        }
        
        string eventInfoJSON = SerializeDictionary (eventInfo);

        _LogEvent (eventName, eventType, eventInfoJSON, eventLength, category);
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

    public void EndUncaughtExceptionLoggin ()
    {
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            return;
        }

        _EndUncaughtExceptionLoggin ();
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

    public void LogException (System.Exception exception, Dictionary<string, string> eventInfo, string message)
    {
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            return;
        }
        
        _LogException (exception.ToString (), exception.Message, exception.StackTrace);
    }

    // eCommerce Transactions
    void LogTransaction (MPProduct product)
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

    public void LogLTVIncrease (double increaseAmount, string eventName, Dictionary<string, string> eventInfo)
    {
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            return;
        }

        string eventInfoJSON = SerializeDictionary (eventInfo);

        _LogLTVIncrease (increaseAmount, eventName, eventInfoJSON);
    }

    // Location
    void BeginLocationTracking (MParticle.LocationRange locationRange, long minTime, double minDistance)
    {
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            return;
        }

        _BeginLocationTracking (locationRange, minDistance);
    }
    
    public void EndLocationTracking ()
    {
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            return;
        }

        _EndLocationTracking ();
    }
    
    // Push Notifications
    public void RegisterForPushNotificationWithTypes (string senderId, uint pushNotificationTypes)
    {
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            return;
        }

        _RegisterForPushNotificationWithTypes (pushNotificationTypes);
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

        _SetUserIdentity (identity, identityType);
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
