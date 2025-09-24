using UnityEngine;

public class PlayerState : MonoBehaviour
{
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isRunning;
    [SerializeField] private bool isJumping;
    [SerializeField] private bool isJetPack;
    [SerializeField] private bool isFlipped;

    public void SetGrounded(bool _state) {isGrounded = _state;}
    public bool GetGrounded() {return isGrounded;}

    public void SetIsRunning(bool _state) {isRunning = _state;}
    public bool GetIsRunning() {return isRunning;}

    public void SetIsJumping(bool _state) {isJumping = _state;}
    public bool GetIsJumping() {return isJumping;}

    public void SetIsJetPack(bool _state) {isJetPack = _state;}
    public bool GetIsJetPack() {return isJetPack;}
    
    public void SetIsFlipped(bool _state) {isFlipped = _state;}
    public bool GetIsFlipped() {return isFlipped;}
}
