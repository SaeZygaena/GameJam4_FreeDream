using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class DoorManager : MonoBehaviour
{
    public static event Action OpenDoor;
    private PlayerState state;
    private Animator anim;

    private PlayerInputAction inputActions;

    private bool canBeOpen = false;
    private bool playerInRange = false;
    void Start()
    {
        anim = GetComponent<Animator>();
        inputActions = GameObject.FindGameObjectWithTag("GameController").GetComponent<PlayerInputAction>();
        LinkActions(inputActions.getInput());
    }

    void OnDisable()
    {
        UnlinkActions(inputActions.getInput());
    }

    void LinkActions(InputSystem_Actions _inputActions)
    {
        _inputActions.Player.Interact.started += NextLevel;
        _inputActions.Player.Interact.performed += NextLevel;
        _inputActions.Player.Interact.canceled += NextLevel;
    }

    void UnlinkActions(InputSystem_Actions _inputActions)
    {
        _inputActions.Player.Interact.started -= NextLevel;
        _inputActions.Player.Interact.performed -= NextLevel;
        _inputActions.Player.Interact.canceled -= NextLevel;
    }

    private void NextLevel(InputAction.CallbackContext context)
    {
        if (canBeOpen && playerInRange)
        {     
            Debug.Log("To LAVA LEVEL!");
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.GetComponent<PlayerState>().GetKeys() == 3)
            {
                anim.SetBool("isOpening", true);
                OpenDoor?.Invoke();
                collision.GetComponent<PlayerState>().SetKeys(3);
            }
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        playerInRange = false;
    }

    public void AfterOpening()
    {
        canBeOpen = true;
    }
}
