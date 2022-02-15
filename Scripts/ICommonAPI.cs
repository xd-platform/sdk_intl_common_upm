using System;

namespace XD.Intl.Common
{
    public interface ICommonAPI
    {
        void InitSDK(Action<bool, string> callback);
        void IsInitialized(Action<bool> callback);
        void SetLanguage(LangType langType);
        void Share(ShareFlavors shareFlavors, string imagePath, XDGShareCallback callback);
        void Share(ShareFlavors shareFlavors, string uri, string message, XDGShareCallback callback);
        void Report(string serverId, string roleId, string roleName);
        void TrackRole(string serverId, string roleId, string roleName, int level);
        void TrackUser(string userId);
        void TrackEvent(string eventName);
        void EventCompletedTutorial();
        void EventCreateRole();
        void GetVersionName(Action<string> callback);
        void TrackAchievement();
        void SetCurrentUserPushServiceEnable(bool enable);
        void IsCurrentUserPushServiceEnable(Action<bool> callback);
        void StoreReview();
        void GetRegionInfo(Action<XDGRegionInfoWrapper> callback);
        void ShowLoading();
        void HideLoading();
        void GetDid(Action<string> callback);
    }
}