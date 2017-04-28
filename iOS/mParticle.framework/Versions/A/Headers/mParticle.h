//
//  mParticle.h
//
//  Copyright 2014. mParticle, Inc. All Rights Reserved.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import <CoreLocation/CoreLocation.h>
#import <mParticle/MPUserSegments.h>
#import <mParticle/MPProduct.h>

// Event Types
typedef NS_ENUM(NSUInteger, MPEventType) {
    /** Use for navigation related events */
    MPEventTypeNavigation = 1,
    /** Use for location related events */
    MPEventTypeLocation,
    /** Use for search related events */
    MPEventTypeSearch,
    /** Use for transaction related events */
    MPEventTypeTransaction,
    /** Use for user content related events */
    MPEventTypeUserContent,
    /** Use for user preference related events */
    MPEventTypeUserPreference,
    /** Use for social related events */
    MPEventTypeSocial,
    /** Use for other types of events not contained in this enum */
    MPEventTypeOther
};

// User Identity constants
typedef NS_ENUM(NSUInteger, MPUserIdentity) {
    /** User identity other */
    MPUserIdentityOther = 0,
    /** User identity customer id. This is an id issue by your own system */
    MPUserIdentityCustomerId,
    /** User identity Facebook */
    MPUserIdentityFacebook,
    /** User identity Twitter */
    MPUserIdentityTwitter,
    /** User identity Goolge */
    MPUserIdentityGoogle,
    /** User identity Microsoft */
    MPUserIdentityMicrosoft,
    /** User identity Yahoo! */
    MPUserIdentityYahoo,
    /** User identity Email */
    MPUserIdentityEmail,
    /** User identity Alias */
    MPUserIdentityAlias
};

// Supported Social Networks
typedef NS_OPTIONS(uint64_t, MPSocialNetworks) {
    /** Social Network Facebook */
    MPSocialNetworksFacebook = 1 << 1,
    /** Social Network Twitter */
    MPSocialNetworksTwitter = 1 << 2
};

// Installation Types
typedef NS_ENUM(NSInteger, MPInstallationType) {
    /** mParticle auto-detects the installation type. This is the default value */
    MPInstallationTypeAutodetect = 0,
    /** Informs mParticle this binary is a new installation */
    MPInstallationTypeKnownInstall,
    /** Informs mParticle this binary is an upgrade */
    MPInstallationTypeKnownUpgrade,
    /** Informs mParticle this binary is the same version. This value is for internal use only. It should not be used by developers */
    MPInstallationTypeKnownSameVersion
};

// eCommerce Product Events
typedef NS_ENUM(NSInteger, MPProductEvent) {
    /** To be used when a product is viewed by a user */
    MPProductEventView = 0,
    /** To be used when a user adds a product to a wishlist */
    MPProductEventAddedToWishList,
    /** To be used when a user removes a product from a wishlist */
    MPProductEventRemovedFromWishList,
    /** To be used when a user adds a product to a cart */
    MPProductEventAddedToCart,
    /** To be used when a user removes a product from a cart */
    MPProductEventRemovedFromCart
};

typedef NS_ENUM(NSUInteger, MPEnvironment) {
    /** Tells the SDK to auto detect the current run environment (initial value) */
    MPEnvironmentAutoDetect = 0,
    /** The SDK is running in development mode (Debug/Development or AdHoc) */
    MPEnvironmentDevelopment,
    /** The SDK is running in production mode (App Store) */
    MPEnvironmentProduction
};

/**
 Social Networks callback handler.
 @param socialNetwork the social network
 @param granted it contains the status of request, if it was granted or not
 @param error if the request was not granted, error will contain the reason, otherwise it will be nil
 */
typedef void(^MPSocialNetworksHandler)(MPSocialNetworks socialNetwork, BOOL granted, NSError *error);

#pragma mark - MParticle
/**
 This is the main class of the mParticle SDK. It interfaces your app with the mParticle API
 so you can report and measure the many different metrics of your app.
 */
@interface MParticle : NSObject

#pragma mark Properties
/**
 Enables or disables the inclusion of location information to messages when your app is running on the
 background. The default value is YES. Setting it to NO will cause the SDK to include location
 information only when your app is running on the foreground.
 @see beginLocationTracking:minDistance:
 */
@property (nonatomic, unsafe_unretained) BOOL backgroundLocationTracking;

/**
 Forwards setting/resetting the debug mode for embedded third party SDKs.
 This is a write only property.
 */
