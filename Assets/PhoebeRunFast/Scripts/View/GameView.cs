using UnityEngine;

public class GameView : MonoBehaviour
{
    [SerializeField] Transform[] lanes;

    public Transform[] Lanes => lanes;
}