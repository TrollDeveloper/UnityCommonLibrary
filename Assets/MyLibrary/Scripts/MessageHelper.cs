using System.Collections;
using System.Collections.Generic;
using CodeControl;
using UnityEngine;
using UnityEngine.Events;
using System;

public class MessageHelper : MonoBehaviourSingleton<MessageHelper>
{
    Queue<Action> delayTaskQueue = new Queue<Action>();

    private void Start()
    {
        StartCoroutine(RemoveMessageCoroutine());
    }

    IEnumerator RemoveMessageCoroutine()
    {
        var wait = new WaitForEndOfFrame();
        while (true)
        {
            yield return wait;
            while (delayTaskQueue.Count > 0)
            {
                delayTaskQueue.Dequeue()();
            }
        }
    }

    public void RemoveListenerEndFrameLocal<T>(Action<T> callback) where T : Message
    {
        delayTaskQueue.Enqueue(() => { Message.RemoveListener<T>(callback); });
    }

    public void AddListenerEndFrameLocal<T>(Action<T> callback) where T : Message
    {
        delayTaskQueue.Enqueue(() => { Message.AddListener<T>(callback); });
    }

    public static void RemoveListenerEndFrame<T>(Action<T> callback) where T : Message
    {
        Instance.RemoveListenerEndFrameLocal<T>(callback);
    }

    public static void AddListenerEndFrame<T>(Action<T> callback) where T : Message
    {
        Instance.AddListenerEndFrameLocal<T>(callback);
    }
}