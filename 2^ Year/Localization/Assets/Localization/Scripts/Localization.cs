using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using System.Globalization;
using System;

public class Localization : MonoBehaviour
{
    public delegate void LanguageChangeDelete();
    public event LanguageChangeDelete onLanguageChange;
    public static Localization instance;
    public Language[] languages;
    public SystemLanguage defaultLanguage;
    public CultureInfo codeCulture;

    Language currentLanguage;

    void Awake()
    {
        codeCulture = new CultureInfo("en-US");

        instance = this;

        SystemLanguage systemLanguage = Application.systemLanguage;

        foreach (var lang in languages)
            lang.Load();

        SystemLanguage languagePreferred = (SystemLanguage)PlayerPrefs.GetInt("language", (int)Application.systemLanguage);

        ChangeLanguage(languagePreferred, false);
    }

    public string Localize(string key)
    {
        return currentLanguage.Localize(key);
    }

    public void ChangeLanguage(SystemLanguage newLanguage, bool reload = true)
    {
        currentLanguage = null;

        foreach (var lang in languages)
        {
            // Imposta di default la lingua di sistema
            if (newLanguage == lang.language)
                currentLanguage = lang;
        }

        // Non ho trovato il linguaggio, scelgo il default
        if (currentLanguage == null)
            currentLanguage = languages.Where(a => a.language == defaultLanguage).FirstOrDefault();

        PlayerPrefs.SetInt("language", (int)currentLanguage.language);

        Debug.LogFormat("La data di oggi è: {0}", currentLanguage.FormatShortDate(DateTime.Now));
        Debug.LogFormat("Numero: {0}", currentLanguage.FormatNumber(123933.23f));
        Debug.LogFormat("La distanza è: {0}", currentLanguage.FormatShortDistance(1.3f));

        if (reload)
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            onLanguageChange.Invoke();

        return;
    }
}