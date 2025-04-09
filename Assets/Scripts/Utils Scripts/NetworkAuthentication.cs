using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkAuthentication : MonoBehaviour
{
    // User data
    static public UserPrivateData userPrivateData { get; private set; }

    // Utils
    [SerializeField] Msg msg;
    [SerializeField] WebSocketClient webSocketClient;

    public void RegisterUser(string username, string password, string confirmPassword)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
        {
            msg.Print(Msg.MsgType.Red, "Complete your data");
            return;
        }

        StartCoroutine(RegisterUserRequest(username, password, confirmPassword));
    }

    IEnumerator RegisterUserRequest(string username, string password, string confirmPassword)
    {
        string url = $"{WebUtils.serverUrl}/user/{username}/{password}/{confirmPassword}";

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            msg.Print(Msg.MsgType.Green, "User Sign Up successfully.\nPlease, back and Login");
        }
        else
        {
            if (WebUtils.ParseErrorMessage(request.downloadHandler.text) != "Unknown error format")
                msg.Print(Msg.MsgType.Red, WebUtils.ParseErrorMessage(request.downloadHandler.text));
            else
                msg.Print(Msg.MsgType.Red, "Not found");
        }
    }

    public void LoginUser(string username, string password, System.Action<UserPrivateData> callBack)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            msg.Print(Msg.MsgType.Red, "Complete your data");
            return;
        }

        StartCoroutine(LoginUserRequest(username, password, callBack));
    }

    IEnumerator LoginUserRequest(string username, string password, System.Action<UserPrivateData> callback)
    {
        string url = $"{WebUtils.serverUrl}/user/{username}/{password}";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                try
                {
                    string jsonResponse = request.downloadHandler.text;
                    UserPrivateData userPrivate_ = JsonUtility.FromJson<UserPrivateData>(jsonResponse);
                    userPrivateData = userPrivate_;
                    callback?.Invoke(userPrivate_);
                    webSocketClient.userId = userPrivateData.user_id;
                    webSocketClient.OpenConnection();
                    msg.Print(Msg.MsgType.Green, "Login successful: " + userPrivateData.username);
                    msg.Deactivate();
                }
                catch
                {
                    msg.Print(Msg.MsgType.Red, "Invalid response format");
                }
            }
            else
            {
                msg.Print(Msg.MsgType.Red, WebUtils.ParseErrorMessage(request.downloadHandler.text));
            }
        }
    }

}
