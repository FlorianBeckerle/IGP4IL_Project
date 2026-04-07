using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private IOTest ioTest;
    [SerializeField]
    private Rigidbody2D _rb;
    [SerializeField]
    private Animator _animator;
    
    [Header("Movement")]
    [SerializeField]
    private Vector2 _velocity;
    public bool _isGrounded = false;
    private PlayerState _state;
    
    [Header("Stats")]
    [SerializeField] private float jumpForce = 3f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _state = PlayerState.Idle;
        _rb = GetComponent<Rigidbody2D>();   
        _animator = GetComponent<Animator>();
        _animator.SetInteger("PlayerState", (int)_state);
    }

    // Update is called once per frame
    void Update()
    {
        //Start game on first input
        /*if (GameController.instance.GetGameState() != GameState.Running &&
            (IOTest.instance.jumpedPressed || IOTest.instance.duckPressed))
        {
            GameController.StartGame();
        }*/
        
        if (GameController.instance.GetGameState() == GameState.GameOver)
        {
            _state = PlayerState.Hurt;
            return;
        }
        
        if (Mathf.Abs(_rb.linearVelocity.y) <= 0)
        {
            _isGrounded = true;
            
        }
        else
        {
            _isGrounded = false;
        }
        
        if (IOTest.instance.jumpedPressed)
        {
            Jump();
        }
        else if(_isGrounded && _state != PlayerState.Duck)
        {
            if (GameController.instance.GetGameState() == GameState.Running)
            {
                _state = PlayerState.Walking;
            }
            else
            {
                _state = PlayerState.Idle;    
            }
        }

        if (IOTest.instance.duckPressed)
        {
            Duck();
        }
        else if (_state == PlayerState.Duck)//if player is ducked
           {
               if (GameController.instance.GetGameState() == GameState.Running)
               {
                   _state = PlayerState.Walking;
               }
               else
               {
                   _state = PlayerState.Idle;    
               }
               
           }
        
        //Optional: Check if state switched since last input
       _animator.SetInteger("PlayerState", (int)_state); 
    }

    void Jump()
    {
        if (!_isGrounded || _state == PlayerState.Duck) return;
        _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); //Add force to y (up) direction 
        if (_state != PlayerState.Jump) _state = PlayerState.Jump;
    }

    void Duck()
    {
        if (!_isGrounded || _state == PlayerState.Jump) return; //if not grounded and not ducked exit 
        if(_state != PlayerState.Duck) _state = PlayerState.Duck;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Obstacle")
        {
            Debug.Log("Obstacle hit: " + other.gameObject.name);
        }
    }
    
}


public enum PlayerState{
 Jump,
 Duck,
 Idle,
 Walking,
 Hurt,
 None
}