#if UNITY_ANDROID
using UnityEngine;
using System;
using System.Collections.Generic;

public class MParticleAndroid : IMParticleSDK
{
	private AndroidJavaObject mp;

	AndroidJavaClass identityTypeClass = new AndroidJavaClass ("com.mparticle.MParticle$IdentityType");
	AndroidJavaClass eventTypeClass = new AndroidJavaClass ("com.mparticle.MParticle$EventType");
	AndroidJavaClass environmentClass = new AndroidJavaClass ("com.mparticle.MParticle$Environment");

	public MParticleAndroid ()
	{
		mp = new AndroidJavaClass ("com.mparticle.MParticle").
CallStatic<AndroidJavaObject> ("getInstance");
	}

	public MParticle.MPEnvironment GetEnvironment ()
	{
		return ConvertToCSharpEnvironment (mp.Call<AndroidJavaObject> (
			"getOptOut"
		));
	}

	public void SetEnvironment (MParticle.MPEnvironment environment)
	{
		mp.Call (
			"setEnvironment", 
			new object[] { ConvertToMpEnvironment (environment) }
		);
	}

	public void SetOptOut (bool optOut)
	{
		mp.Call (
			"setOptOut",
			ConvertToJavaBoolean (optOut)
		);
	}

	public void LeaveBreadcrumb (string breadcrumb, Dictionary<string,string> eventData)
	{
		mp.Call (
			"leaveBreadcrumb", 
			new object[] { breadcrumb }
		);
	}

	public void SetUserAttribute (string key, string value)
	{
		mp.Call (
			"setUserAttribute", 
			new object[] { key, value }
		);
	}

	public void SetUserIdentity (string id, MParticle.UserIdentity type)
	{
		mp.Call (
			"setUserIdentity", 
			new object[] { id, ConvertToMpUserIdentity (type) }
		);
	}

	public void SetUserTag (string tag)
	{
		mp.Call (
			"setUserTag", 
			tag
		);
	}

	public void LogEvent (string name, MParticle.EventType eventType, Dictionary<string, string> eventData, long eventLength)
	{
		mp.Call (
			"logEvent", 
			new object[] { name, ConvertToMpEventType (eventType), ConvertDictToMap (eventData), eventLength }
		);
	}

	public void LogEvent (string name, MParticle.EventType eventType, Dictionary<string, string> eventData)
	{
		mp.Call (
			"logEvent", 
			new object[] { name, ConvertToMpEventType (eventType), ConvertDictToMap (eventData) }
		);
	}

	public void LogEvent (string name, MParticle.EventType eventType, long eventLength)
	{
		mp.Call (
			"logEvent", 
			new object[] { name, ConvertToMpEventType (eventType), eventLength }
		);
	}

	public void LogEvent (string name, MParticle.EventType eventType, Dictionary<string, string> eventData, long eventLength, string category)
	{
		mp.Call (
			"logEvent", 
			new object[] { name, ConvertToMpEventType (eventType), ConvertDictToMap (eventData), eventLength, category }
		);
	}

	public void LogScreen (string screenName)
	{
		mp.Call (
			"logScreen", 
			new object[] { screenName }
		);
	}

	public void LogScreen (string screenName, Dictionary<string, string> eventData)
	{
		mp.Call (
			"logScreen", 
			new object[] { screenName, ConvertDictToMap (eventData) }
		);
	}

	//Utility methods
	private AndroidJavaObject ConvertDictToMap (Dictionary<string, string> data)
	{
		AndroidJavaObject map = new AndroidJavaObject ("java.util.HashMap");
		if (data != null) {
			foreach (KeyValuePair<string, string> entry in data) {
				map.Call<string> (
					"put", 
					new string[]{ entry.Key, entry.Value }
				);
			}
		}
		return map;
	}

	private AndroidJavaObject ConvertToMpEventType (MParticle.EventType eventType)
	{
		return eventTypeClass.CallStatic<AndroidJavaObject> ("valueOf", eventType.ToString ());
	}

	private AndroidJavaObject ConvertToMpUserIdentity (MParticle.UserIdentity userIdentity)
	{
		return identityTypeClass.CallStatic<AndroidJavaObject> ("valueOf", userIdentity.ToString ());
	}

	private AndroidJavaObject ConvertToMpEnvironment (MParticle.MPEnvironment environment)
	{
		return environmentClass.CallStatic<AndroidJavaObject> ("valueOf", environment.ToString ());
	}

	private AndroidJavaObject ConvertToJavaException (Exception e)
	{
		if (e != null) {
			return new AndroidJavaObject (
				"com.mparticle.MPUnityException", 
				e.Message,
				StackTraceUtility.ExtractStringFromException (e)
			);
		}
		return null;
	}

	private AndroidJavaObject ConvertToJavaBoolean (bool value)
	{
		return new AndroidJavaObject ("java.lang.Boolean", value.ToString ());
	}

	private bool ConvertToCSharpBoolean (AndroidJavaObject value)
	{
		return Boolean.Parse (value.Call<string> ("toString"));
	}

	private MParticle.MPEnvironment ConvertToCSharpEnvironment (AndroidJavaObject value)
	{
		return (MParticle.MPEnvironment)Enum.Parse (typeof(MParticle.MPEnvironment), value.Call<string> ("name"));
	}
}

#endif
