using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TextDB", menuName = "ScriptableObjects/TextDB", order = 1)]
public class TextResourceDB : ResourceDatabase<string>
{
    public override string GetItem(int id)
    {
        var retVal = base.GetItem(id);

        if (retVal == string.Empty)
        {
            DebugLog.LogError(name + " DataBase Wrong ID : " + id);
        }
        return retVal;
    }
}