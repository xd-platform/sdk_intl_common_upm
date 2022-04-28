

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN
typedef NS_ENUM(NSInteger,XDGLanguageLocale) {
    XDGLanguageLocaleSimplifiedChinese = 0,                 // 简体中文
    XDGLanguageLocaleTraditionalChinese,                    // 繁体中文
    XDGLanguageLocaleEnglish,                               // 英文
    XDGLanguageLocaleThai,                                  // 泰文
    XDGLanguageLocaleBahasa,                                // 印尼文
    XDGLanguageLocaleKorean,                                // 韩文
    XDGLanguageLocaleJapanese,                              // 日文
    XDGLanguageLocaleGerman,                                // 德语
    XDGLanguageLocaleFrench,                                // 法语
    XDGLanguageLocalePortuguese,                            // 葡萄牙语
    XDGLanguageLocaleSpanish,                               // 西班牙语
    XDGLanguageLocaleTurkish,                               // 土耳其语
    XDGLanguageLocaleRussian,                               // 俄语
};

/// SDK Settings
@interface XDGSDKSettings : NSObject

/// 是否开启收集广告标识符 IDFA,将会开启和关闭所有第三方 SDK 收集。 请在最早调用（任何 SDK 调用之前）
/// @param enable YES: 开启 NO: 关闭。 默认 NO
+ (void)setAdvertiserIDCollectionEnable:(BOOL)enable;

/// 设置调试模式，debug 会输出SDK日志
/// @param debug 是否 debug 模式。默认 NO
+ (void)setDebugMode:(BOOL)debug;

/// 设置SDK显示语言
/// @param locale 语言，在 XDGLanguageLocale 枚举中查看
+ (void)setLanguage:(XDGLanguageLocale)locale;
@end

NS_ASSUME_NONNULL_END
