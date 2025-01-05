using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance { get => instance; }

    private AudioSource bgm;
    private AudioSource sfx;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        transform.Find("BGM").TryGetComponent(out bgm);
        transform.Find("SFX").TryGetComponent(out sfx);
    }

    public void PlayBGM(AudioClip bgmClip)
    {
        if(bgm.isPlaying)
        {
            bgm.Stop();
        }

        bgm.clip = bgmClip;
        bgm.Play();
    }

    public void PlaySFX(AudioClip sfxClip)
    {
        sfx.PlayOneShot(sfxClip);
    }
}
