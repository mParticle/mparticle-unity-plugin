using UnityEngine;
using System;
using System.Collections.Generic;

public class MParticleAndroid : IMParticleSDK
{
	private AndroidJavaObject mp;

	AndroidJavaClass eventTypeClass = new AndroidJavaClass("com.mparticle.MParticle$EventType");

	public MParticleAndroid()
	{
		mp = new AndroidJavaClass ("com.mparticle.MParticle").
								CallStatic<AndroidJavaObject>("getInstance");
	}
	
	public void BeginMeasuringNetworkPerformance()
	{
		mp.Call ("beginMeasuringNetworkPerformance");
	}

	public void BeginSession()
	{
		mp.Call ("beginSession");
	}

	public void EndLocationTracking()
	{
		mp.Call ("disableLocationTracking");
	}

	public void DisablePushNotifications()
	{
		mp.Call ("disablePushNotifications");
	}

	public void EndUncaughtExceptionLogging()
	{
		mp.Call ("disableUncaughtExceptionLogging");
	}

	public void BeginLocationTracking(MParticle.LocationRange range, long minTime, double minDistance)
	{
		string provider = "passive";
		if (range.Equals(MParticle.LocationRange.Network))
		{
			provider = "network";
		}else if (range.Equals(MParticle.LocationRange.GPS))
		{
			provider = "gps";
		}
		mp.Call ("enableLocationTracking", new object[]{provider, minTime, minDistance});
	}

	public void EnablePushNotifications(string senderId, uint type)
	{
		mp.Call("enablePushNotifications", new object[] {senderId});
	}

	public void BeginUncaughtExceptionLogging()
	{
		mp.Call ("enableUncaughtExceptionLogging");
	}

	public void EndMeasuringNetworkPerformance()
	{
		mp.Call ("endMeasuringNetworkPerformance");
	}

	public void EndSession()
	{
		mp.Call ("endSession");
	}

	public void Upload()
	{
		mp.Call ("upload");
	}

	public void ExcludeUrlFromNetworkPerformanceMeasurement(string url)
	{
		mp.Call (
			"excludeUrlFromNetworkPerformanceMeasurement", 
			url
		);
	}

	public bool GetDebugMode()
	{
		return mp.Call<bool>(
			"getDebugMode"
			);
	}

	public void SetDebugMode(bool debugMode)
	{
		//todo
	}

	public bool GetSandboxMode()
	{
		//todo
		return false;
	}

	public void SetSandboxMode(bool sandboxMode)
	{
		//todo
	}

	public bool GetOptOut()
	{
		return mp.Call<bool>(
			"getOptOut"
		);
	}

	public void SetSessionTimeout(double timeout){
		//todo
	}

	public double GetSessionTimeout()
	{
		return (double)mp.Call<int> (
			"getSessionTimeout"
		);
	}

	public bool IsAutoTrackingEnabled()
	{
		return mp.Call<bool>(
			"isAutoTrackingEnabled"
		);
	}

	public void LeaveBreadcrumb(string breadcrumb, Dictionary<string,string> eventData)
	{
		mp.Call(
			"leaveBreadcrumb", 
			new object[] {breadcrumb}
		);
	}

	public void LogError(string message)
	{
		mp.Call(
			"logError", 
			new object[] {message}
		);
	}

	public void LogError(string message, Dictionary<string,string> eventData)
	{
		mp.Call(
			"logError", 
		    new object[] {message, ConvertDictToMap (eventData)}
		);
	}

	public void SetSessionAttribute(string key, string value)
	{
		//todo
	}

	public void SetUserAttribute(string key, string value)
	{
		//todo
	}

	public void SetUserIdentity(string id, MParticle.UserIdentity type)
	{
		//todo
	}

	public void SetUserTag(string tag)
	{
		//todo
	}

	public void LogEvent (string name, MParticle.EventType eventType)
	{
		mp.Call(
			"logEvent", 
		    new object[] {name, ConvertToMpEventType (eventType)}
		);
	}

