#if UNITY_EDITOR && UNITY_IOS
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEngine;

namespace XD.Intl.Common.Editor{
    public static class XDGIOSPostBuildProcessor{
        public static string plistName = "/XDG-Info.plist";

        [PostProcessBuild(104)]
        public static void OnPostprocessBuild(BuildTarget BuildTarget, string path){
            if (BuildTarget == BuildTarget.iOS){
                Debug.Log("开始执行  XDGIOSPostBuildProcessor");
                // 获得工程路径
                var projPath = PBXProject.GetPBXProjectPath(path);
                var proj = new PBXProject();
                proj.ReadFromString(File.ReadAllText(projPath));

                // 2019.3以上有多个target
#if UNITY_2019_3_OR_NEWER
                string unityFrameworkTarget = proj.TargetGuidByName("UnityFramework");
                string target = proj.GetUnityMainTargetGuid();
#else
                string unityFrameworkTarget = proj.TargetGuidByName("Unity-iPhone");
                string target = proj.TargetGuidByName("Unity-iPhone");
#endif

                if (target == null || unityFrameworkTarget == null){
                    Debug.LogError("XDGIOSPostBuildProcessor target 是空");
                    return;
                }
                
                proj.AddBuildProperty(target, "OTHER_LDFLAGS", "-ObjC");
                proj.AddBuildProperty(unityFrameworkTarget, "OTHER_LDFLAGS", "-ObjC ");
                proj.AddFrameworkToProject(unityFrameworkTarget, "Accelerate.framework", false);

                // 添加资源文件，注意文件路径
                var resourcePath = Path.Combine(path, "XDGResource");
                var parentFolder = Directory.GetParent(Application.dataPath)?.FullName;
                if (Directory.Exists(resourcePath)){
                    Directory.Delete(resourcePath, true);
                }

                Directory.CreateDirectory(resourcePath);
                Debug.Log("创建文件夹: " + resourcePath);

                //拷贝资源文件,可能拷贝多个模块，这里只有common有资源
               copyResource(target, projPath, proj, parentFolder, "com.xd.intl.common", "Common", 
                 resourcePath, new[]{"XDGResources.bundle", "LineSDKResource.bundle", "GoogleSignIn.bundle","XDG-Info.plist"});

                // 复制Assets的plist到工程目录
                File.Copy(parentFolder + "/Assets/Plugins/iOS" + plistName, resourcePath + plistName);

                //修改plist
                var bundleId = GetValueFromPlist(resourcePath + plistName, "bundle_id");
                SetPlist(path, resourcePath + plistName, bundleId);

                //插入代码片段
                SetScriptClass(path);
                Debug.Log("XDGIOSPostBuildProcessor Xcode信息配置成功");
            }
        }

        private static void copyResource(string target, string projPath, PBXProject proj, string parentFolder,
            string npmModuleName, string localModuleName, string xcodeResourceFolder, string[] bundleNames){
           
            //拷贝文件夹里的资源
            var tdsResourcePath = XDGFileHelper.FilterFile(parentFolder + "/Library/PackageCache/", $"{npmModuleName}@");
            if (string.IsNullOrEmpty(tdsResourcePath)){ //优先使用npm的，否则用本地的
                tdsResourcePath = parentFolder + "/Assets/XD/Intl/" + localModuleName;
            }
            tdsResourcePath = tdsResourcePath + "/Plugins/iOS/Resource";
            
            Debug.Log("资源路径" + tdsResourcePath);
            if (!Directory.Exists(tdsResourcePath) || tdsResourcePath == ""){
                Debug.LogError("需要拷贝的资源路径不存在");
                return;
            }
            
            XDGFileHelper.CopyAndReplaceDirectory(tdsResourcePath, xcodeResourceFolder);
            foreach (var name in bundleNames){
                proj.AddFileToBuild(target,
                    proj.AddFile(Path.Combine(xcodeResourceFolder, name), Path.Combine(xcodeResourceFolder, name),
                        PBXSourceTree.Source));
            }
            File.WriteAllText(projPath, proj.WriteToString()); //保存
        }

