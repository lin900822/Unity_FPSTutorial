using UnityEngine;
using UnityEngine.UI;

public class PlayerListCell : MonoBehaviour
{
    [SerializeField]
    private Text _playerNameTxt;

    public void SetPlayerName(string playerName)
    {
        _playerNameTxt.text = playerName;
    }
}
