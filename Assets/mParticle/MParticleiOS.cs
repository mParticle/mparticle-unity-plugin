#if UNITY_IPHONE

using UnityEngine;
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using mParticle;

public class MParticleiOS : IMParticleSDK
{

    [DllImport ("__Internal")]
    private static extern void _Initialize (string key, string secret);

    [DllImport ("__Internal")]
    private static extern void _LogEvent (string eventName, int eventType, string eventInfoJSON);

    [DllImport ("__Internal")]
    private static extern void _LogCommerceEvent (string commerceEventJSON);

    [DllImport ("__Internal")]
    private static extern void _LogScreen (string screenName, string eventInfoJSON);

    [DllImport ("__Internal")]
    private static extern int _IncrementUserAttribute (string key, int incrementValue);

    [DllImport ("__Internal")]
    private static extern void _SetUserAttribute (string key, string val);

    [DllImport ("__Internal")]
    private static extern void _SetUserAttributeArray (string key, string[] values, int length);

    [DllImport ("__Internal")]
    private static extern void _SetUserIdentity (string identity, uint identityType);

    [DllImport ("__Internal")]
    private static extern void _SetUserTag (string tag);

    [DllImport ("__Internal")]
    private static extern void _RemoveUserAttribute (string key);

    [DllImport ("__Internal")]
    private static extern void _LeaveBreadcrumb (string breadcrumbName, string eventInfoJSON);

    [DllImport ("__Internal")]
    private static extern void _Logout ();

    [DllImport ("__Internal")]
    private static extern int _GetEnvironment ();

    [DllImport ("__Internal")]
    private static extern void _SetOptOut (bool optOut);

    [DllImport ("__Internal")]
    private static extern void _SetUploadInterval(int uploadInterval);

    /*
     Private variables
     */
    private static readonly DateTime epoch = new DateTime (1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    /*
     Private methods
     */
    public static double DateTimeToSeconds (DateTime sourceDateTime)
    {
        DateTime referenceDateTime = (sourceDateTime.Kind == DateTimeKind.Unspecified) ? DateTime.SpecifyKind (sourceDateTime, DateTimeKind.Utc) : sourceDateTime.ToUniversalTime ();
        return (double)((referenceDateTime - epoch).TotalSeconds);
    }

    private static string SerializeDictionary (Dictionary<string, string> dictionary)
    {
        if (dictionary == null) {
            return null;
        }

        string serializedString = "{";

        foreach (KeyValuePair<string, string> entry in dictionary) {
            serializedString += "\"" + entry.Key + "\":\"" + entry.Value + "\",";
        }

        if (serializedString.Length > 1) {
            serializedString = serializedString.Remove (serializedString.Length - 1);
        }

        serializedString += "}";

        return serializedString;
    }

    public void Initialize (string key, string secret) {
        if (Application.platform == RuntimePlatform.OSXEditor) {
            return;
        }

        _Initialize (key, secret);
    }

    public void LogEvent (string eventName, mParticle.EventType eventType, Dictionary<string, string> eventInfo)
    {
        if (Application.platform == RuntimePlatform.OSXEditor) {
            return;
        }

        string eventInfoJSON = SerializeDictionary (eventInfo);

        _LogEvent (eventName, (int)eventType, eventInfoJSON);
    }

    public void LogCommerceEvent (CommerceEvent commerceEvent)
    {
        if (Application.platform == RuntimePlatform.OSXEditor) {
            return;
        }
            
        string commerceEventJSON = JsonUtility.ToJson (commerceEvent);
        _LogCommerceEvent (commerceEventJSON);
    }

    public void LogScreen (string screenName, Dictionary<string, string> eventInfo)
    {
        if (Application.platform == RuntimePlatform.OSXEditor) {
            return;
        }

        string eventInfoJSON = SerializeDictionary (eventInfo);

        _LogScreen (screenName, eventInfoJSON);
    }

    public void LeaveBreadcrumb (string breadcrumbName, Dictionary<string, string> eventInfo)
    {
        if (Application.platform == RuntimePlatform.OSXEditor) {
            return;
        }

        string eventInfoJSON = SerializeDictionary (eventInfo);

        _LeaveBreadcrumb (breadcrumbName, eventInfoJSON);
    }

    public int IncrementUserAttribute (string key, int incrementValue)
    {
        if (Application.platform == RuntimePlatform.OSXEditor) {
            return 0;
        }

        int newValue = _IncrementUserAttribute (key, incrementValue);
        return newValue;
    }

    public void Logout ()
    {
        if (Application.platform == RuntimePlatform.OSXEditor) {
            return;
        }

        _Logout ();
    }

    public void SetUserAttribute (string key, string val)
    {
        if (Application.platform == RuntimePlatform.OSXEditor) {
            return;
        }

        _SetUserAttribute (key, val);
    }

    public void SetUserAttributeArray (string key, string[] values)
    {
        if (Application.platform == RuntimePlatform.OSXEditor) {
            return;
        }

        _SetUserAttributeArray (key, values, values.Length);
    }

    public void SetUserIdentity (string identity, UserIdentity identityType)
    {
        if (Application.platform == RuntimePlatform.OSXEditor) {
            return;
        }

        _SetUserIdentity (identity, (uint)identityType);
    }

    public void SetUserTag (string tag)
    {
        if (Application.platform == RuntimePlatform.OSXEditor) {
            return;
        }

        _SetUserTag (tag);
    }

    public void RemoveUserAttribute (string key)
    {
        if (Application.platform == RuntimePlatform.OSXEditor) {
            return;
        }

        _RemoveUserAttribute (key);
    }

    public mParticle.Environment GetEnvironment ()
    {
        if (Application.platform == RuntimePlatform.OSXEditor) {
            return mParticle.Environment.Development;
        }

        return (mParticle.Environment)Enum.Parse (typeof(mParticle.Environment), _GetEnvironment ().ToString ());
    }

    public void SetOptOut (bool optOut)
    {
        if (Application.platform == RuntimePlatform.OSXEditor) {
            return;
        }

        _SetOptOut (optOut);        
    }

    public void SetUploadInterval(int uploadInterval)
    {
        if (Application.platform == RuntimePlatform.OSXEditor) {
            return;
        }

        _SetUploadInterval(uploadInterval);
    }
}

#endif