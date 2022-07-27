//
//  TDSGlobalSettingParamsChecker.h
//  XDGCommonSDK
//
//  Created by JiangJiahao on 2021/5/12.
//  配置参数检查

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface TDSGlobalSettingParamsChecker : NSObject

+ (void)checkPlistParams;

+ (void)checkServerParams;

@end

NS_ASSUME_NONNULL_END
