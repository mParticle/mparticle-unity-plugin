using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

#if UNITY_ANDROID
using mParticleAndroid;
#endif
#if UNITY_IOS
using mParticleiOs;
#endif

namespace mParticle
{

	[Serializable]
	public sealed class CommerceEvent
	{
		public TransactionAttributes TransactionAttributes;
		public ProductAction ProductAction;
		public PromotionAction PromotionAction;
		public Product[] Products;
		public Promotion[] Promotions;
		public Impression[] Impressions;
		public string ScreenName;
		public string Currency;
		public Dictionary<string, string> CustomAttributes;
		public string CheckoutOptions;
		public string ProductActionListName;
		public string ProductActionListSource;
		public int? CheckoutStep;
		public bool? NonInteractive;

		public CommerceEvent(ProductAction productAction, Product[] products, TransactionAttributes transactionAttributes = null)
		{
			this.ProductAction = productAction;
			this.Products = products;
			this.TransactionAttributes = transactionAttributes;
		}

		public CommerceEvent(PromotionAction newPromotionAction, Promotion[] newPromotions)
		{
			this.PromotionAction = newPromotionAction;
			this.Promotions = newPromotions;
		}

		public CommerceEvent(Impression[] impressions)
		{
			this.Impressions = impressions;
		}
	}

	[Serializable]
	public sealed class MPEvent
	{
		public EventType EventType;
		public string EventName;
		public string Category;
		public Dictionary<string, string> Info;
		public Double? Duration;
		public Double? StartTime;
		public Double? EndTime;
		public IDictionary<string, List<string>> CustomFlags;

		public MPEvent(string eventName, EventType eventType = EventType.Other)
		{
			this.EventName = eventName;
			this.EventType = eventType;
		}
	}

	[Serializable]
	public sealed class Product
	{

		public string Name;
		public string Sku;
		public double Price;
		public double Quantity;
		public string Brand;
		public string CouponCode;
		public int? Position;
		public string Category;
		public string Variant;
		public Dictionary<string, string> customAttributes;

		private Product()
		{
		}

		public Product(string name, string sku, double price, double quantity)
		{
			this.Name = name;
			this.Sku = sku;
			this.Price = price;
			this.Quantity = quantity;
		}
	}

	[Serializable]
	public sealed class TransactionAttributes
	{
		public string TransactionId;
		public string Affiliation;
		public double? Revenue = null;
		public double? Shipping = null;
		public double? Tax = null;
		public string CouponCode;

		private TransactionAttributes()
		{
		}

		public TransactionAttributes(string transactionId)
		{
			this.TransactionId = transactionId;
		}
	}

	[Serializable]
	public sealed class Impression
	{
		public string ImpressionListName;
		public Product[] Products;

		private Impression()
		{
		}

		public Impression(string impressionListName, Product[] products)
		{
			this.ImpressionListName = impressionListName;
			this.Products = products;
		}
	}

	[Serializable]
	public sealed class Promotion
	{
		public string Id;
		public string Name;
		public string Creative;
		public int? Position;

		private Promotion()
		{
		}

		public Promotion(string id, string name, string creative, int? position)
		{
			this.Id = id;
			this.Name = name;
			this.Creative = creative;
			this.Position = position;
		}
	}

	public enum EventType
	{
		Navigation = 1,
		Location,
		Search,
		Transaction,
		UserContent,
		UserPreference,
		Social,
		Other}

	;

	public enum UserIdentity
	{
		Other = 0,
		CustomerId,
		Facebook,
		Twitter,
		Google,
		Microsoft,
		Yahoo,
		Email,
		Alias,
		FacebookCustomAudienceId,
		Other2,
		Other3,
		Other4,
		Other5,
		Other6,
		Other7,
		Other8,
		Other9,
		Other10,
		MobileNumber,
		PhoneNumber2,
		PhoneNumber3,
		IOSAdvertiserId,
		IOSVendorId,
		PushToken,
		DeviceApplicationStamp}

	;

	public enum Environment
	{
		AutoDetect = 0,
		Development,
		Production}

	;

