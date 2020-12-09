using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ism.Infrastructure.Model
{
    public class SpokenLanguage : Language
    {
        [JsonProperty("level")]
        private LanguageLevel _level;

        public LanguageLevel Level
        {
            get { return _level; }
            set
            {
                SetProperty(ref _level, value);
                PropertyDeletegate?.Invoke(this);
            }
        }
    }

    public enum LanguageLevel
    {
        PASIVE=0,
        ACTIVE=1
    }
}
