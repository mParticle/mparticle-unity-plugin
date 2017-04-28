//
//  mParticleUnity.m
//  mParticle
//
//  Copyright (c) 2014 mParticle. All rights reserved.
//

#import "mParticleUnity.h"
#import "MParticleUnityException.h"

#ifdef __cplusplus
extern "C" {
#endif
    
//    NSDictionary *dictionaryWithJSON(const char *json) {
//        if (json == NULL) {
//            return nil;
//        }
//        
//        NSString *jsonString = [[NSString alloc] initWithCString:json encoding:NSUTF8StringEncoding];
//        
//        NSData *jsonData = [jsonString dataUsingEncoding:NSUTF8StringEncoding];
//        
//        NSError *error = nil;
//        NSDictionary *dictionary = [NSJSONSerialization JSONObjectWithData:jsonData
//                                                                   options:0
//                                                                     error:&error];
//        
//        if (error) {
//            dictionary = nil;
//        }
//        
//        return dictionary;
//    }
    
    //
    // Private functions
    //
    NSDictionary *dictionaryWithJSON(const char *json) {
        if (json == NULL) {
            return nil;
        }
        
        size_t jsonLength = strlen(json);
        CFDataRef jsonData = CFDataCreate(kCFAllocatorDefault, (const UInt8 *)&json, jsonLength);
        
        NSError *error = nil;
        NSDictionary *dictionary = [NSJSONSerialization JSONObjectWithData:(__bridge NSData *)jsonData
                                                                   options:0
                                                                     error:&error];
        
        CFRelease(jsonData);

        if (error) {
            dictionary = nil;
        }
        
        return dictionary;
    }
    
    NSString *stringWithCString(const char *cString) {
        if (cString == NULL) {
            return nil;
        }
        
        NSString *string = [[NSString alloc] initWithCString:cString encoding:NSUTF8StringEncoding];
        return string;
    }
    
    //
    // mParticle SDK Unity functions
    //
    Boolean _GetDebugMode() {
        Boolean debugMode = [MParticle sharedInstance].debugMode;
        return debugMode;
    }
    
    void _SetDebugMode(Boolean debugMode) {
        [MParticle sharedInstance].debugMode = debugMode;
    }

    Boolean _GetSandboxMode() {
        Boolean sandboxMode = [MParticle sharedInstance].sandboxed;
        return sandboxMode;
    }
    
    void _SetSandboxMode(Boolean sandboxMode) {
        [MParticle sharedInstance].sandboxed = sandboxMode;
    }
    
    double _GetSessionTimeout() {
        double sessionTimeout = [MParticle sharedInstance].sessionTimeout;
        return sessionTimeout;
    }
    
    void _SetSessionTimeout(double sessionTimeout) {
        [MParticle sharedInstance].sessionTimeout = sessionTimeout;
    }
	
    //
    // Basic Tracking
    //
    void _LogEvent(const char *eventName, int eventType, const char *eventInfoJSON, double eventLength, const char *category) {
        NSString *eventNameString = stringWithCString(eventName);
        NSString *categoryString = stringWithCString(category);
        NSDictionary *eventInfo = dictionaryWithJSON(eventInfoJSON);
        
        [[MParticle sharedInstance] logEvent:eventNameString
                                   eventType:eventType
                                   eventInfo:eventInfo
                                 eventLength:eventLength
                                    category:categoryString];
    }
    
    void _LogScreen(const char *screenName, const char *eventInfoJSON) {
        NSString *screenNameString = stringWithCString(screenName);
        NSDictionary *eventInfo = dictionaryWithJSON(eventInfoJSON);

        [[MParticle sharedInstance] logScreen:screenNameString
                                    eventInfo:eventInfo];
    }
    
    //
    // Error, Exception, and Crash Handling
    //
    void _BeginUncaughtExceptionLogging() {
        [[MParticle sharedInstance] beginUncaughtExceptionLogging];
    }
    
    void _EndUncaughtExceptionLoggin() {
        [[ MParticle sharedInstance] endUncaughtExceptionLogging];
    }
    
    void _LeaveBreadcrumb(const char *breadcrumbName, const char *eventInfoJSON) {
        NSString *breadcrumbNameString = stringWithCString(breadcrumbName);
        NSDictionary *eventInfo = dictionaryWithJSON(eventInfoJSON);
        
        [[MParticle sharedInstance] leaveBreadcrumb:breadcrumbNameString
                                          eventInfo:eventInfo];
    }
    
    void _LogError(const char *message, const char *eventInfoJSON) {
        NSString *messageString = stringWithCString(message);
        NSDictionary *eventInfo = dictionaryWithJSON(eventInfoJSON);
        
        [[MParticle sharedInstance] logError:messageString
                                   eventInfo:eventInfo];
    }
    
    void _LogException(const char *name, const char *message, const char *stackTrace) {
        NSString *nameString = stringWithCString(name);
        NSString *messageString = stringWithCString(message);
        NSString *stackTraceString = stringWithCString(stackTrace);
        
        MParticleUnityException *exception = [[MParticleUnityException alloc] initWithName:nameString
                                                                                    reason:messageString
                                                                                 callStack:stackTraceString];
        
        [[MParticle sharedInstance] logException:exception];
    }
    
    //
    // eCommerce Transactions
    //
    void _LogTransaction(const char *productName, const char *affiliation, const char *sku, double unitPrice, int quantity, double revenueAmount, double taxAmount, double shippingAmount, const char *transactionId, const char *category, const char *currency) {
        NSString *productNameString = stringWithCString(productName);
        NSString *affiliationString = stringWithCString(affiliation);
        NSString *skuString = stringWithCString(sku);
        NSString *transactionIdString = stringWithCString(transactionId);
        NSString *categoryString = stringWithCString(category);
        NSString *currencyString = stringWithCString(currency);
        
        [[MParticle sharedInstance] logTransaction:productNameString
                                       affiliation:affiliationString
                                               sku:skuString
                                         unitPrice:unitPrice
                                          quantity:quantity
                                     revenueAmount:revenueAmount
                                         taxAmount:taxAmount
                                    shippingAmount:shippingAmount
                                     transactionId:transactionIdString
                                   productCategory:categoryString
                                      currencyCode:currencyString];
    }
    
    void _LogLTVIncrease(double increaseAmount, const char *eventName, const char *eventInfoJSON) {
        NSString *eventNameString = stringWithCString(eventName);
        NSDictionary *eventInfo = dictionaryWithJSON(eventInfoJSON);
        
        [[MParticle sharedInstance] logLTVIncrease:increaseAmount
                                         eventName:eventNameString
                                         eventInfo:eventInfo];
    }
    
    //
    // Location
    //
    void _BeginLocationTracking(double accuracy, double distanceFilter) {
        [[MParticle sharedInstance] beginLocationTracking:accuracy
                                              minDistance:distanceFilter];
    }
    
    void _EndLocationTracking() {
        [[MParticle sharedInstance] endLocationTracking];
    }
    
    //
    // Push Notifications
    //
    void _RegisterForPushNotificationWithTypes(unsigned int pushNotificationTypes) {
        [[MParticle sharedInstance] registerForPushNotificationWithTypes:pushNotificationTypes];
    }
    
    //
    // Session management
    //
    void _BeginSession() {
        [[MParticle sharedInstance] beginSession];
    }
    
    void _EndSession() {
        [[MParticle sharedInstance] endSession];
    }
    
    void _SetSessionAttribute(const char *key, const char *value) {
        NSString *keyString = stringWithCString(key);
        NSString *valueString = stringWithCString(value);
        
        [[MParticle sharedInstance] setSessionAttribute:keyString
                                                  value:valueString];
    }
    
    void _Upload() {
        [[MParticle sharedInstance] upload];
    }

    //
    // User identity
    //
    void _SetUserAttribute(const char *key, const char *value) {
        NSString *keyString = stringWithCString(key);
        NSString *valueString = stringWithCString(value);
        
        [[MParticle sharedInstance] setUserAttribute:keyString
                                               value:valueString];
    }
    
    void _SetUserIdentity(const char *identity, unsigned int identityType) {
        NSString *identityString = stringWithCString(identity);
        
        [[MParticle sharedInstance] setUserIdentity:identityString
                                       identityType:identityType];
    }
    
    void _SetUserTag(const char *tag) {
        NSString *tagString = stringWithCString(tag);
        
        [[MParticle sharedInstance] setUserTag:tagString];
    }

#ifdef __cplusplus
}
#endif
