//
//  XDGCloudLogHelper.h
//  XDGCommonSDK
//
//  Created by Fattycat on 2022/4/12.
//

#import <Foundation/Foundation.h>
#import <XDGCommonSDK/XDGCloudLogProperties.h>

NS_ASSUME_NONNULL_BEGIN

@interface XDGCloudLogHelper : NSObject
@property (nonatomic, strong, nullable) NSString *currentLoginType;

+ (XDGCloudLogHelper *)shareInstance;

+ (void)logEvent:(XDGCloudLogProperties *)properties;
@end

NS_ASSUME_NONNULL_END
