using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerController : MonoBehaviour
{
	public Player player;
	public CharacterMover mover;

	private int _lastButtonPresses = 0;

	private bool GetShouldFire()
	{
		var currentButtonPresses = player.user?.buttonPresses ?? 0;

		bool result;
		if (mover.enableComputerMovement)
			result = Input.GetKeyDown(KeyCode.Space);
		else
			result = _lastButtonPresses < currentButtonPresses;

		_lastButtonPresses = currentButtonPresses;
		return result;
	}


	protected virtual void Update()
	{
		if (GetShouldFire())
		{
			OnButtonPressed();
		}
	}

	protected abstract void OnButtonPressed();
}
