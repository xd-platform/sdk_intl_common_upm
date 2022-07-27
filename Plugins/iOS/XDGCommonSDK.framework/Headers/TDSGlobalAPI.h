//
//  TDSAPI.h
//  TDSSDK
//
//  Created by JiangJiahao on 2020/8/31.
//  Copyright © 2020 JiangJiahao. All rights reserved.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

typedef NSString * const TDSGlobalCoreApiRouter NS_TYPED_EXTENSIBLE_ENUM;

static TDSGlobalCoreApiRouter TDSG_BASE_URL             = @"https://xdg-1c20f-intl.xd.com";

//static TDSGlobalCoreApiRouter TDSG_BASE_URL             = @"https://tds-dev.xindong.com";
//static TDSGlobalCoreApiRouter TDSG_CLIENT_CONFIG        = @"/api/v1/client/config";
static TDSGlobalCoreApiRouter XDG_CLIENT_CONFIG        = @"/api/init/v1/config";

static NSString * const XDGLocURL                          = @"https://ip.xindong.com/myloc2";

@interface TDSGlobalAPI : NSObject

@end

NS_ASSUME_NONNULL_END
