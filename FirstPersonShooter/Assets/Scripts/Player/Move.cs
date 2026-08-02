using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Move : MonoBehaviour
{
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] Transform camHolder;
    [SerializeField] Animator camAnimator;
    [SerializeField] float gravity = -20f;
    [SerializeField] InputActionReference run;
    private CharacterController controller;
    
    Vector2 moveInput;
    private Vector3 velocity;

    public bool canMove = true;

    void OnEnable()
    {
        run.action.Enable();
    }
    void OnDisable()
    {
        run.action.Disable();
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void Update()
    {   
        playerMove();
    }

    private void playerMove()
    {
        if (!canMove) return;
        
        float speed = 5;
        
        Vector3 camForward = Quaternion.Euler(0, camHolder.eulerAngles.y, 0) * Vector3.forward;
        Vector3 camRight = Quaternion.Euler(0, camHolder.eulerAngles.y, 0) * Vector3.right;

        Vector3 moveDir = (camRight * moveInput.x + camForward * moveInput.y).normalized;

        controller.Move(moveDir * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        Vector3 finalMove = velocity * Time.deltaTime;
        controller.Move(finalMove);
    }

}
