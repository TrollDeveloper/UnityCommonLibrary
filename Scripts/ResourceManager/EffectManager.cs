using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviourSingleton<EffectManager>
{
    [SerializeField] PrefabResourceDB effectPrefabDB;

    readonly List<GameObject> activeEffectList = new List<GameObject>();
    readonly ComponentPool<ParticleSystem> particleComponentPool = new ComponentPool<ParticleSystem>();

    protected override void Awake()
    {
        base.Awake();
    }

    public void PlayOneShot(int id, Vector3 position)
    {
        PlayOneShot(id, null, position);
    }

    public void PlayOneShot(int id, Transform parent, Vector3 localPosition)
    {
        PlayEffect(id, parent, localPosition, -1f);
    }

    public void PlayTimer(int id, Vector3 position, float timer)
    {
        PlayTimer(id, null, position, timer);
    }

    public void PlayTimer(int id, Transform parent, Vector3 localPosition, float timer)
    {
        PlayEffect(id, parent, localPosition, timer);
    }

    void PlayEffect(int id, Transform parent, Vector3 localPosition, float timer = -1f)
    {
        var clone = SpawnEffect(id, parent, localPosition);
        if (ReferenceEquals(clone, null))
        {
            return;
        }

        var particle = particleComponentPool.GetComponent(clone);
        if (ReferenceEquals(particle, null))
        {
            //파티클 없는 오브젝트면 GameObjectTimer로.
            DebugLog.LogWarning("Effect Manager Play One Shot Not Particle System - ID : " + id);
            StartCoroutine(PlayGameObjectTimerCoroutine(clone, timer < 0f ? 30f : timer));
        }
        else
        {
            //파티클 있으면 파티클 관련 코루틴실행.
            StartCoroutine(timer < 0f ? PlayOneShotCoroutine(particle) : PlayTimerCoroutine(particle, timer));
        }
    }

    IEnumerator PlayOneShotCoroutine(ParticleSystem particle)
    {
        particle.Play(true);
        while (particle.IsAlive(true))
        {
            yield return null;
        }

        ReleaseEffect(particle.gameObject);
    }

    IEnumerator PlayTimerCoroutine(ParticleSystem particle, float timer)
    {
        particle.Play();
        while (particle.isPlaying && timer > 0f)
        {
            yield return null;
            timer -= Time.deltaTime;
        }

        particle.Stop(true);
        while (particle.IsAlive(true))
        {
            yield return null;
        }

        ReleaseEffect(particle.gameObject);
    }


    IEnumerator PlayGameObjectTimerCoroutine(GameObject obj, float timer = 30f)
    {
        while (obj.activeSelf && timer > 0f)
        {
            yield return null;
            timer -= Time.deltaTime;
        }

        ReleaseEffect(obj);
    }

    public GameObject SpawnEffect(int id, Transform parent, Vector3 localPosition)
    {
        if (ReferenceEquals(effectPrefabDB, null))
        {
            effectPrefabDB = Resources.Load<PrefabResourceDB>("ResourceDB/EffectPrefabDB");
            if (effectPrefabDB == null)
            {
                DebugLog.LogError("Effect Prefab DB is NULL");
                return null;
            }
        }

        GameObject prefab = effectPrefabDB.GetItem(id);
        if (ReferenceEquals(prefab, null))
        {
            DebugLog.LogError("Effect Manager Spawn ID Error - ID : " + id);
            return null;
        }

        var clone = ObjectPoolManager.Instance.SpawnObject(prefab);
        activeEffectList.Add(clone);

        clone.transform.parent = parent;
        clone.transform.localPosition = localPosition;

        return clone;
    }

    public void ReleaseEffect(ParticleSystem particle)
    {
        ReleaseEffect(particle.gameObject);
    }

    public void ReleaseEffect(GameObject clone)
    {
        ObjectPoolManager.Instance.ReleaseObject(clone);
        activeEffectList.Remove(clone);
    }

    public void ReleaseEffectAll()
    {
        for (int i = activeEffectList.Count - 1; i > 0; i--)
        {
            ReleaseEffect(activeEffectList[i]);
        }
    }
}