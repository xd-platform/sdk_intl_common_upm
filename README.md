# XD-Intl
## 1.在Packages/manifest.json中加入如下引用
```
    "com.leancloud.realtime": "https://github.com/leancloud/csharp-sdk-upm.git#realtime-0.10.5",
    "com.leancloud.storage": "https://github.com/leancloud/csharp-sdk-upm.git#storage-0.10.5",
    "com.taptap.tds.bootstrap": "https://github.com/TapTap/TapBootstrap-Unity.git#3.6.3",
    "com.taptap.tds.common": "https://github.com/TapTap/TapCommon-Unity.git#3.6.3",
    "com.taptap.tds.login": "https://github.com/TapTap/TapLogin-Unity.git#3.6.3",
    "com.taptap.tds.tapdb": "https://github.com/TapTap/TapDB-Unity.git#3.6.3",
    "com.xd.intl.common": "https://github.com/xd-platform/sdk_intl_common_upm.git#6.3.0",
    "com.xd.intl.account": "https://github.com/xd-platform/sdk_intl_account_upm.git#6.3.0",
    "com.xd.intl.payment": "https://github.com/xd-platform/sdk_intl_payment_upm.git#6.3.0",
    
    "scopedRegistries": [
    {
      "name": "XD Intl SDK",
      "url": "http://npm.xindong.com",
      "scopes": [
        "com.xd.intl"
      ]
    },
    {
      "name": "Game Package Registry by Google",
      "url": "https://unityregistry-pa.googleapis.com",
      "scopes": [
        "com.google"
      ]
    }
  ]
```

## 2.配置SDK
#### iOS配置
* 将TDS-Info.plist 放在 /Assets/Plugins 中
* 将XDG-Info.plist 放在 /Assets/Plugins/iOS 中
* 在Capabilities中打开In-App Purchase、Push Notifications、Sign In With Apple功能

#### Android配置
* 将XDG_info.json、google-Service.json 文件放在 /Assets/Plugins/Android/assets中

[文件下载](https://github.com/xd-platform/xd_sdk_resource/tree/master/Unity_Intl/ConfigFile)

google-Service.json 文件是从[Firebase控制台下载的](https://console.firebase.google.com/)

## 3.命名空间

```
using XD.Intl.Common;
```

## 4.接口使用
#### 切换语言
```
XDGCommon.SetLanguage(LangType.ZH_CN);
```

#### 初始化SDK
使用sdk前需先初始化
```
 XDGCommon.InitSDK((success => {
                if (success){
              
                }else{
                
                }
            }));
```

#### 是否初始化
```
 XDGCommon.IsInitialized(b => { 
  });
```

#### 埋点
```
  XDGCommon.Report(serverId, roleId, roleName);
  XDGCommon.TrackRole(string serverId, string roleId, string roleName, int level);
  XDGCommon.TrackUser(string userId);
  XDGCommon.TrackEvent(string eventName);
  XDGCommon.TrackAchievement();
```

#### 完成引导教程
```
 XDGCommon.EventCompletedTutorial();
```

#### 创建角色事件
```
  XDGCommon.EventCreateRole();
```

#### 获取版本号
```
XDGCommon.GetVersionName((version) =>{
               
            });
```

#### 通知开关
```
XDGCommon.SetCurrentUserPushServiceEnable(pushServiceEnable);
XDGCommon.IsCurrentUserPushServiceEnable(pushEnable => { 
});
```

#### 分享
```
XDGCommon.Share(ShareFlavors shareFlavors, string imagePath, XDGShareCallback callback);
XDGCommon.Share(ShareFlavors shareFlavors, string uri, string message, XDGShareCallback callback);
```

#### 获取当前获取到的地区信息
```
  XDGCommon.GetRegionInfo(wrapper =>
            {
               if (wrapper != null && wrapper.info != null) {
                    ...
               }
            });
```

[CHANGE LOG](https://github.com/xd-platform/sdk_intl_common_upm/blob/github_upm/ChangeLog.md)