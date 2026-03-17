using System;
using UnityEngine;
using System.IO.Ports;

public class IOTest : MonoBehaviour
{
    private SerialPort serialPort;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        serialPort = new SerialPort("COM5", 9600);
        serialPort.Open();
        serialPort.ReadTimeout = 5000;
    }

    // Update is called once per frame
    void Update()
    {
        if (!serialPort.IsOpen) return;
        string action = "";
        try
        { 
            action = serialPort.ReadLine();
        }
        catch (Exception e){}
        

        switch (action)
        {
            case "Jump Pressed":
                Debug.Log(action);
                break;
            case "Jump Released":
                Debug.Log(action);
                break;
            case "Duck Pressed":
                Debug.Log(action);
                break;
            case "Duck Released":
                Debug.Log(action);
                break;
        }
    }
}