@property (nonatomic, unsafe_unretained) BOOL debugMode;
- (BOOL)debugMode UNAVAILABLE_ATTRIBUTE;

/**
 Enables or disables log outputs to the console. If set to YES development logs will be output to the 
 console, if set to NO the development logs will be suppressed. This property works in conjunction with 
 the environment property. If the environment is Production, consoleLogging will always be NO, 
 regardless of the value you assign to it.
 @see environment
 */
@property (nonatomic, unsafe_unretained, readwrite) BOOL consoleLogging;

/**
 The environment property returns the running SDK environment: Development or Production. You can 
 set it to a different value, from the one initially detected, or set it to auto detection (default 
 initial value). Auto detection forces the SDK to detect whether it is running in Development or 
 Production mode the next time the environment property is used.
 Trying to set this property to MPEnvironmentDevelopment after the SDK has detected the running
 environment to be MPEnvironmentProduction will result in no action. This is to avoid the situation
 of setting the SDK environment to development after the app has been deployed to the App Store.
 @see MPEnvironment
 */
@property (nonatomic, unsafe_unretained, readwrite) MPEnvironment environment;

/**
 Gets/Sets the current location of the active session.
 @see beginLocationTracking:minDistance:
 */
@property (nonatomic, strong) CLLocation *location;

/**
 Flag indicating whether network performance is being measured.
 @see beginMeasuringNetworkPerformance
 */
@property (nonatomic, unsafe_unretained, readonly) BOOL measuringNetworkPerformance;

/**
 Gets/Sets the opt-in/opt-out status for the application. Set it to YES to opt-out of event tracking. Set it to NO to opt-in of event tracking.
 */
@property (nonatomic, unsafe_unretained, readwrite) BOOL optOut;

/**
 Gets/Sets the push notification token for the application.
 @see registerForPushNotificationWithTypes:
 */
@property (nonatomic, strong) NSData *pushNotificationToken;

/**
 Gets/Sets the user session timeout interval. A session is ended if the app goes into the background for longer than the session timeout interval.
 */
@property (nonatomic, unsafe_unretained, readwrite) NSTimeInterval sessionTimeout;

/**
 Gets/Sets the interval to upload data to mParticle servers.
 */
@property (nonatomic, unsafe_unretained, readwrite) NSTimeInterval uploadInterval;

#pragma mark - Initialization

/**
 Returns the shared instance object.
 @returns the Singleton instance of the MParticle class.
 */
+ (instancetype)sharedInstance;

/**
 Starts the API with the api_key and api_secret saved in MParticleConfig.plist.  If you
 use startAPI instead of startAPIWithKey:secret: your API key and secret must
 be added to these parameters in the MParticleConfig.plist.
 @see startWithKey:secret:installationType:
 */
- (void)start;

/**
 Starts the API with your API key and a secret.
 It is required that you use either this method or startAPI to authorize the API before
 using the other API methods.  The apiKey and secret that are passed in to this method
 will override the api_key and api_secret parameters of the (optional) MParticleConfig.plist.
 @param apiKey The API key for your account
 @param secret The API secret for your account
 @see startWithKey:secret:installationType:
 */
- (void)startWithKey:(NSString *)apiKey secret:(NSString *)secret;

/**
 Starts the API with your API key and a secret and installation type.
 It is required that you use either this method or startAPI to authorize the API before
 using the other API methods.  The apiKey and secret that are passed in to this method
 will override the api_key and api_secret parameters of the (optional) MParticleConfig.plist.
 @param apiKey The API key for your account
 @param secret The API secret for your account
 @param installType You can tell the mParticle SDK if this is a new install, an upgrade, or let the SDK detect it automatically.
 */
- (void)startWithKey:(NSString *)apiKey secret:(NSString *)secret installationType:(MPInstallationType)installationType;

#pragma mark - Basic Tracking
/**
 Logs an event. The eventInfo is limited to 100 key value pairs.
 The event name and strings in eventInfo cannot contain more than 255 characters.
 @param eventName The name of the event to be tracked (required not nil)
 @param eventType An enum value that indicates the type of event that is to be tracked
 @see logEvent:eventType:eventInfo:eventLength:category:
 */
- (void)logEvent:(NSString *)eventName eventType:(MPEventType)eventType;

/**
 Logs an event. The eventInfo is limited to 100 key value pairs.
 The event name and strings in eventInfo cannot contain more than 255 characters.
 @param eventName The name of the event to be tracked (required not nil)
 @param eventType An enum value that indicates the type of event that is to be tracked
 @param eventInfo A dictionary containing further information about the event
 @see logEvent:eventType:eventInfo:eventLength:category:
 */
