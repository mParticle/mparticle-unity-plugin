#import "mParticleUnity.h"
#import <mParticle_Apple_SDK/MPEnums.h>
#import <mParticle_Apple_SDK/mParticle.h>
#import <mParticle_Apple_SDK/MPEvent.h>
#import <mParticle_Apple_SDK/MPProduct.h>

@interface MPUnityConvert : NSObject
+ (MPCommerceEvent *)MPCommerceEvent:(NSDictionary *)json;
+ (MParticleOptions *)MParticleOptions:(NSDictionary *)json;
+ (MPIdentityApiRequest *)MPIdentityApiRequest:(NSDictionary *)json;

@end

#ifdef __cplusplus
extern "C" {
#endif
    
    //
    // Private functions
    //
    NSDictionary *dictionaryWithJSON(const char *json) {
        if (json == nil) {
            return nil;
        }
        
        NSString *jsonString = [[NSString alloc] initWithCString:json encoding:NSUTF8StringEncoding];
        NSError *error = nil;
        NSDictionary *dictionary = nil;
        @try {
            dictionary = [NSJSONSerialization JSONObjectWithData:[jsonString dataUsingEncoding:NSUTF8StringEncoding]
            options:0
              error:&error];
        } @catch (NSException *exception) {
            NSLog(@"%@", exception);
        }
        
        if (error) {
            dictionary = nil;
        }
        return dictionary;
    }
    
    NSDictionary *dictionaryWithJSONString(NSString *jsonString) {
        NSError *error = nil;
        NSDictionary *dictionary = nil;
        @try {
            dictionary = [NSJSONSerialization JSONObjectWithData:[jsonString dataUsingEncoding:NSUTF8StringEncoding]
            options:0
              error:&error];
        } @catch (NSException *exception) {
            NSLog(@"%@", exception);
        }
        
        if (error) {
            dictionary = nil;
        }
        return dictionary;
    }
    
    NSString *jsonWithDictionary(NSDictionary *dictionary) {
        NSError *error;
        NSData *jsonData = nil;
        @try {
            jsonData = [NSJSONSerialization dataWithJSONObject:dictionary
            options:0
              error:&error];
        } @catch (NSException *exception) {
            NSLog(@"%@", exception);
        }
        
        if (!jsonData) {
            NSLog(@"Error while writing NSDictionary to JSON: %@", error);
        }
        return [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
    }
    
    NSArray *arrayWithJSON(const char *json) {
        return (NSArray *)dictionaryWithJSON(json);
    }
    
    NSString *stringWithCString(const char *cString) {
        if (cString == nil) {
            return nil;
        }
        
        NSString *string = [[NSString alloc] initWithCString:cString encoding:NSUTF8StringEncoding];
        return string;
    }
    
    char* charStringCopy(const char* value)
    {
        if (value == nil)
            return nil;
        
        char* res = (char*)malloc(strlen(value) + 1);
        strcpy(res, value);
        
        return res;
    }
    
    char *toChar(NSString *value) {
        char *string = (char *)[value UTF8String];
        if (string == nil)
            return NULL;
        
        char* copy = (char*)malloc(strlen(string) + 1);
        strcpy(copy, string);
        
        return copy;
    }
    
    NSString* toNSString(const char* value)
    {
        if (value != nil)
            return [NSString stringWithUTF8String:value];
        else
            return [NSString stringWithUTF8String:""];
    }
    
    //
    // mParticle SDK Unity functions
    //
    
    extern void UnitySendMessage(const char *, const char *, const char *);
    
    void _Initialize (const char *optionsJSON) {
        static dispatch_once_t onceToken;
        NSDictionary *optionsDictionary = dictionaryWithJSON(optionsJSON);
        MParticleOptions *options = [MPUnityConvert MParticleOptions:optionsDictionary];
        if ([[optionsDictionary allKeys]containsObject:@"UploadInterval"]) {
            int value = [optionsDictionary[@"UploadInterval"]intValue];
            if (value >= 0) {
                options.uploadInterval = value;
            }
        }
        if ([[optionsDictionary allKeys]containsObject:@"SessionTimeout"]) {
            int value = [optionsDictionary[@"SessionTimeout"]intValue];
            if (value >= 0) {
                options.sessionTimeout = value;
            }
        }
        if ([[optionsDictionary allKeys]containsObject:@"LogLevel"]) {
            int logLevel = [optionsDictionary[@"LogLevel"]intValue];
            if (logLevel >= 0) {
                options.logLevel = (MPILogLevel)logLevel;
            }
        }
        dispatch_once(&onceToken, ^{
            [[MParticle sharedInstance] startWithOptions:options];
        });
    }
    
    void _SetOptOut(int optOut) {
        [MParticle sharedInstance].optOut = optOut != 0;
    }
    
    
    void _LogEvent(const char *mpEventJSON) {
        NSDictionary *json = dictionaryWithJSON(mpEventJSON);
        MPEvent *event = [[MPEvent alloc] initWithName:json[@"EventName"] type:[json[@"EventType"]intValue]];
        if ([[json allKeys]containsObject:@"Info"]) {
            NSDictionary *eventInfo = json[@"Info"];
            if (eventInfo.count > 0) {
                event.customAttributes = eventInfo;
            }
        }
        NSNumber *duration = json[@"Duration"];
        NSNumber *startTime = json[@"StartTime"];
        NSNumber *endTime = json[@"EndTime"];
        if (duration != nil) {
            event.duration = duration;
        }
        if (startTime != nil) {
            event.startTime = [NSDate dateWithTimeIntervalSince1970:[startTime longLongValue]];
        }
        if (endTime != nil) {
            event.endTime = [NSDate dateWithTimeIntervalSince1970:[endTime longLongValue]];
        }
        if (json[@"CustomFlags"] != nil) {
            NSDictionary *customFlags = json[@"CustomFlags"];
            for (NSString *key in customFlags.keyEnumerator) {
                [event addCustomFlag:customFlags[key]
                             withKey:key];
            }
        }
        [[MParticle sharedInstance] logEvent:event];
    }
    
    void _LogCommerceEvent(const char *commerceEventJSON) {
        NSDictionary *json = dictionaryWithJSON(commerceEventJSON);
        
        MPCommerceEvent *commerceEvent = [MPUnityConvert MPCommerceEvent:json];
        [[MParticle sharedInstance] logEvent:commerceEvent];
    }
    
    void _LogScreen(const char *screenName) {
        MPEvent *event = [[MPEvent alloc] initWithName:stringWithCString(screenName) type:MPEventTypeNavigation];
        [[MParticle sharedInstance] logScreenEvent:event];
    }
    
    void _SetATTStatus(int status, double timestamp) {
        [[MParticle sharedInstance] setATTStatus:status withATTStatusTimestampMillis:timestamp];
    }
    
    void _LeaveBreadcrumb(const char *breadcrumbName) {
        NSString *breadcrumbNameString = stringWithCString(breadcrumbName);
        [[MParticle sharedInstance] leaveBreadcrumb:breadcrumbNameString];
    }
    
    int _GetEnvironment() {
        int environment = (int)[MParticle sharedInstance].environment;
        return environment;
    }
    
    void _SetUploadInterval(int uploadInterval) {
        NSLog(@"Warning: Upload interval could not be set, please specify the interval in SDK initialization options");
    }
    
    char* _Identity_Identify(const char *identityApiRequestJSON) {
        NSDictionary *identityRequestDict = dictionaryWithJSON(identityApiRequestJSON);
        MPIdentityApiRequest *identityApiRequest = [MPUnityConvert MPIdentityApiRequest:identityRequestDict];
        NSString *taskCallbackId = identityRequestDict[@"TaskUUID"];
        [[[MParticle sharedInstance] identity]identify:identityApiRequest completion:^(MPIdentityApiResult * _Nullable apiResult, NSError * _Nullable error) {
            if (error != nil && taskCallbackId != nil) {
                MPIdentityHTTPErrorResponse *errorResponse = error.userInfo[mParticleIdentityErrorKey];
                if ([errorResponse isKindOfClass:[NSString class]]) {
                    return;
                }
                NSString *errorMessage = errorResponse.message;
                NSDictionary *innerError = @{};
                if (errorResponse.innerError != nil && errorResponse.innerError.userInfo != nil) {
                    innerError = errorResponse.innerError.userInfo;
                }
                NSDictionary<NSString *, NSString *> *errorDictionary = @{@"CallbackUuid": taskCallbackId,
                                                                          @"ErrorCode": [NSString stringWithFormat: @"%ld", (long)error.code],
                                                                          @"Domain": error.domain,
                                                                          @"UserInfo": jsonWithDictionary(@{@"Message": errorMessage ? errorMessage : @"",
                                                                                                            @"InnerError": innerError
                                                                                                            })};
                UnitySendMessage("MParticle", "OnTaskFailure", toChar(jsonWithDictionary(errorDictionary)));
            }
            if (apiResult != nil) {
                NSString *mpid = [apiResult.user.userId stringValue];
                NSDictionary<NSString *, NSString *> *messageDictionary = @{@"CallbackUuid": taskCallbackId,
                                                                            @"Mpid": mpid};
                UnitySendMessage("MParticle", "OnTaskSuccess", toChar(jsonWithDictionary(messageDictionary)));
            }
        }];
        return toChar(taskCallbackId);
    }
    
    char* _Identity_Login(const char *identityApiRequestJSON) {
        NSDictionary *identityRequestDict = dictionaryWithJSON(identityApiRequestJSON);
        MPIdentityApiRequest *identityApiRequest = [MPUnityConvert MPIdentityApiRequest:identityRequestDict];
        NSString *taskCallbackId = identityRequestDict[@"TaskUUID"];
        [[[MParticle sharedInstance] identity]login:identityApiRequest completion:^(MPIdentityApiResult * _Nullable apiResult, NSError * _Nullable error) {
            if (error != nil && taskCallbackId != nil) {
                MPIdentityHTTPErrorResponse *errorResponse = error.userInfo[mParticleIdentityErrorKey];
                if ([errorResponse isKindOfClass:[NSString class]]) {
                    return;
                }
                NSString *errorMessage = errorResponse.message;
                NSDictionary *innerError = @{};
                if (errorResponse.innerError != nil && errorResponse.innerError.userInfo != nil) {
                    innerError = errorResponse.innerError.userInfo;
                }
                NSDictionary<NSString *, NSString *> *errorDictionary = @{@"CallbackUuid": taskCallbackId,
                                                                          @"ErrorCode": [NSString stringWithFormat: @"%ld", (long)error.code],
                                                                          @"Domain": error.domain,
                                                                          @"UserInfo": jsonWithDictionary(@{@"Message": errorMessage ? errorMessage : @"",
                                                                                                            @"InnerError": innerError
                                                                                                            })};
                UnitySendMessage("MParticle", "OnTaskFailure", toChar(jsonWithDictionary(errorDictionary)));
            }
            if (apiResult != nil) {
                NSString *mpid = [apiResult.user.userId stringValue];
                NSDictionary<NSString *, NSString *> *messageDictionary = @{@"CallbackUuid": taskCallbackId,
                                                                            @"Mpid": mpid};
                UnitySendMessage("MParticle", "OnTaskSuccess", toChar(jsonWithDictionary(messageDictionary)));
            }
        }];
        return toChar(taskCallbackId);
    }
    
    char* _Identity_Logout(const char *identityApiRequestJSON) {
        NSDictionary *identityRequestDict = dictionaryWithJSON(identityApiRequestJSON);
        MPIdentityApiRequest *identityApiRequest = [MPUnityConvert MPIdentityApiRequest:identityRequestDict];
        NSString *taskCallbackId = identityRequestDict[@"TaskUUID"];
        
        [[[MParticle sharedInstance] identity]logout:identityApiRequest completion:^(MPIdentityApiResult * _Nullable apiResult, NSError * _Nullable error) {
            if (error != nil && taskCallbackId != nil) {
                MPIdentityHTTPErrorResponse *errorResponse = error.userInfo[mParticleIdentityErrorKey];
                if ([errorResponse isKindOfClass:[NSString class]]) {
                    return;
                }
                NSString *errorMessage = errorResponse.message;
                NSDictionary *innerError = @{};
                if (errorResponse.innerError != nil && errorResponse.innerError.userInfo != nil) {
                    innerError = errorResponse.innerError.userInfo;
                }
                NSDictionary<NSString *, NSString *> *errorDictionary = @{@"CallbackUuid": taskCallbackId,
                                                                          @"ErrorCode": [NSString stringWithFormat: @"%ld", (long)error.code],
                                                                          @"Domain": error.domain,
                                                                          @"UserInfo": jsonWithDictionary(@{@"Message": errorMessage ? errorMessage : @"",
                                                                                                            @"InnerError": innerError
                                                                                                            })};
                UnitySendMessage("MParticle", "OnTaskFailure", toChar(jsonWithDictionary(errorDictionary)));
            }
            if (apiResult != nil) {
                NSString *mpid = [apiResult.user.userId stringValue];
                NSDictionary<NSString *, NSString *> *messageDictionary = @{@"CallbackUuid": taskCallbackId,
                                                                            @"Mpid": mpid};
                UnitySendMessage("MParticle", "OnTaskSuccess", toChar(jsonWithDictionary(messageDictionary)));
            }
        }];
        return toChar(taskCallbackId);
    }
    
    char* _Identity_Modify(const char *identityApiRequestJSON) {
        NSDictionary *identityRequestDict = dictionaryWithJSON(identityApiRequestJSON);
        MPIdentityApiRequest *identityApiRequest = [MPUnityConvert MPIdentityApiRequest:identityRequestDict];
        NSString *taskCallbackId = identityRequestDict[@"TaskUUID"];
        
        [[[MParticle sharedInstance] identity]modify:identityApiRequest completion:^(MPIdentityApiResult * _Nullable apiResult, NSError * _Nullable error) {
            if (error != nil && taskCallbackId != nil) {
                MPIdentityHTTPErrorResponse *errorResponse = error.userInfo[mParticleIdentityErrorKey];
                if ([errorResponse isKindOfClass:[NSString class]]) {
                    return;
                }
                NSString *errorMessage = errorResponse.message;
                NSDictionary *innerError = @{};
                if (errorResponse.innerError != nil && errorResponse.innerError.userInfo != nil) {
                    innerError = errorResponse.innerError.userInfo;
                }
                NSDictionary<NSString *, NSString *> *errorDictionary = @{@"CallbackUuid": taskCallbackId,
                                                                          @"ErrorCode": [NSString stringWithFormat: @"%ld", (long)error.code],
                                                                          @"Domain": error.domain,
                                                                          @"UserInfo": jsonWithDictionary(@{@"Message": errorMessage ? errorMessage : @"",
                                                                                                            @"InnerError": innerError
                                                                                                            })};
                UnitySendMessage("MParticle", "OnTaskFailure", toChar(jsonWithDictionary(errorDictionary)));
            }
            if (apiResult != nil) {
                NSString *mpid = [apiResult.user.userId stringValue];
                NSDictionary<NSString *, NSString *> *messageDictionary = @{@"CallbackUuid": taskCallbackId,
                                                                            @"Mpid": mpid};
                UnitySendMessage("MParticle", "OnTaskSuccess", toChar(jsonWithDictionary(messageDictionary)));
            }
        }];
        return toChar(taskCallbackId);
    }
    
    id token;
    int count = 0;
    void _Identity_AddIdentityStateListener(){
        if (count == 0) {
            token = [[NSNotificationCenter defaultCenter] addObserverForName: mParticleIdentityStateChangeListenerNotification
                                                                      object: nil
                                                                       queue: nil
                                                                  usingBlock: ^ (NSNotification * note) {
                                                                      NSDictionary *values = @{@"Mpid": [((MParticleUser *) note.userInfo[mParticleUserKey]).userId stringValue]};
                                                                      NSError *error;
                                                                      NSData *jsonData = nil;
                                                                      @try {
                                                                          jsonData =[NSJSONSerialization dataWithJSONObject:values
                                                                          options:0
                                                                            error:&error];
                                                                      } @catch (NSException *exception) {
                                                                          NSLog(@"%@", exception);
                                                                      }
                                                                      
                                                                      NSString *jsonString = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
                                                                      char *message = toChar(jsonString);
                                                                      UnitySendMessage("MParticle", "OnUserIdentified", message);
                                                                  }];
        }
        count++;
    }
    
    void _Identity_RemoveIdentityStateListener(){
        count--;
        if (count == 0) {
            [[NSNotificationCenter defaultCenter] removeObserver:token name:mParticleIdentityStateChangeListenerNotification object:nil];
            token = nil;
        }
    }
    
    char* _Identity_GetCurrentUser(){
        @try {
            MParticleUser *currentUser = [[[MParticle sharedInstance] identity] currentUser];
            return toChar([currentUser.userId stringValue]);
        }
        @catch (NSException *ex) {
            NSLog(@"%@", ex);
            return toChar(@"0");
        }
    }
    
    void _Upload() {
        [[MParticle sharedInstance] upload];
    }
    
    char* _User_SetUserAttribute(const char *mpid, const char *key, const char *value) {
        @try {
            NSString *mpidString = [[NSString alloc] initWithCString:mpid encoding:NSUTF8StringEncoding];
            [[[MParticle sharedInstance].identity getUser:(@([mpidString longLongValue]))] setUserAttribute:[[NSString alloc] initWithCString:key encoding:NSUTF8StringEncoding]
                                                                                                      value:[[NSString alloc] initWithCString:value encoding:NSUTF8StringEncoding]];
            return toChar(@"true");
        }
        @catch (NSException *ex) {
            NSLog(@"%@", ex);
            return toChar(@"false");
        }
    }
    
    char* _User_SetUserAttributes(const char *mpid, const char *attributesJSON) {
        @try {
            NSDictionary *attributes = dictionaryWithJSON(attributesJSON);
            NSString *mpidString = [[NSString alloc] initWithCString:mpid encoding:NSUTF8StringEncoding];
            [[[MParticle sharedInstance].identity getUser:(@([mpidString longLongValue]))] setUserAttributes:attributes];
            return toChar(@"true");
        }
        @catch(NSException *ex) {
            NSLog(@"%@", ex);
            return toChar(@"false");
        }
    }
    
    char* _User_SetUserTag(const char *mpid, const char *tag) {
        @try {
            NSString *mpidString = [[NSString alloc] initWithCString:mpid encoding:NSUTF8StringEncoding];
            [[[MParticle sharedInstance].identity getUser:(@([mpidString longLongValue]))] setUserTag:[[NSString alloc] initWithCString:tag encoding:NSUTF8StringEncoding]];
            return toChar(@"true");
        }
        @catch (NSException *ex) {
            NSLog(@"%@", ex);
            return toChar(@"false");
        }
    }
    
    char* _User_RemoveUserAttribute(const char *mpid, const char *key) {
        @try {
            NSString *mpidString = [[NSString alloc] initWithCString:mpid encoding:NSUTF8StringEncoding];
            [[[MParticle sharedInstance].identity getUser:(@([mpidString longLongValue]))] removeUserAttribute:[[NSString alloc] initWithCString:key encoding:NSUTF8StringEncoding]];
            return toChar(@"true");
        }
        @catch (NSException *ex) {
            NSLog(@"%@", ex);
            return toChar(@"false");
        }
    }
    
    char* _User_GetUserAttributes(const char *mpid) {
        @try {
            NSString *mpidString = [[NSString alloc] initWithCString:mpid encoding:NSUTF8StringEncoding];
            NSDictionary *userAttributes = [[[MParticle sharedInstance].identity getUser:(@([mpidString longLongValue]))] userAttributes];
            NSError *error;
            NSData *jsonData = nil;
            @try {
                jsonData =[NSJSONSerialization dataWithJSONObject:userAttributes
                options:0
                  error:&error];
            } @catch (NSException *exception) {
                NSLog(@"%@", exception);
            }
            
            NSString *jsonString = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
            return toChar(jsonString);
        }
        @catch (NSException *ex) {
            NSLog(@"%@", ex);
            return toChar(@"{}");
        }
    }
    
    char* _User_GetUserIdentities(const char *mpid) {
        @try {
            NSString *mpidString = [[NSString alloc] initWithCString:mpid encoding:NSUTF8StringEncoding];
            NSDictionary *identities = [[[MParticle sharedInstance].identity getUser:(@([mpidString longLongValue]))] identities];
            NSError *error;
            NSDictionary *userIdentityStrings = [NSMutableDictionary new];
            for (id key in identities) {
                [userIdentityStrings setValue:identities[key] forKey:[key stringValue]];
            }
            NSData *jsonData = nil;
            @try {
                jsonData =[NSJSONSerialization dataWithJSONObject:userIdentityStrings
                options:0
                  error:&error];
            } @catch (NSException *exception) {
                NSLog(@"%@", exception);
            }
            NSString *jsonString = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
            return toChar(jsonString);
        }
        @catch (NSException *ex) {
            NSLog(@"%@", ex);
            return toChar(@"{}");
        }
    }
    
    char* _User_GetCurrentUserMpid(const char *mpid) {
        @try {
            NSNumber *currentMpid = [MParticle sharedInstance].identity.currentUser.userId;
            return toChar([currentMpid stringValue]);
        }
        @catch (NSException *ex) {
            NSLog(@"%@", ex);
            return toChar(@"false");
        }
    }
    
#ifdef __cplusplus
}
#endif

@implementation MPUnityConvert

typedef NS_ENUM(NSUInteger, MPUnityCommerceEventAction) {
    MPUnityCommerceEventActionAddToCart = 1,
    MPUnityCommerceEventActionRemoveFromCart,
    MPUnityCommerceEventActionCheckout,
    MPUnityCommerceEventActionCheckoutOptions,
    MPUnityCommerceEventActionClick,
    MPUnityCommerceEventActionViewDetail,
    MPUnityCommerceEventActionPurchase,
    MPUnityCommerceEventActionRefund,
    MPUnityCommerceEventActionAddToWishList,
    MPUnityCommerceEventActionRemoveFromWishlist
};

+ (MPIdentityApiRequest *) MPIdentityApiRequest:(NSDictionary *)identityRequestDict {
    MPIdentityApiRequest *identityRequest = [MPIdentityApiRequest requestWithEmptyUser];
    if ([[identityRequestDict allKeys] containsObject:@"UserIdentities"]) {
        NSDictionary *identities = identityRequestDict[@"UserIdentities"];
        for (id key in identities) {
            [identityRequest setIdentity:identities[key] identityType:(MPIdentity)[key integerValue]];
        }
    }
    if ([[identityRequestDict allKeys]containsObject:@"UserAliasUUID"]) {
        identityRequest.onUserAlias = ^(MParticleUser * _Nonnull previousUser, MParticleUser * _Nonnull newUser) {
            NSDictionary<NSString *, NSString *> *aliasDictionary = @{@"CallbackUuid": identityRequestDict[@"UserAliasUUID"],
                                                                      @"PreviousMpid": [previousUser.userId stringValue],
                                                                      @"NewMpid": [newUser.userId stringValue]};
            UnitySendMessage("MParticle", "OnUserAlias", toChar(jsonWithDictionary(aliasDictionary)));
        };
    }
    return identityRequest;
}

+ (MPCommerceEventAction)MPCommerceEventAction:(NSNumber *)json {
    int actionInt = [json intValue];
    MPCommerceEventAction action;
    switch (actionInt) {
        case MPUnityCommerceEventActionAddToCart:
            action = MPCommerceEventActionAddToCart;
            break;
            
        case MPUnityCommerceEventActionRemoveFromCart:
            action = MPCommerceEventActionRemoveFromCart;
            break;
            
        case MPUnityCommerceEventActionCheckout:
            action = MPCommerceEventActionCheckout;
            break;
            
        case MPUnityCommerceEventActionCheckoutOptions:
            action = MPCommerceEventActionCheckoutOptions;
            break;
            
        case MPUnityCommerceEventActionClick:
            action = MPCommerceEventActionClick;
            break;
            
        case MPUnityCommerceEventActionViewDetail:
            action = MPCommerceEventActionViewDetail;
            break;
            
        case MPUnityCommerceEventActionPurchase:
            action = MPCommerceEventActionPurchase;
            break;
            
        case MPUnityCommerceEventActionRefund:
            action = MPCommerceEventActionRefund;
            break;
            
        case MPUnityCommerceEventActionAddToWishList:
            action = MPCommerceEventActionAddToWishList;
            break;
            
        case MPUnityCommerceEventActionRemoveFromWishlist:
            action = MPCommerceEventActionRemoveFromWishlist;
            break;
            
        default:
            action = MPCommerceEventActionAddToCart;
            NSAssert(NO, @"Invalid commerce event action");
            break;
    }
    return action;
}

+ (MParticleOptions *)MParticleOptions:(NSDictionary *)json {
    NSString *key = json[@"ApiKey"];
    NSString *secret = json[@"ApiSecret"];
    MParticleOptions *mParticleOptions = [MParticleOptions optionsWithKey:key
                                                                   secret:secret];
    if ([[json allKeys]containsObject:@"Environment"]) {
        int value = [json[@"Environment"]intValue];
        if (value >= 0) {
            mParticleOptions.environment = value;
        }
    }
    if ([[json allKeys]containsObject:@"InstallType"]) {
        int value = [json[@"InstallType"]intValue];
        if (value >= 0) {
            mParticleOptions.installType = value;
        }
    }
    if ([[json allKeys]containsObject:@"IdentifyRequest"]) {
        mParticleOptions.identifyRequest = [MPUnityConvert MPIdentityApiRequest:json[@"IdentifyRequest"]];
    }
    return mParticleOptions;
}



+ (MPCommerceEvent *)MPCommerceEvent:(NSDictionary *)json {
    BOOL isProductAction = [json[@"ProductAction"] intValue] > 0 && [json[@"Products"] count] > 0;
    BOOL isPromotion =  [json[@"Promotions"] count] > 0;
    BOOL isImpression = [json[@"Impressions"] count] > 0;
    BOOL isValid = isProductAction || isPromotion || isImpression;
    
    MPCommerceEvent *commerceEvent = nil;
    if (!isValid) {
        NSAssert(NO, @"Invalid commerce event");
        return commerceEvent;
    }
    
    if (isProductAction) {
        MPCommerceEventAction action = [MPUnityConvert MPCommerceEventAction:json[@"ProductAction"]];
        commerceEvent = [[MPCommerceEvent alloc] initWithAction:action];
    }
    else if (isPromotion) {
        MPPromotionContainer *promotionContainer = [MPUnityConvert MPPromotionContainer:json];
        commerceEvent = [[MPCommerceEvent alloc] initWithPromotionContainer:promotionContainer];
    }
    else {
        commerceEvent = [[MPCommerceEvent alloc] initWithImpressionName:nil product:nil];
    }
    
    commerceEvent.checkoutOptions = json[@"CheckoutOptions"];
    commerceEvent.currency = json[@"Currency"];
    commerceEvent.productListName = json[@"ProductActionListName"];
    commerceEvent.productListSource = json[@"ProductActionListSource"];
    commerceEvent.screenName = json[@"ScreenName"];
    if (json[@"TransactionAttributes"] != nil) {
        commerceEvent.transactionAttributes = [MPUnityConvert MPTransactionAttributes:json[@"TransactionAttributes"]];
    }
    commerceEvent.checkoutStep = [json[@"CheckoutStep"] intValue];
    commerceEvent.nonInteractive = [json[@"NonInteractive"] boolValue];
    
    NSMutableArray *products = [NSMutableArray array];
    NSArray *jsonProducts = json[@"Products"];
    if (jsonProducts != nil && [jsonProducts count] > 0) {
        [jsonProducts enumerateObjectsUsingBlock:^(id  _Nonnull obj, NSUInteger idx, BOOL * _Nonnull stop) {
            MPProduct *product = [MPUnityConvert MPProduct:obj];
            if (product != nil) {
                [products addObject:product];
            }
        }];
        [commerceEvent addProducts:products];
    }
    NSArray *jsonImpressions = json[@"Impressions"];
    if (jsonImpressions != nil && [jsonImpressions count] > 0) {
        [jsonImpressions enumerateObjectsUsingBlock:^(NSDictionary *jsonImpression, NSUInteger idx, BOOL * _Nonnull stop) {
            NSString *listName = jsonImpression[@"ImpressionListName"];
            NSArray *jsonProducts = jsonImpression[@"Products"];
            if (jsonProducts != nil && [jsonProducts count] > 0) {
                [jsonProducts enumerateObjectsUsingBlock:^(id  _Nonnull jsonProduct, NSUInteger idx, BOOL * _Nonnull stop) {
                    MPProduct *product = [MPUnityConvert MPProduct:jsonProduct];
                    if (product != nil && listName != nil) {
                        [commerceEvent addImpression:product listName:listName];
                    }
                }];
            }
        }];
    }
    
    return commerceEvent;
}

+ (MPPromotionContainer *)MPPromotionContainer:(id)json {
    MPPromotionAction promotionAction = [json[@"PromotionAction"] intValue] == 0 ? MPPromotionActionView : MPPromotionActionClick;
    MPPromotionContainer *promotionContainer = [[MPPromotionContainer alloc] initWithAction:promotionAction promotion:nil];
    NSArray *jsonPromotions = json[@"Promotions"];
    [jsonPromotions enumerateObjectsUsingBlock:^(id  _Nonnull obj, NSUInteger idx, BOOL * _Nonnull stop) {
        MPPromotion *promotion = [MPUnityConvert MPPromotion:obj];
        [promotionContainer addPromotion:promotion];
    }];
    
    return promotionContainer;
}

+ (MPPromotion *)MPPromotion:(id)json {
    MPPromotion *promotion = [[MPPromotion alloc] init];
    promotion.creative = json[@"Creative"];
    promotion.name = json[@"Name"];
    promotion.position = json[@"Position"];
    promotion.promotionId = json[@"Id"];
    return promotion;
}

+ (MPTransactionAttributes *)MPTransactionAttributes:(id)json {
    MPTransactionAttributes *transactionAttributes = [[MPTransactionAttributes alloc] init];
    transactionAttributes.affiliation = json[@"Affiliation"];
    transactionAttributes.couponCode = json[@"CouponCode"];
    transactionAttributes.shipping = json[@"Shipping"];
    transactionAttributes.tax = json[@"Tax"];
    transactionAttributes.revenue = json[@"Revenue"];
    transactionAttributes.transactionId = json[@"TransactionId"];
    return transactionAttributes;
}

+ (MPProduct *)MPProduct:(id)json {
    MPProduct *product = [[MPProduct alloc] init];
    product.brand = json[@"Brand"];
    product.category = json[@"Category"];
    product.couponCode = json[@"CouponCode"];
    product.name = json[@"Name"];
    product.price = json[@"Price"];
    product.sku = json[@"Sku"];
    product.variant = json[@"Variant"];
    if (json[@"position"] != nil) {
        product.position = [json[@"Position"] intValue];
    }
    product.quantity = json[@"Quantity"] ?: @1;
    
    NSDictionary *jsonAttributes = json[@"CustomAttributes"] ?: @{};
    for (NSString *key in jsonAttributes) {
        NSString *value = jsonAttributes[key];
        [product setObject:value forKeyedSubscript:key];
    }
    
    return product;
}
@end


