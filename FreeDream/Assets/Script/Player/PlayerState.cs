using System;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isRunning;
    [SerializeField] private bool isJumping;
    [SerializeField] private bool isJetPack;
    [SerializeField] private bool isFlipped;
    [SerializeField] private bool isDead;

    private int nbKeys = 0;

    void OnEnable()
    {
        Collectible_Item.AddKey += AddKeyCount;
    }

    void OnDisable()
    {
        Collectible_Item.AddKey -= AddKeyCount;
    }

    private void AddKeyCount()
    {
        nbKeys += 1;
    }

    public int GetKeys() { return nbKeys; }
    public void SetKeys(int _nb) { nbKeys = _nb; }

    public enum ActionType
    {
        Jump,
        JetPack
    }

    [SerializeField] private ActionType actionType = ActionType.Jump;

    public void SetActionType(ActionType _type) { actionType = _type; }
    public ActionType GetActionType() { return actionType; }

    public void SetGrounded(bool _state) { isGrounded = _state; }
    public bool GetGrounded() { return isGrounded; }

    public void SetIsRunning(bool _state) { isRunning = _state; }
    public bool GetIsRunning() { return isRunning; }

    public void SetIsJumping(bool _state) { isJumping = _state; }
    public bool GetIsJumping() { return isJumping; }

    public void SetIsJetPack(bool _state) { isJetPack = _state; }
    public bool GetIsJetPack() { return isJetPack; }

    public void SetIsFlipped(bool _state) { isFlipped = _state; }
    public bool GetIsFlipped() { return isFlipped; }

    public void SetIsDead(bool _state) { isDead = _state; }
    public bool GetIsDead() { return isDead; }
}
