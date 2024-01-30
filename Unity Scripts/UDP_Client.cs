using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class UDP_Client : MonoBehaviour
{

    public Text text;
    static UdpClient udp;
    private int listenPort = 12345;
    Thread thread;

    static readonly object lockObject = new object();
    string returnData = "";
    bool precessData = false;

    void Start()
    {
        thread = new Thread(new ThreadStart(ThreadMethod));
        thread.Start();
    }

    void Update()
    {
        if (precessData)
        {
            /*lock object to make sure there data is 
             *not being accessed from multiple threads at thesame time*/
            lock (lockObject)
            {
                precessData = false;
                text.text= returnData;

                //Process received data
                Debug.Log("Received: " + returnData);

                //Reset it for next read(OPTIONAL)
                returnData = "";
            }
        }
    }

    private void ThreadMethod()
    {
        Debug.Log("ThreadMethod1");
        udp = new UdpClient(listenPort);
        while (true)
        {
            Debug.Log("ThreadMethod2");
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0); // ..., 0) = all ports?

            Debug.Log("ThreadMethod3");
            byte[] receiveBytes = udp.Receive(ref RemoteIpEndPoint);
            Debug.Log("ThreadMethod4");
            /*lock object to make sure there data is 
            *not being accessed from multiple threads at thesame time*/
            lock (lockObject)
            {
                Debug.Log("ThreadMethod5");
                returnData = Encoding.ASCII.GetString(receiveBytes);

                Debug.Log(returnData);
                if (returnData != "1\n")
                {
                    //Done, notify the Update function
                    precessData = true;
                }
            }
        }
    }
}
