using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RenderImage : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(MyCoroutineImage("https://ih1.redbubble.net/image.1071266540.8760/flat,750x1000,075,f.u1.jpg"));
        StartCoroutine(MyCoroutineImage("https://upload.wikimedia.org/wikipedia/en/7/71/Monster_Hunter_logo.png"));
        StartCoroutine(MyCoroutineSound("https://freetestdata.com/wp-content/uploads/2021/09/Free_Test_Data_100KB_OGG.ogg"));
        StartCoroutine(MyCoroutineText("http://httpbin.org/headers"));

    }

    IEnumerator MyCoroutineImage(string URL)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(URL);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture2D myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            GameObject image = new GameObject("Nome");
            SpriteRenderer mySprite = image.AddComponent<SpriteRenderer>();
            mySprite.sprite = Sprite.Create(myTexture, new Rect(0, 0, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f), 100);
        }

        Debug.Log("Funziona");
    }

    IEnumerator MyCoroutineSound(string URL)
    {
        UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(URL, AudioType.OGGVORBIS);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            AudioClip myClip = ((DownloadHandlerAudioClip)www.downloadHandler).audioClip;
            GetComponent<AudioSource>().clip = myClip;
            GetComponent<AudioSource>().Play();
        }

        Debug.Log("Funziona");
    }

    IEnumerator MyCoroutineText(string URL)
    {
        UnityWebRequest www = UnityWebRequest.Get(URL);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
            Debug.Log(www.error);
        else
            Debug.Log(www.downloadHandler.text);
    }

}