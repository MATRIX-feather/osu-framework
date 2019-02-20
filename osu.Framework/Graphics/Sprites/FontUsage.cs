// Copyright (c) 2007-2018 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu-framework/master/LICENCE

using System;
using JetBrains.Annotations;

namespace osu.Framework.Graphics.Sprites
{
    /// <summary>
    /// Represents a specific usage of a font.
    /// </summary>
    public readonly struct FontUsage : IEquatable<FontUsage>
    {
        private const float default_text_size = 20;

        /// <summary>
        /// Gets the default <see cref="FontUsage"/>, using the fallback font family.
        /// </summary>
        public static FontUsage Default => new FontUsage(null);

        /// <summary>
        /// The font family name.
        /// </summary>
        [CanBeNull]
        public string Family { get; }

        /// <summary>
        /// The font weight.
        /// </summary>
        [CanBeNull]
        public string Weight { get; }

        /// <summary>
        /// Whether the font is italic.
        /// </summary>
        public bool Italics { get; }

        /// <summary>
        /// The size of the text in local space. For a value of 16, a single line will have a height of 16px.
        /// </summary>
        public float Size { get; }

        /// <summary>
        /// Whether all characters should be spaced the same distance apart.
        /// </summary>
        public bool FixedWidth { get; }

        /// <summary>
        /// The font's full name to be used for lookups. This is an aggregate of all other properties of <see cref="FontUsage"/>.
        /// <remarks>
        /// The format is of the form: <br />
        /// {Family} <br />
        /// {Family}-Italic <br />
        /// {Family}-{Weight}Italic
        /// </remarks>
        /// </summary>
        [NotNull]
        public string FontName { get; }

        /// <summary>
        /// Creates an instance of <see cref="FontUsage"/> using the specified font <paramref name="family"/>, font <paramref name="weight"/> and a value indicating whether the used font is italic or not.
        /// </summary>
        /// <param name="family">The font family name.</param>
        /// <param name="size">The size of the text in local space. For a value of 16, a single line will have a height of 16px.</param>
        /// <param name="weight">The font weight.</param>
        /// <param name="italics">Whether the font is italic.</param>
        /// <param name="fixedWidth">Whether all characters should be spaced the same distance apart.</param>
        public FontUsage([CanBeNull] string family = null, float size = default_text_size, [CanBeNull] string weight = null, bool italics = false, bool fixedWidth = false)
        {
            Family = family;
            Size = size >= 0 ? size : throw new ArgumentOutOfRangeException(nameof(size), "Must be non-negative.");
            Weight = weight;
            Italics = italics;
            FixedWidth = fixedWidth;

            FontName = Family + "-";
            if (!string.IsNullOrEmpty(weight))
                FontName += weight;

            if (italics)
                FontName += "Italic";

            FontName = FontName.TrimEnd('-');
            Legacy = false;
        }

        /// <summary>
        /// Creates a new <see cref="FontUsage"/> by applying adjustments to this <see cref="FontUsage"/>.
        /// </summary>
        /// <param name="family">The font family. If null, the value is copied from this <see cref="FontUsage"/>.</param>
        /// <param name="size">The text size. If null, the value is copied from this <see cref="FontUsage"/>.</param>
        /// <param name="weight">The font weight. If null, the value is copied from this <see cref="FontUsage"/>.</param>
        /// <param name="italics">Whether the font is italic. If null, the value is copied from this <see cref="FontUsage"/>.</param>
        /// <param name="fixedWidth">Whether all characters should be spaced apart the same distance. If null, the value is copied from this <see cref="FontUsage"/>.</param>
        /// <returns>The resulting <see cref="FontUsage"/>.</returns>
        public FontUsage With([CanBeNull] string family = null, [CanBeNull] float? size = null, [CanBeNull] string weight = null, [CanBeNull] bool? italics = null,
                              [CanBeNull] bool? fixedWidth = null)
            => new FontUsage(family ?? Family, size ?? Size, weight ?? Weight, italics ?? Italics, fixedWidth ?? FixedWidth);

        public override string ToString() => $"Font={FontName}, Size={Size}, Italics={Italics}, FixedWidth={FixedWidth}";

        public bool Equals(FontUsage other)
        {
            return string.Equals(Family, other.Family) && string.Equals(Weight, other.Weight) && Italics == other.Italics && Size.Equals(other.Size) && FixedWidth == other.FixedWidth;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is FontUsage other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Family != null ? Family.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (Weight != null ? Weight.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Italics.GetHashCode();
                hashCode = (hashCode * 397) ^ Size.GetHashCode();
                hashCode = (hashCode * 397) ^ FixedWidth.GetHashCode();
                return hashCode;
            }
        }

        #region Obsolete

        internal bool Legacy { get; }

        private FontUsage(string fontName)
            : this(family: fontName)
        {
            Legacy = true;
        }

        [Obsolete("Setting font by name is deprecated. Use `Font = new FontUsage(...)` (see: https://github.com/ppy/osu-framework/pull/2043)")]
        public static implicit operator FontUsage(string fontName) => new FontUsage(fontName);

        [Obsolete("Comparing fonts as strings is deprecated. See: https://github.com/ppy/osu-framework/pull/2043")]
        public static bool operator ==(FontUsage fontUsage, string fontName) => fontUsage.FontName == fontName;

        [Obsolete("Comparing fonts as strings is deprecated. See: https://github.com/ppy/osu-framework/pull/2043")]
        public static bool operator !=(FontUsage fontUsage, string fontName) => fontUsage.FontName != fontName;

        #endregion
    }
}
