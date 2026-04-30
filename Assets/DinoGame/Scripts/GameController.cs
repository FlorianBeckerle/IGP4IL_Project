using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class GameController : MonoBehaviour
{
    //Runtime
    private GameState _state;
    private float _timer;
    
    [SerializeField]
    private TMP_Text _text;

    [SerializeField] private List<Transform> floors;
    [SerializeField] private Vector3 startPos;
    [SerializeField] private Vector3 endPos;
    [SerializeField] private float floorSpeed = 4f;
    
    //Singleton
    public static GameController instance;

    void Awake()
    {
        //SetFloor
        startPos = floors[floors.Count-1].transform.position;
        endPos = floors[0].transform.position;
        
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    public GameState GetGameState()
    {
        return _state;
    }

    public void SetGameState(GameState state)
    {
        _state = state;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _state = GameState.Start;
        _timer = 0;
    }

    public void PlayerCollision()
    {
        _state = GameState.GameOver;
    }

    public void Reset()
    {
        if (_state != GameState.GameOver) return;
        _state = GameState.Start;
        _timer = 0;
    }

    public void StartGame()
    {
        if (_state == GameState.Start)
        {
            _state = GameState.Running;
        }
        StartCoroutine(MoveFloor());
    }

    // Update is called once per frame
    void Update()
    {
        if (_state == GameState.Running)
        {
            _timer += Time.deltaTime;
            _timer = Mathf.Floor(_timer); //Round to whole numbers
            _text.text = "Score: " + _timer.ToString();
        }

        
        Invoke("StartGame", 5f);
        
    }


    private IEnumerator MoveFloor()
    {
        while (_state == GameState.Running)
        {
            foreach (var floor in floors)
            {
                if (floor.position.x <= endPos.x)
                {
                    floor.position = new Vector2(startPos.x, startPos.y);
                }
                else
                {
                    floor.position = new Vector2(floor.position.x-(floorSpeed * _timer/100) * Time.deltaTime, floor.position.y);
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }
}

public enum GameState
{
    Start,
    Running,
    GameOver,
    None
}
