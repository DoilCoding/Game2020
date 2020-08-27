using System.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;
using UnityEditor;
using System.Net.Sockets;
using System.Threading;
using System.Text;

public class Server : MonoBehaviour
{

    /*
        lets do basic UI make Initialize work properly
        then make maploading work
        then continue. this way we can do our modules and such.
        skip networking for a little bit while we work on this.
        level loader on server can be just load immediately no loading screen and such.
        ****actually same can be for client, we dont necessary need a loading screen do we? tho.. would be kinda nice?
    */


    private void Start()
    {
        Initialize(25000, 50, "boobs");
    }

    CancellationTokenSource cts = new CancellationTokenSource();
    public async void Initialize(int port, int slots, string map)
    {
        Port = port;
        Slots = slots;
        Map = map;

        await Task.Factory.StartNew(() => LoadLevel(map));
        await Task.Factory.StartNew(() => Receive(), cts.Token);
    }

    private void OnApplicationQuit()
    {
        try
        {
            cts.Cancel();
            cts.Dispose();
        }
        catch (Exception ex) { Debug.LogError(ex.Message); }
    }

    private void Receive()
    {
        using (ServerSocket = new UdpClient(Port))
        {
            IPEndPoint listenEndPoint = new IPEndPoint(IPAddress.Any, Port);
            while (!cts.IsCancellationRequested)
            {
                if (cts.IsCancellationRequested) break;
                byte[] receiveData = ServerSocket.Receive(ref listenEndPoint);
                Debug.LogWarning(Encoding.ASCII.GetString(receiveData));

                byte[] delta = Encoding.ASCII.GetBytes("THIS WORKS");
                ServerSocket.Send(delta, delta.Length, listenEndPoint);

                // question is now: decode it here and send it with an event
                // or broadcast it and let a decoder grab it and then event again?
            }
        }
    }

    public async void LoadLevel(string level)
    {
        await Task.Delay(0);
        Debug.Log($"Loaded level: ${level}");
    }

    public async void Terminate(string reason)
    {
        await Task.Factory.StartNew(() => Terminate(reason, DateTime.Now.AddSeconds(5)));
    }

    public async Task Terminate(string reason, DateTime ending)
    {
        try
        {
            while (ending.CompareTo(DateTime.Now) > 0)
            {
                OnServerShutDownTimer?.Invoke((ending - DateTime.Now).Minutes, (ending - DateTime.Now).Seconds);

                if (ending.CompareTo(DateTime.Now) <= 0) break;
                await Task.Delay(1000);
            }

            // send player kick with reason
            // close down the network socket
        }
        catch (Exception ex)
        {
            Debug.Log($"{ex.Message}\n{ex.StackTrace}");
        }

#if !UNITY_EDITOR
        Application.Quit();
#else
        EditorApplication.isPlaying = false;
#endif
    }

    /*public void Update()
    {
        OnPlayerConnecting?.Invoke(0, new byte[0], "source");
        OnPlayerConnected?.Invoke(0, new byte[0], "source");
        OnPlayerData?.Invoke(0, new byte[0], "source");
        OnPlayerDisconnect?.Invoke(0, new byte[0], "source", "reason");
    }

    private void Start()
    {
        // Terminate("none lel");
    }*/

    public delegate void OnPlayerConnectingEvent(int length, byte[] bytes, string source);
    public static OnPlayerConnectingEvent OnPlayerConnecting;

    public delegate void OnPlayerDataEvent(int length, byte[] bytes, string source);
    public static OnPlayerDataEvent OnPlayerData;

    public delegate void OnPlayerConnectedEvent(int length, byte[] bytes, string source);
    public static OnPlayerConnectedEvent OnPlayerConnected;

    public delegate void OnPlayerDisconnectedEvent(int length, byte[] bytes, string source, string reason);
    public static OnPlayerDisconnectedEvent OnPlayerDisconnect;

    public delegate void OnServerShutDownTimerEvent(int minutes, int seconds);
    public static OnServerShutDownTimerEvent OnServerShutDownTimer;

    public List<object> Players { get; private set; } = new List<object>();     // the player objects that are on the server
    public List<object> PlayersQueue { get; private set; } = new List<object>();// the players that are attempting to connect and are still in queue
    public int Port { get; private set; }                                       // the port this server will be run on
    public int Slots { get; private set; }                                      // the amount of open slots this server will have
    public int OpenSlots => Slots - Players.Count;                              // calculate how many open slots we have left
    public int UsedSlots => OpenSlots - Players.Count;                          // calculate how many slots we are using
    public string Map { get; private set; }                                     // this map will be loaded on the server and clients will be forced to load it
    public DateTime Started { get; private set; }                               // this auto sets when we started the server

    public static UdpClient ServerSocket { get; private set; }
}