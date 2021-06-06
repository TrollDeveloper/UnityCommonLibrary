using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeControl;
using Sirenix.OdinInspector;
using System;
using System.Linq;

[RequireComponent(typeof(Button))]
public class UIButtonClickMsgHelper : SerializedMonoBehaviour
{
    [TypeFilter("GetFilteredTypeList")]
    public VoidMessageBase msg;

    public IEnumerable<Type> GetFilteredTypeList()
    {
        var q = typeof(VoidMessageBase).Assembly.GetTypes()
            .Where(x => !x.IsAbstract)                                          
            .Where(x => typeof(VoidMessageBase).IsAssignableFrom(x));                 

        return q;
    }

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClicked);
    }
    void OnClicked()
    {
        msg.Send();
    }
}
