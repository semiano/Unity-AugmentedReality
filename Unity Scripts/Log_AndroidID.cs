using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Net.NetworkInformation;
public class Log_AndroidID : MonoBehaviour
{
    private int remotePort = 12346;
    //private int myport = 12347;
    public string clientID;
    private string androidID ;

    // Start is called before the first frame update
    void Start()
    {
        androidID = GetMacAddress();
        Debug.Log("STARTING UDP LOGGER" );
        UdpClient udpServer = new UdpClient();
        var remoteEP = new IPEndPoint(IPAddress.Broadcast, remotePort);
        byte[] _bytes = Encoding.ASCII.GetBytes(androidID+","+clientID);

        int result = udpServer.Send(_bytes, _bytes.Length, remoteEP); // reply back
        Debug.Log("UDP LOGGER: "+result.ToString());
        udpServer.Close();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Returns the 1st valid Mac Address
    public string GetMacAddress()
    {
        string macAdress = "";
        NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();

        var i = 0;
        foreach (var n in nics)
        {
            PhysicalAddress address = n.GetPhysicalAddress();
            if (address.ToString() != "")
            {
                macAdress = address.ToString();
                return macAdress;
            }
        }
        return "BADMAC";
    }
}
