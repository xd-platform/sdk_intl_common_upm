//  读取配置文件

#import <Foundation/Foundation.h>
#import <XDGCommonSDK/TDSGlobalGoogleInfo.h>
#import "TDSGlobalFacebookInfo.h"
#import "TDSGlobalTapTapInfo.h"
#import "TDSGlobalAdjustInfo.h"
#import "TDSGlobalAppsFlyerInfo.h"
#import "TDSGlobalLineInfo.h"
#import "TDSGlobalTwitterInfo.h"


NS_ASSUME_NONNULL_BEGIN

@interface TDSGlobalConfigManager : NSObject

@property (nonatomic, assign, getter=isAdvertiserIDCollectionEnabled) BOOL advertiserIDCollectionEnabled;
@property (nonatomic, assign, getter=isDebugMode) BOOL debugMode;


@property (nonatomic,copy) NSString *clientId;
//@property (nonatomic,copy) NSString *appId;
@property (nonatomic,copy) NSString *gameName;
@property (nonatomic,copy) NSString *gameVersion;
@property (nonatomic,copy) NSString *domain;
@property (nonatomic,copy) NSString *channel;
@property (nonatomic,copy) NSArray *logos;


@property (nonatomic,strong) TDSGlobalGoogleInfo    *googleInfo;
@property (nonatomic,strong) TDSGlobalFacebookInfo  *facebookInfo;
@property (nonatomic,strong) TDSGlobalTapTapInfo    *taptapInfo;
@property (nonatomic,strong) TDSGlobalAdjustInfo    *adjustInfo;
@property (nonatomic,strong) TDSGlobalAppsFlyerInfo *appsflyersInfo;
@property (nonatomic,strong) TDSGlobalLineInfo      *lineInfo;
@property (nonatomic,strong) TDSGlobalTwitterInfo   *twitterInfo;

@property (nonatomic,strong) NSDictionary *configDict;
@property (nonatomic,strong) NSDictionary *tapDict;



+ (TDSGlobalConfigManager *)defaultInstance;

+ (BOOL)googleEnable;
+ (BOOL)facebookEnable;
+ (BOOL)taptapEnable;
+ (BOOL)adjustEnable;
+ (BOOL)appsflyersEnable;
+ (BOOL)lineEnable;
+ (BOOL)twitterEnable;

@end

NS_ASSUME_NONNULL_END
