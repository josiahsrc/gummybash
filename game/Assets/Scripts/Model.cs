using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ModelUtils
{
	public static UserType userTypeFromString(string value)
	{
		if (value == "gummyBear") {
			return UserType.gummyBear;
		} else if (value == "gingerBreadHouse") {
			return UserType.gingerBreadHouse;
		} else {
			return UserType.none;
		}
	}

  public static string userTypeToString(UserType value)
  {
    if (value == UserType.gummyBear) {
      return "gummyBear";
    } else if (value == UserType.gingerBreadHouse) {
      return "gingerBreadHouse";
    } else {
      return "none";
    }
  }
}

public enum UserType {
  none,
  gummyBear,
  gingerBreadHouse,
}

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
  public string type;
}

[System.Serializable]
public class GameState
{
	public User[] users;
	public string updatedAt;
	public string startTimestamp;
	public string endTimestamp;
	public string lobbyTimestamp;
	public string winner;
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
	public float joystickX;
	public float joystickY;
	public bool buttonPressed;
}

[System.Serializable]
public class UpdateGameStateRequest
{
	public string runtimeType = "updateGameState";
  public string winner;
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
