using NUnit.Framework;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Settings;

public class UI_AccessOptions : UI_SettingsMenu
{
    [Header("Accessibility")]
    [SerializeField] private TMP_Dropdown cbModeDropdown;
    [SerializeField] private TMP_Dropdown languageDropdown;
    [SerializeField] private Toggle highContrastToggle;
    [SerializeField] private Material cbMaterial;
    [SerializeField] private LocalizationSettings settings;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        int mode = PlayerPrefs.GetInt("CBMode", 0);
        Shader.SetGlobalFloat("_Mode", mode);
    }

    public void SetFirstAccessButton()
    {
        EventSystem.current.SetSelectedGameObject(cbModeDropdown.gameObject);
    }

    void Start()
    {
        InitCBModeDD();
        InitContrastModeDD();
        InitLanguageDD();
        SetFirstAccessButton();

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
        Debug.Log("Dropdown changed: " + index);
        ApplyCBMode(index);
        PlayerPrefs.SetInt("CBMode", index);
        PlayerPrefs.Save();
    }

    protected void ApplyCBMode(int index)
    {
        switch (index)
        {
            case 0: SetCBMode(ColorBlindMode.None); 
                break;
            case 1: SetCBMode(ColorBlindMode.Deuteranopia); 
                break;
            case 2: SetCBMode(ColorBlindMode.Protanopia); 
                break;
            case 3: SetCBMode(ColorBlindMode.Tritanopia); 
                break;
        }
    }

    protected void SetCBMode(ColorBlindMode cbMode)
    {
        float modeValue = 0f;
        var currentCBMode = cbMode;
        switch (currentCBMode)
        {
            case ColorBlindMode.None:
                modeValue = 0f;
                Shader.SetGlobalFloat("_Strength", 0f);
                break;
            case ColorBlindMode.Deuteranopia:
                modeValue = 2f;
                Shader.SetGlobalFloat("_Strength", 1f);
                break;
            case ColorBlindMode.Protanopia:
                modeValue = 1f;
                Shader.SetGlobalFloat("_Strength", 1f);
                break;
            case ColorBlindMode.Tritanopia:
                modeValue = 3f;
                Shader.SetGlobalFloat("_Strength", 1f);
                break;
        }
        Shader.SetGlobalFloat("_Mode", modeValue);
        Debug.Log("Setting shader mode: " + modeValue);
    }

    protected void OnLanguageChanged(int index)
    {
        ChangeLanguage(index);
        PlayerPrefs.SetInt("Language", index);
        PlayerPrefs.Save();
    }

    protected async void InitLanguageDD()
    {
        await settings.GetInitializationOperation().Task;
        var locales = settings.GetAvailableLocales().Locales;
        List<string> options = new();
        int langIndex = 6;
        languageDropdown.ClearOptions();
        for (int i = 0; i < locales.Count; i++)
        {
            var locale = locales[i];
            options.Add(locale.LocaleName);
        }
        languageDropdown.AddOptions(options);
        languageDropdown.value = langIndex;
        languageDropdown.RefreshShownValue();
        ChangeLanguage(langIndex);
        languageDropdown.onValueChanged.AddListener(OnLanguageChanged);

    }

    void ChangeLanguage(int id)
    {
        var localeList = settings.GetAvailableLocales().Locales;
        settings.SetSelectedLocale(localeList[id]);
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
            if (!value)
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