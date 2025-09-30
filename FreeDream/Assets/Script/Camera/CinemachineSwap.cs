using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CinemachineSwap : MonoBehaviour
{
    //private static CinemachineSwap Instance;

    [SerializeField] private CinemachineCamera camFollow;
    private PlayerInputAction inputActions;
    [SerializeField] private Animator anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        /*if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }*/

        camFollow.Follow = GameObject.FindGameObjectWithTag("Player").transform;

        Debug.Log(anim);
    }

    void Start()
    {
        inputActions = GameObject.FindGameObjectWithTag("GameController").GetComponent<PlayerInputAction>();
        //anim = GetComponent<Animator>();
        LinkActions(inputActions.getInput());
        Debug.Log("Start : " + anim);
    }

    void OnDisable()
    {
        UnlinkActions(inputActions.getInput());
    }

    void LinkActions(InputSystem_Actions _inputActions)
    {
        BinocularManager.ObservePanorama += OnSwitch;

    }
    void UnlinkActions(InputSystem_Actions _inputActions)
    {
        BinocularManager.ObservePanorama -= OnSwitch;

    }

    public void OnSwitch(BinocularManager.PanoramaType _type)
    {
        Debug.Log("I'm here OnSwitch : " + anim);
        if (_type == BinocularManager.PanoramaType.PanoramaOne)
            anim.Play("OverWorld");
        else if (_type == BinocularManager.PanoramaType.Following)
            anim.Play("Following");
    }
}
