using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO.Ports;
using System.Threading;
using System;

public class BNOController : MonoBehaviour {

    public int baudRate = 9600;

    private SerialPort controller;
    private string messageFromController;
    private bool runThread = true;

    public bool Connected = false;
    public string portName = "";

    Quaternion sensorRotation;

    public Text azimuth;
    public Text tilt;
    public Text skew;

    public Text lat;
    public Text lon;

    [Range(0, 360)]
    public float x;
    public float y;
    public float z;

    public string latitude;
    public string longitude;

    public int system;
    public int gyro;
    public int accel;
    public int magnet;

    public static float Map(float value, float r1, float r2, float m1, float m2)
    {
        float dist = value - r1;
        float range1 = r2 - r1;
        float range2 = m2 - m1;
        return m1 + ((dist / range1) * range2);
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

    bool mapLoaded = false;

    // Update is called once per frame
    void Update()
    {
        transform.rotation = sensorRotation;

        azimuth.text = "Azimuth: " + Math.Round(x, 4);
        tilt.text = "Tilt: "
            + Math.Round(Map(z, -90.0f, 0.0f, 0.0f, 90.0f), 4);
        skew.text = "Skew: " + Math.Round(y, 4);
        lat.text = "Lat: " + latitude;
        lon.text = "Long: " + longitude;

        
        if (!mapLoaded)
        {
            MapLoader ml = FindObjectOfType<MapLoader>();
            ml.LoadMapAsync();
            mapLoaded = true;
        }             
    }


    void ProcessMessage(string message)
    {
        Debug.Log(message);
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
        if (message.StartsWith("LON:"))
        {
            longitude = message.Substring(4);
        }
        if (message.StartsWith("LAT:"))
        {
            latitude = message.Substring(4);
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
                    Debug.Log(messageFromController);
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
            controller = new SerialPort(portName, baudRate);
            controller.ReadTimeout = 100;
            controller.Open();
        }
    }
}
