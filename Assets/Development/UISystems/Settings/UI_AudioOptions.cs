using FMODUnity;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class UI_AudioOptions : UI_SettingsMenu
{
    [Header("Audio")]
    [SerializeField] private Slider mainVolSlider;
    [SerializeField] private TextMeshProUGUI mainVolText;
    [SerializeField]private Slider sfxVolSlider;
    [SerializeField] private TextMeshProUGUI sfxVolText;
    [SerializeField] private Slider musicVolSlider;
    [SerializeField] private TextMeshProUGUI musicVolText;
    [SerializeField] private Slider ambientVolSlider;
    [SerializeField] private TextMeshProUGUI ambientVolText;
    [SerializeField] private Toggle focusMuteToggle;
    [SerializeField] private TMP_Dropdown outputDropdown;
    // Start is called once before the first execution of Update after the MonoBehaviour is created


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
    }
    protected void InitMainSlider()
    {
        float saved = PlayerPrefs.GetFloat("MasterVolume", 1f);
        mainVolSlider.value = saved;
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

    protected void InitSFXSlider()
    {
        float saved = PlayerPrefs.GetFloat("SFXVolume", 1f);
        sfxVolSlider.value = saved;
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

    protected void InitMusicSlider()
    {
        float saved = PlayerPrefs.GetFloat("MusicVolume", 1f);
        musicVolSlider.value = saved;
        SetMusicVol(saved);
        SetMusicVolText(saved);

        musicVolSlider.onValueChanged.AddListener(OnMusicVolChanged);
    }

    protected void OnMusicVolChanged(float value)
    {
        SetMainVol(value);
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

    protected void InitAmbientSlider()
    {
        float saved = PlayerPrefs.GetFloat("AmbientVolume", 1f);
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
    protected void InitFocusMuteToggle()
    {
        focusMuteToggle.onValueChanged.AddListener(SetFocusMuteState);

        bool value = PlayerPrefs.GetInt("FocusMute", 0) == 1;
        focusMuteToggle.isOn = value;
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


    protected void InitOutputDropdown()
    {
        FMODUnity.RuntimeManager.CoreSystem.getNumDrivers(out int numDrivers);

        List<string> devices = new();

        for (int i = 0; i < numDrivers; i++)
        {
            FMODUnity.RuntimeManager.CoreSystem.getDriverInfo(i, out string name, 256, out _, out _, out _, out _);
            devices.Add(name);
        }

        outputDropdown.ClearOptions();
        outputDropdown.AddOptions(devices);

        int saved = PlayerPrefs.GetInt("AudioDriver", 0);
        outputDropdown.value = saved;

        SetOutputSource(saved);

        outputDropdown.onValueChanged.AddListener(OnOutputChanged);
    }

    protected void OnOutputChanged(int index)
    {
        SetOutputSource(index);
    }

    protected void SetOutputSource(int index)
    {
        FMODUnity.RuntimeManager.CoreSystem.setDriver(index);

        PlayerPrefs.SetInt("AudioDriver", index);
        PlayerPrefs.Save();
    }
}
