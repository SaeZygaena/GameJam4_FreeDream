using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerAction : MonoBehaviour
{
    private PlayerInputAction inputActions;

    private bool isHoldingJump = false;
    private Rigidbody2D rBody;
    private float jumpForce = 2f;
    private float jumpForceMax = 5f;
    private float addJumpForce = 0f;

    private PlayerState state;

    private Coroutine jumpCoroutine;
    private Coroutine jetPackCoroutine;

    private bool enableJetPack = false;
    private bool canCheckGrounded = false;

    public GameObject particleEffect;


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
        _inputActions.Player.Jump.started += OnAction;
        _inputActions.Player.Jump.performed += OnAction;
        _inputActions.Player.Jump.canceled += OnAction;

        _inputActions.Player.Cancel.started += OnCancel;
        _inputActions.Player.Cancel.performed += OnCancel;
        _inputActions.Player.Cancel.canceled += OnCancel;
    }
    void UnlinkActions(InputSystem_Actions _inputActions)
    {
        _inputActions.Player.Jump.started -= OnAction;
        _inputActions.Player.Jump.performed -= OnAction;
        _inputActions.Player.Jump.canceled -= OnAction;

        _inputActions.Player.Cancel.started -= OnCancel;
        _inputActions.Player.Cancel.performed -= OnCancel;
        _inputActions.Player.Cancel.canceled -= OnCancel;
    }

    void Update()
    {
        if (state.GetActionType() == PlayerState.ActionType.JetPack && canCheckGrounded)
        {
            if (state.GetGrounded())
            {
                enableJetPack = false;
                rBody.gravityScale = 1;
                canCheckGrounded = false;
                particleEffect.SetActive(false);
                state.SetIsJetPack(false);
            }
        }
    }

    void OnAction(InputAction.CallbackContext context)
    {
        if (state.GetActionType() == PlayerState.ActionType.Jump)
            JumpAction(context);
        if (state.GetActionType() == PlayerState.ActionType.JetPack)
            JetPackAction(context);
    }

    #region //////////////// JUMP ACTION //////////////////

    private void JumpAction(InputAction.CallbackContext context)
    {
        if (context.started && state.GetGrounded() && !state.GetIsDead())
        {

            isHoldingJump = true;
            state.SetIsJumping(true);
            rBody.AddForce(new Vector2(0, 5f), ForceMode2D.Impulse);
            jumpCoroutine = StartCoroutine(AccumulatingJump());
        }

        if (context.canceled && !state.GetIsDead())
        {
            isHoldingJump = false;
            addJumpForce = 0;
            StopCoroutine(jumpCoroutine);
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

    #endregion

    #region /////////////// JETPACK ///////////////////

    private void JetPackAction(InputAction.CallbackContext context)
    {
        if (context.started && !state.GetIsDead())
        {
            if (!enableJetPack)
            {
                canCheckGrounded = false;
                enableJetPack = true;
                state.SetIsJetPack(true);
                particleEffect.SetActive(true);
                if (state.GetGrounded())
                    rBody.AddForce(new Vector2(0, 10f), ForceMode2D.Impulse);
                jetPackCoroutine = StartCoroutine(JetPackGravity());
            }
            else if (enableJetPack)
            {
                rBody.linearVelocity = Vector2.zero;
                rBody.AddForce(new Vector2(0, 5f), ForceMode2D.Impulse);
            }
        }
    }

    private void OnCancel(InputAction.CallbackContext context)
    {
        if (context.started && enableJetPack)
        {
            rBody.gravityScale = 1;
            if (jetPackCoroutine != null)
                StopCoroutine(jetPackCoroutine);
            enableJetPack = false;
            particleEffect.SetActive(false);

        }
    }



    IEnumerator JetPackGravity()
    {
        yield return new WaitForSeconds(0.5f);
        canCheckGrounded = true;
        rBody.gravityScale = 0.3f;
        rBody.linearVelocity = Vector2.zero;
        state.SetIsJetPack(false);
    }

    #endregion

}