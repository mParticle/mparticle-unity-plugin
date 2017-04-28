//
//  mParticleUnity.h
//  mParticle
//
//  Copyright (c) 2014-2015 mParticle. All rights reserved.
//

#ifndef mParticle_mParticleUnity_h
#define mParticle_mParticleUnity_h

// Properties
Boolean _ConsoleLogging();
void _SetConsoleLogging(Boolean consoleLogging);

Boolean _GetOptOut();
void _SetOptOut(Boolean optOut);

double _GetSessionTimeout();
void _SetSessionTimeout(double sessionTimeout);

// Basic Tracking
void _LogEvent(const char *eventName, int eventType, const char *eventInfoJSON, double startTime, double endTime, double duration, const char *category);

void _LogScreen(const char *screenName, const char *eventInfoJSON, double startTime, double endTime, double duration, const char *category);

// Error, Exception, and Crash Handling
void _BeginUncaughtExceptionLogging();

void _EndUncaughtExceptionLogging();

void _LeaveBreadcrumb(const char *breadcrumbName, const char *eventInfoJSON);

void _LogError(const char *message, const char *eventInfoJSON);

void _LogException(const char *name, const char *message, const char *stackTrace);

// eCommerce Transactions
void _LogTransaction(const char *productName, const char *affiliation, const char *sku, double unitPrice, int quantity, double revenueAmount, double taxAmount, double shippingAmount, const char *transactionId, const char *category, const char *currency);

void _LogLTVIncrease(double increaseAmount, const char *eventName, const char *eventInfoJSON);

// Location
void _BeginLocationTracking(double accuracy, double distanceFilter);

void _EndLocationTracking();

// Network Performance Measurement
void _LogNetworkPerformance(const char *url, long startTime, const char *method, long length, long bytesSent, long bytesReceived);

// Push Notifications
void _RegisterForPushNotificationWithTypes(unsigned int pushNotificationTypes);

void _UnregisterForPushNotifications();

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
