using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using Stage;
using Cute;

namespace changeServerPlugin
{
    [BepInProcess("imascgstage.exe")]
    [BepInPlugin("toumei.changeServerPlugin", "changeServerPlugin", "1.0")]
    public class Main : BaseUnityPlugin
    {
        public static ConfigEntry<string> configServerURL { get; set; }
        public static ConfigEntry<bool> configisUseHTTPS { get; set; }

        public const string PluginGuid = "toumei.cgss.changeServer";
        public const string PluginName = "changeServerPlugin";
        public const string PluginVersion = "1.0.0.0";
        public Main()
        {
            configServerURL = Config.Bind("Server", "ServerURL", "localhost:5000/", "ゲームが接続するサーバー");
            configisUseHTTPS = Config.Bind("Server", "isUseHTTPS", false, "サーバー通信にHTTPSを使用するかどうか");
        }
        public void Awake()
        {
                        Logger.LogInfo($"Plugin changeServerPlugin is loaded!");
            new Harmony(PluginGuid).PatchAll();
        }


        [HarmonyPatch(typeof(NetworkUtil), "GetApplicationServerUrl")]
        internal class ChangeServerURL //サーバーURL変更
        {
            public static bool Prefix(ref string __result)
            {
                __result = configServerURL.Value;
                return false;
            }
        }

        [HarmonyPatch(typeof(NetworkUtil), "GetSchemeType")]
        internal class ChangeSchemeType //サーバーURL変更
        {
            public static bool Prefix(ref CustomPreference.eSchemeType __result)
            {
                if (configisUseHTTPS.Value == true)
                {
                    __result = CustomPreference.eSchemeType.Https;
                }
                else {
                    __result = CustomPreference.eSchemeType.Http;
                }

                return false;
            }
        }
    }
}
