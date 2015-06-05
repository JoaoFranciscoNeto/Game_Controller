using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;



// State object for reading client data asynchronously
public class StateObject
{
    // Client  socket.
    public Socket workSocket = null;
    // Size of receive buffer.
    public const int BufferSize = 1024;
    // Receive buffer.
    public byte[] buffer = new byte[BufferSize];
    // Received data string.
    public StringBuilder sb = new StringBuilder();
}

public class Server_HTTP_Listener : MonoBehaviour
{
    public static int listeningPort = 2225;

    // Thread signal.
    static Thread listenerThread;
    public static Socket listener;
    public static ManualResetEvent allDone = new ManualResetEvent(false);

    void Start()
    {
        init();
    }

    private void init()
    {
        listenerThread = new Thread(
         new ThreadStart(StartListening));

        //listenerThread.IsBackground = true;
        listenerThread.Start();
    }

    public static void StartListening()
    {
        // Data buffer for incoming data.
        byte[] bytes = new Byte[1024];

        // Establish the local endpoint for the socket.
        // The DNS name of the computer
        // running the listener is "host.contoso.com".
        IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
        IPAddress ipAddress = ipHostInfo.AddressList[0];
        Debug.Log("ip: " + ipAddress.ToString());
        Debug.Log("port:" + listeningPort);
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, listeningPort);

        // Create a TCP/IP socket.
        listener = new Socket(AddressFamily.InterNetwork,
            SocketType.Stream, ProtocolType.Tcp);

        // Bind the socket to the local endpoint and listen for incoming connections.
        try
        {
            listener.Bind(localEndPoint);
            listener.Listen(100);
            while (true)
            {
                // Set the event to nonsignaled state.
                allDone.Reset();

                // Start an asynchronous socket to listen for connections.
                Debug.Log("Waiting for a connection...");
                listener.BeginAccept(
                    new AsyncCallback(AcceptCallback),
                    listener);
                // Wait until a connection is made before continuing.
                allDone.WaitOne();
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
        }
    }

    public static void AcceptCallback(IAsyncResult ar)
    {
        
        // Signal the main thread to continue.
        allDone.Set();

        // Get the socket that handles the client request.
        Socket listener = (Socket)ar.AsyncState;
        Socket handler = listener.EndAccept(ar);

        

        // Create the state object.
        StateObject state = new StateObject();
        state.workSocket = handler;
        handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
            new AsyncCallback(ReadCallback), state);
    }

    public static void ReadCallback(IAsyncResult ar)
    {
        String content = String.Empty;

        // Retrieve the state object and the handler socket
        // from the asynchronous state object.
        StateObject state = (StateObject)ar.AsyncState;
        Socket handler = state.workSocket;

        // Read data from the client socket. 
        int bytesRead = handler.EndReceive(ar);

        if (bytesRead > 0)
        {
            // There  might be more data, so store the data received so far.
            state.sb.Append(Encoding.ASCII.GetString(
                state.buffer, 0, bytesRead));

            // Check for end-of-file tag. If it is not there, read 
            // more data.
            content = state.sb.ToString();


            try
            {
                Debug.Log(content);
                /*
                Play_Object received = JsonConvert.DeserializeObject<Play_Object>(content);
                if (received.play.jump)
                {
                    Debug.Log("Player " + received.playerID + " is jumping");
                }
                 * */
                //if (content.IndexOf("<EOF>") > -1)
                //{

                Debug.Log("Read " + content.Length
                            + " bytes from socket. \n Data : "
                            + content
                            + "\n json object:"
                    // +  received.ToString()
                           );
                // Echo the data back to the client.
                Send(handler, content);
                //}
                /*else
                {
                    // Not all data received. Get more.
                    Debug.Log("fetching more data...");
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReadCallback), state);
                }*/
            }
            catch (Exception ex)
            {
                Debug.Log(ex.StackTrace.ToString());
            }
        }
        else
        {
            Debug.Log("Read no bytes");
        }
    }
    private static void Send(Socket handler, String data)
    {
        // Convert the string data to byte data using ASCII encoding.
        byte[] byteData = Encoding.ASCII.GetBytes(data);

        // Begin sending the data to the remote device.
        handler.BeginSend(byteData, 0, byteData.Length, 0,
            new AsyncCallback(SendCallback), handler);
    }

    private static void SendCallback(IAsyncResult ar)
    {
        try
        {
            // Retrieve the socket from the state object.
            Socket handler = (Socket)ar.AsyncState;

            // Complete sending the data to the remote device.
            int bytesSent = handler.EndSend(ar);
            Debug.Log("Sent" + bytesSent + " bytes to client.");

            handler.Shutdown(SocketShutdown.Both);
            handler.Close();
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
        }
    }

    void OnDisable()
    {
        if (listenerThread != null)
            listenerThread.Abort();

        listener.Close();
    }



}






