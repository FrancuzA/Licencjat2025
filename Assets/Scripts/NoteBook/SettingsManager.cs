using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;
using FMOD.Studio;

public class SettingsManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Dropdown resolutionDropdown;
    public Slider sensitivitySlider;
    public Slider FOVSlider;
    public Slider MusicVolumeSlider;
    public Slider SFXVolumeSlider;
    public Slider UIVolumeSlider;
    public GameObject _camera;

    [Header("FOV Settings")]
    private const float FOVMin = 60f;
    private const float FOVMax = 110f;

    [Header("FMOD VCA Paths")]
    private const string MusicVCAPath = "vca:/Music";
    private const string SFXVCAPath = "vca:/SFX";
    private const string UIVCAPath = "vca:/UI";

    private VCA _musicVCA;
    private VCA _sfxVCA;
    private VCA _uiVCA;

    private List<Resolution> _filteredResolutions;
    private int _currentResolutionIndex = 0;

    private const float VolumeMultiplier = 2f;
    private const float DefaultVolumeSlider = 0.5f; 

    private void Awake()
    {
        Dependencies.Instance.RegisterDependency<SettingsManager>(this);
    }

    private void Start()
    {
        InitializeFMOD();
        InitializeResolutions();

        
        LoadAllSettings();

        sensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);
        FOVSlider.onValueChanged.AddListener(OnFOVChanged);
        MusicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        SFXVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        UIVolumeSlider.onValueChanged.AddListener(OnUIVolumeChanged);
        resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);

        StartCoroutine(ApplySensitivityWhenReady());
    }

    // ─── FMOD ────────────────────────────────────────────────────────────────

    private void InitializeFMOD()
    {
        _musicVCA = RuntimeManager.GetVCA(MusicVCAPath);
        _sfxVCA = RuntimeManager.GetVCA(SFXVCAPath);
        _uiVCA = RuntimeManager.GetVCA(UIVCAPath);
    }

    private void SetVCAVolume(VCA vca, float sliderValue)
    {
        float volume = sliderValue * VolumeMultiplier;
        vca.setVolume(volume);
    }

    // ─── LISTENERS ───────────────────────────────────────────────────────────

    private void OnSensitivityChanged(float value)
    {
        Dependencies.Instance.GetDependancy<CameraTilt>().ChangeSens(value);
        PlayerPrefs.SetFloat("sensitivity", value);
    }

    private void OnFOVChanged(float value)
    {
        float fov = Mathf.Lerp(FOVMin, FOVMax, value);
        _camera.GetComponent<CinemachineCamera>().Lens.FieldOfView = fov;
        PlayerPrefs.SetFloat("FOV", value);
    }

    private void OnMusicVolumeChanged(float value)
    {
        SetVCAVolume(_musicVCA, value);
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    private void OnSFXVolumeChanged(float value)
    {
        SetVCAVolume(_sfxVCA, value);
        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    private void OnUIVolumeChanged(float value)
    {
        SetVCAVolume(_uiVCA, value);
        PlayerPrefs.SetFloat("UIVolume", value);
    }
    public void HardSetFOV(float lensValue)
    {
        _camera.GetComponent<CinemachineCamera>().Lens.FieldOfView = lensValue;
    }


    // ─── LOAD / SAVE ─────────────────────────────────────────────────────────

    private void LoadAllSettings()
    {
        MusicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", DefaultVolumeSlider);
        SFXVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", DefaultVolumeSlider);
        UIVolumeSlider.value = PlayerPrefs.GetFloat("UIVolume", DefaultVolumeSlider);

        SetVCAVolume(_musicVCA, MusicVolumeSlider.value);
        SetVCAVolume(_sfxVCA, SFXVolumeSlider.value);
        SetVCAVolume(_uiVCA, UIVolumeSlider.value);

        float savedFOV = PlayerPrefs.GetFloat("FOV", 0.6f);
        FOVSlider.value = savedFOV;
        _camera.GetComponent<CinemachineCamera>().Lens.FieldOfView = Mathf.Lerp(FOVMin, FOVMax, savedFOV);
        sensitivitySlider.value = PlayerPrefs.GetFloat("sensitivity", 0.5f);

        LoadResolutionSettings();
    }

    // ─── RESOLUTION ──────────────────────────────────────────────────────────

    private void InitializeResolutions()
    {
        Resolution[] resolutions = Screen.resolutions;
        _filteredResolutions = new List<Resolution>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            bool isLastEntry = i == resolutions.Length - 1;
            bool isDuplicate = !isLastEntry &&
                               resolutions[i].width == resolutions[i + 1].width &&
                               resolutions[i].height == resolutions[i + 1].height;
            if (!isDuplicate)
                _filteredResolutions.Add(resolutions[i]);
        }

        List<string> options = new List<string>();
        for (int i = 0; i < _filteredResolutions.Count; i++)
        {
            Resolution res = _filteredResolutions[i];
            string option = $"{res.width} x {res.height}";
            if (res.refreshRateRatio.value != 60)
                option += $" ({res.refreshRateRatio.value:0}Hz)";
            options.Add(option);

            if (res.width == Screen.currentResolution.width &&
                res.height == Screen.currentResolution.height)
                _currentResolutionIndex = i;
        }

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(options);
    }

    public void OnResolutionChanged(int index)
    {
        if (_filteredResolutions == null || _filteredResolutions.Count == 0) return;

        Resolution res = _filteredResolutions[index];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
        PlayerPrefs.SetInt("ResolutionWidth", res.width);
        PlayerPrefs.SetInt("ResolutionHeight", res.height);
        PlayerPrefs.SetInt("ResolutionIndex", index);
    }

    private void LoadResolutionSettings()
    {
        int savedIndex = PlayerPrefs.GetInt("ResolutionIndex", _currentResolutionIndex);
        savedIndex = Mathf.Clamp(savedIndex, 0, resolutionDropdown.options.Count - 1);
        resolutionDropdown.value = savedIndex;
        resolutionDropdown.RefreshShownValue();
    }

    // ─── COROUTINES ──────────────────────────────────────────────────────────

    private IEnumerator ApplySensitivityWhenReady()
    {
        yield return new WaitUntil(() => Dependencies.Instance.GetDependancy<CameraTilt>() != null);
        OnSensitivityChanged(sensitivitySlider.value);
    }
}