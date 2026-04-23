using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;


    public Rigidbody RB => rb;
    public Transform GroundCheck => groundCheck;
    public LayerMask GroundLayer => groundLayer;

}