using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using mParticle.android;
using mParticle;


namespace mParticleAndroid
{
	public sealed class MParticleAndroid : IMParticleSDK
	{
		private AndroidJavaObject mp;
		private IdentityApiImpl identityApi;
		private ToAndroidUtils toUtils = new ToAndroidUtils();

		public MParticleAndroid()
		{
		}

		public void Initialize(MParticleOptions options)
		{
			AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			new AndroidJavaClass("com.mparticle.MParticle").CallStatic("start", toUtils.ConvertToMpOptions(options, jc.GetStatic<AndroidJavaObject>("currentActivity")));
			mp = new AndroidJavaClass("com.mparticle.MParticle").
				CallStatic<AndroidJavaObject>("getInstance");
		}

		public IIdentityApi Identity
		{
			get
			{
				if (identityApi == null)
				{
					identityApi = new IdentityApiImpl(mp.Call<AndroidJavaObject>("Identity"));
				}
				if (identityApi.isDead())
				{
					identityApi.setObject(mp.Call<AndroidJavaObject>("Identity"));
				}
				return identityApi;
			}
		}

		public mParticle.Environment Environment
		{
			get
			{
				return ToCSUtils.ConvertToCSharpEnvironment(mp.Call<AndroidJavaObject>(
						"getEnvironment"
					));
			}
		}

		public void SetEnvironment(mParticle.Environment environment)
		{
			mp.Call(
				"setEnvironment", 
				new object[] { toUtils.ConvertToMpEnvironment(environment) }
			);
		}

		public void SetOptOut(bool optOut)
		{
			mp.Call(
				"setOptOut",
				toUtils.ConvertToJavaBoolean(optOut)
			);
		}

		public void LogEvent(CommerceEvent commerceEvent)
		{
			AndroidJavaObject builder;
			if (commerceEvent.ProductAction > 0 && commerceEvent.Products != null && commerceEvent.Products.Length > 0)
			{
				builder = new AndroidJavaObject("com.mparticle.commerce.CommerceEvent$Builder", toUtils.ConvertToMpProductAction(commerceEvent.ProductAction), toUtils.ConvertToMpProduct(commerceEvent.Products[0]));
			}
			else if (commerceEvent.Promotions != null && commerceEvent.Promotions.Length > 0)
			{
				builder = new AndroidJavaObject("com.mparticle.commerce.CommerceEvent$Builder", toUtils.ConvertToMpPromotionAction(commerceEvent.PromotionAction), toUtils.ConvertToMpPromotion(commerceEvent.Promotions[0]));
			}
			else
			{
				builder = new AndroidJavaObject("com.mparticle.commerce.CommerceEvent$Builder", toUtils.ConvertToMpImpression(commerceEvent.Impressions[0]));
			}
			if (commerceEvent.TransactionAttributes != null)
			{
				builder.Call<AndroidJavaObject>("transactionAttributes", toUtils.ConvertToMpTransactionAttributes(commerceEvent.TransactionAttributes));
			}
			if (commerceEvent.ScreenName != null)
			{
				builder.Call<AndroidJavaObject>("screen", commerceEvent.ScreenName);
			}
			if (commerceEvent.Currency != null)
			{
				builder.Call<AndroidJavaObject>("currency", commerceEvent.Currency);
			}
			if (commerceEvent.CustomAttributes != null)
			{
				builder.Call<AndroidJavaObject>("customAttributes", toUtils.ConvertDictToMap(commerceEvent.CustomAttributes));
			}
			if (commerceEvent.CheckoutOptions != null)
			{
				builder.Call<AndroidJavaObject>("checkoutOptions", commerceEvent.CheckoutOptions);
			}
			if (commerceEvent.CheckoutStep != null)
			{
				builder.Call<AndroidJavaObject>("checkoutStep", toUtils.integerClass.CallStatic<AndroidJavaObject>("valueOf", commerceEvent.CheckoutStep));
			}
			if (commerceEvent.NonInteractive.HasValue)
			{
				builder.Call<AndroidJavaObject>("nonInteraction", (bool)commerceEvent.NonInteractive);
			}

			if (commerceEvent.Products != null)
			{
				AndroidJavaObject productList = new AndroidJavaObject("java.util.ArrayList");

				foreach (Product product in commerceEvent.Products)
				{
					productList.Call<bool>(
						"add", 
						new object[]{ toUtils.ConvertToMpProduct(product) }
					);
				}
				builder.Call<AndroidJavaObject>("products", productList);
			}

			if (commerceEvent.Promotions != null)
			{
				AndroidJavaObject promotionList = new AndroidJavaObject("java.util.ArrayList");

				foreach (Promotion promotion in commerceEvent.Promotions)
				{
					promotionList.Call<bool>(
						"add", 
						new object[]{ toUtils.ConvertToMpPromotion(promotion) }
					);
				}
				builder.Call<AndroidJavaObject>("promotions", promotionList);
			}

			if (commerceEvent.Impressions != null)
			{
				AndroidJavaObject impressionList = new AndroidJavaObject("java.util.ArrayList");

				foreach (Impression impression in commerceEvent.Impressions)
				{
					impressionList.Call<bool>(
						"add", 
						new object[]{ toUtils.ConvertToMpImpression(impression) }
					);
				}
				builder.Call<AndroidJavaObject>("impressions", impressionList);
			}
			AndroidJavaObject javaCommerceEvent = builder.Call<AndroidJavaObject>("build");
			mp.Call(
				"logEvent", 
				new object[] { javaCommerceEvent }
			);
		}

