using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeControl;
using Sirenix.OdinInspector;
using System;
using System.Linq;

[RequireComponent(typeof(InputField))]
public class UIInputFieldHelper : SerializedMonoBehaviour
{
    [TypeFilter("GetFilteredTypeList")]
    public StringMessageBase msg;

    InputField input;

    public IEnumerable<Type> GetFilteredTypeList()
    {
        var q = typeof(StringMessageBase).Assembly.GetTypes()
            .Where(x => !x.IsAbstract)                                          
            .Where(x => typeof(StringMessageBase).IsAssignableFrom(x));         

        return q;
    }

    private void Awake()
    {
        input = GetComponent<InputField>();

        input.onEndEdit.AddListener(OnEndEdit);
        GetComponent<Button>().onClick.AddListener(msg.Send);
    }

    private void OnEndEdit(string arg)
    {
        msg.val = input.text;
        msg.Send();
    }
}