	public void LogEvent (string name, MParticle.EventType eventType, Dictionary<string, string> eventData, long eventLength)
	{
		mp.Call(
			"logEvent", 
		    new object[] {name, ConvertToMpEventType (eventType), ConvertDictToMap(eventData), eventLength}
		);
	}

	public void LogEvent (string name, MParticle.EventType eventType, Dictionary<string, string> eventData)
	{
		mp.Call(
			"logEvent", 
		    new object[] {name, ConvertToMpEventType (eventType), ConvertDictToMap(eventData)}
		);
	}
	
	public void LogEvent (string name, MParticle.EventType eventType, long eventLength)
	{
		mp.Call(
			"logEvent", 
		    new object[] {name, ConvertToMpEventType (eventType), eventLength}
		);
	}

	public void LogEvent (string name, MParticle.EventType eventType, Dictionary<string, string> eventData, double eventLength, string category)
	{
		mp.Call(
			"logEvent", 
		    new object[] {name, ConvertToMpEventType (eventType), ConvertDictToMap(eventData), eventLength, category}
		);
	}

	public void LogException(Exception exception)
	{
		mp.Call (
			"logException", 
			ConvertToJavaException (exception)
		);
	}

	public void LogException(Exception exception, Dictionary<string, string> eventData)
	{
		mp.Call (
			"logException", 
			new object[]{ConvertToJavaException (exception), ConvertDictToMap(eventData)}
		);
	}

	public void LogException(Exception exception, Dictionary<string, string> eventData, string message)
	{
		mp.Call (
			"logException", 
			new object[]{ConvertToJavaException (exception), ConvertDictToMap(eventData), message}
		);
	}

	public void LogLTVIncrease(decimal valueIncreased, string eventName, Dictionary<string, string> eventData)
	{
		AndroidJavaObject decimalObject = new AndroidJavaObject ("java.math.BigDecimal", valueIncreased.ToString ());
		mp.Call (
			"logLtvIncrease",
			new object[]{decimalObject, eventName, ConvertDictToMap(eventData)}
		);
	}

	public void LogNetworkPerformance(string url, long startTime, string method, long length, long bytesSent, long bytesReceived)
	{
		mp.Call(
			"logNetworkPerformance",
			new object[]{url, startTime, method, length, bytesSent,	bytesReceived}
		);
	}

	public void LogScreen(string screenName)
	{
		mp.Call ("logScreen", 
		         new object[] {screenName}
		);
	}

	public void LogScreen(string screenName, Dictionary<string, string> eventData)
	{
		mp.Call (
			"logScreen", 
		    new object[] {screenName, ConvertDictToMap(eventData)}
		);
	}

	public void LogTransaction(MPProduct product)
	{
		AndroidJavaObject productBuilder = new AndroidJavaObject("com.mparticle.MPProduct$Builder", new object[] {product.ProductName, product.ProductSku});
		AndroidJavaObject productMap = productBuilder.Call<AndroidJavaObject> ("build");
		productMap.Call ("putAll", ConvertDictToMap (product));
		mp.Call ("logTransaction", productMap);
	}

	private AndroidJavaObject ConvertDictToMap(Dictionary<string, string> data)
	{
		AndroidJavaObject map = new AndroidJavaObject("java.util.HashMap");
		foreach (KeyValuePair<string, string> entry in data) 
		{
			map.Call<string>(
				"put", 
				new string[]{entry.Key, entry.Value}
			);
		}
		return map;
	}

	private AndroidJavaObject ConvertToMpEventType(MParticle.EventType eventType)
	{
		return eventTypeClass.CallStatic<AndroidJavaObject> ("valueOf", eventType.ToString());
	}

	private AndroidJavaObject ConvertToJavaException(Exception e)
	{
		if (e != null) 
		{
		    return new AndroidJavaObject (
											"java.lang.Exception", 
			                            	StackTraceUtility.ExtractStringFromException (e)
			                             );
		}
		return null;
	}
}