using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageManager : MonoBehaviourSingleton<ImageManager>
{
    [SerializeField] SpriteResourceDB spriteDB;
    [SerializeField] TextureResourceDB textureDB;

    public Sprite GetSprite(int id)
    {
        if (ReferenceEquals(spriteDB, null))
        {
            spriteDB = Resources.Load<SpriteResourceDB>("ResourceDB/SpriteDB");
            if (spriteDB == null)
            {
                DebugLog.LogError("SpriteResource DB is NULL");
                return null;
            }
        }

        return spriteDB.GetItem(id);
    }

    public Texture2D GetTexture(int id)
    {
        if (ReferenceEquals(textureDB, null))
        {
            textureDB = Resources.Load<TextureResourceDB>("ResourceDB/TextureDB");
            if (textureDB == null)
            {
                DebugLog.LogError("Texture DB is NULL");
                return null;
            }
        }

        return textureDB.GetItem(id);
    }
}