using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeControl;
using Sirenix.OdinInspector;
using System;
using System.Linq;
using UnityEngine.EventSystems;

public class UIButtonPressMsgHelper : SerializedMonoBehaviour, IPointerDownHandler
{
    [TypeFilter("GetFilteredTypeList")] public VoidMessageBase msg;

    public IEnumerable<Type> GetFilteredTypeList()
    {
        var q = typeof(VoidMessageBase).Assembly.GetTypes()
            .Where(x => !x.IsAbstract)
            .Where(x => typeof(VoidMessageBase).IsAssignableFrom(x));

        return q;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Raycast Filter 넣어야할수도.
        msg.Send();
    }
}