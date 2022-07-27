//
//  TDSTrackThirdPartyManager.h
//  TDSSDK
//
//  Created by JiangJiahao on 2020/10/22.
//  Copyright © 2020 JiangJiahao. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <StoreKit/StoreKit.h>
#import <XDGCommonSDK/XDGEntryType.h>


NS_ASSUME_NONNULL_BEGIN

@interface TDSGlobalTrackThirdPartySDKManager : NSObject
+ (void)initTrackSDK:(UIApplication *)application;

+ (BOOL)application:(UIApplication *)app openURL:(NSURL *)url options:(NSDictionary<UIApplicationOpenURLOptionsKey,id> *)options;

+ (BOOL)application:(UIApplication *)application openURL:(NSURL *)url sourceApplication:(NSString *)sourceApplication annotation:(id)annotation;

+ (BOOL)application:(UIApplication *)application continueUserActivity:(NSUserActivity *)userActivity restorationHandler:(nullable void (^)(NSArray<id<UIUserActivityRestoring>> * _Nullable))restorationHandler;

+ (void)application:(UIApplication *)application didReceiveRemoteNotification:(NSDictionary *)userInfo fetchCompletionHandler:(void (^)(UIBackgroundFetchResult))completionHandler;

#pragma mark - data track
+ (void)setUser:(NSString *)userId;
+ (void)trackUser:(NSString *)userId loginType:(LoginEntryType)loginType properties:(NSDictionary *)properties;
+ (void)event:(NSString *)eventName;
+ (void)event:(NSString *)eventName properties:(nullable NSDictionary *)properties;
+ (void)setName:(NSString *)name;
+ (void)setServer:(NSString *)serverId;
+ (void)setLevel:(NSInteger)level;
+ (void)eventCreateRole;
+ (void)eventCompletedTutorial;
+ (void)eventUnlockedAchievement;
+ (void)eventRate;
+ (void)paymentSuccess:(SKPaymentTransaction *)transaction orderID:(NSString *)orderID price:(double)price currency:(NSString *)currency data:(NSDictionary * _Nullable)data;
@end

NS_ASSUME_NONNULL_END
