using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
	public GameObject gummyBearPrefab;

  private Dictionary<string, Player> _players = new Dictionary<string, Player>();

  private void Update() {
    if (!Server.Instance)
      return;

    var userIds = Server.Instance.state.users.Select(x => x.id).ToHashSet();

    var playersThatLeft = _players.Where(x => !userIds.Contains(x.Key)).ToList();
    foreach (var player in playersThatLeft) {
      _players.Remove(player.Key);
      Destroy(player.Value.gameObject);
      print("Removed player + " + player);
    }

    foreach (var userId in userIds) {
      if (!_players.ContainsKey(userId)) {
        var obj = Instantiate(gummyBearPrefab);
        var player = obj.GetComponent<Player>();
        player.userId = userId;
        _players[userId] = player;
        print("Added player " + userId);
      }
    }
  }
}
