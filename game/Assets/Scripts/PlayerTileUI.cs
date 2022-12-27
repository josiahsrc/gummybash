using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerTileUI : MonoBehaviour
{
	public TMP_Text username;
	public Image color;

	public void SetUser(User user)
	{
		username.text = user.displayName;
		username.color = Utility.HexToColor(user.color);
		color.color = Utility.HexToColor(user.color);
	}
}
