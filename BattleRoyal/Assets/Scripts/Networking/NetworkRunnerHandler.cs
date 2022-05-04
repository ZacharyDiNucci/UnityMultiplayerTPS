using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System;
using System.Linq;

public class NetworkRunnerHandler : MonoBehaviour
{

    public NetworkRunner networkRunnerPrefab;

    NetworkRunner networkRunner;

    private void Start() {
        networkRunner = Instantiate(networkRunnerPrefab);
        networkRunner.name = "Network Runner";

        var clientTask = InitializeNetworkRunner(networkRunner, GameMode.AutoHostOrClient, NetAddress.Any(), SceneManager.GetActiveScene().buildIndex, null);

        Debug.Log($"Server NetworkRunner started.");
    }

    protected virtual Task InitializeNetworkRunner(NetworkRunner runner, GameMode gameMode, NetAddress address, SceneRef scene, Action<NetworkRunner> initialized)
    {
        var sceneObjectProvider = runner.GetComponents(typeof(MonoBehaviour)).OfType<INetworkSceneObjectProvider>().FirstOrDefault();

        if(sceneObjectProvider == null)
        {
            sceneObjectProvider = runner.gameObject.AddComponent<NetworkSceneManagerDefault>();
        }

        runner.ProvideInput = true;

        return runner.StartGame(new StartGameArgs
        {
            GameMode = gameMode,
            Address = address,
            Scene = scene,
            SessionName = "TestRoom",
            Initialized = initialized,
            SceneObjectProvider = sceneObjectProvider
        });
    }






    // public void OnInput(NetworkRunner runner, NetworkInput input) { }
    // public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    // public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    // public void OnConnectedToServer(NetworkRunner runner) { }
    // public void OnDisconnectedFromServer(NetworkRunner runner) { }
    // public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    // public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    // public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    // public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    // public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    // public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    // public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }
    // public void OnSceneLoadDone(NetworkRunner runner) { }
    // public void OnSceneLoadStart(NetworkRunner runner) { }

    // private NetworkRunner _runner;

    // private void OnGUI()
    // {
    //     if (_runner == null)
    //     {
    //         if (GUI.Button(new Rect(0,0,200,40), "Host"))
    //         {
    //             StartGame(GameMode.Host);
    //         }
    //         if (GUI.Button(new Rect(0,40,200,40), "Join"))
    //         {
    //             StartGame(GameMode.Client);
    //         }
    //     }
    // }
    // async void StartGame(GameMode mode)
    // {
    //      // Create the Fusion runner and let it know that we will be providing user input
    //     _runner = gameObject.AddComponent<NetworkRunner>();
    //     _runner.ProvideInput = true;

    //     // Start or join (depends on gamemode) a session with a specific name
    //     await _runner.StartGame(new StartGameArgs()
    //     {
    //         GameMode = mode,
    //         SessionName = "TestRoom",
    //         Scene = SceneManager.GetActiveScene().buildIndex,
    //         SceneObjectProvider = gameObject.AddComponent<NetworkSceneManagerDefault>()
    //     });
    // }

    // [SerializeField] private NetworkPrefabRef _playerPrefab;
    // private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();

    // public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    // {
    //     // Create a unique position for the player
    //     Vector3 spawnPosition = new Vector3((player.RawEncoded%runner.Config.Simulation.DefaultPlayers)*3,1,0);
    //     NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);
    //     // Keep track of the player avatars so we can remove it when they disconnect
    //     _spawnedCharacters.Add(player, networkPlayerObject);
    // }

    // public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    // {
    // // Find and remove the players avatar
    // if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
    // {
    //     runner.Despawn(networkObject);
    //     _spawnedCharacters.Remove(player);
    // }
    // }
}
