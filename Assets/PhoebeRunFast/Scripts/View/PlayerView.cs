using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Collider highCollider;
    [SerializeField] Collider lowCollider;



    public Rigidbody RB => rb;
    public Transform GroundCheck => groundCheck;
    public LayerMask GroundLayer => groundLayer;
    public Collider HighCollider => highCollider;
    public Collider LowCollider => lowCollider;

}