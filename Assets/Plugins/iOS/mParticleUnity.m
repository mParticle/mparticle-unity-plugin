#import "mParticleUnity.h"
#import "MPEnums.h"
#import "mParticle.h"
#import "MPEvent.h"
#import "MPProduct.h"

@interface MPUnityConvert : NSObject
+ (MPCommerceEvent *)MPCommerceEvent:(NSDictionary *)json;
@end

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

    NSArray *arrayWithJSON(const char *json) {
        return (NSArray *)dictionaryWithJSON(json);
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
        static dispatch_once_t onceToken;
        dispatch_once(&onceToken, ^{
            [[MParticle sharedInstance] startWithKey:stringWithCString(key) secret:stringWithCString(secret)];
        });
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
        NSDictionary *json = dictionaryWithJSON(commerceEventJSON);

        MPCommerceEvent *commerceEvent = [MPUnityConvert MPCommerceEvent:json];
        [[MParticle sharedInstance] logCommerceEvent:commerceEvent];
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

    void _SetUserAttributeArray(const char *key, const char *valuesJson[], int length) {
       NSString *keyString = stringWithCString(key);
       NSMutableArray *values = [NSMutableArray array];
       for (int i = 0; i < length; i += 1) {
           NSString *valueString = stringWithCString(valuesJson[i]);
           [values addObject:valueString];
       }
       
       [[MParticle sharedInstance] setUserAttribute:keyString
                                             values:values];
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

    int _IncrementUserAttribute(const char *key, int incrementValue) {
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
    
    void _SetUploadInterval(int uploadInterval) {
        [[MParticle sharedInstance] setUploadInterval:uploadInterval];
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
    commerceEvent.transactionAttributes = [MPUnityConvert MPTransactionAttributes:json[@"TransactionAttributes"]];
    commerceEvent.checkoutStep = [json[@"CheckoutStep"] intValue];
    commerceEvent.nonInteractive = [json[@"NonInteractive"] boolValue];

    NSMutableArray *products = [NSMutableArray array];
    NSArray *jsonProducts = json[@"Products"];
    [jsonProducts enumerateObjectsUsingBlock:^(id  _Nonnull obj, NSUInteger idx, BOOL * _Nonnull stop) {
        MPProduct *product = [MPUnityConvert MPProduct:obj];
        [products addObject:product];
    }];
    [commerceEvent addProducts:products];

    NSArray *jsonImpressions = json[@"Impressions"];
    [jsonImpressions enumerateObjectsUsingBlock:^(NSDictionary *jsonImpression, NSUInteger idx, BOOL * _Nonnull stop) {
        NSString *listName = jsonImpression[@"ImpressionListName"];
        NSArray *jsonProducts = jsonImpression[@"Products"];
        [jsonProducts enumerateObjectsUsingBlock:^(id  _Nonnull jsonProduct, NSUInteger idx, BOOL * _Nonnull stop) {
            MPProduct *product = [MPUnityConvert MPProduct:jsonProduct];
            [commerceEvent addImpression:product listName:listName];
        }];
    }];
    
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
    MPTransactionAttributes *transactionAttributes;
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
    product.position = [json[@"Position"] intValue];
    product.quantity = json[@"Quantity"] ?: @1;

    NSDictionary *jsonAttributes = json[@"CustomAttributes"];
    for (NSString *key in jsonAttributes) {
        NSString *value = jsonAttributes[key];
        [product setObject:value forKeyedSubscript:key];
    }

    return product;
}

@end