- (void)logEvent:(NSString *)eventName eventType:(MPEventType)eventType eventInfo:(NSDictionary *)eventInfo;

/**
 Logs an event. The eventInfo is limited to 100 key value pairs.
 The event name and strings in eventInfo cannot contain more than 255 characters.
 @param eventName The name of the event to be tracked (required not nil)
 @param eventType An enum value that indicates the type of event that is to be tracked
 @param eventInfo A dictionary containing further information about the event
 @param eventLength The duration of the event
 @see logEvent:eventType:eventInfo:eventLength:category:
 */
- (void)logEvent:(NSString *)eventName eventType:(MPEventType)eventType eventInfo:(NSDictionary *)eventInfo eventLength:(NSTimeInterval)eventLength;

/**
 Logs an event. The eventInfo is limited to 100 key value pairs.
 The event name, strings in eventInfo, and the category name cannot contain more than 255 characters.
 @param eventName The name of the event to be tracked (required not nil)
 @param eventType An enum value that indicates the type of event that is to be tracked
 @param eventInfo A dictionary containing further information about the event
 @param eventLength The duration of the event
 @param category A string with the developer/company defined category of the event
 */
- (void)logEvent:(NSString *)eventName eventType:(MPEventType)eventType eventInfo:(NSDictionary *)eventInfo eventLength:(NSTimeInterval)eventLength category:(NSString *)category;

/**
 Logs a screen with a screen name.
 @param screenName The name of the screen to be tracked (required not nil)
 @see logScreen:eventInfo:
 */
- (void)logScreen:(NSString *)screenName;

/**
 Logs a screen with a screen name and an attributes dictionary.
 @param screenName The name of the screen to be tracked (required not nil)
 @param eventInfo A dictionary containing further information about the screen
 */
- (void)logScreen:(NSString *)screenName eventInfo:(NSDictionary *)eventInfo;

#pragma mark - Error, Exception, and Crash Handling
/**
 Enables mParticle exception handling to automatically log events on uncaught exceptions.
 */
- (void)beginUncaughtExceptionLogging;

/**
 Disables mParticle exception handling.
 */
- (void)endUncaughtExceptionLogging;

/**
 Leaves a breadcrumb. Breadcrumbs are send together with crash reports to help with debugging.
 @param breadcrumbName The name of the breadcrumb (required not nil)
 */
- (void)leaveBreadcrumb:(NSString *)breadcrumbName;

/**
 Leaves a breadcrumb. Breadcrumbs are send together with crash reports to help with debugging.
 @param breadcrumbName The name of the breadcrumb (required not nil)
 @param eventInfo A dictionary containing further information about the breadcrumb
 */
- (void)leaveBreadcrumb:(NSString *)breadcrumbName eventInfo:(NSDictionary *)eventInfo;

/**
 Logs an error with a message.
 @param message The name of the error to be tracked (required not nil)
 @see logError:eventInfo:
 */
- (void)logError:(NSString *)message;

/**
 Logs an error with a message and an attributes dictionary. The eventInfo is limited to
 100 key value pairs. The strings in eventInfo cannot contain more than 255 characters.
 @param message The name of the error event (required not nil)
 @param eventInfo A dictionary containing further information about the error
 */
- (void)logError:(NSString *)message eventInfo:(NSDictionary *)eventInfo;

/**
 Logs an exception.
 @param exception The exception which occured
 @see logException:topmostContext:
 */
- (void)logException:(NSException *)exception;

/**
 Logs an exception and a context.
 @param exception The exception which occured
 @param topmostContext The topmost context of the app, typically the topmost view controller
 */
- (void)logException:(NSException *)exception topmostContext:(id)topmostContext;

#pragma mark - eCommerce Transactions
/**
 Logs an event with a product, such as viewing, adding to a shopping cart, etc.
 @param productEvent The event, from the MPProductEvent enum, describing the log action (view, remove from wish list, etc)
 @apram product An instance of MPProduct representing the product in question
 */
- (void)logProductEvent:(MPProductEvent)productEvent product:(MPProduct *)product;

