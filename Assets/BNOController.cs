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