        private static void SetPlist(string pathToBuildProject, string infoPlistPath, string bundleId){
            //添加info
            string _plistPath = pathToBuildProject + "/Info.plist"; //Xcode工程的Info.plist
            PlistDocument _plist = new PlistDocument();
            _plist.ReadFromString(File.ReadAllText(_plistPath));
            PlistElementDict _rootDic = _plist.root;

            List<string> items = new List<string>(){
                "tapsdk",
                "tapiosdk",
                "fbapi",
                "fbapi20130214",
                "fbapi20130410",
                "fbapi20130702",
                "fbapi20131010",
                "fbapi20131219",
                "fbapi20140410",
                "fbapi20140116",
                "fbapi20150313",
                "fbapi20150629",
                "fbapi20160328",
                "fb-messenger-share-api",
                "fbauth2",
                "fbauth",
                "fbshareextension",
                "lineauth2"
            };
            
            //添加Scheme，用添加，不要覆盖
            PlistElementArray _list = null;
            foreach (var item in _rootDic.values){
                if (item.Key.Equals("LSApplicationQueriesSchemes")){
                    _list = (PlistElementArray) item.Value;
                    break;
                }
            }
            if (_list == null){
                _list = _rootDic.CreateArray("LSApplicationQueriesSchemes");
            }

            for (int i = 0; i < items.Count; i++){
                _list.AddString(items[i]);
            }

            Dictionary<string, object> dic = (Dictionary<string, object>) Plist.readPlist(infoPlistPath);
            string facebookId = null;
            string facebookToken = null;
            string taptapId = null;
            string googleId = null;
            string twitterId = null;

            foreach (var item in dic){
                if (item.Key.Equals("facebook")){
                    Dictionary<string, object> facebookDic = (Dictionary<string, object>) item.Value;
                    foreach (var facebookItem in facebookDic){
                        if (facebookItem.Key.Equals("app_id")){
                            facebookId = (string) facebookItem.Value;
                        }else  if (facebookItem.Key.Equals("client_token")){
                            facebookToken = (string) facebookItem.Value;
                        }
                    }
                }
                else if (item.Key.Equals("tapsdk")){
                    Dictionary<string, object> taptapDic = (Dictionary<string, object>) item.Value;
                    foreach (var taptapItem in taptapDic){
                        if (taptapItem.Key.Equals("client_id")){
                            taptapId = "tt" + (string) taptapItem.Value;
                        }
                    }
                }
                else if (item.Key.Equals("google")){
                    Dictionary<string, object> googleDic = (Dictionary<string, object>) item.Value;
                    foreach (var googleItem in googleDic){
                        if (googleItem.Key.Equals("REVERSED_CLIENT_ID")){
                            googleId = (string) googleItem.Value;
                        }
                    }
                }
                else if (item.Key.Equals("twitter")){
                    Dictionary<string, object> twitterDic = (Dictionary<string, object>) item.Value;
                    foreach (var twitterItem in twitterDic){
                        if (twitterItem.Key.Equals("consumer_key")){
                            twitterId = (string) twitterItem.Value;
                        }
                    }
                }
            }

            //添加url 用添加，不要覆盖
            PlistElementDict dict = _plist.root.AsDict();
            PlistElementArray array = null;
            foreach (var item in _rootDic.values){
                if (item.Key.Equals("CFBundleURLTypes")){
                    array = (PlistElementArray) item.Value;
                    break;
                }
            }
            if (array == null){
                array = dict.CreateArray("CFBundleURLTypes");
            }
            
            //添加FacebookAppID 和 FacebookClientToken
            if (facebookId != null && facebookToken != null){
                dict.SetString("FacebookAppID", facebookId);
                dict.SetString("FacebookClientToken", facebookToken);
            }

            PlistElementDict dict2 = array.AddDict();
            if (taptapId != null){
                dict2.SetString("CFBundleURLName", "TapTap");
                PlistElementArray array2 = dict2.CreateArray("CFBundleURLSchemes");
                array2.AddString(taptapId);
            }

            if (googleId != null){
                dict2 = array.AddDict();
                dict2.SetString("CFBundleURLName", "Google");
                PlistElementArray array2 = dict2.CreateArray("CFBundleURLSchemes");
                array2 = dict2.CreateArray("CFBundleURLSchemes");
                array2.AddString(googleId);
            }

            if (facebookId != null){
                dict2 = array.AddDict();
                dict2.SetString("CFBundleURLName", "Facebook");
                PlistElementArray array2 = dict2.CreateArray("CFBundleURLSchemes");
                array2 = dict2.CreateArray("CFBundleURLSchemes");
                array2.AddString("fb" + facebookId);
            }

            if (bundleId != null){
                dict2 = array.AddDict();
                dict2.SetString("CFBundleURLName", "Line");
                PlistElementArray array2 = dict2.CreateArray("CFBundleURLSchemes");
                array2 = dict2.CreateArray("CFBundleURLSchemes");
                array2.AddString("line3rdp." + bundleId);
            }

            if (twitterId != null){
                dict2 = array.AddDict();
                dict2.SetString("CFBundleURLName", "Twitter");
                PlistElementArray array2 = dict2.CreateArray("CFBundleURLSchemes");
                array2 = dict2.CreateArray("CFBundleURLSchemes");
                array2.AddString("tdsg.twitter." + twitterId);
            }

            File.WriteAllText(_plistPath, _plist.WriteToString());
        }

