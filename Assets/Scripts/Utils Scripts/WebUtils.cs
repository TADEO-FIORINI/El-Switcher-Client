using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WebUtils : MonoBehaviour
{
    public static string serverUrl;
    public static string relativeServerUrl;

    void Start()
    {
        StartCoroutine(GetServerUrl());
    }

    IEnumerator GetServerUrl()
    {
        string serverUrlRemote = "https://gist.githubusercontent.com/TADEO-FIORINI/33a8ccf61e4836388b1c0f0f1b1a6ab8/raw/ngrok_url.txt";
        UnityWebRequest www = UnityWebRequest.Get(serverUrlRemote);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            serverUrl = www.downloadHandler.text.Trim();
            relativeServerUrl = serverUrl.Replace("https://", "").Replace("http://", "");
            Debug.Log($"URL actual: {serverUrl}");
            Debug.Log($"relative URL actual: {relativeServerUrl}");
        }
        else
        {
            Debug.LogError($"Error al obtener URL de Ngrok: {www.error}");
        }
    }

    public static string ParseErrorMessage(string jsonResponse)
    {
        try
        {
            var errorResponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonResponse);
            if (errorResponse != null && errorResponse.ContainsKey("detail"))
            {
                return errorResponse["detail"];
            }
            else
            {
                return "Unknown error format";
            }
        }
        catch
        {
            return "Failed to parse error response";
        }
    }

}
