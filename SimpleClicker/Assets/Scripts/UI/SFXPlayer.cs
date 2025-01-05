using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
    public AudioClip clip;

    public void Play()
    {
        AudioManager.Instance.PlaySFX(clip);
    }
}
