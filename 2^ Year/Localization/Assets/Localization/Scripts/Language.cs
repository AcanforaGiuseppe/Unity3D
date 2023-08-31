using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;
using System;

[System.Serializable]
public class Language
{
    public TextAsset file;
    public SystemLanguage language;
    public string cultureCode;
    public bool imperial;

    Dictionary<string, string> keys;
    CultureInfo culture;

    public void Load()
    {
        culture = new CultureInfo(cultureCode);

        string text = file.text;
        string[] lines = text.Split(new char[] { '\n', '\r' });
        keys = new Dictionary<string, string>();

        foreach (var line in lines)
        {
            string[] key_value = line.Split(new char[] { '=' });

            if (key_value != null && key_value.Length == 2)
                keys.Add(key_value[0], key_value[1]);
        }
    }

    internal string Localize(string key)
    {
        if (keys.ContainsKey(key))
            return keys[key];
        else
            return string.Format("_UNDEF_{0}", key);
    }

    public string FormatShortDate(DateTime date)
    {
        return date.ToString(culture.DateTimeFormat.ShortDatePattern);
    }

    public string FormatNumber(float number)
    {
        return string.Format(culture, "{0:N2}", number);
    }

    public string FormatShortDistance(float distanceInMeter)
    {
        if (imperial)
            return string.Format("{0} in", FormatNumber(distanceInMeter * 0.0254f));

        return string.Format("{0} cm", FormatNumber(distanceInMeter * 100));
    }

}