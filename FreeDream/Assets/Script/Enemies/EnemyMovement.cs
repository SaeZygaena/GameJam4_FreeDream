using System.Runtime.CompilerServices;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Transform[] targetPos;
    [SerializeField] private float speed;
    [SerializeField] private float bonusSpeed = 0;
    [SerializeField] private Transform forwardPoint;
    [SerializeField] private Transform backwardPoint;
    [SerializeField] private Transform limitChaseOne;
    [SerializeField] private Transform limitChaseTwo;

    [SerializeField] private bool isPatrol;
    [SerializeField] private bool isChase;
    private int currentTargetIndex;

    private float direction = 1;

    [SerializeField] private bool flipHasBeenFixed = false;

    public void SetBonusSpeed(float _bonus) { bonusSpeed = _bonus; }
    public void SetPatrol(bool _bo) { isPatrol = _bo; }
    public void SetChase(bool _bo) { isChase = _bo; }
    public bool GetChase() { return isChase; }
    void Start()
    {
        isPatrol = true;
    }



    // Update is called once per frame
    void Update()
    {
        if (isPatrol)
        {
            transform.position = Vector2.MoveTowards(transform.position,
            targetPos[currentTargetIndex].position,
            (speed + bonusSpeed) * Time.deltaTime);

            if (Vector2.Distance(transform.position, targetPos[currentTargetIndex].position) < 0.05f)
            {
                currentTargetIndex++;

                if (flipHasBeenFixed)
                    flipHasBeenFixed = false;
                else
                    transform.localScale = new Vector3(transform.localScale.x * -1f, 1f, 1f);
                if (currentTargetIndex == targetPos.Length)
                    currentTargetIndex = 0;
            }
        }

        if (isChase)
        {
            CheckForward();

            if (Vector2.Distance(transform.position, limitChaseOne.position) > 1f
            && Vector2.Distance(transform.position, limitChaseTwo.position) > 1f)
            {
                transform.position += new Vector3(direction * (speed + bonusSpeed) * Time.deltaTime, 0f, 0f);
            }
        }
    }

    public void FixFlip()
    {
        Vector3 tempo = transform.localScale;

        transform.localScale = transform.position.x < targetPos[0].position.x ? new Vector3(-1f, 1f, 1f) : new Vector3(1f, 1f, 1f);

        if (tempo != transform.localScale)
            flipHasBeenFixed = true;
    }

    private void CheckForward()
    {
        direction = forwardPoint.position.x > backwardPoint.position.x ? 1 : -1;
    }
}
