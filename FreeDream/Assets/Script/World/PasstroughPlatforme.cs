using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using System;

public class PasstroughPlatform : MonoBehaviour
{
    [SerializeField] private PlatformEffector2D effector;
    [SerializeField] private float waitTime = 1f;

    [SerializeField] private bool[] activatedPassthought = new bool[2];

    private PlayerInputAction inputActions;

    private bool canBeActivated = false;

    void Start()
    {
        inputActions = GameObject.FindGameObjectWithTag("GameController").GetComponent<PlayerInputAction>();
        LinkActions(inputActions.getInput());
    }

    void OnDisable()
    {
        UnlinkActions(inputActions.getInput());
    }

    void LinkActions(InputSystem_Actions _inputActions)
    {
        _inputActions.Player.GoDown.started += OnPressDown;
        _inputActions.Player.GoDown.performed += OnPressDown;
        _inputActions.Player.GoDown.canceled += OnPressDown;

        _inputActions.Player.Jump.started += OnInteraction;
        _inputActions.Player.Jump.performed += OnInteraction;
        _inputActions.Player.Jump.canceled += OnInteraction;


    }
    void UnlinkActions(InputSystem_Actions _inputActions)
    {
        _inputActions.Player.GoDown.started -= OnPressDown;
        _inputActions.Player.GoDown.performed -= OnPressDown;
        _inputActions.Player.GoDown.canceled -= OnPressDown;

        _inputActions.Player.Jump.started -= OnInteraction;
        _inputActions.Player.Jump.performed -= OnInteraction;
        _inputActions.Player.Jump.canceled -= OnInteraction;

    }

    public void OnInteraction(InputAction.CallbackContext context)
    {
        if (canBeActivated)
        {
            if (context.started)
            {
                activatedPassthought[1] = true;
                OnPassthough();
            }
            if (context.canceled)
                    activatedPassthought[1] = false;
        }
    }

    public void OnPressDown(InputAction.CallbackContext context)
    {
        if (canBeActivated)
        {
            if (context.started)
            {
                activatedPassthought[0] = true;
                OnPassthough();
            }
            if (context.canceled)
                activatedPassthought[0] = false;
        }
    }

    public void OnPassthough()
    {
        Debug.Log("Here");
        if (activatedPassthought[0] && activatedPassthought[1])
        {
            canBeActivated = false;
            StartCoroutine(DisableCollision());
        }
    }

    private IEnumerator DisableCollision()
    {
        effector.colliderMask = effector.colliderMask & ~(1 << LayerMask.NameToLayer("Player"));
        yield return new WaitForSeconds(waitTime);
        effector.colliderMask = effector.colliderMask | (1 << LayerMask.NameToLayer("Player"));
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canBeActivated = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canBeActivated = false;
            activatedPassthought[0] = false;
            activatedPassthought[1] = false;
        }
    }

}
