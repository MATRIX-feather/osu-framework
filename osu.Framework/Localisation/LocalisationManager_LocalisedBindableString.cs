// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using NGettext;
using osu.Framework.Bindables;
using osu.Framework.IO.Stores;

namespace osu.Framework.Localisation
{
    public partial class LocalisationManager
    {
        private class LocalisedBindableString : Bindable<string>, ILocalisedBindableString
        {
            private readonly IBindable<IResourceStore<string>> storage = new Bindable<IResourceStore<string>>();
            private readonly IBindable<bool> preferUnicode = new Bindable<bool>();

            private LocalisedString text;
            private bool useLegacyUnicode;
            private readonly IBindable<ICatalog> catalog = new Bindable<ICatalog>();

            public LocalisedBindableString(
                LocalisedString text,
                IBindable<IResourceStore<string>> storage,
                IBindable<bool> preferUnicode,
                IBindable<ICatalog> catalog)
            {
                this.text = text;
                this.catalog.BindTo(catalog);

                this.storage.BindTo(storage);
                this.preferUnicode.BindTo(preferUnicode);

                this.storage.BindValueChanged(_ => updateValue(), true);
                this.preferUnicode.BindValueChanged(_ => updateValue());
                this.catalog.BindValueChanged(_ => updateValue());
            }

            private void updateValue()
            {
                string newText;

                //todo: 使用`useLegacyUnicode`来判断是否要使用旧的unicode方案
                //bug: 如果某一个歌名和翻译的源文本一样会一带翻译...
                if (text.Text.Original == text.Text.Fallback)
                {
                    newText = preferUnicode.Value ? text.Text.Original : text.Text.Fallback;

                    if (text.ShouldLocalise && storage.Value != null)
                        newText = storage.Value.Get(newText);
                }
                else
                {
                    newText = catalog.Value != null
                        ? catalog.Value.GetString(text.Text.Original)
                        : text.Text.Original;
                }

                if (text.Args?.Length > 0 && !string.IsNullOrEmpty(newText))
                {
                    try
                    {
                        newText = string.Format(newText, text.Args);
                    }
                    catch (FormatException)
                    {
                        // Prevent crashes if the formatting fails. The string will be in a non-formatted state.
                    }
                }

                Value = newText;
            }

            LocalisedString ILocalisedBindableString.Text
            {
                set
                {
                    if (text.Equals(value))
                        return;

                    text = value;

                    updateValue();
                }
            }

            bool ILocalisedBindableString.UseLegacyUnicode
            {
                get => useLegacyUnicode;
                set
                {
                    useLegacyUnicode = value;

                    updateValue();
                }
            }
        }
    }
}
