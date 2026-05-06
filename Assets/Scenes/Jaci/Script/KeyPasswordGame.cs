using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine;
using System.CodeDom.Compiler;
using System.IO.IsolatedStorage;
using Unity.VisualScripting;
using System.ComponentModel.Design;

public class KeyPasswordGame : BombMinigame
{

    
    [SerializeField] private int codeLength = 4; //ist die max. Codelänge
    [SerializeField] private TMP_Text codeText; // Ausgabetextfeld

    private string correctCode;  // String das den correcten code angibt
    private string playerInput = ""; // string das der Player dann eingeben soll


  

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateRandomCode();
        
    }

    void GenerateRandomCode()
    {
        correctCode ="";
        for (int i = 0; i < codeLength; i++)
        {
            int randomDigit = Random.Range(0,10);
            correctCode += randomDigit.ToString();

        }

        codeText.text = correctCode;
        Debug.Log("Code ist: " + correctCode); 
    }

    void FadingNumber()
    {
        
    }

    public void PressNumber(string number)
    {
        if (playerInput.Length >= codeLength)
            return;


        playerInput += number;

        Debug.Log ("Eingabe: " + playerInput);

        if (playerInput.Length == codeLength)
        {
            CheckCode();
        }
    }

    void CheckCode()
        {
            if (playerInput == correctCode)
            {
                Debug.Log("Richtig!");

            }
            else
            {
                Debug.Log("Falsch!");
                playerInput ="";
                
            }
        }

    public override void SetEventListeners()
    {
        throw new System.NotImplementedException();
    }

    public override void UnbindEventListeners()
    {
        throw new System.NotImplementedException();
    }

    public override void StartMinigame()
    {
        throw new System.NotImplementedException();
    }

    public override Awaitable<bool> EnterMinigame()
    {
        throw new System.NotImplementedException();
    }

    public override void ExitMinigame()
    {
        throw new System.NotImplementedException();
    }
}