using NUnit.Framework;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI_AccessOptions : UI_SettingsMenu
{
    [Header("Accessibility")]
    [SerializeField] private TMP_Dropdown cbModeDropdown;
    [SerializeField] private TMP_Dropdown languageDropdown;
    [SerializeField] private Toggle highContrastToggle;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitCBModeDD();
        InitContrastModeDD();
        InitLanguageDD();


    }

    protected void InitCBModeDD()
    {
        cbModeDropdown.ClearOptions();
        var options = new List<string>
        {
            "Normal",
            "Deuteranopia",
            "Protanopia",
            "Tritanopia"
        };
        cbModeDropdown.AddOptions(options);
        int index = PlayerPrefs.GetInt("CBMode", 0);
        cbModeDropdown.value = index;
        cbModeDropdown.RefreshShownValue();

        ApplyCBMode(index);
        cbModeDropdown.onValueChanged.AddListener(OnCBModeChanged);
    }

    protected void OnCBModeChanged(int index)
    {
        ApplyCBMode(index);
        PlayerPrefs.SetInt("CBMode", index);
        PlayerPrefs.Save();
    }

    protected void ApplyCBMode(int index)
    {
        switch (index)
        {
            case 0: SetCBMode(ColorBlindMode.None); break;
            case 1: SetCBMode(ColorBlindMode.Deuteranopia); break;
            case 2: SetCBMode(ColorBlindMode.Protanopia); break;
            case 3: SetCBMode(ColorBlindMode.Tritanopia); break;
        }
    }

    protected void SetCBMode(ColorBlindMode cbMode)
    {
         var currentCBMode = cbMode;
        switch (currentCBMode)
        {
            case ColorBlindMode.None:break;
            case ColorBlindMode.Deuteranopia: break;
            case ColorBlindMode.Protanopia: break;
            case ColorBlindMode.Tritanopia: break;
        }
    }

    protected void OnLanguageChanged(int index)
    {
        ApplyLanguage(index);
        PlayerPrefs.SetInt("Language", index);
        PlayerPrefs.Save();
    }

    protected void InitLanguageDD()
    {
        languageDropdown.ClearOptions();
        var options = new List<string>
        {
            "English",
            "French",
            "Spanish",
            "Chinese",
            "Japanese",
            "Hindi",

        };
        languageDropdown.AddOptions(options);
        int index = PlayerPrefs.GetInt("Language", 0);
        languageDropdown.value = index;
        languageDropdown.RefreshShownValue();

        ApplyLanguage(index);
        PlayerPrefs.SetInt("Language", index);
        PlayerPrefs.Save();

    }

    protected void ApplyLanguage(int index)
    {
        switch (index)
        {
            case 0: SetLanguage(Language.English); break;
            case 1: SetLanguage(Language.French); break;
            case 2: SetLanguage(Language.Spanish); break;
            case 3: SetLanguage(Language.Chinese); break;
            case 4: SetLanguage(Language.Japanese); break;
            case 5: SetLanguage(Language.Hindi); break;
        }
    }

    //this will need refs to whatever will change the text language
    protected void SetLanguage(Language language)
    {
        switch (language)
        {
            case Language.English: break;
            case Language.French: break;
            case Language.Spanish: break;
            case Language.Chinese: break;
            case Language.Japanese: break;
            case Language.Hindi: break;

        }
    }

    protected void InitContrastModeDD()
    {
        bool value = PlayerPrefs.GetInt("HighContrastMode", 0) == 1;
        highContrastToggle.isOn = value;

        ApplyContrastMode(value);
        highContrastToggle.onValueChanged.AddListener(OnContrastModeChanged);
    }

    protected void OnContrastModeChanged(bool value)
    {
        ApplyContrastMode(value);
        PlayerPrefs.SetInt("HighContrastMode", value ? 1 : 0);
        PlayerPrefs.Save();
    }

    protected void ApplyContrastMode(bool value)
    {
        SetContrastMode(value);
    }

    protected void SetContrastMode(bool value)
    {
        var ui = FindObjectsByType<UnityEngine.UI.Graphic>(FindObjectsSortMode.None);
        foreach(var element in ui)
        {
            if (enabled)
            {
                if (element is UnityEngine.UI.Text || element is TMPro.TextMeshProUGUI)
                    element.color = Color.black;
                else
                    element.color = Color.white;
            }
            else
            {
                if (element is UnityEngine.UI.Text || element is TMPro.TextMeshProUGUI)
                    element.color = Color.white;
                else
                    element.color = Color.gray;
            }
        }
    }
}

public enum ColorBlindMode
{
    None,
    Deuteranopia,
    Protanopia,
    Tritanopia
}

public enum Language
{
    English,
    French,
    Spanish,
    Chinese,
    Japanese,
    Hindi
}
