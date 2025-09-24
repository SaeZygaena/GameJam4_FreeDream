using UnityEngine;

public class PlayerInputAction : MonoBehaviour
{
    private InputSystem_Actions inputActions;
    void Awake()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Enable();
    }

    public InputSystem_Actions getInput()
    {
        return inputActions;
    }
}
