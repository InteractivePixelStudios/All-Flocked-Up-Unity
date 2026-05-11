using FMODUnity;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*
---Notes---
- There were some bugs with the audio settings.
  The Music slider was adjusting the Master bus & music/master reset was wired wrong - this has been fixed - IPM.
-
-
-
-
*/


public class UI_AudioOptions : UI_SettingsMenu
{
    [Header("Audio")]
    [SerializeField] private Slider mainVolSlider;
    private const float mainVolBase = 1f;
    [SerializeField] private TextMeshProUGUI mainVolText;
    [SerializeField] private Slider sfxVolSlider;
    private const float sfxVolBase = 0.4f;
    [SerializeField] private TextMeshProUGUI sfxVolText;
    [SerializeField] private Slider musicVolSlider;
    private const float musicVolBase = 0.7f;
    [SerializeField] private TextMeshProUGUI musicVolText;
    [SerializeField] private Slider ambientVolSlider;
    private const float ambientBase = 0.5f;
    [SerializeField] private TextMeshProUGUI ambientVolText;
    [SerializeField] private Toggle focusMuteToggle;
    private const bool focusBase = true;
    [SerializeField] private TMP_Dropdown outputDropdown;
    private const int outputBase = 0;

    [Header("Defaults")]
    [SerializeField] private Button defaultButton;
    [SerializeField] private Button confirmDefaultButton;
    [SerializeField] private Button cancelDefaultButton;
    [SerializeField] private GameObject confirmDefaultWindow;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void ShowConfirmWindow()
    {
        confirmDefaultWindow.SetActive(true);
    }

    private void HideConfirmWindow()
    {
        confirmDefaultWindow.SetActive(false);

    }

    private void DefaultSettings()
    {
        ResetMain();
        ResetMusic();
        ResetSFX();
        ResetAmbient();
        ResetFocus();
        ResetOutput();
        HideConfirmWindow();
    }

    public void SetFirstAudioButton()
    {
        EventSystem.current.SetSelectedGameObject(mainVolSlider.gameObject);
    }

    void Start()
    {
        InitMainSlider();
        InitMusicSlider();
        InitAmbientSlider();
        InitSFXSlider();
        InitFocusMuteToggle();
        InitOutputDropdown();
        defaultButton.onClick.AddListener(ShowConfirmWindow);
        cancelDefaultButton.onClick.AddListener(HideConfirmWindow);
        confirmDefaultButton.onClick.AddListener(DefaultSettings);
    }
    protected void InitMainSlider()
    {
        float saved = PlayerPrefs.GetFloat("MasterVolume", mainVolBase);
        mainVolSlider.value = saved;
        mainVolSlider.SetValueWithoutNotify(saved);
        SetMainVol(saved);
        SetMainVolText(saved);
        mainVolSlider.onValueChanged.AddListener(OnMainVolChanged);
    }


    protected void OnMainVolChanged(float value)
    {
        SetMainVol(value);
        SetMainVolText(value);

        PlayerPrefs.SetFloat("MasterVolume", value);
        PlayerPrefs.Save();
    }

    protected void SetMainVol(float value)
    {
        AudioWizard.Instance.masterVolume = value;
    }

    protected void SetMainVolText(float value)
    {
        mainVolText.SetText($"{Mathf.RoundToInt(value * 100)}%");
    }

    void ResetMain()
    {
        OnMainVolChanged(mainVolBase);
        musicVolSlider.value = mainVolBase;
        musicVolSlider.SetValueWithoutNotify(mainVolBase);
        SetMainVol(mainVolBase);
        SetMainVolText(mainVolBase);
    }

    protected void InitSFXSlider()
    {
        float saved = PlayerPrefs.GetFloat("SFXVolume", sfxVolBase);
        sfxVolSlider.value = saved;
        sfxVolSlider.SetValueWithoutNotify(saved);
        SetSFXVol(saved);
        SetSFXVolText(saved);
        sfxVolSlider.onValueChanged.AddListener(OnSFXVolChanged);
    }

    protected void OnSFXVolChanged(float value)
    {
        SetSFXVol(value);
        SetSFXVolText(value);
        PlayerPrefs.SetFloat("SFXVolume", value);
        PlayerPrefs.Save();
    }

    protected void SetSFXVol(float value)
    {
        AudioWizard.Instance.sfxVolume = value;
    }

    protected void SetSFXVolText(float value)
    {
        sfxVolText.SetText($"{Mathf.RoundToInt(value * 100)}%");
    }
    void ResetSFX()
    {
        OnSFXVolChanged(sfxVolBase);
        sfxVolSlider.value = sfxVolBase;
        sfxVolSlider.SetValueWithoutNotify(sfxVolBase);
        SetSFXVol(sfxVolBase);
        SetSFXVolText(sfxVolBase);
    }

