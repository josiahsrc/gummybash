using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ArenaUI : MonoBehaviour
{
	public TMP_Text timer;
	public RectTransform houseHealthBar;
	public TMP_Text winnerText;
	public GingerBreadHouseInfo houseInfo;

	private void Update()
	{
		var server = Server.Instance;
		if (!server)
			return;

		var state = server.state;
		if (state == null)
			return;

		var started = state.IsGameInProgress;
		if (started)
		{
			var endTime = System.DateTime.Parse(state.endTimestamp);
			var now = System.DateTime.Now;
			var timeLeft = endTime - now;
			timer.text = $"{timeLeft.Minutes:00}:{timeLeft.Seconds:00}";
		}
		else
		{
			timer.text = "00:00";
		}

		if (houseInfo && houseInfo.maxHealth > 0)
		{
			var health = houseInfo.health;
			var maxHealth = houseInfo.maxHealth;
			var healthPercent = (float)health / maxHealth;
			houseHealthBar.localScale = new Vector3(healthPercent, 1, 1);
		}

		var winner = Utility.userTypeFromString(state.winner);
		if (winner != UserType.none)
		{
			winnerText.enabled = true;
			winnerText.text = winner == UserType.gummyBear ? "Bears\nWin!" : "House\nWins!";
		}
		else
		{
			winnerText.enabled = false;
		}
	}
}
