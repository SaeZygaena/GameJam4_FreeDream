using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerAction : MonoBehaviour
{
    private PlayerInputAction inputActions;

    private bool isHoldingJump = false;
    private Rigidbody2D rBody;
    [SerializeField] private float jumpForce = 10f;
    //[SerializeField] private float jumpForceMax = 5f;
    //private float addJumpForce = 0f;

    private PlayerState state;

    private Coroutine jumpCoroutine;
    private Coroutine jetPackCoroutine;

    private bool enableJetPack = false;
    private bool canCheckGrounded = false;

    public GameObject particleEffect;

    private bool isPressDown = false;

    private bool timerJumpFinished = false;


    void Start()
    {
        inputActions = GameObject.FindGameObjectWithTag("GameController").GetComponent<PlayerInputAction>();
        state = GetComponent<PlayerState>();
        rBody = GetComponent<Rigidbody2D>();
        LinkActions(inputActions.getInput());
    }

    private void FixedUpdate()
    {
        if (isHoldingJump && !timerJumpFinished)
        {
            rBody.linearVelocity = new Vector2(0, jumpForce * Time.fixedDeltaTime);
        }
    }

    IEnumerator TimerJump()
    {
        yield return new WaitForSeconds(0.4f);
        timerJumpFinished = true;
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

        _inputActions.Player.GoDown.started += ctx => isPressDown = true;
        _inputActions.Player.GoDown.canceled += ctx => isPressDown = false;
    }
    void UnlinkActions(InputSystem_Actions _inputActions)
    {
        _inputActions.Player.Jump.started -= OnAction;
        _inputActions.Player.Jump.performed -= OnAction;
        _inputActions.Player.Jump.canceled -= OnAction;

        _inputActions.Player.Cancel.started -= OnCancel;
        _inputActions.Player.Cancel.performed -= OnCancel;
        _inputActions.Player.Cancel.canceled -= OnCancel;

        _inputActions.Player.GoDown.started -= ctx => isPressDown = true;
        _inputActions.Player.GoDown.canceled -= ctx => isPressDown = false;
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
        if (!isPressDown && !state.GetIsNotMovable())
        {
            if (state.GetActionType() == PlayerState.ActionType.Jump)
                JumpAction(context);
            if (state.GetActionType() == PlayerState.ActionType.JetPack)
                JetPackAction(context);
        }
    }

    #region //////////////// JUMP ACTION //////////////////

    private void JumpAction(InputAction.CallbackContext context)
    {
        if (context.started && state.GetGrounded() && !state.GetIsDead())
        {
            
            jumpCoroutine = StartCoroutine(TimerJump());
            isHoldingJump = true;
            state.SetIsJumping(true);
            //rBody.AddForce(new Vector2(0, 5f), ForceMode2D.Impulse);
            //jumpCoroutine = StartCoroutine(AccumulatingJump());
        }

        if (context.canceled && !state.GetIsDead())
        {
            timerJumpFinished = false;
            isHoldingJump = false;
            //addJumpForce = 0;
            rBody.gravityScale = 3;
            if (jumpCoroutine != null)
                StopCoroutine(jumpCoroutine);
        }
    }

   /* IEnumerator AccumulatingJump()
    {
        while (isHoldingJump && addJumpForce < jumpForceMax)
        {
            yield return new WaitForSeconds(0.05f);
            rBody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            addJumpForce += 1;
        }
    }*/

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
            state.SetIsJetPack(false);
            particleEffect.SetActive(false);
        }
    }



    IEnumerator JetPackGravity()
    {
        yield return new WaitForSeconds(0.5f);
        canCheckGrounded = true;
        rBody.gravityScale = 0.3f;
        rBody.linearVelocity = Vector2.zero;
    }

    #endregion

}