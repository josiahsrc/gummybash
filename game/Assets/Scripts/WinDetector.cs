using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WinDetector : MonoBehaviour
{
	public UnityEvent onWin;

	private UserType prevWinner = UserType.none;

	void Update()
	{
		var server = Server.Instance;
		if (!server)
			return;

		var state = server.state;
		if (state == null)
			return;

		var newWinner = Utility.userTypeFromString(state.winner);
		if (newWinner != prevWinner && newWinner != UserType.none)
		{
			onWin?.Invoke();
		}

		prevWinner = newWinner;
	}
}
