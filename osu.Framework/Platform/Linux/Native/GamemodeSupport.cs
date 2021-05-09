using System;
using System.Runtime.InteropServices;
using osu.Framework.Logging;

namespace osu.Framework.Platform.Linux.Native
{
    public static class GamemodeSupport
    {
        [DllImport("libgamemode.so")]
        private static extern int real_gamemode_request_start();

        [DllImport("libgamemode.so")]
        private static extern int real_gamemode_request_end();

        private static bool gamemodeActivated;

        public static void RequestStart()
        {
            try
            {
                if (!gamemodeActivated)
                {
                    if (real_gamemode_request_start() != 0)
                        Logger.Log("无法激活Gamemode。", LoggingTarget.Runtime, LogLevel.Important);
                    gamemodeActivated = true;
                }
            }
            catch (Exception e)
            {
                Logger.Error(e, "激活Gamemode时出现了异常, 请确保libgamemode已安装并且文件 libgamemode.so 存在。");
            }
        }

        public static void RequestEnd()
        {
            try
            {
                if (gamemodeActivated)
                {
                    if (real_gamemode_request_end() != 0)
                        Logger.Log("无法结束Gamemode。", LoggingTarget.Runtime, LogLevel.Important);
                    gamemodeActivated = false;
                }
            }
            catch (Exception e)
            {
                Logger.Error(e, "结束Gamemode时出现了异常, 请确保libgamemode已安装并且文件 libgamemode.so 存在。");
            }
        }
    }
}
