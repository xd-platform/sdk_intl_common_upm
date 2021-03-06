using System;
using System.Collections.Generic;
using AppsFlyerSDK;
using TapTap.Bootstrap;
using TapTap.Common;
using UnityEngine;

namespace XD.Intl.Common
{
    public class XDGCommonImpl : ICommonAPI
    {
        private static readonly string COMMON_MODULE_UNITY_BRIDGE_NAME = "XDGCoreService";
        private static volatile XDGCommonImpl _instance;
        private static readonly object locker = new object();
        private static bool UseIDFA = false;

        private XDGCommonImpl()
        {
            XDGTool.Log("===> Init XDG Bridge Service");
            EngineBridge.GetInstance()
                .Register(XDGUnityBridge.COMMON_SERVICE_NAME, XDGUnityBridge.COMMON_SERVICE_IMPL);
        }

        public static XDGCommonImpl GetInstance()
        {
            if (_instance != null) return _instance;
            lock (locker)
            {
                if (_instance == null)
                {
                    _instance = new XDGCommonImpl();
                }
            }

            return _instance;
        }

        public void InitAppsFlyer(string devKey, string appId){
            if (XDGTool.IsEmpty(devKey)){
                XDGTool.LogError("AppsFlyer的配置devKey是空：" + devKey);
            } else{
                XDGTool.Log("初始化AppsFlyer成功 devKey：" + devKey + "  APPID：" + appId);
            }
                
            //AppsFlyer.setIsDebug(true);
            AppsFlyer.initSDK(devKey, appId);
            AppsFlyer.startSDK();
        }

        
        public void InitSDK(Action<bool, string> callback)
        {
            var command = new Command.Builder()
                .Service(COMMON_MODULE_UNITY_BRIDGE_NAME)
                .Method("initSDK")
                .Callback(true)
                .CommandBuilder();
            
            EngineBridge.GetInstance().CallHandler(command, (result) =>
            {
                XDGTool.Log("===> Init XDG SDK result: " + result.ToJSON());
                if (!checkResultSuccess(result))
                {

                    callback(false, "Init SDK Fail");
                    XDGTool.LogError("初始化失败 result：" + result.ToJSON());
                    return;
                }

                var wrapper = new XDGInitResultWrapper(result.content);
                XDGTool.Log("===> Init XDG SDK wrapper.localConfigInfo.tapSdkConfig: " + JsonUtility.ToJson(wrapper.localConfigInfo.tapSdkConfig));
                if (wrapper.localConfigInfo.tapSdkConfig != null && wrapper.isSuccess)
                {
                    var info = wrapper.localConfigInfo.tapSdkConfig;
                    var gameChannel = wrapper.localConfigInfo.channel ?? "";
                    if (Platform.IsAndroid())
                    {
                        if (!string.IsNullOrEmpty(info.tapDBChannel))
                        {
                            gameChannel = info.tapDBChannel;
                        }
                    }
                    
                    var config = new TapConfig.Builder()
                        .ClientID(info.clientId) // 必须，开发者中心对应 Client ID
                        .ClientToken(info.clientToken) // 必须，开发者中心对应 Client Token
                        .ServerURL(info.serverUrl) // 开发者中心 > 你的游戏 > 游戏服务 > 云服务 > 数据存储 > 服务设置 > 自定义域名 绑定域名
                        .RegionType(RegionType.IO) // 非必须，默认 CN 表示国内
                        .TapDBConfig(info.enableTapDB, gameChannel, "",UseIDFA)
                        .ConfigBuilder();
                    TapBootstrap.Init(config);
                    XDGTool.Log($"初始化 TapBootstrap 成功：clientId:{info.clientId} clientToken:{info.clientToken} channel:{gameChannel} gameVersion:{Application.version} userIDFA:{UseIDFA}");
                }

                callback(wrapper.isSuccess, wrapper.message);
            });
        }
        
        public void IsInitialized(Action<bool> callback)
        {
            var command = new Command.Builder()
                .Service(COMMON_MODULE_UNITY_BRIDGE_NAME)
                .Method("isInitialized")
                .Callback(true)
                .CommandBuilder();
            EngineBridge.GetInstance().CallHandler(command, result =>
            {
                XDGTool.Log("===> IsInitialized: " + result.ToJSON());
                if (!checkResultSuccess(result))
                {
                    callback(false);
                    return;
                }

                callback("true".Equals(result.content.ToLower()));
            });
        }

