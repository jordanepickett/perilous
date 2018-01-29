using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Barebones.MasterServer;
using Barebones.Networking;
using UnityEngine.UI;

public class ConnectionInformation : MonoBehaviour
{

    public Text Text;
    private IClientSocket _connection;

    // Use this for initialization
    void Start()
    {
        Text = gameObject.GetComponent<Text>();
        _connection = GetConnection();

        _connection.StatusChanged += UpdateStatusView;
        UpdateStatusView(_connection.Status);
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected IClientSocket GetConnection()
    {
        return Msf.Connection;
    }

    protected void UpdateStatusView(ConnectionStatus status)
    {
        switch (status)
        {
            case ConnectionStatus.Connected:
                Text.text = "Connected";
                break;
            case ConnectionStatus.Disconnected:
                Text.text = "Offline";
                break;
            case ConnectionStatus.Connecting:
                Text.text = "Connecting";
                break;
            default:
                Text.text = "Unknown";
                break;
        }

    }

    protected void OnDestroy()
    {
        _connection.StatusChanged -= UpdateStatusView;
    }
}