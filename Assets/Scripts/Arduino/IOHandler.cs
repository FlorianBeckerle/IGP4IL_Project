using System;
using System.IO.Ports;
using UnityEngine;
using UnityEngine.Events;

public class IOHandler : MonoBehaviour
{
    private SerialPort serialPort;
    private float lastCheckup;
    public int timeoutDuration = 500;
    
    public static IOHandler instance;
    
    //Events
    public UnityEvent joystickUsed; 
    public UnityEvent buttonUsed; 
    public UnityEvent potentiometerUsed; 
    public UnityEvent numberpadUsed; 
    
    //Values of Inputs
    public Vector2 joystickInput; //what direction is the joystick pushed
    public bool buttonInput; //is pressed or not
    public float potentiometerInput; //current value of potentiometer
    public string numberpadInput; //last pressed button

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
        SetupEvents();
        serialPort = new SerialPort("COM5", 9600);
        serialPort.Open();
        serialPort.ReadTimeout = (timeoutDuration + 100); //slightly bigger to not get into timeouts from serialport
        serialPort.WriteTimeout = (timeoutDuration + 100);
        
        lastCheckup = Time.time;
    }

    private void SetupEvents()
    {
        joystickUsed = new UnityEvent();
        buttonUsed = new UnityEvent();
        potentiometerUsed = new UnityEvent();
        numberpadUsed = new UnityEvent();
        
        joystickInput = new Vector2(0f, 0f);
        buttonInput = false;
        potentiometerInput = 0f;
        numberpadInput = "";

    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            string[] action = GetInputUpdate();
            if (action == null) return;

            switch (action[0])
            {
                case "Joystick":
                    SetJoyStickInput(action[1]);
                    break;
                case "Button":
                    SetButtonInput(action[1]);
                    break;
                case "Potentiometer":
                    SetPotentiometerInput(action[1]);
                    break;
                case "Numberpad":
                    SetNumberpadInput(action[1]);
                    break;
                case "Checkup":
                    RefreshTimeout();
                    break;
                default:
                    Debug.Log("Undefined Action-Call: " + action[0], this);
                    break;
            }
        }
        catch(Exception e)
        {
            //ignore for now
            //to improve increade timeouts and add checkup calls from the scripts to check connection
        }

        CheckIfTimedOut();

    }

    private void CheckIfTimedOut()
    {
        if (Time.time - lastCheckup > timeoutDuration) //if no checkups have been sent in a while, close application
        {
            Debug.LogError("Connection to Arduino Timed out! Closing Connection.");
            serialPort.Close();
            serialPort.Dispose();
            
            Application.Quit();
        }
    }

    private void SetButtonInput(string s)
    {
        if (s.Equals("released"))
        {
            buttonInput = false;
        }else if (s.Equals("pressed"))
        {
            buttonInput = true;
        }
        
        buttonUsed?.Invoke();
    }

    private void SetPotentiometerInput(string s)
    {
        potentiometerInput = float.Parse(s);
        
        potentiometerUsed?.Invoke();
    }

    private void SetNumberpadInput(string s)
    {
        numberpadInput = s;
        
        numberpadUsed?.Invoke();
    }

    private void RefreshTimeout()
    {
        float curTime = Time.time;
        Debug.Log("Refresh Timeout - " + (curTime - lastCheckup) + " seconds since last checkup");
        lastCheckup = curTime;
    }

    private void SetJoyStickInput(string s)
    {
        string[] input = s.Split(";"); //s having the shema X;Y -- X and Y being Number values between -1 and 1
        
        joystickInput = new Vector2(float.Parse(input[0]), float.Parse(input[1]));
        
        joystickUsed?.Invoke();
    }

    private string[] GetInputUpdate()
    {
        string action = serialPort.ReadLine();
        if (action.Length < 3) //meaning something is wrong with the read or it had a timeout
        {
            return null;
        }
        string[] actions = action.Split(';'); // Example action = "Potentiometer;45"
        Debug.Log("New Action:" + actions[0] + ": " + actions[1]);
        return actions;
    }
}

public enum InputDevices{
    Joystick,
    Button,
    Potentiometer,
    Numberpad,
    
}

public enum Minigames
{
    Frequency,
    Bounce,
}
