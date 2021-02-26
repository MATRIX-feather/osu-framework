// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.ComponentModel;

namespace osu.Framework.Configuration
{
    public enum FrameSync
    {
        [Description("垂直同步")]
        VSync,

        [Description("2倍刷新率")]
        Limit2x,

        [Description("4倍刷新率")]
        Limit4x,

        [Description("8倍刷新率")]
        Limit8x,

        [Description("无限制")]
        Unlimited,
    }
}
