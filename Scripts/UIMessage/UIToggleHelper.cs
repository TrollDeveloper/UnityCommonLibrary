using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeControl;
using Sirenix.OdinInspector;
using System;
using System.Linq;

[RequireComponent(typeof(Toggle))]
public class UIToggleHelper : SerializedMonoBehaviour
{
    [TypeFilter("GetFilteredTypeList")]
    public BoolMessageBase msg;

    Toggle toggle;

    public IEnumerable<Type> GetFilteredTypeList()
    {
        var q = typeof(BoolMessageBase).Assembly.GetTypes()
            .Where(x => !x.IsAbstract)                                          
            .Where(x => typeof(BoolMessageBase).IsAssignableFrom(x));           

        return q;
    }

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnValueChanged(bool arg)
    {
        msg.val = arg;
        msg.Send();
    }
}
