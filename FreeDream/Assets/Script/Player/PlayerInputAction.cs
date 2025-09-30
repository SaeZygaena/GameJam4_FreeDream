using UnityEngine;

public class PlayerInputAction : MonoBehaviour
{
    private static PlayerInputAction Instance;
    private InputSystem_Actions inputActions;
    void Awake()
    {

        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        inputActions = new InputSystem_Actions();
        inputActions.Enable();
    }

    public InputSystem_Actions getInput()
    {
        return inputActions;
    }

    public void OnDisable()
    {
        inputActions?.Disable();
    }
}
