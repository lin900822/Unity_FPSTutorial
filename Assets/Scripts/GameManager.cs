using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public string PlayerName { get; set; }
    public string RoomName { get; set; }

    [SerializeField]
    private NetworkRunner _networkRunner;

    [SerializeField]
    private NetworkEvents _networkEvents;

    [SerializeField]
    private PlayerNetworkData _playerNetworkDataPrefab;

    public Dictionary<PlayerRef, PlayerNetworkData> PlayerList => _playerList;
    
    private Dictionary<PlayerRef, PlayerNetworkData> _playerList = new Dictionary<PlayerRef, PlayerNetworkData>();
    
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            
            _networkEvents.PlayerJoined.AddListener(OnPlayerJoined);
            _networkEvents.PlayerLeft.AddListener(OnPlayerLeft);
            
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void SetPlayerNetworkData(PlayerRef player, PlayerNetworkData playerNetworkData)
    {
        _playerList.Add(player, playerNetworkData);
        
        playerNetworkData.transform.SetParent(transform);
    }
    
    private void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        runner.Spawn(_playerNetworkDataPrefab, transform.position, Quaternion.identity, player);
    }

    private void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (_playerList.TryGetValue(player, out var playerNetworkData))
        {
            runner.Despawn(playerNetworkData.Object);
            _playerList.Remove(player);
        }
    }

    private async void Start()
    {
        var result = await _networkRunner.JoinSessionLobby(SessionLobby.ClientServer);

        if (result.Ok)
        {
            SceneManager.LoadScene("Menu");
        }
        else
        {
            Debug.LogError($"Failed To Join Lobby: {result.ShutdownReason}");
        }
    }

    public async Task CreateRoom()
    {
        var result = await _networkRunner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Host,
            SessionName = RoomName,
            PlayerCount = 20,
            Scene = SceneManager.GetActiveScene().buildIndex,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>(),
        });

        if (result.Ok)
        {
            var menuManager = FindObjectOfType<MenuManager>();
            
            menuManager.SwitchMenuType(MenuManager.MenuType.Room);
            menuManager.SetStartBtnVisible(true);
        }
        else
        {
            Debug.LogError($"Failed To Create Room: {result.ShutdownReason}");
        }
    }
    
    public async Task JoinRoom()
    {
        var result = await _networkRunner.StartGame(new StartGameArgs()
        {
            GameMode     = GameMode.Client,
            SessionName  = RoomName,
            PlayerCount  = 20,
            Scene        = SceneManager.GetActiveScene().buildIndex,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>(),
        });

        if (result.Ok)
        {
            var menuManager = FindObjectOfType<MenuManager>();
            
            menuManager.SwitchMenuType(MenuManager.MenuType.Room);
            menuManager.SetStartBtnVisible(false);
        }
        else
        {
            Debug.LogError($"Failed To Join Room: {result.ShutdownReason}");
        }
    }

    public void UpdatePlayerList()
    {
        var playerNames = new List<string>();
        foreach (var playerNetworkData in _playerList.Values)
        {
            playerNames.Add(playerNetworkData.PlayerName);
        }

        var menuManager = FindObjectOfType<MenuManager>();
        menuManager.UpdatePlayerList(playerNames);
    }

    public void StartGame()
    {
        _networkRunner.SetActiveScene("GamePlay");
    }
}
