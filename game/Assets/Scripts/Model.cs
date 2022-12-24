using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class User
{
	public string id;
	public string displayName;
	public string updatedAt;
	public float joystickX;
	public float joystickY;
	public int buttonPresses;
	public string color;
}

[System.Serializable]
public class GameState
{
	public User[] users;
	public string updatedAt;
	public string startTimestamp;
	public string endTimestamp;
	public string lobbyTimestamp;
	public string winnerId;
}

[System.Serializable]
public class JoinGameRequest
{
	public string runtimeType = "joinGame";
	public string displayName;
	public string userId;
}

[System.Serializable]
public class UpdateUserRequest
{
	public string runtimeType = "updateUser";
	public string userId;
	public float? joystickX;
	public float? joystickY;
	public bool? buttonPressed;
}

[System.Serializable]
public class UpdateGameStateRequest
{
	public string runtimeType = "updateGameState";
	public string winnerId;
	public bool start;
	public bool lobby;
}

[System.Serializable]
public class GameStateResponse
{
	public string runtimeType = "gameState";
	public GameState gameState;
}

[System.Serializable]
public class ErrorResponse
{
	public string runtimeType = "error";
	public string message;
}

[System.Serializable]
public class RuntimeTypeJson
{
	public string runtimeType;
}
