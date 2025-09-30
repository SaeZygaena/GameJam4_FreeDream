using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class BinocularManager : MonoBehaviour
{
    public enum PanoramaType
    {
        Following,
        PanoramaOne
    }
    public static event Action<PanoramaType> ObservePanorama;

    [SerializeField] private AudioManager.CodeOST panoramaOST;
    [SerializeField] private AudioManager.CodeOST currentOST;
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
                ObservePanorama?.Invoke(PanoramaType.PanoramaOne);
                AudioManager.Instance.PlayMusic(AudioManager.CodeOST.dragon);
                isLooking = true;
            }
            else
            {
                ObservePanorama?.Invoke(PanoramaType.Following);
                AudioManager.Instance.PlayMusic(AudioManager.CodeOST.level_volcano);
                isLooking = false;
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