	public enum InstallType
	{
		AutoDetect = 0,
		KnownInstall,
		KnownUpgrade
	}

	public enum ProductAction
	{
		AddToCart = 1,
		RemoveFromCart,
		Checkout,
		CheckoutOption,
		Click,
		ViewDetail,
		Purchase,
		Refund,
		AddToWishlist,
		RemoveFromWishlist}

	;

	public enum PromotionAction
	{
		View = 0,
		Click}

	;

	public enum ATTAuthStatus
	{
		NotDetermined = 0,
		Restricted,
		Denied,
		Authorized}

	;

	public static class UserAttribute
	{
		public const string
			FirstName = "$FirstName",
			LastName = "$LastName",
			Address = "$Address",
			State = "$State",
			City = "$City",
			Zipcode = "$Zipcode",
			Country = "$Country",
			Age = "$Age",
			Gender = "$Gender",
			MobileNumber = "$MobileNumber";
	};

	public enum LogLevel
	{
		NONE,
		/**
         * Used for critical issues with the SDK or its configuration.
         */
		ERROR,
		/**
         * (default) Used to warn developers of potentially unintended consequences of their use of the SDK.
         */
		WARNING,
		/**
         * Used to communicate the internal state and processes of the SDK.
         */
		DEBUG,
		/*
         * Used to relay fine-grained issues with the usage of the SDK
         */
		VERBOSE,
		/*
         * Used to communicate
         */
		INFO
	}

	[Serializable]
	public sealed class LocationTracking
	{
        public Boolean Enabled;

        public string Provider;

        public long? MinTime;

        public long? MinDistance;

        public long? MinAccuracy;

		public LocationTracking(Boolean enabled)
		{
			this.Enabled = enabled;
		}

		public LocationTracking(String provider, long minTime, long minDistance, long minAccuracy)
		{
			this.Enabled = true;
			this.Provider = provider;
			this.MinTime = minTime;
			this.MinDistance = minDistance;
			this.MinAccuracy = minAccuracy;
		}
	}

	[Serializable]
	public sealed class PushRegistration
	{
		public String AndroidSenderId;
		public String AndroidInstanceId;
		public String IOSToken;
	}

	[Serializable]
	public sealed class MParticleOptions
	{
		public InstallType? InstallType;
		public Environment? Environment;
		public String ApiKey;
		public String ApiSecret;
		public IdentityApiRequest IdentifyRequest;
		public Boolean? DevicePerformanceMetricsDisabled;
		public Boolean? IdDisabled;
		public int? UploadInterval;
		public int? SessionTimeout;
		public Boolean? UnCaughtExceptionLogging;
		public LogLevel? LogLevel;
		public LocationTracking LocationTracking;
		public PushRegistration PushRegistration;
		public OnUserIdentified IdentityStateListener;
	}

	[Serializable]
	public sealed class IdentityApiRequest
	{
		public IdentityApiRequest()
		{

		}

		public IdentityApiRequest(MParticleUser user)
		{
			if (user != null)
			{
				UserIdentities = user.GetUserIdentities();
			}
		}

		public Dictionary<UserIdentity, String> UserIdentities = new Dictionary<UserIdentity, string>();
		public OnUserAlias UserAliasHandler;
	}

	public delegate void OnUserAlias(MParticleUser previousUser,MParticleUser newUser);

	public abstract class MParticleUser
	{
		public abstract long Mpid { get; }

		/// <summary>
		/// Sets a single user tag or attribute. The tag will be combined with any existing attributes.
		/// There is a 100 count limit to user attributes.
		/// </summary>
		/// <param name="tag">The user tag/attribute.</param>
		public abstract bool SetUserTag(String tag);

		public abstract Dictionary<UserIdentity, string> GetUserIdentities();

		public abstract Dictionary<string, string> GetUserAttributes();

		public abstract bool SetUserAttributes(Dictionary<string, string> userAttributes);

		public abstract bool SetUserAttribute(string key, string val);

		/// <summary>
		///  Removes a single user attribute.
		/// </summary>
		/// <param name="tag">The user attribute key.</param>
		public abstract bool RemoveUserAttribute(string key);

