using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Music Settings")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip[] musicClips = new AudioClip[4];
    [SerializeField] [Range(0f, 1f)] private float musicVolume = 0.7f;

    [Header("SFX Settings")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip[] sfxClips = new AudioClip[5];
    [SerializeField] [Range(0f, 1f)] private float sfxVolume = 1f;
    [SerializeField] private int maxSFXSources = 5; // Aynı anda çalabilecek SFX sayısı

    private List<AudioSource> sfxSourcePool = new List<AudioSource>();
    private int currentMusicIndex = 0;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudioSources();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeAudioSources()
    {
        // Music source oluştur
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
        }
        musicSource.loop = true;
        musicSource.volume = musicVolume;
        musicSource.playOnAwake = false;

        // SFX source oluştur
        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
        }
        sfxSource.loop = false;
        sfxSource.volume = sfxVolume;
        sfxSource.playOnAwake = false;

        // SFX source pool oluştur (aynı anda birden fazla SFX çalabilmesi için)
        for (int i = 0; i < maxSFXSources; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.loop = false;
            source.volume = sfxVolume;
            source.playOnAwake = false;
            sfxSourcePool.Add(source);
        }
    }

    #region Music Methods

    /// <summary>
    /// Belirtilen index'teki müziği çalar
    /// </summary>
    public void PlayMusic(int musicIndex)
    {
        if (musicIndex < 0 || musicIndex >= musicClips.Length || musicClips[musicIndex] == null)
        {
            Debug.LogWarning($"Geçersiz müzik index: {musicIndex}");
            return;
        }

        currentMusicIndex = musicIndex;
        musicSource.clip = musicClips[musicIndex];
        musicSource.Play();
    }

    /// <summary>
    /// İlk müziği çalar
    /// </summary>
    public void PlayMusic()
    {
        PlayMusic(0);
    }

    /// <summary>
    /// Müziği durdurur
    /// </summary>
    public void StopMusic()
    {
        musicSource.Stop();
    }

    /// <summary>
    /// Müziği duraklatır
    /// </summary>
    public void PauseMusic()
    {
        musicSource.Pause();
    }

    /// <summary>
    /// Duraklatılmış müziği devam ettirir
    /// </summary>
    public void ResumeMusic()
    {
        musicSource.UnPause();
    }

    /// <summary>
    /// Müzik sesini ayarlar (0-1 arası)
    /// </summary>
    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        musicSource.volume = musicVolume;
    }

    /// <summary>
    /// Müziği fade ile çalar
    /// </summary>
    public void PlayMusicWithFade(int musicIndex, float fadeDuration = 1f)
    {
        StartCoroutine(FadeMusic(musicIndex, fadeDuration));
    }

    private IEnumerator FadeMusic(int newMusicIndex, float duration)
    {
        // Fade out
        float startVolume = musicSource.volume;
        for (float t = 0; t < duration / 2; t += Time.deltaTime)
        {
            musicSource.volume = Mathf.Lerp(startVolume, 0, t / (duration / 2));
            yield return null;
        }

        // Müziği değiştir
        PlayMusic(newMusicIndex);

        // Fade in
        for (float t = 0; t < duration / 2; t += Time.deltaTime)
        {
            musicSource.volume = Mathf.Lerp(0, musicVolume, t / (duration / 2));
            yield return null;
        }

        musicSource.volume = musicVolume;
    }

    #endregion

    #region SFX Methods

    /// <summary>
    /// Belirtilen index'teki ses efektini çalar
    /// </summary>
    public void PlaySFX(int sfxIndex)
    {
        if (sfxIndex < 0 || sfxIndex >= sfxClips.Length || sfxClips[sfxIndex] == null)
        {
            Debug.LogWarning($"Geçersiz SFX index: {sfxIndex}");
            return;
        }

        // Boş bir AudioSource bul ve kullan
        AudioSource availableSource = GetAvailableSFXSource();
        if (availableSource != null)
        {
            availableSource.PlayOneShot(sfxClips[sfxIndex], sfxVolume);
        }
        else
        {
            // Tüm source'lar doluysa varsayılan source'u kullan
            sfxSource.PlayOneShot(sfxClips[sfxIndex], sfxVolume);
        }
    }

    /// <summary>
    /// Belirtilen ses efektini özel volume ile çalar
    /// </summary>
    public void PlaySFX(int sfxIndex, float volumeScale)
    {
        if (sfxIndex < 0 || sfxIndex >= sfxClips.Length || sfxClips[sfxIndex] == null)
        {
            Debug.LogWarning($"Geçersiz SFX index: {sfxIndex}");
            return;
        }

        AudioSource availableSource = GetAvailableSFXSource();
        if (availableSource != null)
        {
            availableSource.PlayOneShot(sfxClips[sfxIndex], sfxVolume * volumeScale);
        }
        else
        {
            sfxSource.PlayOneShot(sfxClips[sfxIndex], sfxVolume * volumeScale);
        }
    }

    /// <summary>
    /// SFX sesini ayarlar (0-1 arası)
    /// </summary>
    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        sfxSource.volume = sfxVolume;
        foreach (var source in sfxSourcePool)
        {
            source.volume = sfxVolume;
        }
    }

    /// <summary>
    /// Tüm SFX'leri durdurur
    /// </summary>
    public void StopAllSFX()
    {
        sfxSource.Stop();
        foreach (var source in sfxSourcePool)
        {
            source.Stop();
        }
    }

    private AudioSource GetAvailableSFXSource()
    {
        foreach (var source in sfxSourcePool)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }
        return null;
    }

    #endregion

    #region Utility Methods

    /// <summary>
    /// Tüm sesleri kapatır/açar
    /// </summary>
    public void ToggleMute()
    {
        AudioListener.volume = AudioListener.volume > 0 ? 0 : 1;
    }

    /// <summary>
    /// Genel ses seviyesini ayarlar
    /// </summary>
    public void SetMasterVolume(float volume)
    {
        AudioListener.volume = Mathf.Clamp01(volume);
    }

    /// <summary>
    /// Şu an çalan müziğin index'ini döndürür
    /// </summary>
    public int GetCurrentMusicIndex()
    {
        return currentMusicIndex;
    }

    /// <summary>
    /// Müzik çalıyor mu kontrol eder
    /// </summary>
    public bool IsMusicPlaying()
    {
        return musicSource.isPlaying;
    }

    #endregion
}