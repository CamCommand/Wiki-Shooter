using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Networking;

public static class WebRequest
{

    private class WebRequestMonoBehavior : MonoBehaviour { }
    private static WebRequestMonoBehavior webRequestMonoBehaviour;


    private static void Init()
    {
        if (webRequestMonoBehaviour == null)
        {
            GameObject GameManager = GameObject.FindGameObjectWithTag("GameController");
            webRequestMonoBehaviour = GameManager.AddComponent<WebRequestMonoBehavior>();
        }
    }
    public static void GetData(string url, Action<string> onError, Action<string> onSuccess)
    {
        Init();
        webRequestMonoBehaviour.StartCoroutine(GetWebData(url, onError, onSuccess));
    }

    private static IEnumerator GetWebData(string url, Action<string> onError, Action<string> onSuccess)
    {
        using (UnityWebRequest getWikiArticle = UnityWebRequest.Get(url))
        {
            yield return getWikiArticle.SendWebRequest();

            //if (getWikiArticle.result == getWikiArticle.result.ProtocolError)
            if (getWikiArticle.isNetworkError || getWikiArticle.isHttpError)
            {
                onError(getWikiArticle.error);
            }
            else
            {
                onSuccess(getWikiArticle.downloadHandler.text);
            }
        }
    }

    public static void GetTexture(string url, Action<string> onError, Action<Texture2D> onSuccess)
    {
        Init();
        webRequestMonoBehaviour.StartCoroutine(GetTextureData(url, onError, onSuccess));
    }

    private static IEnumerator GetTextureData(string url, Action<string> onError, Action<Texture2D> onSuccess)
    {
        using (UnityWebRequest getWikiArticle = UnityWebRequestTexture.GetTexture(url))
        {
            yield return getWikiArticle.SendWebRequest();

            if (getWikiArticle.isNetworkError || getWikiArticle.isHttpError)
            {
                onError(getWikiArticle.error);
            }
            else
            {
                DownloadHandlerTexture downloadHandlerTexture = getWikiArticle.downloadHandler as DownloadHandlerTexture;
                onSuccess(downloadHandlerTexture.texture);
            }
        }
    }
}

/*
 * WebRequest.GetData(url, (string error) =>
        {
            //Error
            Debug.LogError(error);
        },
        (string text) =>
        {
            //Successfully contacted URL
            Debug.Log("Received: " + text);
        });

        WebRequest.GetTexture(url, (string error) =>
        {
            //Error
            Debug.LogError("error: " + error);
        },
        (Texture2D texture2D) =>
        {
            //Successfully contacted URL
            Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(.5f, .5f), 10);
            image.sprite = sprite;
        });
*/