using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System.Threading;

public class PitchBendController : MonoBehaviour
{
    private SerialPort controller;
    private string messageFromController;
    private bool runThread = true;

    public bool Connected = false;
    public string portName = "COM10";
    
    public volatile float left;
    public volatile float right;
    public volatile bool selector;
    public volatile float pressure;

    float scale = 1.0f;

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

    }

    void ProcessMessage(string message)
    {
        string[] decoded = message.Split(':');
        float value = float.Parse(decoded[1]);
        switch (decoded[0])
        {
            case "L":
                left = value * scale;
                break;
            case "R":
                right = value * scale;
                break;
            case "S":
                selector = (value == 1);
                break;
            case "P":
                pressure = value / 10.0f;
                break;
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