        public void SetLanguage(LangType langType)
        {
            XDGTool.Log("===> SetLanguage langType: " + langType);
            var command = new Command.Builder()
                .Service(COMMON_MODULE_UNITY_BRIDGE_NAME)
                .Method("setLanguage")
                .Args("langType", (int)langType)
                .OnceTime(true)
                .CommandBuilder();
            EngineBridge.GetInstance().CallHandler(command);
            
            //设置 TapCommon
            TapLanguage tType = TapLanguage.AUTO;
            if (langType == LangType.ZH_CN){
                tType = TapLanguage.ZH_HANS;
            }else if (langType == LangType.ZH_TW){
                tType = TapLanguage.ZH_HANT;
            }else if (langType == LangType.JP){
                tType = TapLanguage.JA;
            }else if (langType == LangType.KR){
                tType = TapLanguage.KO;
            }else if (langType == LangType.EN){
                tType = TapLanguage.EN;
            }else if (langType == LangType.TH){
                tType = TapLanguage.TH;
            }else if (langType == LangType.ID){
                tType = TapLanguage.ID;
            }
            TapCommon.SetLanguage(tType);
        }

        public void Share(ShareFlavors shareFlavors, string imagePath, XDGShareCallback callback)
        {
            var command = new Command.Builder()
                .Service(COMMON_MODULE_UNITY_BRIDGE_NAME)
                .Method("shareWithImage")
                .Args("shareFlavors", (int)shareFlavors)
                .Args("imagePath", imagePath)
                .Callback(true)
                .CommandBuilder();
            EngineBridge.GetInstance().CallHandler(command, result =>
            {
                XDGTool.Log("===> share with type: " + shareFlavors + " result: " + result.ToJSON());
                if (!checkResultSuccess(result))
                {
                    callback.ShareFailed($"Share Failed:{result.message}");
                    return;
                }

                var shareWrapper = new XDGShareResultWrapper(result.content);
                if (shareWrapper.cancel)
                {
                    callback.ShareCancel();
                    return;
                }

                if (shareWrapper.error != null)
                {
                    if (!string.IsNullOrEmpty(shareWrapper.error.error_msg))
                    {
                        callback.ShareFailed(shareWrapper.error.error_msg);
                        return;
                    }
                }

                callback.ShareSuccess();
            });
        }

        public void Share(ShareFlavors shareFlavors, string uri, string message, XDGShareCallback callback)
        {
            var command = new Command.Builder()
                .Service(COMMON_MODULE_UNITY_BRIDGE_NAME)
                .Method("shareWithUriMessage")
                .Args("shareFlavors", (int)shareFlavors)
                .Args("uri", uri)
                .Args("message", message)
                .Callback(true)
                .CommandBuilder();
            EngineBridge.GetInstance().CallHandler(command, result =>
            {
                XDGTool.Log("===> share with type: " + shareFlavors + " result: " + result.ToJSON());
                if (!checkResultSuccess(result))
                {
                    callback.ShareFailed($"Share Failed:{result.message}");
                    return;
                }

                var shareWrapper = new XDGShareResultWrapper(result.content);
                if (shareWrapper.cancel)
                {
                    callback.ShareCancel();
                    return;
                }

                if (shareWrapper.error != null)
                {
                    if (!string.IsNullOrEmpty(shareWrapper.error.error_msg))
                    {
                        callback.ShareFailed(shareWrapper.error.error_msg);
                        return;
                    }
                }

                callback.ShareSuccess();
            });
        }

        public void Report(string serverId, string roleId, string roleName)
        {
            var argsDic = new Dictionary<string, object>
            {
                { "serverId", serverId },
                { "roleId", roleId },
                { "roleName", roleName }
            };
            var command = new Command.Builder()
                .Service(COMMON_MODULE_UNITY_BRIDGE_NAME)
                .Method("report")
                .Args(argsDic)
                .OnceTime(true)
                .CommandBuilder();
            EngineBridge.GetInstance().CallHandler(command);
            XDGTool.Log($"===> Report:  {serverId} -- {roleId} -- {roleName}");
        }

