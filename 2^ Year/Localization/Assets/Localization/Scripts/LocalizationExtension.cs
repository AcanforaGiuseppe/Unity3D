using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LocalizationExtension
{
    public static string Localize(this string key)
    {
        if (string.IsNullOrEmpty(key))
            return "";
        else
            return Localization.instance.Localize(key);
    }

    public static bool HasContent(this string value)
    {
        return !string.IsNullOrEmpty(value);
    }

}