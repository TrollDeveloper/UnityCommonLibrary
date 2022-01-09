using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextManager : MonoBehaviourSingleton<TextManager>
{
    [SerializeField] TextResourceDB textDB;

    public string GetText(int id)
    {
        if (ReferenceEquals(textDB, null))
        {
            textDB = Resources.Load<TextResourceDB>("ResourceDB/TextDB");
            if (textDB == null)
            {
                DebugLog.LogError("Text DB is NULL");
                return null;
            }
        }

        return textDB.GetItem(id);
    }
}