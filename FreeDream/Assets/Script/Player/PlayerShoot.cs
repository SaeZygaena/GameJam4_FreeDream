using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerShoot : MonoBehaviour
{
    private PlayerInputAction inputActions;
    private PlayerState state;
    private Animator anim;

    [SerializeField] Transform bulletOriginPos;

    public GameObject bullet;

    private bool isAttacking;
    void Start()
    {
        inputActions = GameObject.FindGameObjectWithTag("GameController").GetComponent<PlayerInputAction>();
        state = GetComponent<PlayerState>();
        anim = GetComponent<Animator>();
        LinkActions(inputActions.getInput());
    }

    void OnDisable()
    {
        UnlinkActions(inputActions.getInput());
    }

    void LinkActions(InputSystem_Actions _inputActions)
    {
        _inputActions.Player.Attack.started += OnShoot;
        _inputActions.Player.Attack.performed += OnShoot;
        _inputActions.Player.Attack.canceled += OnShoot;
    }
    void UnlinkActions(InputSystem_Actions _inputActions)
    {
        _inputActions.Player.Attack.started -= OnShoot;
        _inputActions.Player.Attack.performed -= OnShoot;
        _inputActions.Player.Attack.canceled -= OnShoot;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnShoot(InputAction.CallbackContext callback)
    {
        Debug.Log("Here");
        if (callback.performed && !isAttacking)
        {
            anim.SetTrigger("attack");
            isAttacking = true;
        }
    }

    public void animEventAttack()
    {
        GameObject tempo;
        isAttacking = false;
        tempo = Instantiate(bullet, bulletOriginPos.position, Quaternion.identity);
        if (state.GetIsFlipped())
            tempo.GetComponent<BulletBehavior>().direction = -1;
        else
            tempo.GetComponent<BulletBehavior>().direction = 1;
    }
}
