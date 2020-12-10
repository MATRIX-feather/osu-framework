﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Globalization;
using System.IO;
using osu.Framework.Configuration;
using osu.Framework.IO.Stores;
using JetBrains.Annotations;
using osu.Framework.Bindables;
using NGettext;

namespace osu.Framework.Localisation
{
    public partial class LocalisationManager
    {
        private readonly List<LocaleMapping> locales = new List<LocaleMapping>();

        private readonly Bindable<bool> preferUnicode;
        private readonly Bindable<string> configLocale;
        private readonly Bindable<IResourceStore<string>> currentStorage = new Bindable<IResourceStore<string>>();
        private readonly Bindable<ICatalog> catalog = new Bindable<ICatalog>();

        public LocalisationManager(FrameworkConfigManager config)
        {
            preferUnicode = config.GetBindable<bool>(FrameworkSetting.ShowUnicode);

            configLocale = config.GetBindable<string>(FrameworkSetting.Locale);
            configLocale.BindValueChanged(updateLocale, true);
        }

        public void AddLanguage(string language, IResourceStore<string> storage)
        {
            locales.Add(new LocaleMapping { Name = language, Storage = storage });
            configLocale.TriggerChange();
        }

        /// <summary>
        /// Creates an <see cref="ILocalisedBindableString"/> which automatically updates its text according to information provided in <see cref="ILocalisedBindableString.Text"/>.
        /// </summary>
        /// <returns>The <see cref="ILocalisedBindableString"/>.</returns>
        [NotNull]
        public ILocalisedBindableString GetLocalisedString(LocalisedString original, bool useLegacyUnicode = false)
            => new LocalisedBindableString(
                original,
                currentStorage,
                preferUnicode,
                catalog,
                useLegacyUnicode);

        private void updateLocale(ValueChangedEvent<string> args)
        {
            if (locales.Count == 0)
                return;

            var validLocale = locales.Find(l => l.Name == args.NewValue);

            if (validLocale == null)
            {
                var culture = string.IsNullOrEmpty(args.NewValue) ? CultureInfo.CurrentCulture : new CultureInfo(args.NewValue);

                for (var c = culture; !EqualityComparer<CultureInfo>.Default.Equals(c, CultureInfo.InvariantCulture); c = c.Parent)
                {
                    validLocale = locales.Find(l => l.Name == c.Name);
                    if (validLocale != null)
                        break;
                }

                validLocale ??= locales[0];
            }

            if (validLocale.Name != args.NewValue)
                configLocale.Value = validLocale.Name;
            else
                currentStorage.Value = validLocale.Storage;
        }

        public void SetCatalog(Stream moStream)
        {
            catalog.Value = new Catalog(moStream);
        }

        private class LocaleMapping
        {
            public string Name;
            public IResourceStore<string> Storage;
        }
    }
}
