using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using static HsMod.PluginConfig;

namespace HsMod
{
    public class LocalizationManager
    {
        public static string GetCurrentLang()
        {
            var localName = Localization.GetLocale();
            string res;
            if (localName == Locale.UNKNOWN)
            {
                Utils.MyLogger(BepInEx.Logging.LogLevel.Warning, $"Hearthstone Locale Not Found, now using enUS");
                res = "enUS";
            }
            else { res = localName.ToString(); }

            Utils.MyLogger(BepInEx.Logging.LogLevel.Warning, $"HsMod Languages: {res}");
            return res;
        }

        public static string GetLangFileContext(string lang)
        {
            //string localName = GetCurrentLang();
            string fileName = $"./Languages/{lang}.json";
            string context = FileManager.ReadEmbeddedFile(fileName);
            if (String.IsNullOrEmpty(context))
            {
                fileName = $"./Languages/enUS.json";
                Utils.MyLogger(BepInEx.Logging.LogLevel.Warning, $"HsMod languages file not found or empty, now using {fileName}");
            }
            context = FileManager.ReadEmbeddedFile(fileName);
            return context;
        }

        public static string GetLangValue(string lang_key)
        {
            var lang_file = GetLangFileContext(pluginInitLanague.Value);
            var jsonObject = JsonConvert.DeserializeObject<Dictionary<string, string>>(lang_file);
            string res;
            if (jsonObject.TryGetValue(lang_key, out res))
            {
                return res;
            }
            else
            {
                var file = GetLangFileContext("enUS");
                var job = JsonConvert.DeserializeObject<Dictionary<string, string>>(file);
                job.TryGetValue(lang_key, out res);
                if (String.IsNullOrEmpty(res))
                {
                    Utils.MyLogger(BepInEx.Logging.LogLevel.Error, $"Languages key '{lang_key}' not found.");
                    return "KEY_NOT_FOUND";
                }
                return res;
            }
        }


        public static Locale StrToLocale(string lang)
        {
            STRING_TO_LOCALE.TryGetValue(lang, out var value);
            return value;
        }
        public static readonly Dictionary<string, Locale> STRING_TO_LOCALE = new Dictionary<string, Locale>
    {
        {
            "enUS",
            Locale.enUS
        },
        {
            "enGB",
            Locale.enGB
        },
        {
            "frFR",
            Locale.frFR
        },
        {
            "deDE",
            Locale.deDE
        },
        {
            "koKR",
            Locale.koKR
        },
        {
            "esES",
            Locale.esES
        },
        {
            "esMX",
            Locale.esMX
        },
        {
            "ruRU",
            Locale.ruRU
        },
        {
            "zhTW",
            Locale.zhTW
        },
        {
            "zhCN",
            Locale.zhCN
        },
        {
            "itIT",
            Locale.itIT
        },
        {
            "ptBR",
            Locale.ptBR
        },
        {
            "plPL",
            Locale.plPL
        },
        {
            "jaJP",
            Locale.jaJP
        },
        {
            "thTH",
            Locale.thTH
        }
    };

    }


}