
using UnityEngine;
using UnityEngine.EventSystems;

public class JackPot : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    public float speed;
    public float speedSpace;

    public RectTransform[] imgs = new RectTransform[] { };

	


	public void OnBeginDrag(PointerEventData eventData)
	{
		
	}

	public void OnDrag(PointerEventData eventData)
	{
		GetComponent<RectTransform>().anchoredPosition += new Vector2(eventData.delta.x, 0);
		for(int i = 0; i < imgs.Length; ++i)
		{
			float curspeed = speed - i * speedSpace;
			float curDelta = eventData.delta.x * curspeed;
			imgs[i].anchoredPosition += new Vector2(curDelta, 0);
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		//当达到限制位置时，切换（显示内容）
		if(GetComponent<RectTransform>().anchoredPosition.x > 500)
		{
			Debug.Log("达到限制");
		}

		GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
		foreach (var item in imgs)
		{
			item.anchoredPosition = Vector2.zero;
		}
	}


	// Start is called before the first frame update
	void Start()
    {
		speedSpace = speed / imgs.Length;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
