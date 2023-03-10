using System.Threading.Tasks;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.UI;

public class Relay : MonoBehaviour
{
    [SerializeField] private TMP_Text _joinCodeText;
    [SerializeField] private TMP_InputField _joinInput;
    [SerializeField] private GameObject _buttons;
    [SerializeField] private Button _joinButton;
    [SerializeField] private Button _createButton;

    private UnityTransport _transport;
    private const int max_players = 2;

    private async void Awake()
    {
        //ui-experiment
        _joinButton.onClick.AddListener(() =>
        {
            JoinGame();
        });
        _createButton.onClick.AddListener(() =>
        {
            CreateGame();
        });

        //everything-below-works
        _transport = FindObjectOfType<UnityTransport>();

        _buttons.SetActive(false);

        await Authenticate();

        _buttons.SetActive(true);
    }

    private static async Task Authenticate()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    private async void CreateGame()
    {
        _buttons.SetActive(false);

        Allocation a = await RelayService.Instance.CreateAllocationAsync(max_players);
        _joinCodeText.text = await RelayService.Instance.GetJoinCodeAsync(a.AllocationId);

        _transport.SetHostRelayData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData);

        NetworkManager.Singleton.StartHost();
    }

    private async void JoinGame()
    {
        _buttons.SetActive(false);

        JoinAllocation a = await RelayService.Instance.JoinAllocationAsync(_joinInput.text);

        _joinCodeText.text = _joinInput.text;

        _transport.SetClientRelayData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData, a.HostConnectionData);

        NetworkManager.Singleton.StartClient();
    }
}
