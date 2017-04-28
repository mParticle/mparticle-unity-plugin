//
//  mParticleUnity.m
//  mParticle
//
//  Copyright (c) 2014-2015 mParticle. All rights reserved.
//

#import "mParticleUnity.h"
#import "MParticleUnityException.h"
#import <mParticle/MPEnums.h>
#import <mParticle/mParticle.h>
#import <mParticle/MPEvent.h>
#import <mParticle/MPProduct.h>

#ifdef __cplusplus
extern "C" {
#endif
    
    //
    // Private functions
    //
    NSDictionary *dictionaryWithJSON(const char *json) {
        if (json == NULL) {
            return nil;
        }
        
        NSString *jsonString = [[NSString alloc] initWithCString:json encoding:NSUTF8StringEncoding];

        NSError *error = nil;
        NSDictionary *dictionary = [NSJSONSerialization JSONObjectWithData:[jsonString dataUsingEncoding:NSUTF8StringEncoding]
                                                                   options:0
                                                                     error:&error];

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

    //
    // Properties
    //
    Boolean _ConsoleLogging() {
        Boolean consoleLogging = [MParticle sharedInstance].consoleLogging;
        return consoleLogging;
    }

    void _SetConsoleLogging(Boolean consoleLogging) {
        [MParticle sharedInstance].consoleLogging = consoleLogging;
    }

    Boolean _GetOptOut() {
        Boolean optOut = [MParticle sharedInstance].optOut;
        return optOut;
    }

    void _SetOptOut(Boolean optOut) {
        [MParticle sharedInstance].optOut = optOut;
    }

    double _GetSessionTimeout() {
        double sessionTimeout = [MParticle sharedInstance].sessionTimeout;
        return sessionTimeout;
    }
    
    void _SetSessionTimeout(double sessionTimeout) {
        [MParticle sharedInstance].sessionTimeout = sessionTimeout;
    }
	
    double _GetUploadInterval() {
        double uploadInterval = [MParticle sharedInstance].uploadInterval;
        return uploadInterval;
    }

    void _SetUploadInterval(double uploadInterval) {
        [MParticle sharedInstance].uploadInterval = uploadInterval;
    }

    //
    // Basic Tracking
    //
    void _LogEvent(const char *eventName, int eventType, const char *eventInfoJSON, double startTime, double endTime, double duration, const char *category) {
        MPEvent *event = [[MPEvent alloc] initWithName:stringWithCString(eventName) type:eventType];
        event.info = dictionaryWithJSON(eventInfoJSON);

        if (startTime > 0) {
            event.startTime = [NSDate dateWithTimeIntervalSince1970:startTime];
            event.endTime = [NSDate dateWithTimeIntervalSince1970:endTime];
        }

        event.duration = @(duration);

        if (category != NULL) {
            event.category = stringWithCString(category);
        }

        [[MParticle sharedInstance] logEvent:event];
    }
    
    void _LogScreen(const char *screenName, const char *eventInfoJSON, double startTime, double endTime, double duration, const char *category) {
        MPEvent *event = [[MPEvent alloc] initWithName:stringWithCString(screenName) type:MPEventTypeNavigation];
        event.info = dictionaryWithJSON(eventInfoJSON);

        if (startTime > 0) {
            event.startTime = [NSDate dateWithTimeIntervalSince1970:startTime];
            event.endTime = [NSDate dateWithTimeIntervalSince1970:endTime];
        }

        event.duration = @(duration);

        if (category != NULL) {
            event.category = stringWithCString(category);
        }

        [[MParticle sharedInstance] logScreenEvent:event];
    }
    
    //
    // Error, Exception, and Crash Handling
    //
    void _BeginUncaughtExceptionLogging() {
        [[MParticle sharedInstance] beginUncaughtExceptionLogging];
    }
    
    void _EndUncaughtExceptionLogging() {
        [[MParticle sharedInstance] endUncaughtExceptionLogging];
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

    void _LogProductEvent(int productEvent, const char *productName, const char *affiliation, const char *sku, double unitPrice, int quantity, double totalAmount, double taxAmount, double shippingAmount, const char *transactionId, const char *category, const char *currency) {
        MPProduct *product = [[MPProduct alloc] initWithName:stringWithCString(productName)
                                                    category:stringWithCString(category)
                                                    quantity:quantity
                                               revenueAmount:totalAmount];

        product.affiliation = stringWithCString(affiliation);
        product.sku = stringWithCString(sku);
        product.unitPrice = unitPrice;
        product.taxAmount = taxAmount;
        product.shippingAmount = shippingAmount;
        product.transactionId = stringWithCString(transactionId);
        product.currency = stringWithCString(currency);
        
        [[MParticle sharedInstance] logProductEvent:productEvent product:product];
    }

    void _LogTransaction(const char *productName, const char *affiliation, const char *sku, double unitPrice, int quantity, double revenueAmount, double taxAmount, double shippingAmount, const char *transactionId, const char *category, const char *currency) {
        MPProduct *product = [[MPProduct alloc] initWithName:stringWithCString(productName)
                                                    category:stringWithCString(category)
                                                    quantity:quantity
                                               revenueAmount:revenueAmount];

        product.affiliation = stringWithCString(affiliation);
        product.sku = stringWithCString(sku);
        product.unitPrice = unitPrice;
        product.taxAmount = taxAmount;
        product.shippingAmount = shippingAmount;
        product.transactionId = stringWithCString(transactionId);
        product.currency = stringWithCString(currency);
        
        [[MParticle sharedInstance] logTransaction:product];
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
    // Network Performance Measurement
    //
    void _LogNetworkPerformance(const char *url, long startTime, const char *method, long length, long bytesSent, long bytesReceived) {
        NSString *urlString = stringWithCString(url);
        NSString *httpMethod = stringWithCString(method);
        NSTimeInterval startTimeInSeconds = startTime * 1000.0;
        NSTimeInterval duration = length * 1000.0;

        [[MParticle sharedInstance] logNetworkPerformance:urlString
                                               httpMethod:httpMethod
                                                startTime:startTimeInSeconds
                                                 duration:duration
                                                bytesSent:bytesSent
                                            bytesReceived:bytesReceived];
    }

    //
    // Session management
    //

    long _IncrementSessionAttribute(const char *key, long incrementValue) {
        NSString *keyString = stringWithCString(key);

        NSNumber *newValue = [[MParticle sharedInstance] incrementSessionAttribute:keyString byValue:@(incrementValue)];

        if (!newValue) {
            return 0;
        }

        return [newValue integerValue];
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
    long _IncrementUserAttribute(const char *key, long incrementValue) {
        NSString *keyString = stringWithCString(key);

        NSNumber *newValue = [[MParticle sharedInstance] incrementUserAttribute:keyString byValue:@(incrementValue)];

        if (!newValue) {
            return 0;
        }

        return [newValue integerValue];
    }

    void _Logout() {
        [[MParticle sharedInstance] logout];
    }

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

    void _RemoveUserAttribute (const char *key) {
        NSString *keyString = stringWithCString(key);

        [[MParticle sharedInstance] removeUserAttribute:keyString];
    }

#ifdef __cplusplus
}
#endif
