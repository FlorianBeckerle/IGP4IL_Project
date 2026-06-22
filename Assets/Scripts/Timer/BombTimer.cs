using System.Collections;
using TMPro;
using UnityEngine;

public class BombTimer : MonoBehaviour
{

    [SerializeField] private int startTime = 300; //5 min default
    [SerializeField] private TMP_Text timerText;

    [Header("Runtime Info")] private int currentTime = 0;
    
    
    [Header("Screen UI")]
    [SerializeField]
    private TMP_Text t_Score;

    private int highScore = 0;
    [SerializeField]
    private TMP_Text t_Info;
    

    [SerializeField]
    public bool isPaused = false;

    //singleton patter
    public static BombTimer instance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentTime = startTime;

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        timerText.text = calculateTime(startTime);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartTimer()
    {
        StartCoroutine(StartBombTimer());
    }
    private IEnumerator StartBombTimer()
    {
        highScore = 0;
        t_Score.text = highScore.ToString();
        
        while (currentTime > 0)
        {
            while (isPaused)
            {
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(1f); //every second
            currentTime -= 1;
            highScore +=10;
            timerText.text = calculateTime(currentTime);
            
            //update highscore:
            t_Score.text = "Score: " + highScore.ToString();
        }
        
        timerText.text = "Bomb Exploded";
    }

    public int GetTime()
    {
        return currentTime;
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
        highScore += seconds*10;
        t_Score.text = "Score: " + highScore.ToString();
        StartCoroutine(ShowInfoText());
    }

    public void PauseResumeTimer()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            timerText.color = Color.red;
        }
        else
        {
            timerText.color = Color.white;
        }
    }

    private IEnumerator ShowInfoText()
    {
        t_Info.enabled = true;
        yield return new WaitForSeconds(1f);
        t_Info.enabled = false;
    }
}
