using Fusion;
using UnityEngine;

public class PlayerSpawner : NetworkBehaviour
{
    [SerializeField]
    private NetworkPrefabRef _playerPrefab;

    public override void Spawned()
    {
        if (!Object.HasStateAuthority)
            return;

        var gameManager = GameManager.Instance;

        foreach (var player in gameManager.PlayerList.Keys)
        {
            var spawnPoint = new Vector3(0, 2, 0);
            Runner.Spawn(_playerPrefab, spawnPoint, Quaternion.identity, player);
        }
    }
}