        public void TrackRole(string serverId, string roleId, string roleName, int level)
        {
            var argsDic = new Dictionary<string, object>
            {
                { "serverId", serverId },
                { "roleId", roleId },
                { "roleName", roleName },
                { "level", level }
            };
            var command = new Command.Builder()
                .Service(COMMON_MODULE_UNITY_BRIDGE_NAME)
                .Method("trackRole")
                .Args(argsDic)
                .OnceTime(true)
                .CommandBuilder();
            EngineBridge.GetInstance().CallHandler(command);
            XDGTool.Log($"===> TrackRole:  {serverId} -- {roleId} -- {roleName} -- {level}");
        }

        public void TrackUser(string userId)
        {
            var command = new Command.Builder()
                .Service(COMMON_MODULE_UNITY_BRIDGE_NAME)
                .Method("trackUser")
                .Args("userId", userId)
                .OnceTime(true)
                .CommandBuilder();
            EngineBridge.GetInstance().CallHandler(command);
            XDGTool.Log($"===> TrackUser:  {userId}");
        }

        public void TrackEvent(string eventName)
        {
            var command = new Command.Builder()
                .Service(COMMON_MODULE_UNITY_BRIDGE_NAME)
                .Method("trackEvent")
                .Args("eventName", eventName)
                .OnceTime(true)
                .CommandBuilder();
            EngineBridge.GetInstance().CallHandler(command);
            XDGTool.Log($"===> TrackEvent:  {eventName}");
            
            // if (XDGTool.IsEmpty(eventName)){
            //     XDGTool.LogError("打点名称是空：" + eventName);
            // } else{
            //     AppsFlyer.sendEvent(eventName, null);
            //     XDGTool.Log($"打点名称TrackEvent:  {eventName}");   
            // }
        }
        
        public  void TrackEvent(string eventName, Dictionary<string, string> eventValues){
            if (XDGTool.IsEmpty(eventName)){
                XDGTool.LogError("打点名称是空：" + eventName);
            } else{
                AppsFlyer.sendEvent(eventName, eventValues);
                XDGTool.Log($"打点名称TrackEvent:  {eventName}  eventValues: {eventValues}");   
            }
        }

        public void EventCompletedTutorial()
        {
            var command = new Command.Builder()
                .Service(COMMON_MODULE_UNITY_BRIDGE_NAME)
                .Method("eventCompletedTutorial")
                .OnceTime(true)
                .CommandBuilder();
            EngineBridge.GetInstance().CallHandler(command);
            XDGTool.Log("===> eventCompletedTutorial");
        }

        public void EventCreateRole()
        {
            var command = new Command.Builder()
                .Service(COMMON_MODULE_UNITY_BRIDGE_NAME)
                .Method("eventCreateRole")
                .OnceTime(true)
                .CommandBuilder();
            EngineBridge.GetInstance().CallHandler(command);
            XDGTool.Log("===> eventCreateRole");
        }

        public void GetVersionName(Action<string> callback)
        {
            var command = new Command.Builder()
                .Service(COMMON_MODULE_UNITY_BRIDGE_NAME)
                .Method("getSDKVersionName")
                .Callback(true)
                .CommandBuilder();
            EngineBridge.GetInstance().CallHandler(command, result =>
            {
                XDGTool.Log("===> GetVersionName: " + result.ToJSON());
                if (!checkResultSuccess(result))
                {
                    callback($"GetVersionName Failed:{result.message}");
                    return;
                }

                callback(result.content);
            });
        }

        public void TrackAchievement()
        {
            var command = new Command.Builder()
                .Service(COMMON_MODULE_UNITY_BRIDGE_NAME)
                .Method("trackAchievement")
                .OnceTime(true)
                .CommandBuilder();
            EngineBridge.GetInstance().CallHandler(command);
            XDGTool.Log("===> trackAchievement");
        }

        public void SetCurrentUserPushServiceEnable(bool enable)
        {
            var command = new Command.Builder()
                .Service(COMMON_MODULE_UNITY_BRIDGE_NAME)
                .Method("setCurrentUserPushServiceEnable")
                .Args("enable", enable)
                .OnceTime(true)
                .CommandBuilder();
            EngineBridge.GetInstance().CallHandler(command);
            XDGTool.Log("===> SetCurrentUserPushServiceEnable");
        }

