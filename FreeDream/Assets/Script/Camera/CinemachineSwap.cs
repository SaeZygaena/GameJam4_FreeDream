using UnityEngine;
using UnityEngine.InputSystem;

public class CinemachineSwap : MonoBehaviour
{
    private PlayerInputAction inputActions;
    private Animator anim;

    private bool overworldCam = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputActions = GameObject.FindGameObjectWithTag("GameController").GetComponent<PlayerInputAction>();
        anim = GetComponent<Animator>();
        LinkActions(inputActions.getInput());
    }

    void OnDisable()
    {
        UnlinkActions(inputActions.getInput());
    }

    void LinkActions(InputSystem_Actions _inputActions)
    {
        /*_inputActions.Player.Camera.started += OnSwitch;
        _inputActions.Player.Camera.performed += OnSwitch;
        _inputActions.Player.Camera.canceled += OnSwitch;*/

        BinocularManager.ObservePanorama += OnSwitch;
        BinocularManager.ObservePanorama += OnSwitch;
        BinocularManager.ObservePanorama += OnSwitch;

    }
    void UnlinkActions(InputSystem_Actions _inputActions)
    {
        /*_inputActions.Player.Camera.started -= OnSwitch;
        _inputActions.Player.Camera.performed -= OnSwitch;
        _inputActions.Player.Camera.canceled -= OnSwitch;*/

        BinocularManager.ObservePanorama += OnSwitch;
        BinocularManager.ObservePanorama += OnSwitch;
        BinocularManager.ObservePanorama += OnSwitch;

    }

    public void OnSwitch(bool _bo)
    {
        if (overworldCam)
            anim.Play("OverWorld");
        else
            anim.Play("Following");
        overworldCam = !overworldCam;
    }

    /*public void OnSwitch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (overworldCam)
            {
                anim.Play("Following");
            }
            else
            {
                anim.Play("OverWorld");
            }
            overworldCam = !overworldCam;
        }

    }*/
}
