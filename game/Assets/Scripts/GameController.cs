using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
	public GameObject gummyBearPrefab;
	public GameObject gingerBreadHousePrefab;

	private Dictionary<string, Player> _players = new Dictionary<string, Player>();

	private void Update()
	{
		if (!Server.Instance)
			return;

		var userIds = Server.Instance.state.users.Select(x => x.id).ToHashSet();
		var userDict = Server.Instance.state.users.ToDictionary(x => x.id);

		var playersThatLeft = _players.Where(x => !userIds.Contains(x.Key)).ToList();
		foreach (var player in playersThatLeft)
		{
			_players.Remove(player.Key);
			Destroy(player.Value.gameObject);
		}

		foreach (var userId in userIds)
		{
			if (!_players.ContainsKey(userId))
			{
				var user = userDict[userId];
				var type = ModelUtils.userTypeFromString(user.type);
				GameObject prefab;
				if (type == UserType.gingerBreadHouse)
				{
					prefab = gingerBreadHousePrefab;
				}
				else if (type == UserType.gummyBear)
				{
					prefab = gummyBearPrefab;
				}
				else
				{
					continue;
				}

				var obj = Instantiate(prefab);
				var player = obj.GetComponent<Player>();
				player.userId = userId;
				_players[userId] = player;
				print("Added player " + userId);
			}
		}
	}
}
