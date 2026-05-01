using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;

public class PipeBouncingGame : MonoBehaviour
{

    //Frage: Welches Objekt ist der Balken?
    [SerializeField] private Transform movingBar;
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
    
    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Pipe Minigame startet");
    }

    // Update is called once per frame
    void Update()
    {

        Debug.Log("Update wird ausgeführt");

        if (isMoving)
        {
            MoveBar();
        }
        


    }

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

        Vector3 position = movingBar.position;

        position.y += direction * moveSpeed * Time.deltaTime; //Time.deltaTime ist die Framrate damit es flüssig ausschaut. 

        if (position.y >= maxY)  //Cube geht bis zum Max Wert
        {
            direction = -1;
        }
        if (position.y <= minY) // Cube geht bis zum Min Wert
        {
            direction = 1;
        }

        movingBar.position = position;

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


}
