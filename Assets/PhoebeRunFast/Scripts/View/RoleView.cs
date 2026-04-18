
using UnityEngine;

public class RoleView : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] Animator animator;

    public Rigidbody RB => rb;
    public Animator Animator => animator;
}
