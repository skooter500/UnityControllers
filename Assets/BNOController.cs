using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System.Threading;

public class BNOController : MonoBehaviour {

    private SerialPort controller;
    private string messageFromController;
    private bool runThread = true;

    public bool Connected = false;
    public string portName = "COM10";

    Quaternion sensorRotation;

    [Range(0, 360)]
    public float x;
    public float y;
    public float z;

    public int system;
    public int gyro;
    public int accel;
    public int magnet;

    void OnDrawGizmos()
    {

        float dist = 10;
        Gizmos.color = Color.green;
        Vector3 to = transform.position + ((Quaternion.Euler(0, x, 0) * Vector3.forward) * dist);
        Gizmos.DrawLine(transform.position, to);
        
        Gizmos.color = Color.red;
        to = transform.position + ((Quaternion.Euler(0, y, 0) * Vector3.forward) * dist);
        Gizmos.DrawLine(transform.position, to);
        
        Gizmos.color = Color.cyan;
        to = transform.position + ((Quaternion.Euler(0, z, 0) * Vector3.forward) * dist);
        Gizmos.DrawLine(transform.position, to);
        
    }

    // Use this for initialization
    void Start()
    {
        LookForController();

        // create the thread
        runThread = true;
        Thread ThreadForController = new Thread(new ThreadStart(ThreadWorker));
        ThreadForController.Start();

    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = sensorRotation;
    }

    void ProcessMessage(string message)
    {
        if (message.StartsWith("Q:"))
        {
            string[] decoded = message.Substring(2).Split(',');
            Quaternion q = new Quaternion(
                float.Parse(decoded[0])
                , float.Parse(decoded[1])
                , float.Parse(decoded[2])
                , float.Parse(decoded[3]));
            sensorRotation = q;
        }
        if (message.StartsWith("S:"))
        {
            system = int.Parse(message.Substring(2));
        }
        if (message.StartsWith("A:"))
        {
            accel = int.Parse(message.Substring(2));
        }
        if (message.StartsWith("M:"))
        {
            magnet = int.Parse(message.Substring(2));
        }
        if (message.StartsWith("G:"))
        {
            gyro = int.Parse(message.Substring(2));
        }
        if (message.StartsWith("OX:"))
        {
            x = float.Parse(message.Substring(3));
        }
        if (message.StartsWith("OY:"))
        {
            y = float.Parse(message.Substring(3));
        }
        if (message.StartsWith("OZ:"))
        {
            z = float.Parse(message.Substring(3));
        }


    }

    void ThreadWorker()
    {
        while (runThread)
        {
            if (controller != null && controller.IsOpen)
            {
                try
                {
                    messageFromController = controller.ReadLine();
                    ProcessMessage(messageFromController);
                }
                catch (System.Exception) { }
            }
            else
            {
                Thread.Sleep(50);
            }
        }
    }


    void OnApplicationQuit()
    {
        controller.Close();
        runThread = false;
    }

    public void LookForController()
    {
        string[] ports = SerialPort.GetPortNames();
        Debug.Log(ports.Length);

        if (ports.Length == 0)
        {
            Debug.Log("No controller detected");
        }
        else
        {
            Debug.Log("Ports: " + string.Join(", ", ports));
            portName = "\\\\.\\" + ports[ports.Length - 1];
            Debug.Log("Port Name: " + portName);
            Connected = true;

            // check the default port
            controller = new SerialPort(portName, 9600);
            controller.ReadTimeout = 100;
            controller.Open();
        }
    }
}
