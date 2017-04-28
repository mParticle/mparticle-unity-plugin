#if UNITY_IPHONE || UNITY_ANDROID
using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class MPCommerceEvent
{
    public TransactionAttributes transactionAttributes;
    public MParticle.ProductAction productAction;
    public MParticle.PromotionAction promotionAction;
    public Product[] products;
    public Promotion[] promotions;
    public Impression[] impressions;
    public string screenName;
    public string currency;
    public Dictionary<string, string> customAttributes;
    public string checkoutOptions;
    public string productActionListName;
    public string productActionListSource;
    public int checkoutStep;
    public bool nonInteractive;

    public MPCommerceEvent (MParticle.ProductAction newProductAction, Product[] newProducts, TransactionAttributes newTransactionAttributes)
    {
        this.productAction = newProductAction;
        this.products = newProducts;
        this.transactionAttributes = newTransactionAttributes;
    }

    public MPCommerceEvent (MParticle.PromotionAction newPromotionAction, Promotion[] newPromotions)
    {
        this.promotionAction = newPromotionAction;
        this.promotions = newPromotions;
    }

    public MPCommerceEvent (Impression[] newImpressions)
    {
        this.impressions = newImpressions;
    }
}

[Serializable]
public class Product
{

    public string name;
    public string sku;
    public Double price;
    public Double quantity;
    public string brand;
    public string couponCode;
    public Double position;
    public string category;
    public string variant;
    public Dictionary<string, string> customAttributes;

    public Product (string newName, string newSku, Double newPrice, Double newQuantity)
    {
        this.name = newName;
        this.sku = newSku;
        this.price = newPrice;
        this.quantity = newQuantity;
    }

    public Product SetBrand (string newBrand)
    {
        this.brand = newBrand;
        return this;
    }

    public Product SetCouponCode (string newCouponCode)
    {
        this.couponCode = newCouponCode;
        return this;
    }

    public Product SetPosition (Double newPosition)
    {
        this.position = newPosition;
        return this;
    }

    public Product SetCategory (string newCategory)
    {
        this.category = newCategory;
        return this;
    }

    public Product SetVariant (string newVariant)
    {
        this.variant = newVariant;
        return this;
    }

    public Product SetCustomAttributes (Dictionary<string, string> newCustomAttributes)
    {
        this.customAttributes = newCustomAttributes;
        return this;
    }
}

[Serializable]
public class TransactionAttributes
{
    public string transactionId;
    public string affiliation;
    public Double revenue;
    public Double shipping;
    public Double tax;
    public string couponCode;

    public TransactionAttributes (string newTransactionId)
    {
        this.transactionId = newTransactionId;
    }

    public TransactionAttributes SetAffiliation (string newAffiliation)
    {
        this.affiliation = newAffiliation;
        return this;
    }

    public TransactionAttributes SetRevenue (Double newRevenue)
    {
        this.revenue = newRevenue;
        return this;
    }

    public TransactionAttributes SetShipping (Double newShipping)
    {
        this.shipping = newShipping;
        return this;
    }

    public TransactionAttributes SetTax (Double newTax)
    {
        this.tax = newTax;
        return this;
    }

    public TransactionAttributes SetCouponCode (string newCouponCode)
    {
        this.couponCode = newCouponCode;
        return this;
    }
}

[Serializable]
public class Impression
{
    public string impressionListName;
    public Product[] products;

    public Impression (string newImpressionListName, Product[] newProducts)
    {
        this.impressionListName = newImpressionListName;
        this.products = newProducts;
    }
}

[Serializable]
public class Promotion
{
    public string id;
    public string name;
    public string creative;
    public int position;

    public Promotion (string newId, string newName, string newCreative, int newPosition)
    {
        this.id = newId;
        this.name = newName;
        this.creative = newCreative;
        this.position = newPosition;
    }
}

public interface IMParticleSDK
{
    void Initialize(string key, string secret);

    void LogEvent (string eventName, MParticle.EventType eventType, Dictionary<string, string> eventInfo);

