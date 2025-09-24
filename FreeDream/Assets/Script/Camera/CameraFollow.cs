using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private Vector3 pos_offset;

    [SerializeField]
    [Range(0f, 10f)]
    private float time_offset;

    private Vector3 velocity;

    public bool following_player = true;

 

    public bool isFixed = false;

    public float zoomSpeed = 50f;


    void OnEnable()
    {
      
    }

    void OnDisable()
    {
    }



    void Update()
    {
      
        if (following_player)
            follow_player();
    }

    private void follow_player()
    {
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(player.transform.position.x, player.transform.position.y, -10) + pos_offset, ref velocity, time_offset);
    }
}