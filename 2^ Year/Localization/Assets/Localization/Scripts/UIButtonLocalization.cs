using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtonLocalization : MonoBehaviour
{
    public SystemLanguage language;

    public void ChangeLanguage()
    {
        Localization.instance.ChangeLanguage(language);
    }

}