		public void LogEvent(MPEvent mpEvent)
		{
			AndroidJavaObject builder = new AndroidJavaObject("com.mparticle.MPEvent$Builder", new object[]{ mpEvent.EventName, toUtils.ConvertToMpEventType(mpEvent.EventType) });
			if (mpEvent.Category != null)
			{
				builder.Call<AndroidJavaObject>("category", new object[]{ mpEvent.Category });
			} 
			if (mpEvent.CustomFlags != null)
			{
				mpEvent.CustomFlags.ToList().ForEach(pair =>
					{
						if (pair.Value != null && pair.Key != null)
						{
							pair.Value.ForEach(val => builder.Call<AndroidJavaObject>("addCustomFlag", new object[]{ pair.Key, val }));
						}
					});
			}
			if (mpEvent.Duration.HasValue)
			{
				builder.Call<AndroidJavaObject>("duration", new object[]{ mpEvent.Duration.Value });
			}
			if (mpEvent.EndTime.HasValue)
			{
				builder.Call<AndroidJavaObject>("endTime", new object[]{ mpEvent.EndTime.Value });
			}
			if (mpEvent.Info != null && mpEvent.Info.Count > 0)
			{
				builder.Call<AndroidJavaObject>("info", new object[]{ toUtils.ConvertDictToMap(mpEvent.Info) });
			}
			if (mpEvent.StartTime.HasValue)
			{
				builder.Call<AndroidJavaObject>("startTime", new object[]{ mpEvent.StartTime.Value });
			}
			var mpEventAndroidObject = builder.Call<AndroidJavaObject>("build");
			mp.Call("logEvent", new object[]{ mpEventAndroidObject });
		}



		public void LeaveBreadcrumb(string breadcrumb)
		{
			mp.Call(
				"leaveBreadcrumb", 
				new object[] { breadcrumb }
			);
		}

		public void LogScreen(string screenName)
		{
			mp.Call(
				"logScreen", 
				new object[] { screenName }
			);
		}

		public void SetATTStatus(ATTAuthStatus status, double timestamp)
		{
		// Unnecessary for Android devices
		}

		public void Upload()
		{
			mp.Call("upload");
		}
	}

	class IdentityApiImpl : IIdentityApi
	{
		AndroidJavaObject identityObject;
		Dictionary<OnUserIdentified, AndroidOnUserIdentified> identityStateListenerMap = new Dictionary<OnUserIdentified, AndroidOnUserIdentified>();
		ToAndroidUtils toUtils = new ToAndroidUtils();

		internal IdentityApiImpl(AndroidJavaObject identityObject)
		{
			this.identityObject = identityObject;
		}

		public void AddIdentityStateListener(OnUserIdentified listener)
		{
			if (identityStateListenerMap.ContainsKey(listener))
			{
				return;
			}
			var androidOnUserIdentified = new AndroidOnUserIdentified(listener);
			identityStateListenerMap.Add(listener, androidOnUserIdentified);
			identityObject.Call("addIdentityStateListener", new object[]{ androidOnUserIdentified });
		}

		public void RemoveIdentityStateListener(OnUserIdentified listener)
		{
			if (identityStateListenerMap.ContainsKey(listener))
			{
				identityObject.Call("removeIdentityStateListener", new object[]{ identityStateListenerMap[listener] });
				identityStateListenerMap.Remove(listener);
			}
						
		}

		public IMParticleTask<IdentityApiResult> Identify(IdentityApiRequest request = null)
		{
			var task = identityObject.Call<AndroidJavaObject>("identify", new object[]{ toUtils.ConvertToMpIdentifyRequest(request) });
			return new IdentifyTaskImpl(task);
		}

		public IMParticleTask<IdentityApiResult> Login(IdentityApiRequest request = null)
		{
			AndroidJavaObject task;
			if (request == null)
			{
				task = identityObject.Call<AndroidJavaObject>("login");
			}
			else
			{
				task = identityObject.Call<AndroidJavaObject>("login", new object[]{ toUtils.ConvertToMpIdentifyRequest(request) });
			}
			return new IdentifyTaskImpl(task);
		}