		public override string ToString()
		{
			return "User: \n" + "\tMPID = " + Mpid + "\n\tUser Identitites = " + GetUserIdentities().Aggregate("", (aggrigate, pair) => aggrigate + pair.Key.ToString() + ":" + pair.Value + ", ") + "\n\tUser Attributes = " + GetUserAttributes().Aggregate("", (aggrigate, pair) => aggrigate + pair.Key.ToString() + ":" + pair.Value + ", ");
		}
	}

	public interface IIdentityApi
	{
		MParticleUser CurrentUser { get; }

		MParticleUser GetUser(long mpid);

		void AddIdentityStateListener(OnUserIdentified listener);

		void RemoveIdentityStateListener(OnUserIdentified listener);

		IMParticleTask<IdentityApiResult> Identify(IdentityApiRequest request);

		IMParticleTask<IdentityApiResult> Login(IdentityApiRequest request = null);

		IMParticleTask<IdentityApiResult> Logout(IdentityApiRequest request = null);

		IMParticleTask<IdentityApiResult> Modify(IdentityApiRequest request);
	}

	public delegate void OnUserIdentified(MParticleUser user);

	public interface IMParticleTask<T>
	{
		Boolean IsComplete();

		Boolean IsSuccessful();

		T GetResult();

		IMParticleTask<T> AddSuccessListener(OnSuccess listener);

		IMParticleTask<T> AddFailureListener(OnFailure listener);
	}

	public delegate void OnSuccess(IdentityApiResult result);
	public delegate void OnFailure(IdentityHttpResponse result);

	public sealed class IdentityApiResult
	{
		public MParticleUser User;
	}

	public class IdentityHttpResponse
	{
		public Boolean IsSuccessful;
		public List<Error> Errors = new List<Error>();
		public int HttpCode;
	}

	public sealed class Error
	{
		public String Message;
		public String Code;
	}

	public interface IMParticleSDK
	{
		void LogEvent(MPEvent mpEvent);

		void LogEvent(CommerceEvent commerceEvent);

		void LogScreen(string screenName);

		void SetATTStatus(ATTAuthStatus status, double timestamp);

		void LeaveBreadcrumb(string breadcrumbName);

		void SetOptOut(bool optOut);

		Environment Environment { get; }

		void Initialize(MParticleOptions options);

		IIdentityApi Identity { get; }

		void Upload();
	}

