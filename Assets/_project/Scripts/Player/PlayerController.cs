using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float xclamp = 7.5f;
    [SerializeField] float zclamp = 4.5f;
    
    Vector2 movement;
    bool jumpRequested;
    Rigidbody rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        HandleMovementInput();
        HandleJumpInput();
    }
    public void OnMove(InputValue value)
    {
        movement = value.Get<Vector2>();
    }
    public void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            jumpRequested = true;
        }
    }
    public void HandleMovementInput()
    {
        Vector3 currentPos = rb.position;
        Vector3 moveDirection = new Vector3(movement.x, 0f, movement.y);
        Vector3 updatedPos = currentPos + moveDirection * moveSpeed * Time.fixedDeltaTime;
        updatedPos.x = Mathf.Clamp(updatedPos.x, -xclamp, xclamp);
        updatedPos.z = Mathf.Clamp(updatedPos.z, -zclamp, zclamp);
        rb.MovePosition(updatedPos);
    }
    public void HandleJumpInput()
    {
        if (jumpRequested)
        {
            rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
            jumpRequested = false;
            Debug.Log("Jumping");
        }
       
    }
    


}
