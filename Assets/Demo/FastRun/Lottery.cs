using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Lottery : MonoBehaviour
{
    const float basesix = 0.25f;
    const float basefive = 10f;
    const float basefour = 20f;
    const float basethree = 65;
    const float basetwo = 4.5f;
    const float baseone = 0.25f;

    const int upDrawCount = 60;
    const float priceUp = 0.5f;

    const int maxFiveCount = 10;
    const int maxSixCount = 80;


    public Dictionary<string, float> pirces = new Dictionary<string, float>()
    {
        { "Six",0.25f },{ "Five",10f },{ "Four",20f },{ "Three",65f },{ "Two",4.5f },{ "One",0.25f }
    };


    public RectTransform[] points = new RectTransform[10];
    public RectTransform prefab;
    public RectTransform startPoint;
    public List<GameObject> cache = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
		Random.InitState(System.Environment.TickCount);
	}

    int curDrawCount = 0;
    int curFiveFrag = 0;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Draw();
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            foreach (var item in cache)
            {
                Destroy(item);
            }
            cache.Clear();

			Sequence sequence = DOTween.Sequence();
            for (int i = 0; i < 10; i++)
            {
                Draw();
                RectTransform target = Instantiate(prefab, transform);
                target.anchoredPosition = startPoint.anchoredPosition;
                target.localScale = Vector3.zero;
                cache.Add(target.gameObject);
                Vector2 endValue = points[i].anchoredPosition;
                sequence.Append(target.DOAnchorPos(endValue, 0.25f).SetEase(Ease.OutQuad));
				sequence.Join(target.DOScale(1f, 0.2f).SetEase(Ease.OutBack));
			}
            sequence.Play();
        }
    }



    private void Draw()
    {
		curDrawCount++;
		curFiveFrag++;
		if (curDrawCount >= maxSixCount)
		{
			curDrawCount = 0;
			pirces["Six"] = basesix;
			DrawLog("Six");
			return;
		}
		else if (curDrawCount > upDrawCount)
		{
			pirces["Six"] = basesix + (curDrawCount - upDrawCount) * priceUp;
		}

		float totalpirce = 0;
		foreach (var item in pirces)
		{
			totalpirce += item.Value;
		}

		float random = Random.Range(0f, totalpirce);

		float cur = 0;
		foreach (var item in pirces)
		{
			cur += item.Value;

			if (random <= cur)
			{
				if (item.Key == "Six") { curDrawCount = 0; pirces["Six"] = basesix; }
				else if (item.Key == "Five") curFiveFrag = 0;
				else if (curFiveFrag >= maxFiveCount) curFiveFrag = 0;
				DrawLog(item.Key);
				break;
			}
		}
	}

    private void DrawLog(string name)
    {
        Debug.Log($"当前MaxSix：{curDrawCount}，当前MaxFive：{curFiveFrag}，Draw：{name}，当前概率：{pirces["Six"]}");
    }
}
