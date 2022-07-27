//
//  XDGCommonTool.h
//  XDGCommonSDK
//
//  Created by jessy on 2021/9/8.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface XDGCommonTool : NSObject

/**通过CoreTelephony 框架获取国家代码*/
+ (NSString *)countryCodeFromTelePhony;

/**获取手机设置的countryCode*/
+ (NSString *)countryCodeFromLocale;

/**获取系统时区*/
+ (NSString *)systemTimeName;

/**获取本地时区*/
+ (NSString *)localTimeName;

/**获取默认时区*/
+ (NSString *)defalutTimeName;

/**获取sim卡国家代码，如果没有取系统国家代码*/
+ (NSString *)countryCodeFormLocalOrTelPhony;

/*sim卡或者本地标志*/
+ (NSString *)locationInfoType;

@end

NS_ASSUME_NONNULL_END
