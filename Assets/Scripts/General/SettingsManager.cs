using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public Slider sensitivitySlider;
    public Slider FOWSlider;
    public Slider MusicVolume;
    public Slider SFXVolume;
    public Slider UIVolume;
    public GameObject _camera;

    public void SetSensitivity(float value)
    {
        Dependencies.Instance.GetDependancy<CameraTilt>().mouseSensitivity = value;
        PlayerPrefs.SetFloat("sensitivity", value);
    }

    public void SetFOV(float value)
    {
        _camera.GetComponent<CinemachineCamera>().Lens.FieldOfView = value;
        PlayerPrefs.SetFloat("FOV", value);
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
}
