using System;
using UnityEngine;
using System.IO.Ports;
using NUnit.Framework.Constraints;
using UnityEngine.InputSystem;

public class IOTest : MonoBehaviour
{
    private SerialPort serialPort;
    
    public bool jumpedPressed = false;
    public bool duckPressed = false;

    private InputAction jumpAction;
    private PlayerMovement pm;

    //Singleton
    public static IOTest instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        serialPort = new SerialPort("COM5", 9600);
        serialPort.Open();
        serialPort.ReadTimeout = 100;
        serialPort.WriteTimeout = 100;

        
    }

    // Update is called once per frame
    void Update()
    {
        if (!serialPort.IsOpen) return;

        try
        {
            string action = serialPort.ReadLine();
            switch (action)
            {
                case "Jump Pressed":
                    Debug.Log(action);
                    jumpedPressed = true;
                    break;
                case "Jump Released":
                    Debug.Log(action);
                    jumpedPressed = false;
                    break;
                case "Duck Pressed":
                    Debug.Log(action);
                    duckPressed = true;
                    break;
                case "Duck Released":
                    Debug.Log(action);
                    duckPressed = false;
                    break;
            }
        }
        catch (Exception e)
        {
            //Ignore
        }
        
        
    }

    private void OnApplicationQuit()
    {
        serialPort.Close();
    }
}
