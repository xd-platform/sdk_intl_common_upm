//
//  XDGCoreService.h
//  XDGCommonSDK
//
//  Created by JiangJiahao on 2020/11/23.
//

/// unity 桥文件

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface XDGCoreService : NSObject
+ (NSString *)getSDKVersionName;
+ (NSString *)getDid;

+ (void)enableIDFA:(NSNumber *)enableIDFA;

// 获取当前用户位置
+ (void)getRegionInfo:(void (^)(NSString *result))callback;

+ (void)setDebugMode:(NSNumber *)setDebugMode;

+ (void)langType:(NSNumber *)langType;

+ (void)initSDK:(void (^)(NSString *result))callback;

+ (NSNumber *)isInitialized;

+ (void)showLoading;

+ (void)hideLoading;

// report
+ (void)serverId:(NSString *)serverId roleId:(NSString *)roleId roleName:(NSString *)roleName;

+ (void)storeReview;

#pragma mark - 分享
// shareWithImage
+ (void)shareFlavors:(NSNumber *)type imagePath:(NSString *)imagePath bridgeCallback:(void (^)(NSString *result))callback;

//shareWithUriMessage
+ (void)shareFlavors:(NSNumber *)type uri:(NSString *)uri message:(NSString *)message bridgeCallback:(void (^)(NSString *result))callback;


//trackUser
+ (void)userId:(NSString *)userId;

//trackEvent
+ (void)eventName:(NSString *)eventName;

//setCurrentUserPushServiceEnable
+ (void)enable:(NSNumber *)enable;


// trackRole
+ (void)serverId:(NSString *)serverId
          roleId:(NSString *)roleId
        roleName:(NSString *)roleName
           level:(NSNumber *)level;

+ (void)trackAchievement;

+ (void)eventCompletedTutorial;

+ (void)eventCreateRole;

#pragma mark - 推送
/// Open or close firebase push service for current user. Call after user logged in.
/// @param enable enable

/// The user need push service or not. Call after user logged in.
+ (NSNumber *)isCurrentUserPushServiceEnable;

+ (void)loginSuccessEvent;

+ (void)loginFailMsg:(NSString *)loginFailMsg;

+ (NSString *)getImagePath;


@end

NS_ASSUME_NONNULL_END
