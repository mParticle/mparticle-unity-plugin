//
//  mParticleUnity.h
//  mParticle
//
//  Copyright (c) 2014 mParticle. All rights reserved.
//

#ifndef mParticle_mParticleUnity_h
#define mParticle_mParticleUnity_h

#import "mParticle.h"

Boolean _GetDebugMode();
void _SetDebugMode(Boolean debugMode);

Boolean _GetSandboxMode();
void _SetSandboxMode(Boolean sandboxMode);

double _GetSessionTimeout();
void _SetSessionTimeout(double sessionTimeout);

// Basic Tracking
void _LogEvent(const char *eventName, int eventType, const char *eventInfoJSON, double eventLength, const char *category);

void _LogScreen(const char *screenName, const char *eventInfoJSON);

// Error, Exception, and Crash Handling
void _BeginUncaughtExceptionLogging();

void _EndUncaughtExceptionLoggin();

void _LeaveBreadcrumb(const char *breadcrumbName, const char *eventInfoJSON);

void _LogError(const char *message, const char *eventInfoJSON);

void _LogException(const char *name, const char *message, const char *stackTrace);

// eCommerce Transactions
void _LogTransaction(const char *productName, const char *affiliation, const char *sku, double unitPrice, int quantity, double revenueAmount, double taxAmount, double shippingAmount, const char *transactionId, const char *category, const char *currency);

void _LogLTVIncrease(double increaseAmount, const char *eventName, const char *eventInfoJSON);

// Location
void _BeginLocationTracking(double accuracy, double distanceFilter);

void _EndLocationTracking();

// Push Notifications
void _RegisterForPushNotificationWithTypes(unsigned int pushNotificationTypes);

// Session management
void _BeginSession();

void _EndSession();

void _SetSessionAttribute(const char *key, const char *value);

void _Upload();

// User identity
void _SetUserAttribute(const char *key, const char *value);

void _SetUserIdentity(const char *identity, unsigned int identityType);

void _SetUserTag(const char *tag);

#endif
