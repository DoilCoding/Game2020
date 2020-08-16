using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class Client : MonoBehaviour
{
    // Start is called before the first frame update

    private void Start()
    {
        Task.Factory.StartNew(() => Receive(), cts.Token);
    }

    private void Receive()
    {
        IPEndPoint listenEndPoint = new IPEndPoint(IPAddress.Any, ((IPEndPoint)ClientSocket.Client.LocalEndPoint).Port);
        while (!cts.IsCancellationRequested)
        {
            if (cts.IsCancellationRequested) break;
            byte[] receiveData = ClientSocket.Receive(ref listenEndPoint);
            Debug.LogWarning(Encoding.ASCII.GetString(receiveData));
        }
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


    public void ClickMe()
    {
        IPEndPoint ep = new IPEndPoint(IPAddress.Parse("192.168.1.70"), 25000);
        byte[] data = Encoding.ASCII.GetBytes($"Waddap doil you sexy bitch {DateTime.Now}");
        ClientSocket.Send(data, data.Length, ep);
    }

    UdpClient ClientSocket = new UdpClient(0);
    CancellationTokenSource cts = new CancellationTokenSource();

}