/**
 Logs an e-commerce transaction event.
 @param productName The name of the product
 @param affiliation An entity with which the transaction should be affiliated (e.g. a particular store). If nil, mParticle will use an empty string
 @param sku The SKU of a product
 @param unitPrice The price of a product. If free or non-applicable use 0
 @param quantity The quantity of a product. If non-applicable use 0
 @param revenueAmount The total revenue of a transaction, including tax and shipping. If free or non-applicable use 0
 @param taxAmount The total tax for a transaction. If free or non-applicable use 0
 @param shippingAmount The total cost of shipping for a transaction. If free or non-applicable use 0
 
 @see logTransaction:affiliation:sku:unitPrice:quantity:revenueAmount:taxAmount:shippingAmount:transactionId:productCategory:currencyCode:
 */
- (void)logTransaction:(NSString *)productName affiliation:(NSString *)affiliation sku:(NSString *)sku unitPrice:(double)unitPrice quantity:(NSInteger)quantity revenueAmount:(double)revenueAmount taxAmount:(double)taxAmount shippingAmount:(double)shippingAmount;

/**
 Logs an e-commerce transaction event.
 @param productName The name of the product
 @param affiliation An entity with which the transaction should be affiliated (e.g. a particular store). If nil, mParticle will use an empty string
 @param sku The SKU of a product
 @param unitPrice The price of a product. If free or non-applicable use 0
 @param quantity The quantity of a product. If non-applicable use 0
 @param revenueAmount The total revenue of a transaction, including tax and shipping. If free or non-applicable use 0
 @param taxAmount The total tax for a transaction. If free or non-applicable use 0
 @param shippingAmount The total cost of shipping for a transaction. If free or non-applicable use 0
 @param transactionId A unique ID representing the transaction. This ID should not collide with other transaction IDs. If nil, mParticle will generate a random string
 
 @see logTransaction:affiliation:sku:unitPrice:quantity:revenueAmount:taxAmount:shippingAmount:transactionId:productCategory:currencyCode:
 */
- (void)logTransaction:(NSString *)productName affiliation:(NSString *)affiliation sku:(NSString *)sku unitPrice:(double)unitPrice quantity:(NSInteger)quantity revenueAmount:(double)revenueAmount taxAmount:(double)taxAmount shippingAmount:(double)shippingAmount transactionId:(NSString *)transactionId;

/**
 Logs an e-commerce transaction event.
 @param productName The name of the product
 @param affiliation An entity with which the transaction should be affiliated (e.g. a particular store). If nil, mParticle will use an empty string
 @param sku The SKU of a product
 @param unitPrice The price of a product. If free or non-applicable use 0
 @param quantity The quantity of a product. If non-applicable use 0
 @param revenueAmount The total revenue of a transaction, including tax and shipping. If free or non-applicable use 0
 @param taxAmount The total tax for a transaction. If free or non-applicable use 0
 @param shippingAmount The total cost of shipping for a transaction. If free or non-applicable use 0
 @param transactionId A unique ID representing the transaction. This ID should not collide with other transaction IDs. If nil, mParticle will generate a random string
 @param productCategory A category to which the product belongs
 
 @see logTransaction:affiliation:sku:unitPrice:quantity:revenueAmount:taxAmount:shippingAmount:transactionId:productCategory:currencyCode:
 */
- (void)logTransaction:(NSString *)productName affiliation:(NSString *)affiliation sku:(NSString *)sku unitPrice:(double)unitPrice quantity:(NSInteger)quantity revenueAmount:(double)revenueAmount taxAmount:(double)taxAmount shippingAmount:(double)shippingAmount transactionId:(NSString *)transactionId productCategory:(NSString *)productCategory;

/**
 Logs an e-commerce transaction event.
 @param productName The name of the product
 @param affiliation An entity with which the transaction should be affiliated (e.g. a particular store). If nil, mParticle will use an empty string
 @param sku The SKU of a product
 @param unitPrice The price of a product. If free or non-applicable use 0
 @param quantity The quantity of a product. If non-applicable use 0
 @param revenueAmount The total revenue of a transaction, including tax and shipping. If free or non-applicable use 0
 @param taxAmount The total tax for a transaction. If free or non-applicable use 0
 @param shippingAmount The total cost of shipping for a transaction. If free or non-applicable use 0
 @param transactionId A unique ID representing the transaction. This ID should not collide with other transaction IDs. If nil, mParticle will generate a random string
 @param productCategory A category to which the product belongs
 @param currencyCode The local currency of a transaction. If nil, mParticle will use "USD"
 */
- (void)logTransaction:(NSString *)productName affiliation:(NSString *)affiliation sku:(NSString *)sku unitPrice:(double)unitPrice quantity:(NSInteger)quantity revenueAmount:(double)revenueAmount taxAmount:(double)taxAmount shippingAmount:(double)shippingAmount transactionId:(NSString *)transactionId productCategory:(NSString *)productCategory currencyCode:(NSString *)currencyCode;