		public IMParticleTask<IdentityApiResult> Logout(IdentityApiRequest request = null)
		{
			AndroidJavaObject task;
			if (request == null)
			{
				task = identityObject.Call<AndroidJavaObject>("logout");
			}
			else
			{
				task = identityObject.Call<AndroidJavaObject>("logout", new object[]{ toUtils.ConvertToMpIdentifyRequest(request) });
			}
			return new IdentifyTaskImpl(task);
		}

		public IMParticleTask<IdentityApiResult> Modify(IdentityApiRequest request)
		{
			var task = identityObject.Call<AndroidJavaObject>("modify", new object[]{ toUtils.ConvertToMpIdentifyRequest(request) });
			return new IdentifyTaskImpl(task);
		}

		public MParticleUser CurrentUser
		{
			get
			{
				var userObject = identityObject.Call<AndroidJavaObject>("getCurrentUser");
				if (userObject == null)
				{
					return null;
				}
				else
				{
					return new MParticleUserImpl(userObject);
				}
			}
		}

		public MParticleUser GetUser(long mpid)
		{
			var userObject = identityObject.Call<AndroidJavaObject>("getUser", new object[]{ mpid });
			if (userObject == null)
			{
				return null;
			}
			else
			{
				return new MParticleUserImpl(userObject);
			}
		}

		internal Boolean isDead()
		{
			return identityObject == null;
		}

		internal void setObject(AndroidJavaObject identityObject)
		{
			this.identityObject = identityObject;
		}
	}

	class MParticleUserImpl : MParticleUser
	{

		AndroidJavaObject userObject;
		ToAndroidUtils toUtils = new ToAndroidUtils();

		internal MParticleUserImpl(AndroidJavaObject userObject)
		{
			this.userObject = userObject;
		}

		public override long Mpid
		{ 
			get
			{ 
				return userObject.Call<long>("getId");
			}

		}

		public override bool SetUserTag(string tag)
		{
			return userObject.Call<bool>("setUserTag", new object[]{ tag });
		}

		public override Dictionary<UserIdentity, string> GetUserIdentities()
		{
			var unityUserIdentities = new Dictionary<UserIdentity, string>();
			AndroidJavaObject identitiesMap = userObject.Call<AndroidJavaObject>("getUserIdentities");
			foreach (AndroidJavaObject ent in identitiesMap.Call<AndroidJavaObject>("entrySet").Call<AndroidJavaObject[]>("toArray"))
			{
				var key = ent.Call<AndroidJavaObject>("getKey").Call<int>("getValue");
				var value = ent.Call<string>("getValue");
				unityUserIdentities.Add((UserIdentity)key, value);
			}
			return unityUserIdentities;
		}

		public override Dictionary<string, string> GetUserAttributes()
		{
			var unityUserAttributes = new Dictionary<string, string>();
			AndroidJavaObject userAttributeMap = userObject.Call<AndroidJavaObject>("getUserAttributes");
			foreach (AndroidJavaObject ent in userAttributeMap.Call<AndroidJavaObject>("entrySet").Call<AndroidJavaObject[]>("toArray"))
			{
				var key = ent.Call<string>("getKey");
				var value = ent.Call<string>("getValue");
				unityUserAttributes.Add(key, value);
			}
			return unityUserAttributes;
		}

		public override bool SetUserAttributes(Dictionary<string, string> userAttributes)
		{
			return userObject.Call<bool>("setUserAttributes", new object[]{ toUtils.ConvertDictToMap(userAttributes) });
		}

		public override bool SetUserAttribute(string key, string val)
		{
			return userObject.Call<bool>("setUserAttribute", new object[]{ key, val });
		}

		public override bool RemoveUserAttribute(string key)
		{
			return userObject.Call<bool>("removeUserAttribute", new object[]{ key });
		}
	}

	class IdentifyTaskImpl : IMParticleTask<IdentityApiResult>
	{

		AndroidJavaObject taskObject;

		internal IdentifyTaskImpl(AndroidJavaObject taskObject)
		{
			this.taskObject = taskObject;
		}

		public bool IsComplete()
		{
			return taskObject.Call<bool>("isComplete");
		}

		public bool IsSuccessful()
		{
			return taskObject.Call<bool>("isSuccessful");
		}

		public IdentityApiResult GetResult()
		{
			return new IdentityApiResult()
			{
				User = taskObject.Call<AndroidJavaObject>("getResult") == null ? new MParticleUserImpl(taskObject.Call<AndroidJavaObject>("getResult")) : null
			};
		}

		public IMParticleTask<IdentityApiResult> AddSuccessListener(OnSuccess listener)
		{
			taskObject.Call<AndroidJavaObject>("addSuccessListener", new object[] { new AndroidOnSuccessListener(listener) });
			return this;
		}

		public IMParticleTask<IdentityApiResult> AddFailureListener(OnFailure listener)
		{
			taskObject.Call<AndroidJavaObject>("addFailureListener", new object[]{ new AndroidOnFailureListener(listener) });
			return this;
		}

	}
}