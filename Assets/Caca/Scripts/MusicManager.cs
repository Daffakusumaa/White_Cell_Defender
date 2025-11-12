using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [SerializeField] private MusicLibrary musicLibrary;
    [SerializeField] private AudioMixerGroup musicMixerGroup;

    private AudioSource musicSource;
    private string currentScene;
    private string currentTrackName = "";

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.outputAudioMixerGroup = musicMixerGroup;
        musicSource.loop = true;

        SceneManager.sceneLoaded += OnSceneLoaded;

        Debug.Log("[MusicManager] Initialized.");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentScene = scene.name;
        Debug.Log("[MusicManager] Scene loaded: " + currentScene);

        string targetTrack = GetTrackForScene(currentScene);

        if (targetTrack == currentTrackName)
        {
            Debug.Log("[MusicManager] Scene sama, musik tetap dilanjutkan.");
            return;
        }

        currentTrackName = targetTrack;
        PlayLoopMusic(targetTrack, 1.0f);
    }

    private string GetTrackForScene(string sceneName)
    {
        switch (sceneName)
        {
            case "MainMenu":
                return "MainMenuMusic";
            case "level 1":
                return "IngameMusic";
            case "level 2":
                return "IngameMusic";
            case "level 3":
                return "IngameMusic";
            default:
                return "DefaultMusic";
        }
    }

    public void PlayLoopMusic(string trackName, float fadeDuration = 1.0f)
    {
        AudioClip clip = musicLibrary.GetClipFromName(trackName);

        if (clip != null)
        {
            StartCoroutine(FadeToNewTrack(clip, fadeDuration));
        }
        else
        {
            Debug.LogWarning($"[MusicManager] Track '{trackName}' tidak ditemukan di MusicLibrary!");
        }
    }

    private IEnumerator FadeToNewTrack(AudioClip newTrack, float fadeDuration)
    {
        float startVolume;
        musicMixerGroup.audioMixer.GetFloat("MusicVolume", out startVolume);
        startVolume = Mathf.Pow(10, startVolume / 20);

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
            musicMixerGroup.audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume + 0.0001f) * 20);
            yield return null;
        }

        musicSource.clip = newTrack;
        musicSource.Play();

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float volume = Mathf.Lerp(0, startVolume, t / fadeDuration);
            musicMixerGroup.audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume + 0.0001f) * 20);
            yield return null;
        }

        Debug.Log($"[MusicManager] Now playing: {newTrack.name}");
    }
}
