#import "mParticleUnity.h"
#import "MPEnums.h"
#import "mParticle.h"
#import "MPEvent.h"
#import "MPProduct.h"

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

    void _Initialize (const char *key, const char *secret) {
        [[MParticle sharedInstance] startWithKey:stringWithCString(key) secret:stringWithCString(secret)];
        //TODO(pjenkins): Remove
        [MParticle sharedInstance].logLevel = MPILogLevelVerbose;
    }

    void _SetOptOut(Boolean optOut) {
        [MParticle sharedInstance].optOut = optOut;
    }

    
    void _LogEvent(const char *eventName, int eventType, const char *eventInfoJSON) {
        MPEvent *event = [[MPEvent alloc] initWithName:stringWithCString(eventName) type:eventType];
        event.info = dictionaryWithJSON(eventInfoJSON);

        [[MParticle sharedInstance] logEvent:event];
    }

    void _LogCommerceEvent(const char *commerceEventJSON) {
        // //TODO: ...commerceEventJSON
        // MPCommerceEvent *commerceEvent = nil; [[MPEvent alloc] initWithName:stringWithCString(eventName) type:eventType];
        // event.info = dictionaryWithJSON(eventInfoJSON);

        // [[MParticle sharedInstance] logCommerceEvent:event];
    }
    
    void _LogScreen(const char *screenName, const char *eventInfoJSON) {
        MPEvent *event = [[MPEvent alloc] initWithName:stringWithCString(screenName) type:MPEventTypeNavigation];
        event.info = dictionaryWithJSON(eventInfoJSON);

        [[MParticle sharedInstance] logScreenEvent:event];
    }

    void _SetUserAttribute(const char *key, const char *value) {
        NSString *keyString = stringWithCString(key);
        NSString *valueString = stringWithCString(value);
        
        [[MParticle sharedInstance] setUserAttribute:keyString
                                               value:valueString];
    }

    //TODO: Method signature.
    void _SetUserAttributeArray(const char *key, const char *value) {
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

    long _IncrementUserAttribute(const char *key, long incrementValue) {
        NSString *keyString = stringWithCString(key);

        NSNumber *newValue = [[MParticle sharedInstance] incrementUserAttribute:keyString byValue:@(incrementValue)];

        if (!newValue) {
            return 0;
        }

        return [newValue integerValue];
    }

    void _LeaveBreadcrumb(const char *breadcrumbName, const char *eventInfoJSON) {
        NSString *breadcrumbNameString = stringWithCString(breadcrumbName);
        NSDictionary *eventInfo = dictionaryWithJSON(eventInfoJSON);
        
        [[MParticle sharedInstance] leaveBreadcrumb:breadcrumbNameString
                                          eventInfo:eventInfo];
    }
    
    void _Logout() {
        [[MParticle sharedInstance] logout];
    }

    int _GetEnvironment() {
        int environment = [MParticle sharedInstance].environment;
        return environment;
    }

#ifdef __cplusplus
}
#endif
