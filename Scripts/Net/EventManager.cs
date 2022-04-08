using System;

namespace XD.Intl.Common{
    public class EventManager{
        
        //IP信息
        private readonly static string IP_INFO = "https://ip.xindong.com/myloc2";
        
        public static void SendEvent(){
            Net.GetRequest(IP_INFO, null, (data) => {
              XDGTool.Log("打点测试成功：" + data);
            }, (code, msg) => {
                XDGTool.Log("打点测试失败：" + msg);
            });
        }
    }
}