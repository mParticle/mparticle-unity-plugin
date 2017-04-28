//
//  TrackAndAd.h
//  TrackAndAd
//
//  Copyright (c) 2013-2014 Kochava. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

@interface TrackAndAd : NSObject
@end

@protocol KochavaNetworkAccessDelegate;
@protocol KochavaNetworkAccessDelegate <NSObject>

@optional
- (void) KochavaConnectionDidFinishLoading:(NSDictionary *)responseDict;
- (void) KochavaIdentityLinkResult:(bool)identityLinkSuccess;
- (void) KochavaConnectionDidFailWithError:(NSError *)error;

@end


#pragma mark - -------------------------------------
#pragma mark - Kochava Client

@protocol KochavaTrackerClientDelegate;

@interface KochavaTracker : NSObject <KochavaNetworkAccessDelegate>

- (id) initWithKochavaAppId:(NSString*)appId;
- (id) initWithKochavaAppId:(NSString*)appId :(NSString*)currency;
- (id) initWithKochavaAppId:(NSString*)appId :(NSString*)currency :(bool)enableLogging;
- (id) initWithKochavaAppId:(NSString*)appId :(NSString*)currency :(bool)enableLogging :(bool)limitAdTracking;
- (id) initWithKochavaAppId:(NSString*)appId :(NSString*)currency :(bool)enableLogging :(bool)limitAdTracking :(bool)isNewUser;
- (id) initKochavaWithParams:(NSDictionary*)initDict;

- (void) enableConsoleLogging:(bool)enableLogging;

- (void) trackEvent:(NSString*)eventTitle :(NSString*)eventValue;
- (void) identityLinkEvent:(NSDictionary*)identityLinkData;
- (void) spatialEvent:(NSString*)eventTitle :(float)x :(float)y :(float)z;
- (void) setLimitAdTracking:(bool)limitAdTracking;
- (NSDictionary*) retrieveAttribution;
- (bool) presentInitAd;

@property (nonatomic, assign) id <KochavaTrackerClientDelegate> trackerDelegate;

@end


@protocol KochavaTrackerClientDelegate <NSObject>
@optional
- (void) Kochava_identityLinkResult:(bool)identityLinkResult;
- (void) Kochava_attributionResult:(NSDictionary*)attributionResult;
- (void) Kochava_presentInitAd:(bool)presentInitAdResult;

@end



#pragma mark - -------------------------------------
#pragma mark - Ad Client

@protocol KochavaAdClientDelegate;

@interface KochavaAdClient : UIView <UIWebViewDelegate, UIGestureRecognizerDelegate, KochavaNetworkAccessDelegate>

- (void) displayAdWebView:(UIViewController*)callingController :(UIView*)callingView :(bool)isInterstitial;
- (void) presentClickAd;

@property (nonatomic, assign) id <KochavaAdClientDelegate> adDelegate;
@end

@protocol KochavaAdClientDelegate <NSObject>
@optional
- (void) Kochava_adLoaded:(KochavaAdClient*)adView :(bool)isInterstitial;
- (void) Kochava_fullScreenAdWillLoad:(KochavaAdClient*)adView;
- (void) Kochava_fullScreenAdDidUnload:(KochavaAdClient*)adView;

@end

