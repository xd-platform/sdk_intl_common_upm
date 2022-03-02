
#import <Foundation/Foundation.h>
#import <XDGCommonSDK/XDGRegionInfo.h>
@class XDGGlobalGame;

NS_ASSUME_NONNULL_BEGIN
@interface XDGGameDataManager : NSObject
+ (XDGGameDataManager *)shareInstance;

+ (XDGGlobalGame *)currentGameData;
+ (NSArray *)currentLoginEntries;
+ (NSArray *)currentBindEntries;
+ (NSString *)serviceTermsUrl;
+ (NSString *)serviceAgreementUrl;
+ (NSString *)californiaPrivacyUrl;
+ (NSArray *)gameLogos;
+ (NSString *)tapServerUrl;
+ (NSDictionary *)configData;
+ (NSDictionary *)configTapDict;
+ (NSString *)clientID;


+ (void)setLanguageLocale:(NSInteger)locale;

+ (void)getClientConfigFirstRequest:(BOOL)first  com:(void (^)(BOOL success, NSString *msg))handler;


+ (XDGRegionInfo *)getRegionInfo;

+ (void)getRegionInfo:(void (^)(XDGRegionInfo *result))completeHandler;

/// 是否已经初始化
+ (BOOL)isGameInited;
/// 是否需要客服
+ (BOOL)needReportService;
#pragma mark - configs
+ (BOOL)isGameInKoreaAndPushServiceEnable;
+ (BOOL)isGameInNA;

+ (BOOL)googleEnable;
+ (BOOL)facebookEnable;
+ (BOOL)twitterEnable;
+ (BOOL)taptapEnable;
+ (BOOL)adjustEnable;
+ (BOOL)appsflyersEnable;
+ (BOOL)lineEnable;
+ (BOOL)tapDBEnable;
@end

NS_ASSUME_NONNULL_END