        private static void SetScriptClass(string pathToBuildProject){
            //读取Xcode中 UnityAppController.mm文件
            string unityAppControllerPath = pathToBuildProject + "/Classes/UnityAppController.mm";
            XDGScriptHandlerProcessor UnityAppController = new XDGScriptHandlerProcessor(unityAppControllerPath);

            //在指定代码后面增加一行代码
            UnityAppController.WriteBelow(@"#include <assert.h>", @"#import <XDGCommonSDK/XDGCommonSDK.h>");
            UnityAppController.WriteBelow(@"#include <assert.h>", @"#import <XDGCommonSDK/XDGSDKSettings.h>");

            UnityAppController.WriteBelow(@"[KeyboardDelegate Initialize];",
                @"[XDGSDK application:application didFinishLaunchingWithOptions:launchOptions];");
            UnityAppController.WriteBelow(@"[KeyboardDelegate Initialize];",
                @"[XDGSDKSettings setDebugMode:YES];");
            UnityAppController.WriteBelow(@"AppController_SendNotificationWithArg(kUnityOnOpenURL, notifData);",
                @"[XDGSDK application:app openURL:url options:options];");
            if (CheckoutUniversalLinkHolder(unityAppControllerPath, @"NSURL* url = userActivity.webpageURL;")){
                UnityAppController.WriteBelow(@"NSURL* url = userActivity.webpageURL;",
                    @"[XDGSDK application:application continueUserActivity:userActivity restorationHandler:restorationHandler];");
            }
            else{
                UnityAppController.WriteBelow(@"- (void)preStartUnity               {}",
                    @"-(BOOL) application:(UIApplication *)application continueUserActivity:(NSUserActivity *)userActivity restorationHandler:(void (^)(NSArray<id<UIUserActivityRestoring>> * _Nullable))restorationHandler{[XDGSDK application:application continueUserActivity:userActivity restorationHandler:restorationHandler];return YES;}");
            }

            UnityAppController.WriteBelow(@"handler(UIBackgroundFetchResultNoData);",
                @"[XDGSDK application:application didReceiveRemoteNotification:userInfo fetchCompletionHandler:completionHandler];");
        }

        private static bool CheckoutUniversalLinkHolder(string filePath, string below){
            StreamReader streamReader = new StreamReader(filePath);
            string all = streamReader.ReadToEnd();
            streamReader.Close();
            int beginIndex = all.IndexOf(below, StringComparison.Ordinal);
            return beginIndex != -1;
        }

        private static string GetValueFromPlist(string infoPlistPath, string key){
            if (infoPlistPath == null){
                return null;
            }

            Dictionary<string, object> dic = (Dictionary<string, object>) Plist.readPlist(infoPlistPath);
            foreach (var item in dic){
                if (item.Key.Equals(key)){
                    return (string) item.Value;
                }
            }

            return null;
        }
    }
}
#endif