    void LogCommerceEvent (MPCommerceEvent commerceEvent);

    void LogScreen (string screenName, Dictionary<string, string> eventInfo);

    void SetUserAttribute (string key, string val);

    void SetUserAttributeArray (string key, string[] values);

    void SetUserIdentity (string identity, MParticle.UserIdentity identityType);

    void SetUserTag (string tag);

    void RemoveUserAttribute (string key);

    long IncrementUserAttribute (string key, long incrementValue);

    void LeaveBreadcrumb (string breadcrumbName, Dictionary<string, string> eventInfo);

    void SetOptOut (bool optOut);

    void Logout ();

    MParticle.MPEnvironment GetEnvironment ();
}

public class MParticle : MonoBehaviour, IMParticleSDK
{
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
        FacebookCustomAudienceId}

    ;

    public enum MPEnvironment
    {
        AutoDetect = 0,
        Development,
        Production}

    ;

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

    private static MParticle instance;

    public static MParticle Instance {
        get { return instance ?? (instance = new GameObject ("MParticle").AddComponent<MParticle> ()); }
    }

    private IMParticleSDK mp;

    private IMParticleSDK mParticleInstance {
        get {
            if (mp == null) {
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

    }

    /// <summary>
    /// Starts the mParticle SDK.
    /// </summary>
    /// <param name="key">App Key</param>
    /// <param name="secret">App Secret</param>
    public void Initialize(string key, string secret)
    {
        mParticleInstance.Initialize (key, secret);
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
        mParticleInstance.LogEvent (eventName, eventType, eventInfo);
    }

    /// <summary>
    /// Logs a product action, promotion or impression event.
    /// </summary>
    /// <param name="commerceEvent">The commerce event (required not null)</param>
    public void LogCommerceEvent (MPCommerceEvent commerceEvent)
    {
        mParticleInstance.LogCommerceEvent (commerceEvent);
    }

    /// <summary>
    /// Logs a screen.
    /// </summary>
    /// <param name="screenName">The name of the screen to be tracked (required not null)</param>
    /// <param name="eventInfo">A dictionary containing further information about the screen.</param>
    public void LogScreen (string screenName, Dictionary<string, string> eventInfo)
    {
        mParticleInstance.LogScreen (screenName, eventInfo);
    }

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
    /// Sets a single user attribute. The property will be combined with any existing attributes.
    /// There is a 100 count limit to user attributes.
    /// </summary>
    /// <param name="key">The attribute key.</param>
    /// <param name="values">The attribute values.</param>
    public void SetUserAttributeArray (string key, string[] values)
    {
        mParticleInstance.SetUserAttributeArray (key, values);
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

    /// <summary>
    ///  Removes a single user attribute.
    /// </summary>
    /// <param name="tag">The user attribute key.</param>
    public void RemoveUserAttribute (string key)
    {
        mParticleInstance.RemoveUserAttribute (key);
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
    public long IncrementUserAttribute (string key, long incrementValue)
    {
        long newValue = mParticleInstance.IncrementUserAttribute (key, incrementValue);
        return newValue;
    }

    /// <summary>
    /// Leaves a breadcrumb.
    /// </summary>
    /// <param name="breadcrumbName">The name of the breadcrumb (required not null)</param>
    /// <param name="eventInfo">A dictionary containing further information about the breadcrumb.</param>
    public void LeaveBreadcrumb (string breadcrumbName, Dictionary<string, string> eventInfo)
    {
        mParticleInstance.LeaveBreadcrumb (breadcrumbName, eventInfo);
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
    /// Logs a user out.
    /// </summary>
    public void Logout ()
    {
        mParticleInstance.Logout ();
    }

    /// <summary>
    /// Gets the SDK running environment. The possible values are Development or Production.
    /// </summary>
    /// <returns>Whether the SDK is running in Development or Production mode.</returns>
    public MParticle.MPEnvironment GetEnvironment ()
    {
        return mParticleInstance.GetEnvironment ();
    }
}
#endif