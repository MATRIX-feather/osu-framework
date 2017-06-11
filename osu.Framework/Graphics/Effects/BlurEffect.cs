﻿using OpenTK;
using OpenTK.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.MathUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osu.Framework.Graphics.Effects
{
    /// <summary>
    /// A blur effect that wraps a drawable in a <see cref="BufferedContainer"/> which applies a blur effect to it.
    /// </summary>
    public class BlurEffect : IEffect<BufferedContainer>
    {
        /// <summary>
        /// The strength of the blur. Default is 1.
        /// </summary>
        public float Strength { get; set; } = 1f;
        /// <summary>
        /// The sigma of the blur. Default is (2, 2).
        /// </summary>
        public Vector2 Sigma { get; set; } = new Vector2(2f, 2f);

        public BufferedContainer ApplyTo(Drawable drawable)
        {
            return new BufferedContainer
            {
                RelativeSizeAxes = drawable.RelativeSizeAxes,
                AutoSizeAxes = Axes.Both & ~drawable.RelativeSizeAxes,
                BlurSigma = Sigma,
                Anchor = drawable.Anchor,
                Origin = drawable.Origin,
                Padding = new MarginPadding
                {
                    Horizontal = Blur.KernelSize(Sigma.X),
                    Vertical = Blur.KernelSize(Sigma.Y)
                },
                Alpha = Strength,
                Children = new[]
                {
                    drawable
                }
            };
        }
    }
}
