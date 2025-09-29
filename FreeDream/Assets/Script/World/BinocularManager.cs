using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class BinocularManager : MonoBehaviour
{
    public static event Action<bool> ObservePanorama;
    private bool playerInRange = false;
    private bool isLooking = false;
    private PlayerInputAction inputActions;
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
        _inputActions.Player.Interact.started += EnableParonama;
        _inputActions.Player.Interact.performed += EnableParonama;
        _inputActions.Player.Interact.canceled += EnableParonama;
    }
    void UnlinkActions(InputSystem_Actions _inputActions)
    {
        _inputActions.Player.Interact.started -= EnableParonama;
        _inputActions.Player.Interact.performed -= EnableParonama;
        _inputActions.Player.Interact.canceled -= EnableParonama;

    }

    private void EnableParonama(InputAction.CallbackContext context)
    {
        if (playerInRange == true && context.started)
        {
            if (!isLooking)
            {
                ObservePanorama?.Invoke(true);
                isLooking = true;
            }
            else
            {
                ObservePanorama?.Invoke(false);
                isLooking = true;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
