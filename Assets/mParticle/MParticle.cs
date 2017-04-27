#if UNITY_IPHONE || UNITY_ANDROID
using UnityEngine;
using System.Collections.Generic;
using System;

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

        public CommerceEvent(ProductAction productAction, Product[] products, TransactionAttributes transactionAttributes)
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

        private TransactionAttributes(){}

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

        private Impression(){}

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

        private Promotion(){}

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
        Other
    };

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
        FacebookCustomAudienceId
    };

    public enum Environment
    {
        AutoDetect = 0,
        Development,
        Production
    };

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
        RemoveFromWishlist
    };

    public enum PromotionAction
    {
        View = 0,
        Click
    };

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

    public interface IMParticleSDK
    {
        void LogEvent(string eventName, EventType eventType, Dictionary<string, string> eventInfo);

        void LogCommerceEvent(CommerceEvent commerceEvent);

        void LogScreen(string screenName, Dictionary<string, string> eventInfo);

        void SetUserAttribute(string key, string val);

        void SetUserAttributeArray(string key, string[] values);

        void SetUserIdentity(string identity, UserIdentity identityType);

        void SetUserTag(string tag);

        void RemoveUserAttribute(string key);

        long IncrementUserAttribute(string key, int incrementValue);

        void LeaveBreadcrumb(string breadcrumbName, Dictionary<string, string> eventInfo);

        void SetOptOut(bool optOut);

        void Logout();

        Environment GetEnvironment();

        void Initialize(string apiKey, string apiSecret);
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
                    mp = new MParticleiOS ();
                #endif
                }
                return mp;
            }
        }

        void Awake()
        {

        }

        /// <summary>
        /// Starts the mParticle SDK
        /// </summary
        public void Initialize(string apiKey, string apiSecret)
        {
            mParticleInstance.Initialize(apiKey, apiSecret);
        }
        /// <summary>
        /// Logs an event. The eventInfo is limited to 100 key value pairs.
        /// The eventName and strings in eventInfo cannot contain more than 255 characters.
        /// </summary>
        /// <param name="eventName">The name of the event to be tracked (required not null)</param>
        /// <param name="eventType">An enum value that indicates the type of event that is to be tracked.</param>
        /// <param name="eventInfo">A dictionary containing further information about the event.</param>
        public void LogEvent(string eventName, EventType eventType, Dictionary<string, string> eventInfo)
        {
            mParticleInstance.LogEvent(eventName, eventType, eventInfo);
        }

        /// <summary>
        /// Logs a product action, promotion or impression event.
        /// </summary>
        /// <param name="commerceEvent">The commerce event (required not null)</param>
        public void LogCommerceEvent(CommerceEvent commerceEvent)
        {
            mParticleInstance.LogCommerceEvent(commerceEvent);
        }

        /// <summary>
        /// Logs a screen.
        /// </summary>
        /// <param name="screenName">The name of the screen to be tracked (required not null)</param>
        /// <param name="eventInfo">A dictionary containing further information about the screen.</param>
        public void LogScreen(string screenName, Dictionary<string, string> eventInfo)
        {
            mParticleInstance.LogScreen(screenName, eventInfo);
        }

        /// <summary>
        /// Sets a single user attribute. The property will be combined with any existing attributes.
        /// There is a 100 count limit to user attributes.
        /// </summary>
        /// <param name="key">The attribute key.</param>
        /// <param name="val">The attribute value.</param>
        public void SetUserAttribute(string key, string val)
        {
            mParticleInstance.SetUserAttribute(key, val);
        }

        /// <summary>
        /// Sets a single user attribute. The property will be combined with any existing attributes.
        /// There is a 100 count limit to user attributes.
        /// </summary>
        /// <param name="key">The attribute key.</param>
        /// <param name="values">The attribute values.</param>
        public void SetUserAttributeArray(string key, string[] values)
        {
            mParticleInstance.SetUserAttributeArray(key, values);
        }

        /// <summary>
        /// Sets User/Customer Identity.
        /// </summary>
        /// <param name="identity">A string representing the user identity.</param>
        /// <param name="identityType">An enum with the user identity type.</param>
        public void SetUserIdentity(string identity, UserIdentity identityType)
        {
            mParticleInstance.SetUserIdentity(identity, identityType);
        }

        /// <summary>
        /// Sets a single user tag or attribute. The tag will be combined with any existing attributes.
        /// There is a 100 count limit to user attributes.
        /// </summary>
        /// <param name="tag">The user tag/attribute.</param>
        public void SetUserTag(string tag)
        {
            mParticleInstance.SetUserTag(tag);
        }

        /// <summary>
        ///  Removes a single user attribute.
        /// </summary>
        /// <param name="tag">The user attribute key.</param>
        public void RemoveUserAttribute(string key)
        {
            mParticleInstance.RemoveUserAttribute(key);
        }

        /// <summary>
        /// Increments the value of a user attribute by the provided amount. If the key does not
        /// exist among the current user attributes, this method will add the key to the user attributes
        /// and set the value to the provided amount. If the key already exists and the existing value is not
        /// a number, the operation will abort and the returned value will be zero.
        /// </summary>
        /// <param name="key">The attribute key.</param>
        /// <param name="incrementValue">The increment amount.</param>
        /// <returns>The new value amount or zero, in case of failure.</returns>
        public long IncrementUserAttribute(string key, int incrementValue)
        {
            long newValue = mParticleInstance.IncrementUserAttribute(key, incrementValue);
            return newValue;
        }

        /// <summary>
        /// Leaves a breadcrumb.
        /// </summary>
        /// <param name="breadcrumbName">The name of the breadcrumb (required not null)</param>
        /// <param name="eventInfo">A dictionary containing further information about the breadcrumb.</param>
        public void LeaveBreadcrumb(string breadcrumbName, Dictionary<string, string> eventInfo)
        {
            mParticleInstance.LeaveBreadcrumb(breadcrumbName, eventInfo);
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
        /// Logs a user out.
        /// </summary>
        public void Logout()
        {
            mParticleInstance.Logout();
        }

        /// <summary>
        /// Gets the SDK running environment. The possible values are Development or Production.
        /// </summary>
        /// <returns>Whether the SDK is running in Development or Production mode.</returns>
        public Environment GetEnvironment()
        {
            return mParticleInstance.GetEnvironment();
        }

    }
}
#endif