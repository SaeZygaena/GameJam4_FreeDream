using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerJump : MonoBehaviour
{
    private PlayerInputAction inputActions;

    private bool isHoldingJump = false;
    private Rigidbody2D rBody;
    private float jumpForce = 2f;
    private float jumpForceMax = 5f;
    private float addJumpForce = 0f;

    private PlayerState state;


    void Start()
    {
        inputActions = GameObject.FindGameObjectWithTag("GameController").GetComponent<PlayerInputAction>();
        state = GetComponent<PlayerState>();
        rBody = GetComponent<Rigidbody2D>();
        LinkActions(inputActions.getInput());
    }

    void OnDisable()
    {
        UnlinkActions(inputActions.getInput());
    }

    void LinkActions(InputSystem_Actions _inputActions)
    {
        _inputActions.Player.Jump.started += OnJump;
        _inputActions.Player.Jump.performed += OnJump;
        _inputActions.Player.Jump.canceled += OnJump;
    }
    void UnlinkActions(InputSystem_Actions _inputActions)
    {
        _inputActions.Player.Jump.started -= OnJump;
        _inputActions.Player.Jump.performed -= OnJump;
        _inputActions.Player.Jump.canceled -= OnJump;
    }

    void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && state.GetGrounded())
            {

                isHoldingJump = true;
                state.SetIsJumping(true);
                rBody.AddForce(new Vector2(0, 5f), ForceMode2D.Impulse);
                StartCoroutine(AccumulatingJump());
            }

            if (context.canceled)
            {
                isHoldingJump = false;
                addJumpForce = 0;
                StopAllCoroutines();
            }
        }

    IEnumerator AccumulatingJump()
    {
        while (isHoldingJump && addJumpForce < jumpForceMax)
        {
            yield return new WaitForSeconds(0.05f);
            rBody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            addJumpForce += jumpForce;
        }
    }

}