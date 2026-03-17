using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private IOTest ioTest;
    [SerializeField]
    private Rigidbody2D _rb;
    
    [Header("Movement")]
    [SerializeField]
    private Vector2 velocity;
    public bool _isGrounded = false;
    public bool _isDucked = false;
    
    [Header("Stats")]
    [SerializeField] private float jumpForce = 3f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();   
    }

    // Update is called once per frame
    void Update()
    {
        if( Mathf.Abs(_rb.linearVelocity.y) <= 0){ _isGrounded = true;}else{ _isGrounded = false; }
        
        if (IOTest.instance.jumpedPressed)
        {
            Jump();
        }

        if (IOTest.instance.duckPressed)
        {
            Duck(true);
        }
        else
        {
           Duck(false); 
        }
    }

    void Jump()
    {
        if (!_isGrounded) return;
        _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); //Add force to y (up) direction 
    }

    void Duck(bool isDucked)
    {
        _isDucked = isDucked;
        float yScale = 1;
        if(isDucked) yScale /= 2;
        
        this.transform.localScale = new Vector3(this.transform.localScale.x, yScale, this.transform.localScale.z);
        
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Obstacle")
        {
            Debug.Log("Obstacle hit: " + other.gameObject.name);
        }
    }
    
}
