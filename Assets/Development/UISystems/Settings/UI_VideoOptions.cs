
using System.Linq;
using TMPro;

using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI_VideoOptions : UI_SettingsMenu
{
    [Header("Video")]
    [SerializeField] private TMP_Dropdown resolDropdown;
    [SerializeField] private Resolution[] resolutions;
    [SerializeField] private TMP_Dropdown fsDropDown;
    [SerializeField] private TMP_Dropdown qualityDropdown;
    [SerializeField] private string[] qualityLevels;
    [SerializeField] private TMP_Dropdown AADropdown;
    [SerializeField] private TMP_Dropdown frameLimitDropdown;
    [SerializeField] private TMP_Dropdown textureQualDropdown;
    [SerializeField] private TMP_Dropdown shadowQualDropdown;
    [SerializeField] private Slider renderDisSlider;
    [SerializeField] private TextMeshProUGUI renderDisText;
    [SerializeField] private const float baseRendDist = 1000f;
    [SerializeField] private const float minRendDist = 800f;
    [SerializeField] private const float maxRendDist = 1200f;
    [SerializeField] private Slider camSensSlider;
    [SerializeField] private TextMeshProUGUI camSensText;
    [SerializeField] private const float baseCamSens = 15f;
    [SerializeField] private const float minCamSens = 10f;
    [SerializeField] private const float maxCamSens = 20f;
    [SerializeField] private Slider fovSlider;
    [SerializeField] private TextMeshProUGUI fovText;
    [SerializeField] private const float baseFOV = 81f;
    [SerializeField] private const float minFOV = 60f;
    [SerializeField] private const float maxFOV = 100f;
    [SerializeField] private Toggle vsyncToggle;
    [SerializeField] private Toggle invertCamToggle;
    [SerializeField] private bool isFullScreen;
    [SerializeField] private Camera mainCameraRef;
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    void Start()
    {
        var cams = FindObjectsByType<Camera>(FindObjectsSortMode.None);
        foreach (var cam in cams)
        {
            if (cam.CompareTag("MainCamera"))
            {
                mainCameraRef = cam;
            }
        }

        //Init Menus
        InitResolutionDD();
        InitFullscreenDD();
        InitQualityDD();
        InitAAQualityDD();
        InitFrameLimitDD();
        InitTextureQualityDD();
        InitShadowQualityDD();
        InitRenderSlider();
        InitCamSensSlider();
        InitFOVSlider();
        InitInvertToggle();
        InitVsyncToggle();

        LoadSettings();
    }

    protected void LoadSettings()
    {
       // loads settings if found in PlayerPrefs
        if (PlayerPrefs.HasKey("RenderDistance"))
            SetRenderDistance(PlayerPrefs.GetFloat("RenderDistance"));
        if (PlayerPrefs.HasKey("FOV"))
            SetCameraFOV(PlayerPrefs.GetFloat("FOV"));
        if (PlayerPrefs.HasKey("CameraSensitivity"))
            SetCamSensitivity(PlayerPrefs.GetFloat("CameraSensitivity"));
        if (PlayerPrefs.HasKey("Vsync"))
            SetVsyncState(PlayerPrefs.GetInt("Vsync") == 1);
        if (PlayerPrefs.HasKey("AAQuality"))
            SetAAQuality(PlayerPrefs.GetInt("AAQuality"));
        if (PlayerPrefs.HasKey("ResolutionIndex"))
            SetResolution(PlayerPrefs.GetInt("ResolutionIndex"), true);
        if (PlayerPrefs.HasKey("FullscreenMode"))
            OnFullScreenChanged(PlayerPrefs.GetInt("FullscreenMode"));
        if (PlayerPrefs.HasKey("QualityLevel"))
            SetQuality(PlayerPrefs.GetInt("QualityLevel"));
        if (PlayerPrefs.HasKey("TextureQuality"))
            SetTextureQuality(PlayerPrefs.GetInt("TextureQuality"));
        if (PlayerPrefs.HasKey("ShadowQuality"))
            SetShadowQuality(PlayerPrefs.GetInt("ShadowQuality"));

    }

    protected void InitResolutionDD()
    {
        resolDropdown.onValueChanged.RemoveAllListeners();
        resolutions = Screen.resolutions;
        var options = new List<string>();

        int savedIndex = PlayerPrefs.GetInt("ResolutionIndex", -1);
        int currentIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            if (savedIndex == -1 &&
                resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentIndex = i;
            }
            else if (i == savedIndex)
            {
                currentIndex = i;
            }
        }

        resolDropdown.ClearOptions();
        resolDropdown.AddOptions(options);
        resolDropdown.SetValueWithoutNotify(currentIndex);
        resolDropdown.RefreshShownValue();
        SetResolution(currentIndex, isFullScreen);
        resolDropdown.onValueChanged.AddListener(OnResolutionChanged);

    }
    protected void OnResolutionChanged(int index)
    {
       SetResolution(index, isFullScreen);
    }

    protected void SetResolution(int index, bool fullscreen)
    {
        var res = resolutions[index];
        Screen.SetResolution(res.width, res.height,fullscreen);
        PlayerPrefs.SetInt("ResolutionIndex", index);
        PlayerPrefs.Save();
        Debug.Log(res.ToString());
        
    }

    protected void InitFullscreenDD()
    {
        fsDropDown.onValueChanged.RemoveAllListeners();
        fsDropDown.ClearOptions();
        var saved = PlayerPrefs.GetInt("FullscreenMode",-1);
        var modes = new List<string>
    {
        "Windowed",
        "Exclusive Fullscreen",
        "Maximized Window",
        "Fullscreen Windowed"
    };

        fsDropDown.AddOptions(modes);
        if (saved != -1) { fsDropDown.SetValueWithoutNotify(saved); }
        else
        {
            int currentMode = (int)Screen.fullScreenMode;
            fsDropDown.SetValueWithoutNotify(currentMode);
        }
        fsDropDown.RefreshShownValue();

        fsDropDown.onValueChanged.AddListener(OnFullScreenChanged);

    }

    protected void OnFullScreenChanged(int index) 
    {
        switch (index)
        {
            case 0: Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen; break;
            case 1: Screen.fullScreenMode = FullScreenMode.FullScreenWindow; break;
            case 2: Screen.fullScreenMode = FullScreenMode.Windowed; break;
            case 3: Screen.fullScreenMode = FullScreenMode.MaximizedWindow; break;
        }

        isFullScreen = Screen.fullScreenMode == FullScreenMode.ExclusiveFullScreen ||
               Screen.fullScreenMode == FullScreenMode.FullScreenWindow;

        PlayerPrefs.SetInt("FullscreenMode",index);
        PlayerPrefs.Save();
        Debug.Log("FSChanged!");
    }

    protected void InitQualityDD()
    {
        qualityDropdown.onValueChanged.RemoveAllListeners();
        qualityDropdown.ClearOptions();
        var saved = PlayerPrefs.GetInt("QualityLevel", -1);
        var qualityNames = QualitySettings.names.ToList();
        qualityDropdown.AddOptions(qualityNames);
        int index;
        if (saved != -1)
        {
            index = saved;
        }
        else
        {
            index = QualitySettings.GetQualityLevel();
        }
        qualityDropdown.SetValueWithoutNotify(index);
        qualityDropdown.RefreshShownValue();
        qualityDropdown.onValueChanged.AddListener(OnQualityChanged);
    }

    protected void OnQualityChanged(int index)
    {
        SetQuality(index);
    }

    protected void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index, true);
        PlayerPrefs.SetInt("QualityLevel", index);
        PlayerPrefs.Save();
        Debug.Log("QualitySet!");
    }

    protected void InitAAQualityDD()
    {
        AADropdown.onValueChanged.RemoveAllListeners();
        AADropdown.ClearOptions();
        AADropdown.AddOptions(new List<string> {"None", "FXAA","SMAA" });
        var camData = mainCameraRef.GetComponent<UniversalAdditionalCameraData>();
        var saved = PlayerPrefs.GetInt("AAQuality", -1);
        int index = 0;
        if (saved != -1)
        {
            index = saved;
        }
        else
        {
            index = (int)camData.antialiasing;
        }
        AADropdown.SetValueWithoutNotify(index);
        AADropdown.RefreshShownValue();
        AADropdown.onValueChanged.AddListener(OnAAChanged);
    }
    protected void OnAAChanged(int index)
    {
        SetAAQuality(index);
    }

    protected void SetAAQuality(int index)
    {
        var camData = mainCameraRef.GetComponent<UniversalAdditionalCameraData>();
        if (camData == null) return;
        switch (index)
        {
            case 0: camData.antialiasing = AntialiasingMode.None; break;
            case 1: camData.antialiasing = AntialiasingMode.FastApproximateAntialiasing; break;
            case 2: camData.antialiasing = AntialiasingMode.SubpixelMorphologicalAntiAliasing; break;
        }
        PlayerPrefs.SetInt("AAQuality", index);
        PlayerPrefs.Save();
        Debug.Log("AASet!");
    }

    protected void InitFrameLimitDD()
    {
        frameLimitDropdown.onValueChanged.RemoveAllListeners();
        frameLimitDropdown.ClearOptions();
        var options = new List<string>()
        {
            "30FPS",
            "60FPS",
            "120FPS",
            "Unlimited"
        };
        frameLimitDropdown.AddOptions(new List<string> {"30FPS", "60FPS", "120FPS","Unlimited" });
        var saved = PlayerPrefs.GetInt("FrameLimit", -1);
        int index = 0;
        if(saved != -1)
        {
            index = saved;
        }
        else
        {
            switch (Application.targetFrameRate)
            {
                case 30: index = 0; break;
                case 60: index = 1; break;
                case 120: index = 2; break;
                default: index = 3; break;
            }
        }
        frameLimitDropdown.SetValueWithoutNotify(index);
        frameLimitDropdown.RefreshShownValue();
        frameLimitDropdown.onValueChanged.AddListener(OnFrameLimitChanged);
    }

    protected void OnFrameLimitChanged(int index)
    {
        ApplyFrameLimit(index);
    }

    protected void ApplyFrameLimit(int rate)
    {
        int targetRate = 60;
        switch (rate)
        {
            case 0: targetRate = 30; break;
            case 1: targetRate = 60; break;
            case 2: targetRate = 90; break;
            case 3: targetRate = 120; break;
            case 4: targetRate = -1; break;
        }
         SetFrameLimit(targetRate);
    }

    protected void SetFrameLimit(int rate)
    {
        if (rate < 0)
        {
            Application.targetFrameRate = -1;
            QualitySettings.vSyncCount = 0;
        }
        else
        {
            Application.targetFrameRate = rate;
            QualitySettings.vSyncCount = 0;
        }
        PlayerPrefs.SetInt("FrameLimit", rate);
        PlayerPrefs.Save();
        Debug.Log("FrameLimitSet!");
    }

    protected void InitTextureQualityDD()
    {
        textureQualDropdown.onValueChanged.RemoveAllListeners();
        textureQualDropdown.ClearOptions();
        var options = new List<string>()
        {
            "Full Resolution",
            "Half Resolution",
            "Quarter Resolution",
            "Eighth Resolution"
        };
        textureQualDropdown.AddOptions(options);
        int saved = PlayerPrefs.GetInt("TextureQuality", -1);
        int index = 0;
        if (saved != -1)
        {
            index = saved;
        }
        else
        {
            index = textureQualDropdown.value;
        }
        textureQualDropdown.SetValueWithoutNotify(index);
        textureQualDropdown.RefreshShownValue();
        textureQualDropdown.onValueChanged.AddListener(OnTextureQualityChanged);
    }

    protected void OnTextureQualityChanged(int index)
    {
        SetTextureQuality(index);
    }

    protected void SetTextureQuality(int index)
    {
        QualitySettings.globalTextureMipmapLimit = index;
        PlayerPrefs.SetInt("TextureQuality", index);
        PlayerPrefs.Save();
        Debug.Log("TextureQualitySet!");
    }

    protected void InitShadowQualityDD()
    {
        shadowQualDropdown.onValueChanged.RemoveAllListeners();
        shadowQualDropdown.ClearOptions();
        var options = new List<string>()
        {
            "High",
            "Medium",
            "Low"
        };
        shadowQualDropdown.AddOptions(options);
        int saved = PlayerPrefs.GetInt("ShadowQuality", -1);
        int index = 0;
        if (saved != -1)
        {
            index = saved;
        }
        else
        {
            index= GetShadowQualityIndex();
        }
        shadowQualDropdown.SetValueWithoutNotify(index);
        shadowQualDropdown.RefreshShownValue();
        shadowQualDropdown.onValueChanged.AddListener(OnShadowQualityChanged);
    }

    protected void OnShadowQualityChanged(int index)
    {
        SetShadowQuality(index);
    }

    protected void SetShadowQuality(int index)
    {
        switch (index)
        {
            case 0: QualitySettings.shadowResolution = UnityEngine.ShadowResolution.Low; break;
            case 1: QualitySettings.shadowResolution= UnityEngine.ShadowResolution.Medium; break;
            case 2: QualitySettings.shadowResolution = UnityEngine.ShadowResolution.High; break;
            case 3: QualitySettings.shadowResolution = UnityEngine.ShadowResolution.VeryHigh; break;
        }
        PlayerPrefs.SetInt("ShadowQuality", index);
        PlayerPrefs.Save();
        Debug.Log("ShadowQualitySet!");
    }

    private int GetShadowQualityIndex()
    {
        switch (QualitySettings.shadowResolution)
        {
            case UnityEngine.ShadowResolution.Low: return 0;
            case UnityEngine.ShadowResolution.Medium: return 1;
            case UnityEngine.ShadowResolution.High: return 2;
            case UnityEngine.ShadowResolution.VeryHigh: return 3;
            default: return 2;
        }
    }
    //Clamp sliders and adjust since used 0-1 range
    protected void InitRenderSlider()
    {
        renderDisSlider.onValueChanged.RemoveAllListeners();
        renderDisSlider.onValueChanged.AddListener(OnRenderDistanceChanged);
    }
    protected void OnRenderDistanceChanged(float value)
    {
        var distance = Mathf.Lerp(minRendDist, maxRendDist, value);
        SetRenderDistance(distance);
        SetRenderDistanceText(distance);
    }

    protected void SetRenderDistance(float value)
    {
        if(mainCameraRef != null)
        {
            mainCameraRef.farClipPlane  = value;
        }
        PlayerPrefs.SetFloat("RenderDistance", value);
        PlayerPrefs.Save();
        Debug.Log("RenderDistanceSet!");
    }

    protected void SetRenderDistanceText(float value)
    {
        renderDisText.SetText(Mathf.RoundToInt(value).ToString());
    }
    //Clamp sliders and adjust since used 0-1 range
    protected void InitCamSensSlider()
    {
        camSensSlider.onValueChanged.RemoveAllListeners();
        camSensSlider.onValueChanged.AddListener(OnCamSensitivityChanged);
    }

    protected void OnCamSensitivityChanged(float value)
    {
        var camSens = Mathf.Lerp(minCamSens,maxCamSens, value);
        SetCamSensitivity(camSens);
        SetCamSensitivityText(camSens);
    }

    protected void SetCamSensitivity(float value)
    {
        if(mainCameraRef != null)
        {
            //Need to link this to player camera controller
        }
    }

    protected void SetCamSensitivityText(float value)
    {
        camSensText.SetText(Mathf.RoundToInt(value).ToString());
    }
    //Clamp sliders and adjust since used 0-1 range
    protected void InitFOVSlider()
    {
        fovSlider.onValueChanged.RemoveAllListeners();
        fovSlider.onValueChanged.AddListener(OnFOVChanged);
    }

    protected void OnFOVChanged(float value)
    {
        float mappedFOV = Mathf.Lerp(minFOV, maxFOV, value);
        SetCameraFOV(mappedFOV);
        SetCameraFOVText(mappedFOV);
    }

    protected void SetCameraFOV(float value)
    {
        if(mainCameraRef != null)
        {
            mainCameraRef.fieldOfView = value;
        }
        PlayerPrefs.SetFloat("FOV", value);
        PlayerPrefs.Save();
        Debug.Log("CamFOVSet!");
    }

    protected void SetCameraFOVText(float value)
    {
        fovText.SetText(Mathf.RoundToInt(value).ToString());
    }

    protected void InitVsyncToggle()
    {
        vsyncToggle.onValueChanged.RemoveAllListeners();
        vsyncToggle.onValueChanged.AddListener(OnVsyncToggled);
    }

    protected void OnVsyncToggled(bool value)
    {
        SetVsyncState(value);
    }

    protected void SetVsyncState(bool value)
    {
        QualitySettings.vSyncCount = value ? 1 : 0;
        PlayerPrefs.SetInt("Vsync",value ? 1 : 0);
        PlayerPrefs.Save();
        Debug.Log("VsyncStateSet!");
    }

    protected void InitInvertToggle()
    {
        invertCamToggle.onValueChanged.RemoveAllListeners();
        invertCamToggle.onValueChanged.AddListener(OnInvertCamToggled);
    }

    protected void OnInvertCamToggled(bool value)
    {
        SetInvertCamState(value);
    }

    protected void SetInvertCamState(bool value)
    {
        //need to use player cam controller
    }
}
