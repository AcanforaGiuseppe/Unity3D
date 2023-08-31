using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class UITextProLocalizator : MonoBehaviour
{
    public string localizationKey;

    TextMeshProUGUI text;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
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