using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance; 

    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxSource;

    [Header("Clips")]
    public AudioClip swordAttackClip;
    public AudioClip swordUltiClip;
    public AudioClip enemyDie;

    private void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    
    public void PlayAttackSound()
    {
        PlaySFX(swordAttackClip);
    }

    public void PlayUltiSound()
    {
        PlaySFX(swordUltiClip);
    }
    public void PlayenemyDieSound()
    {
        PlaySFX(enemyDie);
    }
}
