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

                this.preferUnicode.BindValueChanged(_ => updateValue());
                this.catalog.BindValueChanged(_ => updateValue(), true);
            }

            private void updateValue()
            {
                string newText = text.Text.Msgid;

                if (!string.IsNullOrEmpty(text.Text.Msgid)
                    && catalog.Value != null
                    && text.ShouldLocalise)
                {
                    newText = text.ShouldLocalise
                        ? catalog.Value.GetString(newText)
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
        }
    }
}
