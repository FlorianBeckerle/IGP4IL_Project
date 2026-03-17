using UnityEngine;
using System.IO.Ports;

public class IOTest : MonoBehaviour
{
    private SerialPort serialPort;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        serialPort = new SerialPort("COM3", 9600);
        serialPort.Open();
        serialPort.ReadTimeout = 100;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
