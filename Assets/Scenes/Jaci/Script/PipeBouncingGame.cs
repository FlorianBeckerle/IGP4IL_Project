using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;
using System.Threading.Tasks;

public class PipeBouncingGame : BombMinigame
{

    //Frage: Welches Objekt ist der Balken?
    [SerializeField] private GameObject movingBar;
    [SerializeField] private GameObject[] movingBalls;
    //Frage: Wie schnell bewegt er sich?
    [SerializeField] private float moveSpeed = 1.5f;
    //Frage: Wie weit darf er sich bewegen?
    [SerializeField] private float minY = 0.6902707f;  
    [SerializeField] private float maxY = 1.72f;
    
    [SerializeField] private float cooldownTime = 5f; // Einstellung für die Strafwartezeit vom Bouncingspiel

    // Frage: In welche Richtung bewegt es sich gerade?
    private int direction = 1;

    private bool isMoving = true; //Kontrolle ob sich das Ding bewegt

    private bool isInTarget = false; //Kontrolle ob es in der Zielfläche drin ist.

    private bool isFinish = false; // bedeutet das es noch nicht startet.


    void Start()
    {
        StartMinigame();
    }
    
    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Stop()
    {
        if (!isMoving) return; 

        isMoving = false; 

        //→ isMoving wird false
        //→ Update ruft MoveBar NICHT mehr auf
        //→ Balken bleibt stehen

        if (isInTarget)
        {
            Debug.Log("Treffer!");
        }
        else
        {
            Debug.Log("NONE");
            StartCoroutine(CooldownRoutine());
        }
    }

    private IEnumerator CooldownRoutine()
    {
        yield return new WaitForSeconds(cooldownTime);

        isMoving = true;
    }


    private void MoveBar()
    {
        //Aktuelle Position vom Balken holen
        //Y-Position etwas erhöhen
        //Prüfen: Ist der Balken oben angekommen?
        //Wenn ja: Richtung umdrehen
        //Prüfen: Ist der Balken unten angekommen?
        //Wenn ja: Richtung umdrehen
        //Neue Position wieder speichern
        foreach(var ball in movingBalls)
        {
                Vector3 position = ball.transform.position;

                position.y += direction * moveSpeed * Time.deltaTime; //Time.deltaTime ist die Framrate damit es flüssig ausschaut. 

            if (position.y >= maxY)  //Cube geht bis zum Max Wert
                {
                    direction = -1;
                }
            if (position.y <= minY) // Cube geht bis zum Min Wert
                {
                    direction = 1;
                }

            ball.transform.position = position;
        }

  

    }

    private void OnTriggerEnter(Collider other) // Kontrolliert ob Ball im Cylinder ist
    {
        if (other.CompareTag("Target"))
        {
            isInTarget = true;    

        }
    
    }

    private void OnTriggerExit(Collider other) // Kontrolliert ob Ball nicht im Cylinder ist
    {
        if (other.CompareTag("Target"))
        {
            isInTarget = false;    
        }
    }

    private IEnumerator LoopBallBounce()
    {
        while (isStarted)
        {
            MoveBar();

            yield return new WaitForEndOfFrame();
        }
    }

    public override void SetEventListeners()
    {
        IOHandler.buttonUsed.AddListener(Stop);

    }

    public override void UnbindEventListeners()
    {
        IOHandler.buttonUsed.RemoveListener(Stop);
    }

    public override void StartMinigame()
    {
        isStarted = true; 
        movingBar = movingBalls[0];
        StartCoroutine(LoopBallBounce()); 
    }

    public override async Awaitable<bool> EnterMinigame()
    {
        SetEventListeners();
        while (!isFinish)
        {
            await Awaitable.NextFrameAsync();
        }
        isFinish = false; 
        return true;
    }

    public override void ExitMinigame()
    {
        UnbindEventListeners();
    }
}
