using System.Collections;
using TMPro;
using UnityEngine;

public class BombTimer : MonoBehaviour
{

    [SerializeField] private int startTime = 300; //5 min default
    [SerializeField] private TMP_Text timerText;

    [Header("Runtime Info")] private int currentTime = 0; 

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentTime = startTime;
        StartCoroutine(StartBombTimer());
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private IEnumerator StartBombTimer()
    {
        while (currentTime > 0)
        {
            yield return new WaitForSeconds(1f); //every second
            currentTime -= 1;
            timerText.text = calculateTime(currentTime);
        }
        
        timerText.text = "Bomb Exploded";
    }


    private string calculateTime(int time)
    {
        int minutes = time / 60;
        int seconds = time % 60;
        string timeString = string.Format("{0:00}:{1:00}", minutes, seconds);
        
        
        return timeString;
    }

    public void AddSeconds(int seconds)
    {
        currentTime += + seconds;
    }
}
