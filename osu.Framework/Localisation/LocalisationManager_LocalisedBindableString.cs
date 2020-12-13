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
            private readonly IBindableList<ICatalog> catalogs = new BindableList<ICatalog>();
            private readonly IBindable<bool> preferUnicode = new Bindable<bool>();

            private LocalisedString text;
            private bool useLegacyUnicode;

            public LocalisedBindableString(
                LocalisedString text,
                IBindable<IResourceStore<string>> storage,
                IBindable<bool> preferUnicode,
                IBindableList<ICatalog> catalogs,
                bool useLegacyUnicode)
            {
                this.text = text;
                this.catalogs.BindTo(catalogs);
                this.useLegacyUnicode = useLegacyUnicode;

                this.storage.BindTo(storage);
                this.preferUnicode.BindTo(preferUnicode);

                this.storage.BindValueChanged(_ => updateValue(), true);
                this.preferUnicode.BindValueChanged(_ => updateValue());
                this.catalogs.BindCollectionChanged((_, __) => updateValue());
            }

            private void updateValue()
            {
                string newText = "";

                if (useLegacyUnicode)
                {
                    newText = preferUnicode.Value ? text.Text.Original : text.Text.Fallback;

                    if (text.ShouldLocalise && storage.Value != null)
                        newText = storage.Value.Get(newText);

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
                }
                else
                {
                    if (text.Args?.Length > 0)
                    {
                        foreach (var catalog in catalogs)
                        {
                            newText = catalog.GetString(text.Text.Original, text.Args);
                            if (!string.IsNullOrEmpty(newText)) break;
                        }
                    }
                    else
                    {
                        foreach (var catalog in catalogs)
                        {
                            newText = catalog.GetString(text.Text.Original);
                            if (!string.IsNullOrEmpty(newText)) break;
                        }
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
                set => useLegacyUnicode = value;
            }
        }
    }
}
