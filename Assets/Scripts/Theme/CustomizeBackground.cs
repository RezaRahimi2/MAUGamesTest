using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class CustomizeBackground
{
    public BackgroundsEnum Type;
    public string SpritePath;
    public Sprite Thumbnail;

    public async Task<Sprite> LoadSprite()
    {
        var request = Resources.LoadAsync<Sprite>(SpritePath);
        await TaskUtils.WaitUntil(() => !request.isDone);
        return request.asset as Sprite;
    }
}