        public void IsCurrentUserPushServiceEnable(Action<bool> callback)
        {
            var command = new Command.Builder()
                .Service(COMMON_MODULE_UNITY_BRIDGE_NAME)
                .Method("isCurrentUserPushServiceEnable")
                .Callback(true)
                .CommandBuilder();
            EngineBridge.GetInstance().CallHandler(command, result =>
            {
                XDGTool.Log("===> isCurrentUserPushServiceEnable: " + result.ToJSON());
                if (!checkResultSuccess(result))
                {
                    callback(false);
                    return;
                }

                callback("true".Equals(result.content.ToLower()));
            });
        }

        public void StoreReview()
        {
            var command = new Command.Builder()
                .Service(COMMON_MODULE_UNITY_BRIDGE_NAME)
                .Method("storeReview")
                .OnceTime(true)
                .CommandBuilder();
            EngineBridge.GetInstance().CallHandler(command);
            XDGTool.Log("===> StoreReview");
        }

        public void GetRegionInfo(Action<XDGRegionInfoWrapper> callback)
        {
            var command = new Command.Builder()
                .Service(COMMON_MODULE_UNITY_BRIDGE_NAME)
                .Method("getRegionInfo")
                .Callback(true)
                .CommandBuilder();
            EngineBridge.GetInstance().CallHandler(command, result =>
            {
                XDGTool.Log("GetRegionInfo result --> " + JsonUtility.ToJson(result));
                callback(new XDGRegionInfoWrapper(result.content));
            });
        }
        
        public void ShowLoading()
        {
            var command = new Command.Builder()
                .Service(COMMON_MODULE_UNITY_BRIDGE_NAME)
                .Method("showLoading")
                .Callback(false)
                .CommandBuilder();
            EngineBridge.GetInstance().CallHandler(command);
        }
        
        public void HideLoading()
        {
            var command = new Command.Builder()
                .Service(COMMON_MODULE_UNITY_BRIDGE_NAME)
                .Method("hideLoading")
                .Callback(false)
                .CommandBuilder();
            EngineBridge.GetInstance().CallHandler(command);
        }
        
        public void LoginSuccessEvent(){
            var command = new Command.Builder()
                .Service(COMMON_MODULE_UNITY_BRIDGE_NAME)
                .Method("loginSuccessEvent")
                .Callback(false)
                .CommandBuilder();
            EngineBridge.GetInstance().CallHandler(command);
        }
        
        public void LoginFailEvent(string loginFailMsg){
            var command = new Command.Builder()
                .Service(COMMON_MODULE_UNITY_BRIDGE_NAME)
                .Method("loginFailEvent")
                .Args("loginFailMsg", loginFailMsg)
                .OnceTime(true)
                .CommandBuilder();
            EngineBridge.GetInstance().CallHandler(command);
        }
        
        public void SetDebugMode(){
            var command = new Command.Builder()
                .Service(COMMON_MODULE_UNITY_BRIDGE_NAME)
                .Method("setDebugMode")
                .Args("setDebugMode", 1)
                .OnceTime(true)
                .CommandBuilder();
            EngineBridge.GetInstance().CallHandler(command);
        }
        
        public void SetDevelopUrl(){
            var command = new Command.Builder()
                .Service(COMMON_MODULE_UNITY_BRIDGE_NAME)
                .Method("setDevelopUrl")
                .OnceTime(true)
                .CommandBuilder();
            EngineBridge.GetInstance().CallHandler(command);
        }

        public void EnableIDFA(bool enable){
#if UNITY_IOS
            UseIDFA = enable;
            var command = new Command.Builder()
                .Service(COMMON_MODULE_UNITY_BRIDGE_NAME)
                .Method("enableIDFA")
                .Args("enableIDFA", enable ? 1 : 0)
                .Callback(false)
                .CommandBuilder();
            EngineBridge.GetInstance().CallHandler(command); 
#endif
        }

        private bool checkResultSuccess(Result result)
        {
            return result.code == Result.RESULT_SUCCESS && !string.IsNullOrEmpty(result.content);
        }
    }
}