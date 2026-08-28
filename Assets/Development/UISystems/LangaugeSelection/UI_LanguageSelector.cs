using UnityEngine;
using UnityEngine.Localization.Settings;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_LanguageSelector : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown selector;
    [SerializeField] private Button confirmButton;
    [SerializeField] private LocalizationSettings settings;
    [SerializeField] private UI_CanvasController canvasController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasController = FindAnyObjectByType<UI_CanvasController>();
        Init();
        confirmButton.onClick.AddListener(DestroyCanvas);
        //EventSystem.current.SetSelectedGameObject(confirmButton.gameObject);
    }


    async void Init()
    {
        await settings.GetInitializationOperation().Task;
        var locales = settings.GetAvailableLocales().Locales;
        List<string> options = new();
        int index = 3;
        selector.ClearOptions();
        for (int i = 0; i<locales.Count; i++)
        {
            var locale = locales[i];
            options.Add(locale.LocaleName);
        }
        selector.AddOptions(options);
        selector.value = index;
        selector.RefreshShownValue();
        selector.onValueChanged.AddListener(ChangeLanguage);
        if (!confirmButton.gameObject)
        {
            EventSystem.current.SetSelectedGameObject(confirmButton.gameObject);
        }
    }

    void ChangeLanguage(int id)
    {
        var localeList = settings.GetAvailableLocales().Locales;
        settings.SetSelectedLocale(localeList[id]);
    }

    void DestroyCanvas()
    {
        canvasController.CloseLanguageSelect();
    }
}
