using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SoundManager : MonoBehaviourSingleton<SoundManager>
{
    [SerializeField] AudioClipResourceDB audioClipDB;

    Transform root;

    ObjectPool<AudioSource> sfxSourcePool;
    ObjectPool<AudioSource> bgmSourcePool;

    List<AudioSource> activeSfxSourceList = new List<AudioSource>();
    List<AudioSource> activeBgmSourceList = new List<AudioSource>();

    protected override void Awake()
    {
        base.Awake();
        //Warm Pool;
        root = transform;
        sfxSourcePool = new ObjectPool<AudioSource>(CreateAudioSource, null, 20);
        bgmSourcePool = new ObjectPool<AudioSource>(CreateAudioSource, null, 5);
    }

    AudioSource CreateAudioSource()
    {
        GameObject newObject = new GameObject();
        newObject.transform.parent = root;
        newObject.transform.name = "AudioSource";
        return newObject.AddComponent<AudioSource>();
    }

    AudioClip GetClip(int id)
    {
        if (ReferenceEquals(audioClipDB, null))
        {
            audioClipDB = Resources.Load<AudioClipResourceDB>("ResourceDB/AudioClipDB");
            if (audioClipDB == null)
            {
                DebugLog.LogError("Audio DB is NULL");
                return null;
            }
        }

        return audioClipDB.GetItem(id);
    }

    bool PlaySfx(int id, float volume, out AudioSource source)
    {
        var audioClip = GetClip(id);
        source = sfxSourcePool.GetItem();
        if (ReferenceEquals(audioClip, null))
        {
            source.clip = audioClip;
            source.volume = volume;
            source.Play();
            activeSfxSourceList.Add(source);
            return true;
        }

        return false;
    }

    void ReleaseSource(AudioSource source, List<AudioSource> sourceList, ObjectPool<AudioSource> pool)
    {
        if (source == null)
        {
            return;
        }

        source.clip = null;
        source.Stop();
        sourceList.Remove(source);
        pool.ReleaseItem(source);
    }

    void ReleaseSfxSource(AudioSource source)
    {
        ReleaseSource(source, activeSfxSourceList, sfxSourcePool);
    }

    void ReleaseBgmSource(AudioSource source)
    {
        ReleaseSource(source, activeBgmSourceList, bgmSourcePool);
    }

    public AudioSource PlaySfxOneShot(int id, float volume = 1f)
    {
        if (PlaySfx(id, volume, out AudioSource source))
        {
            source.loop = false;
            StartCoroutine(PlaySfxOneShotCoroutine(source));
            return source;
        }

        return null;
    }

    public AudioSource PlaySfxTimer(int id, float timer, float volume = 1f)
    {
        if (PlaySfx(id, volume, out AudioSource source))
        {
            source.loop = true;
            StartCoroutine(PlaySfxTimerCoroutine(source, timer));
            return source;
        }

        return null;
    }

    IEnumerator PlaySfxOneShotCoroutine(AudioSource source)
    {
        while (source.isPlaying)
        {
            yield return null;
        }

        ReleaseSfxSource(source);
    }

    IEnumerator PlaySfxTimerCoroutine(AudioSource source, float timer)
    {
        while (source.isPlaying && timer > 0f)
        {
            yield return null;
            timer -= Time.deltaTime;
        }

        ReleaseSfxSource(source);
    }

    AudioSource FindActiveBgmSource(int id)
    {
        var clip = GetClip(id);
        return FindActiveBgmSource(clip);
    }

    AudioSource FindActiveBgmSource(AudioClip clip)
    {
        if (clip == null)
        {
            return null;
        }

        for (int i = 0; i < activeBgmSourceList.Count; i++)
        {
            if (activeBgmSourceList[i].clip == clip)
            {
                return activeBgmSourceList[i];
            }
        }

        return null;
    }

    public void PlayBgm(int id, float volume = 1f, float fadeTime = -1f)
    {
        AudioClip clip = GetClip(id);
        if (ReferenceEquals(clip, null))
        {
            return;
        }

        var source = FindActiveBgmSource(clip);
        if (ReferenceEquals(source, null))
        {
            source = bgmSourcePool.GetItem();
        }

        source.clip = clip;
        source.volume = volume;
        source.loop = true;
        activeBgmSourceList.Add(source);
        if (source.isPlaying == false)
        {
            source.Play();
            if (fadeTime > 0f)
            {
                source.volume = 0f;
                source.DOFade(volume, fadeTime);
            }
        }
    }

    public void StopBgm(int id, float fadeTime = -1f)
    {
        var source = FindActiveBgmSource(id);
        StopBgm(source, fadeTime);
    }

    public void StopBgm(AudioSource source, float fadeTime = -1f)
    {
        if (!ReferenceEquals(source, null))
        {
            StopSource(source, ReleaseBgmSource, fadeTime);
        }
    }

    public void StopSfx(AudioSource source, float fadeTime = -1f)
    {
        if (!ReferenceEquals(source, null))
        {
            StopSource(source, ReleaseSfxSource, fadeTime);
        }
    }

    void StopSource(AudioSource source, UnityEngine.Events.UnityAction<AudioSource> releaseAction, float fadeTime = -1f)
    {
        source.DOKill();

        if (fadeTime > 0f)
        {
            source.DOFade(0f, fadeTime).onComplete = () => { releaseAction(source); };
        }
        else
        {
            releaseAction(source);
        }
    }

    public void StopSoundAll(float fadeTime = -1f)
    {
        StopSfxAll(fadeTime);
        StopBgmAll(fadeTime);
    }

    public void StopSfxAll(float fadeTime = -1f)
    {
        for (int i = activeSfxSourceList.Count - 1; i >= 0; i--)
        {
            StopSfx(activeSfxSourceList[i], fadeTime);
        }
    }

    public void StopBgmAll(float fadeTime = -1f)
    {
        for (int i = activeBgmSourceList.Count - 1; i >= 0; i--)
        {
            StopBgm(activeBgmSourceList[i], fadeTime);
        }
    }

    public void ChangeBgm(int to, float volume = 1f, float fadeTime = -1f)
    {
        StopBgmAll(fadeTime);
        PlayBgm(to, volume, fadeTime);
    }

    public void ChangeBgm(int from, int to, float volume = 1f, float fadeTime = -1f)
    {
        StopBgm(from, fadeTime);
        PlayBgm(to, volume, fadeTime);
    }
}