using Fusion;

public class PlayerNetworkData : NetworkBehaviour
{
    [Networked(OnChanged = nameof(OnPlayerNameChanged))]
    public string PlayerName { get; set; }

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            SetPlayerName_RPC(GameManager.Instance.PlayerName);
        }
        
        GameManager.Instance.SetPlayerNetworkData(Object.InputAuthority, this);
    }

    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
    private void SetPlayerName_RPC(string playerName)
    {
        PlayerName = playerName;
    }

    private static void OnPlayerNameChanged(Changed<PlayerNetworkData> changed)
    {
        GameManager.Instance.UpdatePlayerList();
    }
}
