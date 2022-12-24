using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
	[System.NonSerialized]
	public string userId;

	[System.NonSerialized]
	public User user = null;

	private void Start()
	{
		Sync();
	}

	private void Update()
	{
		Sync();
	}

	private void Sync()
	{
		var server = Server.Instance;
		if (!server || userId == null)
		{
			user = null;
			return;
		}

    var users = server.state.users;
    var userDict = users.ToDictionary(u => u.id);
    if (userDict.ContainsKey(userId))
      user = userDict[userId];
    else
      user = null;
	}
}
