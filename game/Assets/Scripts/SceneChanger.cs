using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
	public readonly string LOBBY_SCENE = "Lobby";
	public readonly string ARENA_SCENE = "Arena";

	private static string prevScene = null;

	private void Update()
	{
		var server = Server.Instance;
		if (!server)
			return;

		var state = server.state;
		if (state == null)
			return;

		var isLobby = state.IsInLobby;
		var nextScene = isLobby ? LOBBY_SCENE : ARENA_SCENE;

		if (prevScene != nextScene)
		{
			prevScene = nextScene;
			SceneManager.LoadScene(nextScene);
		}
	}
}
