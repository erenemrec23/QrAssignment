using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using TemplateProject.Application;
using TemplateProject.Application.Interfaces;

namespace TemplateProject.Infrastructure.Localization
{
    public class AppLocalizer : IAppLocalizer
    {
        private readonly IStringLocalizer<SharedResource> _localizer;

        public AppLocalizer(IStringLocalizer<SharedResource> localizer)
        {
            _localizer = localizer;
        }

        public string this[string key] => _localizer[key];

        public string GetString(string key, params object[] arguments) => _localizer[key, arguments];
    }
}
