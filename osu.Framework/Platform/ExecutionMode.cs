// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.ComponentModel;

namespace osu.Framework.Platform
{
    public enum ExecutionMode
    {
        [Description("单线程")]
        SingleThread,

        [Description("多线程")]
        MultiThreaded
    }
}
