﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Drawing;
using osu.Framework.Configuration.Tracking;
using osu.Framework.Extensions;
using osu.Framework.Input;
using osu.Framework.Platform;
using osuTK;

namespace osu.Framework.Configuration
{
    public class FrameworkConfigManager : IniConfigManager<FrameworkSetting>
    {
        internal const string FILENAME = @"framework.ini";

        protected override string Filename => FILENAME;

        protected override void InitialiseDefaults()
        {
            SetDefault(FrameworkSetting.ShowLogOverlay, false);

            SetDefault(FrameworkSetting.WindowedSize, new Size(1366, 768), new Size(640, 480));
            SetDefault(FrameworkSetting.ConfineMouseMode, ConfineMouseMode.Fullscreen);
            SetDefault(FrameworkSetting.ExecutionMode, ExecutionMode.MultiThreaded);
            SetDefault(FrameworkSetting.WindowedPositionX, 0.5, -0.5, 1.5);
            SetDefault(FrameworkSetting.WindowedPositionY, 0.5, -0.5, 1.5);
            SetDefault(FrameworkSetting.LastDisplayDevice, DisplayIndex.Default);
            SetDefault(FrameworkSetting.AudioDevice, string.Empty);
            SetDefault(FrameworkSetting.VolumeUniversal, 1.0, 0.0, 1.0, 0.01);
            SetDefault(FrameworkSetting.VolumeMusic, 1.0, 0.0, 1.0, 0.01);
            SetDefault(FrameworkSetting.VolumeEffect, 1.0, 0.0, 1.0, 0.01);
            SetDefault(FrameworkSetting.SizeFullscreen, new Size(9999, 9999), new Size(320, 240));
            SetDefault(FrameworkSetting.FrameSync, FrameSync.Limit2x);
            SetDefault(FrameworkSetting.WindowMode, WindowMode.Windowed);
            SetDefault(FrameworkSetting.ShowUnicode, false);
            SetDefault(FrameworkSetting.Locale, string.Empty);

#pragma warning disable 618
            SetDefault(FrameworkSetting.MapAbsoluteInputToWindow, false);
            SetDefault(FrameworkSetting.IgnoredInputHandlers, string.Empty);
            SetDefault(FrameworkSetting.CursorSensitivity, 1.0, 0.1, 6, 0.01);
#pragma warning restore 618
        }

        public FrameworkConfigManager(Storage storage, IDictionary<FrameworkSetting, object> defaultOverrides = null)
            : base(storage, defaultOverrides)
        {
        }

        public override TrackedSettings CreateTrackedSettings() => new TrackedSettings
        {
            new TrackedSetting<FrameSync>(FrameworkSetting.FrameSync, v => new SettingDescription(v, "帧数限制", v.GetDescription(), "Ctrl+F7")),
            new TrackedSetting<string>(FrameworkSetting.AudioDevice, v => new SettingDescription(v, "音频设备", string.IsNullOrEmpty(v) ? "默认" : v, v)),
            new TrackedSetting<bool>(FrameworkSetting.ShowLogOverlay, v => new SettingDescription(v, "调试日志", v ? "显示" : "隐藏", "Ctrl+F10")),
            new TrackedSetting<Size>(FrameworkSetting.WindowedSize, v => new SettingDescription(v, "屏幕分辨率", $"{v.Width}x{v.Height}")),
            new TrackedSetting<WindowMode>(FrameworkSetting.WindowMode, v => new SettingDescription(v, "窗口模式", v.GetDescription(), "Alt+Enter")),
#pragma warning disable 618
            new TrackedSetting<double>(FrameworkSetting.CursorSensitivity, v => new SettingDescription(v, "光标灵敏度", v.ToString(@"0.##x"), "使用Ctrl+Alt+R重置")),
            new TrackedSetting<string>(FrameworkSetting.IgnoredInputHandlers, v =>
            {
                bool raw = !v.Contains("Raw");
                return new SettingDescription(raw, "原始输入", raw ? "已启用" : "已禁用", "使用Ctrl+Alt+R重置");
            }),
#pragma warning restore 618
        };
    }

    public enum FrameworkSetting
    {
        ShowLogOverlay,

        AudioDevice,
        VolumeUniversal,
        VolumeEffect,
        VolumeMusic,

        WindowedSize,
        WindowedPositionX,
        WindowedPositionY,
        LastDisplayDevice,

        SizeFullscreen,

        WindowMode,
        ConfineMouseMode,
        FrameSync,
        ExecutionMode,

        ShowUnicode,
        Locale,

        [Obsolete("Input-related settings are now stored in InputConfigManager. Adjustments should be made via Host.AvailableInputHandlers bindables directly.")] // can be removed 20210911
        IgnoredInputHandlers,

        [Obsolete("Input-related settings are now stored in InputConfigManager. Adjustments should be made via Host.AvailableInputHandlers bindables directly.")] // can be removed 20210911
        CursorSensitivity,

        [Obsolete("Input-related settings are now stored in InputConfigManager. Adjustments should be made via Host.AvailableInputHandlers bindables directly.")] // can be removed 20210911
        MapAbsoluteInputToWindow,
    }
}