/**
 Logs an e-commerce transaction event.
 @param product An instance of MPProduct
 @see MPProduct
 */
- (void)logTransaction:(MPProduct *)product;

/**
 Increases the LTV (LifeTime Value) amount of a user.
 @param increaseAmount The amount to be added to LTV
 @param eventName The name of the event (Optional). If not applicable, pass nil
 */
- (void)logLTVIncrease:(double)increaseAmount eventName:(NSString *)eventName;

/**
 Increases the LTV (LifeTime Value) amount of a user.
 @param increaseAmount The amount to be added to LTV
 @param eventName The name of the event (Optional). If not applicable, pass nil
 @param eventInfo A dictionary containing further information about the LTV
 */
- (void)logLTVIncrease:(double)increaseAmount eventName:(NSString *)eventName eventInfo:(NSDictionary *)eventInfo;

#pragma mark - Location
/**
 Begins geographic location tracking.

 The desired accuracy of the location is determined by a passed in constant for accuracy.
 Choices are kCLLocationAccuracyBestForNavigation, kCLLocationAccuracyBest,
 kCLLocationAccuracyNearestTenMeters, kCLLocationAccuracyHundredMeters,
 kCLLocationAccuracyKilometer, and kCLLocationAccuracyThreeKilometers. 
 @param accuracy The desired accuracy
 @param distanceFilter The minimum distance (measured in meters) a device must move before an update event is generated.
 */
- (void)beginLocationTracking:(CLLocationAccuracy)accuracy minDistance:(CLLocationDistance)distanceFilter;

/**
 Ends geographic location tracking.
 */
- (void)endLocationTracking;

#pragma mark - Network Performance
/**
 Begins measuring and reporting network performance.
 */
- (void)beginMeasuringNetworkPerformance;

/**
 Ends measuring and reporting network performance.
 */
- (void)endMeasuringNetworkPerformance;

/**
 Excludes a URL from network performance measurement. You can call this method multiple times, passing a URL at a time.
 @param url A URL to be removed from measurements
 */
- (void)excludeURLFromNetworkPerformanceMeasuring:(NSURL *)url;

/**
 Allows you to log a network performance measurement independently from the mParticle SDK measurement. In case you use sockets or other
 form of network communication not based on NSURLConnection.
 @param urlString The absolute URL being measured
 @param httpMethod The method used in the network communication (e.g. GET, POST, etc)
 @param startTime The time when the network communication started measured in seconds since Unix Epoch Time: [[NSDate date] timeIntervalSince1970]
 @param duration The number of seconds it took for the network communication took to complete
 @param bytesSent The number of bytes sent
 @param bytesReceived The number of bytes received
 */
- (void)logNetworkPerformance:(NSString *)urlString httpMethod:(NSString *)httpMethod startTime:(NSTimeInterval)startTime duration:(NSTimeInterval)duration bytesSent:(NSUInteger)bytesSent bytesReceived:(NSUInteger)bytesReceived;

/**
 By default mParticle SDK will remove the query part of all URLs. Use this method to add an exception to the default
 behavior and include the query compoment of any URL containing queryString. You can call this method multiple times, passing a query string at a time.
 @param queryString A string with the query component to be included and reported in network performance measurement.
 */
- (void)preserveQueryMeasuringNetworkPerformance:(NSString *)queryString;

/**
 Resets all network performance measurement filters and URL exclusions.
 */
- (void)resetNetworkPerformanceExclusionsAndFilters;

#pragma mark - Session management
/**
 Begins a new user session. It will end the current session, if one is active.
 */
- (void)beginSession;

/**
 Ends the current session.
 */
- (void)endSession;

/**
 Increments the value of a session attribute by the provided amount. If the key does not
 exist among the current session attributes, this method will add the key to the session attributes
 and set the value to the provided amount. If the key already exists and the existing value is not 
 a number, the operation will abort and the returned value will be nil.
 @param key The attribute key
 @param value The increment amount
 @returns The new value amount or nil, in case of failure
 */
- (NSNumber *)incrementSessionAttribute:(NSString *)key byValue:(NSNumber *)value;

/**
 Set a single session attribute. The property will be combined with any existing attributes.
 There is a 100 count limit to existing session attributes. Passing in a nil value for an
 existing key will remove the session attribute.
 @param key The attribute key
 @param value The attribute value
 */