	public sealed class MParticle : MonoBehaviour, IMParticleSDK
	{

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
					mp = new MParticleiOS();
					#endif
				}
				return mp;
			}
		}

		private IIdentityApi _identity;

		public IIdentityApi Identity
		{
			get
			{
				return mParticleInstance.Identity;
			}
		}

		void Awake()
		{

		}

		/// <summary>
		/// Starts the mParticle SDK
		/// </summary>
		/// <param name="options">Your application's startup options</param>
		public void Initialize(MParticleOptions options)
		{
			mParticleInstance.Initialize(options);
		}

		/// <summary>
		/// Logs a product action, promotion or impression event.
		/// </summary>
		/// <param name="commerceEvent">The commerce event (required not null)</param>
		public void LogEvent(CommerceEvent commerceEvent)
		{
			mParticleInstance.LogEvent(commerceEvent);
		}

		public void LogEvent(MPEvent mpEvent)
		{
			mParticleInstance.LogEvent(mpEvent);
		}

		/// <summary>
		/// Logs a screen.
		/// </summary>
		/// <param name="screenName">The name of the screen to be tracked (required not null)</param>
		/// <param name="eventInfo">A dictionary containing further information about the screen.</param>
		public void LogScreen(string screenName)
		{
			mParticleInstance.LogScreen(screenName);
		}

		/// <summary>
		/// Set the ATT status for iOS devices.
		/// </summary>
		/// <param name="status">The App Tracking Transparency Status of the iOS device (required not null)</param>
		/// <param name="timestamp">An optional timestamp (in ms) for when this status was set.</param>
		public void SetATTStatus(ATTAuthStatus status, double timestamp)
		{
			mParticleInstance.SetATTStatus(status, timestamp);
		}

		/// <summary>
		/// Leaves a breadcrumb.
		/// </summary>
		/// <param name="breadcrumbName">The name of the breadcrumb (required not null)</param>
		/// <param name="eventInfo">A dictionary containing further information about the breadcrumb.</param>
		public void LeaveBreadcrumb(string breadcrumbName)
		{
			mParticleInstance.LeaveBreadcrumb(breadcrumbName);
		}

		/// <summary>
		/// Sets the opt-out status for the application. Set it to true to opt-out of event tracking. Default value is false.
		/// </summary>
		/// <param name="optOut">The opt-out status.</param>
		public void SetOptOut(bool optOut)
		{
			mParticleInstance.SetOptOut(optOut);
		}

		/// <summary>
		/// Gets the SDK running environment. The possible values are Development or Production.
		/// </summary>
		/// <returns>Whether the SDK is running in Development or Production mode.</returns>
		public Environment Environment
		{
			get
			{
				return mParticleInstance.Environment;
			}
		}

		public void Upload()
		{
			mParticleInstance.Upload();
		}
			
		/*
		 * Callback methods, these will be called by the native objective-c sdk
		 */
		public void OnUserIdentified(string userIdentified)
		{
			Console.WriteLine("OnUserIdentified callback triggered:\n" + userIdentified + "\n");
#if UNITY_IOS
			if (Identity is mParticleiOs.IdentityCallbacks)
			{
				(Identity as mParticleiOs.IdentityCallbacks).OnUserIdentified(userIdentified);
			}
#endif
		}

		public void OnUserAlias(string usersMessage)
		{
			Console.WriteLine("OnUserAlias callback triggered:\n" + usersMessage + "\n");
#if UNITY_IOS
			if (Identity is mParticleiOs.IdentityCallbacks)
			{
				(Identity as mParticleiOs.IdentityCallbacks).OnUserAlias(usersMessage);
			}
#endif
		}

		public void OnTaskSuccess(string successMessage)
		{
			Console.WriteLine("OnTaskSuccess callback triggered:\n" + successMessage + "\n");
#if UNITY_IOS
			if (Identity is mParticleiOs.IdentityCallbacks)
			{
				(Identity as mParticleiOs.IdentityCallbacks).OnTaskSuccess(successMessage);
			}
#endif
		}

		public void OnTaskFailure(string failureMessage)
		{
			Console.WriteLine("OnTaskFailure callback triggered:\n" + failureMessage + "\n");
#if UNITY_IOS
			if (Identity is mParticleiOs.IdentityCallbacks)
			{
				(Identity as mParticleiOs.IdentityCallbacks).OnTaskFailure(failureMessage);
			}
#endif
		}

		public void Message(string message)
		{
			Console.WriteLine("Message Received: " + message);
		}
	}

	public sealed class IdentityApi : IIdentityApi
	{
		public const int ThrottleError = 429;

		public MParticleUser GetUser(long mpid)
		{
			return MParticle.Instance.Identity.GetUser(mpid);
		}

		public void AddIdentityStateListener(OnUserIdentified listener)
		{
			MParticle.Instance.Identity.AddIdentityStateListener(listener);
		}

		public void RemoveIdentityStateListener(OnUserIdentified listener)
		{
			MParticle.Instance.Identity.RemoveIdentityStateListener(listener);
		}

		public IMParticleTask<IdentityApiResult> Identify(IdentityApiRequest request = null)
		{
			return MParticle.Instance.Identity.Identify(request);
		}

		public IMParticleTask<IdentityApiResult> Login(IdentityApiRequest request = null)
		{
			return MParticle.Instance.Identity.Login(request);
		}

		public IMParticleTask<IdentityApiResult> Logout(IdentityApiRequest request = null)
		{
			return MParticle.Instance.Identity.Logout(request);
		}

		public IMParticleTask<IdentityApiResult> Modify(IdentityApiRequest request)
		{
			return MParticle.Instance.Identity.Modify(request);
		}

		public MParticleUser CurrentUser
		{
			get
			{
				return MParticle.Instance.Identity.CurrentUser;
			}
		}

	

	}
}