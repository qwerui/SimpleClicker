using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "SoundVolumeModel", menuName = "Model/SoundVolumeModel")]
public class SoundVolumeModel : ScriptableObject
{
    public AudioMixer mixer;

    public Action OnChangeBGMVolume;
    public Action OnChangeSFXVolume;

    public float BGMVolume
    {
        set
        {
            PlayerPrefs.SetFloat("BGM", value);
            mixer.SetFloat("BGM", value);
            OnChangeBGMVolume?.Invoke();
        }
        get
        {
            mixer.GetFloat("BGM", out float value);
            return value;
        }
    }
    public float SFXVolume
    {
        set
        {
            PlayerPrefs.SetFloat("SFX", value);
            mixer.SetFloat("SFX", value);
            OnChangeSFXVolume?.Invoke();
        }
        get
        {
            mixer.GetFloat("SFX", out float value);
            return value;
        }
    }
}
