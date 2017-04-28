//
//  MPEnums.h
//
//  Copyright 2014-2015. mParticle, Inc. All Rights Reserved.
//

#ifndef mParticle_MPEnums_h
#define mParticle_MPEnums_h

/// Running Environment
typedef NS_ENUM(NSUInteger, MPEnvironment) {
    /** Tells the SDK to auto detect the current run environment (initial value) */
    MPEnvironmentAutoDetect = 0,
    /** The SDK is running in development mode (Debug/Development or AdHoc) */
    MPEnvironmentDevelopment,
    /** The SDK is running in production mode (App Store) */
    MPEnvironmentProduction
};

/// Event Types
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
    MPEventTypeOther,
    /** Use for media related events */
    MPEventTypeMedia
};

/// Installation Types
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

/// Location Tracking Authorization Request
typedef NS_ENUM(NSUInteger, MPLocationAuthorizationRequest) {
    /** Requests authorization to always use location services */
    MPLocationAuthorizationRequestAlways = 0,
    /** Requests authorization to use location services when the app is in use */
    MPLocationAuthorizationRequestWhenInUse
};

/// eCommerce Product Events
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

/// Supported Social Networks
typedef NS_OPTIONS(uint64_t, MPSocialNetworks) {
    /** Social Network Facebook */
    MPSocialNetworksFacebook = 1 << 1,
    /** Social Network Twitter */
    MPSocialNetworksTwitter = 1 << 2
};

/// Survey Providers
typedef NS_ENUM(NSUInteger, MPSurveyProvider) {
    MPSurveyProviderForesee = 64
};

/// User Identities
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
    MPUserIdentityAlias,
    /** User identity Facebook Custom Audience Third Party Id, or User App Id */
    MPUserIdentityFacebookCustomAudienceId
};

/** Posted immediately after a new session has began.
 
 You can register to receive this notification using NSNotificationCenter. This notification contains a userInfo dictionary, you can
 access the respective session id by using the mParticleSessionId constant.
 */
extern NSString *const mParticleSessionDidBeginNotification;

/** Posted right before the current session ends.
 
 You can register to receive this notification using NSNotificationCenter. This notification contains a userInfo dictionary, you can
 access the respective session id by using the mParticleSessionId constant.
 */
extern NSString *const mParticleSessionDidEndNotification;

/** This constant is used as key for the userInfo dictionary in the
 mParticleSessionDidBeginNotification and mParticleSessionDidEndNotification notifications. The value
 of this key is the id of the session.
 */
extern NSString *const mParticleSessionId;

#endif
