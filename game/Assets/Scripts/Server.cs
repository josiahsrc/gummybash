using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NativeWebSocket;

public class Server : Singleton<Server>
{
	private static readonly string url = "ws://10.0.0.12:8080";

	WebSocket websocket = null;
	public GameState state = new GameState();

	async void Start()
	{
		websocket = new WebSocket(url);

		websocket.OnOpen += () =>
		{
			Debug.Log("Connection open!");
		};

		websocket.OnError += (e) =>
		{
			Debug.LogError("Error! " + e);
		};

		websocket.OnClose += (e) =>
		{
			Debug.Log("Connection closed!");
		};

		websocket.OnMessage += (bytes) =>
		{
			var msg = System.Text.Encoding.UTF8.GetString(bytes);
			var rt = JsonUtility.FromJson<RuntimeTypeJson>(msg);
			if (rt.runtimeType == "gameState")
			{
				var gameState = JsonUtility.FromJson<GameStateResponse>(msg);
				state = gameState.gameState;
			}
			else if (rt.runtimeType == "error")
			{
				var error = JsonUtility.FromJson<ErrorResponse>(msg);
				Debug.LogError("Error: " + error.message);
			}
			else
			{
				Debug.LogError("Unknown message type: " + rt.runtimeType);
			}
		};

		await websocket.Connect();
	}

	void Update()
  {
    #if !UNITY_WEBGL || UNITY_EDITOR
      websocket.DispatchMessageQueue();
    #endif
  }

	private void SendUpdateGameStateRequest(UpdateGameStateRequest req)
	{
		var json = JsonUtility.ToJson(req);
		websocket.SendText(json);
	}

	public void SendWinnerRequest(UserType winner)
	{
		SendUpdateGameStateRequest(new UpdateGameStateRequest()
		{
			winner = ModelUtils.userTypeToString(winner),
		});
	}

	public void SendQuitToLobbyRequest()
	{
		SendUpdateGameStateRequest(new UpdateGameStateRequest()
		{
			lobby = true,
		});
	}

	private void OnDestroy()
	{
		SendQuitToLobbyRequest();
		if (websocket != null)
			websocket.Close();
		websocket = null;
	}
}
