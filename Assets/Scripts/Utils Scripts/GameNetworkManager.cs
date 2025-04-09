using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using Newtonsoft.Json;


public class GameNetworkManager : MonoBehaviour
{

    [SerializeField] Msg msg;
    static public GamePublicData publicGameData { get; private set; }


    public void StartGame(string userId, string roomId)
    {
        StartCoroutine(StartGameRequest(userId, roomId));
    }

    IEnumerator StartGameRequest(string userId, string roomId)
    {
        string url = $"{WebUtils.serverUrl}/game/{userId}/{roomId}";
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            publicGameData = JsonConvert.DeserializeObject<GamePublicData>(request.downloadHandler.text);
        }
    }

    public void GetGame(string userId, string roomId, System.Action<GamePublicData> callback)
    {
        StartCoroutine(GetGameRequest(userId, roomId, callback));
    }

    IEnumerator GetGameRequest(string userId, string roomId, System.Action<GamePublicData> callback)
    {
        string url = $"{WebUtils.serverUrl}/game/{userId}/{roomId}";
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                try
                {
                    publicGameData = JsonConvert.DeserializeObject<GamePublicData>(request.downloadHandler.text);
                    for (int i = 0; i < publicGameData.board.tiles.Count; i++)
                    {
                        TileData tileData = publicGameData.board.tiles[i];
                    }
                    callback?.Invoke(publicGameData);
                }
                catch (System.Exception)
                {
                }
            }
        }
    }


    public void NextTurn(string userId, string roomId)
    {
        StartCoroutine(NextTurnRequest(userId, roomId));
    }

    IEnumerator NextTurnRequest(string userId, string roomId)
    {
        string url = $"{WebUtils.serverUrl}/game/next_turn/{userId}/{roomId}";
        using (UnityWebRequest request = UnityWebRequest.Put(url, new byte[0]))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();
        }
    }

    public void LeaveGame(string userId, string roomId)
    {
        StartCoroutine(LeaveGameRequest(userId, roomId));
    }

    IEnumerator LeaveGameRequest(string userId, string roomId)
    {
        string url = $"{WebUtils.serverUrl}/game/leave/{userId}/{roomId}";
        using (UnityWebRequest request = UnityWebRequest.Put(url, new byte[0]))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();
        }
    }

    public void Switch(string userId, string roomId, int movCardIndex, int tile1_x, int tile1_y, int tile2_X, int tile2_y)
    {
        StartCoroutine(SwitchRequest(userId, roomId, movCardIndex, tile1_x, tile1_y, tile2_X, tile2_y));
    }

    IEnumerator SwitchRequest(string userId, string roomId, int movCardIndex, int tile1_x, int tile1_y, int tile2_x, int tile2_y)
    {
        string url = $"{WebUtils.serverUrl}/game/switch/{userId}/{roomId}/{movCardIndex}/{tile1_x}/{tile1_y}/{tile2_x}/{tile2_y}";
        using (UnityWebRequest request = UnityWebRequest.Put(url, new byte[0]))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();
        }
    }

    public void FigureDiscard(string userId, string roomId, int figCardIndex, int tile_x, int tile_y)
    {
        StartCoroutine(FigureDiscardRequest(userId, roomId, figCardIndex, tile_x, tile_y));
    }

    IEnumerator FigureDiscardRequest(string userId, string roomId, int figCardIndex, int tile_x, int tile_y)
    {
        string url = $"{WebUtils.serverUrl}/game/figure/discard/{userId}/{roomId}/{figCardIndex}/{tile_x}/{tile_y}";
        using (UnityWebRequest request = UnityWebRequest.Put(url, new byte[0]))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();
        }
    }

    public void FigureBlock(string userId, string roomId, string playername, int figCardIndex, int tile_x, int tile_y)
    {
        StartCoroutine(FigureBlockRequest(userId, roomId, playername, figCardIndex, tile_x, tile_y));
    }

    IEnumerator FigureBlockRequest(string userId, string roomId, string playername, int figCardIndex, int tile_x, int tile_y)
    {
        string url = $"{WebUtils.serverUrl}/game/figure/block/{userId}/{roomId}/{playername}/{figCardIndex}/{tile_x}/{tile_y}";
        using (UnityWebRequest request = UnityWebRequest.Put(url, new byte[0]))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();
        }
    }

    private void OnApplicationQuit()
    {
        UserPrivateData userPrivateData = NetworkAuthentication.userPrivateData;
        if (userPrivateData != null && publicGameData != null)
        {
            LeaveGame(userPrivateData.user_id, publicGameData.room.room_id);
        }
    }
}
