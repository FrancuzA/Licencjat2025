using FMOD.Studio;
using FMODUnity;
using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio References")]
    public EventReference PlayerJump;
    public EventReference PlayerStep;
    public EventReference PlayerPickUp;

    [Header("Audio Parameters")]// 0     1   2
    public string GroundType = "Sand";  // Sand  Dirt Wood   GroundType
    public string JumpPhase = "Jump";   // Jump  Land        JumpLand
    public string PickDrop = "PickUp";   // PickUp Drop      PickDrop

    void Start()
    {
        Dependencies.Instance.RegisterDependency<AudioManager>(this);
    }

    public void PlayStep()
    {
        EventInstance i = CreateInstance(PlayerStep);
        i.setParameterByNameWithLabel("GroundType", GroundType);
        i.start();
        i.release();
    }

    public void PlayJump()
    {
        EventInstance i = CreateInstance(PlayerJump);
        i.setParameterByNameWithLabel("GroundType", GroundType);
        i.setParameterByNameWithLabel("JumpLand", JumpPhase);
        i.start();
        i.release();
    }

    public void PlayPickDrop()
    {
        EventInstance i = CreateInstance(PlayerPickUp);
        i.setParameterByNameWithLabel("PickDrop", PickDrop);
        i.start();
        i.release();
    }


    public EventInstance CreateInstance( EventReference _ref)
    {
         
        return RuntimeManager.CreateInstance(_ref);
    }
}
