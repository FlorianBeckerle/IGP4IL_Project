using UnityEngine;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using System.Threading.Tasks;
using UnityEngine.InputSystem;

public class KeyPasswordGame : BombMinigame
{

    
    [SerializeField] private int codeLength = 4; //ist die max. Codelänge
    [SerializeField] private TMP_Text codeText; // Ausgabetextfeld
    [SerializeField] float fadeSpeed = 5f;
    private string correctCode;  // String das den correcten code angibt
    private string playerInput = ""; // string das der Player dann eingeben soll




  

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartMinigame();
    }

    void GenerateRandomCode()
    {
        correctCode ="";
        for (int i = 0; i < codeLength; i++)
        {
            int randomDigit = Random.Range(0,10);
            correctCode += randomDigit.ToString();

        }
        
        Debug.Log("Code ist: " + correctCode); 
    }

    IEnumerator FadeText()
    {
        codeText.text = correctCode;

        yield return new WaitForSeconds(5f);
        
        Color textColor = codeText.color;

        while (textColor.a > 0)
        {
            textColor.a -= fadeSpeed*Time.deltaTime;
            codeText.color = textColor;

            yield return null;
        }
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
                BombTimer.instance.AddSeconds(10);
                light.TurnOff();
                ExitMinigame();
            }
            else
            {
                Debug.Log("Falsch!");
                playerInput ="";
                
            }
        }

    public override void SetEventListeners()
    {
        IOHandler.numberpadUsed.AddListener(KeyPressed);
    }

    private void KeyPressed()
    {
        PressNumber(IOHandler.numberpadInput);
    }

    public override void UnbindEventListeners()
    {
        IOHandler.numberpadUsed.RemoveListener(KeyPressed);
    }

    public override void StartMinigame()
    {
        wantsToExit = false;
        light.TurnOn();
        GenerateRandomCode();
    }

    public override async Awaitable<bool> EnterMinigame()
    {
        StartCoroutine(FadeText());
        SetEventListeners();
        while (!wantsToExit)
        {
            await Awaitable.NextFrameAsync();

        }
        wantsToExit = false;
        return true;
    }

    public override void ExitMinigame()
    {
        UnbindEventListeners();
        wantsToExit = true;

        playerInput = "";
        DelayTillNextStart();

    }
}