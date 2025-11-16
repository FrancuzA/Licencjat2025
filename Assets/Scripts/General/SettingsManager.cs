using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Dropdown resolutionDropdown;
    public Slider sensitivitySlider;
    public Slider FOWSlider;
    public Slider MusicVolume;
    public Slider SFXVolume;
    public Slider UIVolume;
    public GameObject _camera;
    private List<Resolution> filteredResolutions;
    private Resolution[] resolutions;
    private float fovMin = 40f;
    private float fovMax = 100f;
    private int currentResolutionIndex = 0;


    private void Start()
    {
        sensitivitySlider.onValueChanged.AddListener(SetSensitivity);
        FOWSlider.onValueChanged.AddListener(SetFOV);
        MusicVolume.onValueChanged.AddListener(SetMusicVolume);
        SFXVolume.onValueChanged.AddListener(SetSFXVolume);
        UIVolume.onValueChanged.AddListener(SetUIVolume);
        StartCoroutine(TryGetDep());
        SetFOV(0f);
        InitializeResolutions();
        LoadResolutionSettings();
    }
    public void SetSensitivity(float value)
    {
        Dependencies.Instance.GetDependancy<CameraTilt>().ChangeSens(value);
        PlayerPrefs.SetFloat("sensitivity", value);
        Debug.Log("sensitivity " + value);
    }

    public void SetFOV(float value)
    {
        float fovValue = Mathf.Lerp(fovMin, fovMax, value);
        _camera.GetComponent<CinemachineCamera>().Lens.FieldOfView = fovValue;
        PlayerPrefs.SetFloat("FOV", value);
        Debug.Log("FOV " + value + " " + fovValue);
    }

    public void SetMusicVolume(float value)
    {
        
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    public void SetSFXVolume(float value)
    {

        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    public void SetUIVolume(float value)
    {

        PlayerPrefs.SetFloat("UIVolume", value);
    }


    void InitializeResolutions()
    {
        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            if (i == resolutions.Length - 1 ||
                resolutions[i].width != resolutions[i + 1].width ||
                resolutions[i].height != resolutions[i + 1].height)
            {
                filteredResolutions.Add(resolutions[i]);
            }
        }

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            string option = $"{filteredResolutions[i].width} x {filteredResolutions[i].height}";
            if (filteredResolutions[i].refreshRateRatio.value != 60)
            {
                option += $" ({filteredResolutions[i].refreshRateRatio.value:0}Hz)";
            }
            options.Add(option);

            if (filteredResolutions[i].width == Screen.currentResolution.width &&
                filteredResolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        
        resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
    }

    public void OnResolutionChanged(int dropdownIndex)
    {
        if (filteredResolutions == null || filteredResolutions.Count == 0) return;

        Resolution selectedResolution = filteredResolutions[dropdownIndex];

        Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreen);

        PlayerPrefs.SetInt("ResolutionWidth", selectedResolution.width);
        PlayerPrefs.SetInt("ResolutionHeight", selectedResolution.height);
        PlayerPrefs.SetInt("ResolutionIndex", dropdownIndex);

        Debug.Log($"Resolution changed to: {selectedResolution.width}x{selectedResolution.height}");
    }

    private void LoadResolutionSettings()
    {
        if (PlayerPrefs.HasKey("ResolutionWidth") && PlayerPrefs.HasKey("ResolutionHeight"))
        {
            int savedWidth = PlayerPrefs.GetInt("ResolutionWidth");
            int savedHeight = PlayerPrefs.GetInt("ResolutionHeight");
            int savedIndex = PlayerPrefs.GetInt("ResolutionIndex", 0);

            if (resolutionDropdown != null && savedIndex < resolutionDropdown.options.Count)
            {
                resolutionDropdown.value = savedIndex;
                resolutionDropdown.RefreshShownValue();
            }
        }
        else
        {
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();
        }
    }

    IEnumerator TryGetDep()
    {
        yield return new WaitUntil(() => Dependencies.Instance.GetDependancy<CameraTilt>() != null);
        SetSensitivity(0.1f);
    }

}

