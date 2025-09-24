using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInputAction inputActions;
    private Vector2 moveDirection;
    private Rigidbody2D rBody;

    private PlayerState state;

    [SerializeField] private float speed = 100;
    void Start()
    {
        state = GetComponent<PlayerState>();
        rBody = GetComponent<Rigidbody2D>();
        if (!rBody)
            Debug.LogError("Missing rBody on Player");

        //inputActions = GetComponent<PlayerInputAction>();
        LinkActions(inputActions.getInput());
    }

    void OnDisable()
    {
        UnlinkActions(inputActions.getInput());
    }

    void FixedUpdate()
    {
        rBody.linearVelocity = new Vector2(moveDirection.x * Time.deltaTime * speed, rBody.linearVelocityY);
    }

    void LinkActions(InputSystem_Actions _inputActions)
    {
        _inputActions.Player.Move.started += OnMove; 
        _inputActions.Player.Move.performed += OnMove; 
        _inputActions.Player.Move.canceled += OnMove;
    }

    void UnlinkActions(InputSystem_Actions _inputActions)
    {
        _inputActions.Player.Move.started -= OnMove;
        _inputActions.Player.Move.performed -= OnMove;
        _inputActions.Player.Move.canceled -= OnMove;
    }

    void FlipPlayer()
    {
        if (moveDirection.x < 0)
        {
            state.SetIsFlipped(false);
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveDirection.x > 0)
        {
            state.SetIsFlipped(true);
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            moveDirection = context.ReadValue<Vector2>();
            FlipPlayer();
        }

        if (context.canceled)
        {
            moveDirection = Vector2.zero;
        }
    }
}