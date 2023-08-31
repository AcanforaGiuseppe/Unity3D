using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class UITextLocalizator : MonoBehaviour
{
    public string localizationKey;

    Text text;

    void Awake()
    {
        text = GetComponent<Text>();
    }

    void OnEnable()
    {
        text.text = localizationKey.Localize();
        Localization.instance.onLanguageChange += Instance_onLanguageChange;
    }

    void OnDisable()
    {
        Localization.instance.onLanguageChange -= Instance_onLanguageChange;
    }

    private void Instance_onLanguageChange()
    {
        text.text = localizationKey.Localize();
    }

}