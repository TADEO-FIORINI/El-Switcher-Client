using UnityEngine;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.WebSockets;

public class WebSocketClient : MonoBehaviour
{
    [SerializeField] WsMsgProcessor wsMsgProcessor;

    public string userId = null;

    ClientWebSocket ws;
    CancellationTokenSource cancelToken;

    public async void OpenConnection()
    {
        if (userId != null)
            await ConnectWebSocket();
    }

    async Task ConnectWebSocket()
    {
        // 🕓 Esperar a que la URL esté lista
        while (string.IsNullOrEmpty(WebUtils.relativeServerUrl))
        {
            await Task.Delay(100); // esperar 100ms antes de volver a chequear
        }

        string protocol = WebUtils.relativeServerUrl.Contains("localhost") ? "ws" : "wss";
        string wsServerUrl = $"{protocol}://{WebUtils.relativeServerUrl}/ws";
        string fullUrl = $"{wsServerUrl}/{userId}";

        ws = new ClientWebSocket();
        cancelToken = new CancellationTokenSource();

        try
        {
            Debug.Log($"Conectando a {fullUrl}...");
            await ws.ConnectAsync(new Uri(fullUrl), CancellationToken.None);
            Debug.Log("✅ Conectado al WebSocket");

            _ = ReceiveMessages(); // Iniciar recepción de mensajes en segundo plano
        }
        catch (Exception ex)
        {
            Debug.LogError($"❌ Error conectando al WebSocket: {ex.Message}");
            if (ex.InnerException != null)
            {
                Debug.LogError($"Detalle de InnerException: {ex.InnerException.Message}");
            }
        }
    }

    async Task ReceiveMessages()
    {
        ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[1024]);

        while (ws.State == WebSocketState.Open)
        {
            try
            {
                WebSocketReceiveResult result = await ws.ReceiveAsync(buffer, cancelToken.Token);
                string message = Encoding.UTF8.GetString(buffer.Array, 0, result.Count);
                Debug.Log("📩 Mensaje recibido: " + message);
                wsMsgProcessor.ProcessWsMessage(message);
            }
            catch (Exception ex)
            {
                Debug.LogError("❌ Error recibiendo mensaje: " + ex.Message);
                break;
            }
        }
    }

    public async void SendMsg(string message)
    {
        if (ws != null && ws.State == WebSocketState.Open)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(message);
            await ws.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }

    void OnApplicationQuit()
    {
        CloseConnection();
    }

    public async void CloseConnection()
    {
        if (ws != null && ws.State == WebSocketState.Open)
        {
            await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Aplicación cerrada", CancellationToken.None);
            Debug.Log("🛑 WebSocket cerrado correctamente");
        }
    }
}
