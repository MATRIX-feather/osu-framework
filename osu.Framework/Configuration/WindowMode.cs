// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.ComponentModel;

namespace osu.Framework.Configuration
{
    public enum WindowMode
    {
        [Description("窗口化")]
        Windowed = 0,

        [Description("无边框")]
        Borderless = 1,

        [Description("全屏")]
        Fullscreen = 2
    }
}
