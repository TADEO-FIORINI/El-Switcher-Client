using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;


public class RoomNetworkManager : MonoBehaviour
{
    
    [SerializeField] Msg msg;

    static public List<RoomPublicData> publicRoomsData { get; private set; } = new List<RoomPublicData>();
    static public RoomPublicData publicRoomData { get; private set; }


    public void CreateRoom(string userId, string roomName, System.Action<RoomPublicData> callback)
    {
        if (string.IsNullOrEmpty(roomName))
        {
            msg.Print(Msg.MsgType.Red, "Complete your data");
            return;
        }
        StartCoroutine(CreateRoomRequest(userId, roomName, callback));
    }

    IEnumerator CreateRoomRequest(string userId, string roomName, System.Action<RoomPublicData> callback)
    {
        string url = $"{WebUtils.serverUrl}/room/{userId}/{roomName}";
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            publicRoomData = JsonConvert.DeserializeObject<RoomPublicData>(request.downloadHandler.text);
            callback?.Invoke(publicRoomData);
        }
    }

    public void GetRooms(string userId, System.Action<List<RoomPublicData>> callback)
    {
        StartCoroutine(GetRoomsRequest(userId, callback));
    }

    IEnumerator GetRoomsRequest(string userId, System.Action<List<RoomPublicData>> callback)
    {
        string url = $"{WebUtils.serverUrl}/room/{userId}";
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                try
                {
                    publicRoomsData = JsonConvert.DeserializeObject<List<RoomPublicData>>(request.downloadHandler.text);
                    callback?.Invoke(publicRoomsData);
                }
                catch (System.Exception)
                {
                }
            }
        }
    }

    public void GetRoom(string userId, string roomId, System.Action<RoomPublicData> callback)
    {
        StartCoroutine(GetRoomRequest(userId, roomId, callback));
    }

    IEnumerator GetRoomRequest(string userId, string roomId, System.Action<RoomPublicData> callback)
    {
        string url = $"{WebUtils.serverUrl}/room/{userId}/{roomId}";
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                try
                {
                    publicRoomData = JsonConvert.DeserializeObject<RoomPublicData>(request.downloadHandler.text);
                    callback?.Invoke(publicRoomData);
                }
                catch (System.Exception)
                {
                }
            }

        }
    }

    public void JoinRoom(string userId, string roomId, System.Action<RoomPublicData> callback)
    {
        StartCoroutine(JoinRoomRequest(userId, roomId, callback));
    }

    IEnumerator JoinRoomRequest(string userId, string roomId, System.Action<RoomPublicData> callback)
    {
        string url = $"{WebUtils.serverUrl}/room/join/{userId}/{roomId}";
        using (UnityWebRequest request = UnityWebRequest.Put(url, new byte[0]))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                try
                {
                    publicRoomData = JsonConvert.DeserializeObject<RoomPublicData>(request.downloadHandler.text);
                    callback?.Invoke(publicRoomData);
                }
                catch (System.Exception)
                {
                }
            }
        }
    }

    public void LeaveRoom(string userId, string roomId)
    {
        StartCoroutine(LeaveRoomRequest(userId, roomId));
    }

    IEnumerator LeaveRoomRequest(string userId, string roomId)
    {
        string url = $"{WebUtils.serverUrl}/room/leave/{userId}/{roomId}";
        using (UnityWebRequest request = UnityWebRequest.Put(url, new byte[0]))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();
        }
    }

    public void DeleteRoom(string userId, string roomId)
    {
        StartCoroutine(DeleteRoomRequest(userId, roomId));
    }

    IEnumerator DeleteRoomRequest(string userId, string roomId)
    {
        string url = $"{WebUtils.serverUrl}/room/{userId}/{roomId}";
        using (UnityWebRequest request = UnityWebRequest.Delete(url))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();
        }
    }

    private void OnApplicationQuit()
    {
        UserPrivateData userPrivateData = NetworkAuthentication.userPrivateData;
        if (userPrivateData != null && publicRoomData != null)
        {
            if (userPrivateData.username == publicRoomData.room_users[0].username)
            {
                DeleteRoom(NetworkAuthentication.userPrivateData.user_id, publicRoomData.room_id);
            }
            else
            {
                LeaveRoom(NetworkAuthentication.userPrivateData.user_id, publicRoomData.room_id);
            }
        } 
    }
}
