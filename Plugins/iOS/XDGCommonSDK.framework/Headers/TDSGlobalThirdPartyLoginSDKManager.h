//
//  TDSThirdPaytyLoginSDKManager.h
//  TDSSDK
//
//  Created by JiangJiahao on 2020/10/26.
//  Copyright © 2020 JiangJiahao. All rights reserved.
//  第三方登录SDK管理和配置

#import <UIKit/UIKit.h>
#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface TDSGlobalThirdPartyLoginSDKManager : NSObject

// 配置各第三方SDK
+ (BOOL)setupThirdPartySDK:(UIApplication *)application didFinishLaunchingWithOptions:(NSDictionary *)launchOptions;
// 初始化各第三方SDK
+ (void)initThirdPartySDK;

#pragma mark - Applicaiton Delegate
+ (void)application:(UIApplication *)app openURL:(NSURL *)url options:(nullable NSDictionary<UIApplicationOpenURLOptionsKey,id> *)options;

+ (void)application:(UIApplication *)application openURL:(NSURL *)url sourceApplication:(NSString *)sourceApplication annotation:(id)annotation;

+ (void)application:(UIApplication *)application continueUserActivity:(NSUserActivity *)userActivity restorationHandler:(nullable void (^)(NSArray<id<UIUserActivityRestoring>> * _Nullable))restorationHandler;

+ (void)application:(UIApplication *)application didReceiveRemoteNotification:(NSDictionary *)userInfo fetchCompletionHandler:(void (^)(UIBackgroundFetchResult))completionHandler;
@end

NS_ASSUME_NONNULL_END
