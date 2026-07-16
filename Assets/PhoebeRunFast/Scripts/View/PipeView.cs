using UnityEngine;
using UnityEngine.UI;

public class PipeView : MonoBehaviour
{
    [SerializeField] RectTransform rectTransform;

    public RectTransform RectTransform => rectTransform;

    [SerializeField] BoxCollider2D scoreArea;

    [SerializeField] Image top;
    [SerializeField] Image bottom;

    public Image Top => top;
    public Image Bottom => bottom;

    public void SetScoreAreaActive(float height)
    {
        Vector2 size = scoreArea.size;
        size.y = height;
        scoreArea.size = size;
    }

}