    protected void InitMusicSlider()
    {
        float saved = PlayerPrefs.GetFloat("MusicVolume", musicVolBase);
        musicVolSlider.value = saved;
        musicVolSlider.SetValueWithoutNotify(saved);
        SetMusicVol(saved);
        SetMusicVolText(saved);
        musicVolSlider.onValueChanged.AddListener(OnMusicVolChanged);
    }

    protected void OnMusicVolChanged(float value)
    {
        SetMusicVol(value);
        SetMusicVolText(value);
        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save();
    }

    protected void SetMusicVol(float value)
    {
        AudioWizard.Instance.musicVolume = value;
    }

    protected void SetMusicVolText(float value)
    {
        musicVolText.SetText($"{Mathf.RoundToInt(value * 100)}%");
    }

    void ResetMusic()
    {
        OnMusicVolChanged(musicVolBase);
        musicVolSlider.value = musicVolBase;
        musicVolSlider.SetValueWithoutNotify(musicVolBase);
        SetMusicVol(musicVolBase);
        SetMusicVolText(musicVolBase);
    }

    protected void InitAmbientSlider()
    {
        float saved = PlayerPrefs.GetFloat("AmbientVolume", ambientBase);
        ambientVolSlider.value = saved;
        SetAmbientVol(saved);
        SetAmbientVolText(saved);
        ambientVolSlider.onValueChanged.AddListener(OnAmbientVolChanged);
    }
    protected void OnAmbientVolChanged(float value)
    {
        SetAmbientVol(value);
        SetAmbientVolText(value);
        PlayerPrefs.SetFloat("AmbientVolume", value);
        PlayerPrefs.Save();
    }

    protected void SetAmbientVol(float value)
    {
        AudioWizard.Instance.ambienceVolume = value;
    }

    protected void SetAmbientVolText(float value)
    {
        ambientVolText.SetText($"{Mathf.RoundToInt(value * 100)}%");
    }

    void ResetAmbient()
    {
        OnAmbientVolChanged(ambientBase);
        ambientVolSlider.value = ambientBase;
        ambientVolSlider.SetValueWithoutNotify(ambientBase);
        SetAmbientVol(ambientBase);
        SetAmbientVolText(ambientBase);
    }
    protected void InitFocusMuteToggle()
    {
        focusMuteToggle.onValueChanged.AddListener(SetFocusMuteState);
        bool value = PlayerPrefs.GetInt("FocusMute", focusBase ? 1 : 0) == 1;
        focusMuteToggle.isOn = value;
        focusMuteToggle.SetIsOnWithoutNotify(focusBase);
        SetFocusMuteState(value);
    }

    protected void SetFocusMuteState(bool value)
    {
        focusMuteEnabled = value;
        PlayerPrefs.SetInt("FocusMute", value ? 1 : 0);
        PlayerPrefs.Save();

    }

    private bool focusMuteEnabled;

    private void OnApplicationFocus(bool hasFocus)
    {
        if (!focusMuteEnabled) return;

        AudioListener.pause = !hasFocus;
    }

    void ResetFocus()
    {
        SetFocusMuteState(focusBase);
        focusMuteToggle.isOn = focusBase;
        focusMuteToggle.SetIsOnWithoutNotify(focusBase);
    }

    protected void InitOutputDropdown()
    {
        outputDropdown.ClearOptions();
        FMODUnity.RuntimeManager.CoreSystem.getNumDrivers(out int numDrivers);

        List<string> devices = new();

        for (int i = 0; i < numDrivers; i++)
        {
            FMODUnity.RuntimeManager.CoreSystem.getDriverInfo(i, out string name, 256, out _, out _, out _, out _);
            devices.Add(name);
        }

        outputDropdown.AddOptions(devices);

        int saved = PlayerPrefs.GetInt("AudioDriver", outputBase);
        outputDropdown.value = saved;

        SetOutputSource(saved);
        outputDropdown.RefreshShownValue();
        outputDropdown.onValueChanged.AddListener(OnOutputChanged);
    }

    protected void OnOutputChanged(int index)
    {
        SetOutputSource(index);
        outputDropdown.RefreshShownValue();
    }

    protected void SetOutputSource(int index)
    {
        FMODUnity.RuntimeManager.CoreSystem.setDriver(index);

        PlayerPrefs.SetInt("AudioDriver", index);
        PlayerPrefs.Save();
    }

    void ResetOutput()
    {
        outputDropdown.value = outputBase;
        OnOutputChanged(outputBase);
    }
}
