using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
	public GameObject playerPrefab;
	public GameObject playersParent;

	private List<GameObject> userObjs = new();
	private HashSet<string> userIds = new();

	private void DestroyOldUsers()
	{
		foreach (var userObj in userObjs)
			Destroy(userObj);
		userObjs.Clear();
	}

	private void Update()
	{
		var newUsers = Server.Instance.state?.users;
		if (newUsers == null)
		{
			DestroyOldUsers();
			return;
		}

		var newUserIds = newUsers.Select(u => u.id).ToHashSet();
		if (newUserIds.SetEquals(userIds))
			return;

		DestroyOldUsers();

		foreach (var user in newUsers)
		{
			var userObj = Instantiate(playerPrefab, playersParent.transform);
			userObj.GetComponent<PlayerTileUI>().SetUser(user);
			userObjs.Add(userObj);
		}
		userIds = newUserIds;
	}
}