- (void)setSessionAttribute:(NSString *)key value:(id)value;

/**
 Force uploads queued messages to mParticle.
 */
- (void)upload;

#pragma mark - Social Networks
/**
 Requests access to social networks.
 @param socialNetwork a bitmask of MPSocialNetworks with the social networks of interest. For example: (MPSocialNetworksFacebook | MPSocialNetworksTwitter)
 @param completionHandler a completion handler containing the status of the request and an error. This handler may be called more than once. One time per social
 network with it respective result.
 */
- (void)askForAccessToSocialNetworks:(MPSocialNetworks)socialNetwork completionHandler:(MPSocialNetworksHandler)completionHandler;

#pragma mark - User Identity
/**
 Increments the value of a user attribute by the provided amount. If the key does not
 exist among the current user attributes, this method will add the key to the user attributes
 and set the value to the provided amount. If the key already exists and the existing value is not
 a number, the operation will abort and the returned value will be nil.
 @param key The attribute key
 @param value The increment amount
 @returns The new value amount or nil, in case of failure
 */
- (NSNumber *)incrementUserAttribute:(NSString *)key byValue:(NSNumber *)value;

/**
 Logs a user out.
 */
- (void)logout;

/**
 Sets a single user attribute. The property will be combined with any existing attributes.
 There is a 100 count limit to user attributes. Passing in an empry string value (@"") for an
 existing key will remove the user attribute.
 @param key The user attribute key
 @param value The user attribute value
 */
- (void)setUserAttribute:(NSString *)key value:(id)value;

/**
 Sets User/Customer Identity
 @param identityString A string representing the user identity
 @param identityType The user identity type
 */
- (void)setUserIdentity:(NSString *)identityString identityType:(MPUserIdentity)identityType;

/**
 Sets a single user tag or attribute.  The property will be combined with any existing attributes.
 There is a 100 count limit to user attributes.
 @param tag The user tag/attribute
 */
- (void)setUserTag:(NSString *)tag;

/**
 Removes a single user attribute.
 @param key The user attribute key
 */
- (void)removeUserAttribute:(NSString *)key;

#pragma mark - User Segments
/**
 Retrieves user segments from mParticle's servers and returns the result as an array of MPUserSegments objects.
 If the method takes longer than timeout seconds to return, the local cached segments will be returned instead,
 and the newly retrieved segments will update the local cache once the results arrive.
 @param timeout The maximum number of seconds to wait for a response from mParticle's servers. This value can be fractional, like 0.1 (100 milliseconds)
 @param completionHandler A block to be called when the results are available. The user segments array is passed to this block
 @returns An array of MPUserSegments objects in the completion handler
 */
- (void)userSegments:(NSTimeInterval)timeout endpointId:(NSString *)endpointId completionHandler:(MPUserSegmentsHandler)completionHandler;

#pragma mark - Web Views
/**
 Updates isIOS flag to true in JS API via given webview.
 @param webView The web view to be initialized
 */
- (void)initializeWebView:(UIWebView *)webView;

/**
 Verifies if the url is mParticle sdk url i.e mp-sdk://
 @param requestUrl The request URL
 */
- (BOOL)isMParticleWebViewSdkUrl:(NSURL *)requestUrl;

/**
 Process log event from hybrid apps that are using iOS UIWebView control.
 @param requestUrl The request URL
 */
- (void)processWebViewLogEvent:(NSURL *)requestUrl;

#pragma mark - Deprecated and/or Unavailable
/**
 @deprecated
 @see environment property
 */
@property (nonatomic, readwrite) BOOL sandboxed __attribute__((unavailable("use the environment property instead")));
- (BOOL)sandboxed UNAVAILABLE_ATTRIBUTE;
- (void)setSandboxed:(BOOL)sandboxed UNAVAILABLE_ATTRIBUTE;

- (void)registerForPushNotificationWithTypes:(UIRemoteNotificationType)pushNotificationTypes __attribute__((unavailable("use Apple's own methods to register for remote notifications.")));

- (void)unregisterForPushNotifications __attribute__((unavailable("use Apple's own methods to unregister from remote notifications.")));

/**
 @deprecated use startWithKey:secret:installationType: instead
 @see environment property
 */
- (void)startWithKey:(NSString *)apiKey secret:(NSString *)secret sandboxMode:(BOOL)sandboxMode installationType:(MPInstallationType)installationType __attribute__((unavailable("use startWithKey:secret:installationType: instead")));

@end
