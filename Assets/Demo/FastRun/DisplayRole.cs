using System.Collections.Generic;
using System.Xml;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayRole : MonoBehaviour
{
    public List<int> roles = new List<int>();
    public Transform cur;
    public int curIndex;
    public Transform prefab;

    public Transform left;
    public Transform center;
    public Transform right;


    public Button leftBtn;
    public Button rightBtn;

    public RectTransform roleMenu;

    public CanvasGroup canvasGroup;


    public Button btn1;
    public Button btn2;
    public Button btn3;

    public Button setBtn;
    public Button exitSetBtn;

    public Button joinBtn;

    public GameObject shopObj;
    public RectTransform setObj;

    public string state = "Home";

	public Image mask;
	public Image load;

	public TMP_Text appName;

	void Start()
    {
		//mask.material.SetFloat("_Aspect", mask.rectTransform.rect.width / mask.rectTransform.rect.height);
		Transitions();

		cur = Instantiate(prefab);
        cur.gameObject.SetActive(true);
		cur.position = center.position + new Vector3(0, 1, 0);
		leftBtn.onClick.AddListener(ToLeft);
        rightBtn.onClick.AddListener(ToRight);

        btn1.onClick.AddListener(Btn1Pressed);
        btn2.onClick.AddListener(Btn2Pressed);
        btn3.onClick.AddListener(Btn3Pressed);

        setBtn.onClick.AddListener(SetPressed);
        exitSetBtn.onClick.AddListener(ExitSetPressed);

		canvasGroup.interactable = true;
	}

    bool isBusy = false;

    private void ToLeft()
    {
        if (isBusy) return;
        isBusy = true;
        Transform leftRole = Instantiate(prefab);
        leftRole.gameObject.SetActive(true);
        leftRole.position = left.position + new Vector3(0, 1, 0);
        if (curIndex == 0) curIndex = roles.Count - 1;
        else curIndex = curIndex - 1;
        Transform one = cur;
        cur = leftRole;

        canvasGroup.interactable = false;
		Sequence mainSequence = DOTween.Sequence();
        Sequence sequence1 = DOTween.Sequence();
        sequence1.Append(one.DORotate(new Vector3(0, -90, 0), 0.5f));
		sequence1.Join(roleMenu.DOAnchorPosY(-roleMenu.rect.height, 0.5f));
		sequence1.Append(one.DOMoveX(right.position.x, 2f));
        Sequence sequence2 = DOTween.Sequence();
        sequence2.Append(cur.DORotate(new Vector3(0, -90, 0), 0.5f));
        sequence2.Append(cur.DOMoveX(center.position.x, 2f));
        sequence2.Append(cur.DORotate(new Vector3(0, 0, 0), 0.5f));
		sequence2.Join(roleMenu.DOAnchorPosY(150f, 0.5f));
		mainSequence.Join(sequence1);
        mainSequence.Join(sequence2);
		mainSequence.OnComplete(() => { Destroy(one.gameObject); isBusy = false; canvasGroup.interactable = true; });
        mainSequence.Play();
    }

    private void ToRight()
    {
		if (isBusy) return;
		isBusy = true;
		Transform rightRole = Instantiate(prefab);
		rightRole.gameObject.SetActive(true);
		rightRole.position = right.position + new Vector3(0, 1, 0);
		if (curIndex == roles.Count - 1) curIndex = 0;
		else curIndex = curIndex + 1;
		Transform one = cur;
		cur = rightRole;

        canvasGroup.interactable = false;
		Sequence mainSequence = DOTween.Sequence();
		Sequence sequence1 = DOTween.Sequence();
		sequence1.Append(one.DORotate(new Vector3(0, 90, 0), 0.5f));
		sequence1.Join(roleMenu.DOAnchorPosY(-roleMenu.rect.height, 0.5f));
		sequence1.Append(one.DOMoveX(left.position.x, 2f));
		Sequence sequence2 = DOTween.Sequence();
		sequence2.Append(cur.DORotate(new Vector3(0, 90, 0), 0.5f));
		sequence2.Append(cur.DOMoveX(center.position.x, 2f));
		sequence2.Append(cur.DORotate(new Vector3(0, 0, 0), 0.5f));
		sequence2.Join(roleMenu.DOAnchorPosY(150f, 0.5f));
		mainSequence.Join(sequence1);
		mainSequence.Join(sequence2);
		mainSequence.OnComplete(() => { Destroy(one.gameObject); isBusy = false; canvasGroup.interactable = true; });
		mainSequence.Play();
	}


    private Sequence RoleSequence(bool open)
    {
        Sequence sequence = DOTween.Sequence();
        if (open)
        {
			sequence.Join(leftBtn.GetComponent<RectTransform>().DOAnchorPosX(125f, 0.5f));
			sequence.Join(rightBtn.GetComponent<RectTransform>().DOAnchorPosX(-125f, 0.5f));
			sequence.Join(roleMenu.DOAnchorPosY(150f, 0.5f));
		}
        else
        {
			sequence.Join(leftBtn.GetComponent<RectTransform>().DOAnchorPosX(-125f, 0.5f));
			sequence.Join(rightBtn.GetComponent<RectTransform>().DOAnchorPosX(125f, 0.5f));
            sequence.Join(roleMenu.DOAnchorPosY(-roleMenu.rect.height, 0.5f));
		}
        return sequence;
	}

    private Sequence HomeSequence(bool open)
    {
		Sequence sequence = DOTween.Sequence();
        if (open)
        {
            sequence.Append(joinBtn.GetComponent<RectTransform>().DOAnchorPosY(300, 1f));
		}
        else
        {
			sequence.Append(joinBtn.GetComponent<RectTransform>().DOAnchorPosY(-300, 1f));
		}
		return sequence;
	}

    private Sequence SetSequence(bool open)
    {
		Sequence sequence = DOTween.Sequence();
		if (open)
		{
			sequence.Append(setObj.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack));
		}
		else
		{
			sequence.Append(setObj.DOScale(new Vector3(0, 0, 1), 0.15f).SetEase(Ease.InBack));
		}
		return sequence;
	}

	private Sequence TransitionSequence(bool open)
	{
		Sequence sequence = DOTween.Sequence();
		if (open)
		{
			sequence.Append(DOTween.To(() => mask.material.GetFloat("_Radius"), x => mask.material.SetFloat("_Radius", x), 0, 0.4f).SetEase(Ease.InOutQuad));
		}
		else
		{
			sequence.Append(DOTween.To(() => mask.material.GetFloat("_Radius"), x => mask.material.SetFloat("_Radius", x), 1, 0.5f).SetEase(Ease.OutBack));
		}
		return sequence;
	}



	private void Btn1Pressed()
    {
        switch (state)
        {
            case "Home":
				canvasGroup.interactable = false;
				Sequence mainSequence = DOTween.Sequence();
				Sequence sequence1 = HomeSequence(false);
				Sequence sequence2 = RoleSequence(true);
                mainSequence.Append(sequence1);
				mainSequence.Append(sequence2);
                mainSequence.OnComplete(() => canvasGroup.interactable = true);
                mainSequence.Play();
                state = "Role";
				break;
            case "Shop":
				canvasGroup.interactable = false;
				shopObj.gameObject.SetActive(false);
				Sequence sequence = RoleSequence(true);
                sequence.OnComplete(() => canvasGroup.interactable = true);
                sequence.Play();
				state = "Role";
				break;
            default:
                break;
        }
	}

    private void Btn2Pressed()
    {
        switch (state)
        {
            case "Role":
				canvasGroup.interactable = false;
				Sequence mainSequence = DOTween.Sequence();
				Sequence sequence1 = RoleSequence(false);
				Sequence sequence2 = HomeSequence(true);
				mainSequence.Append(sequence1);
				mainSequence.Append(sequence2);
				mainSequence.OnComplete(() => canvasGroup.interactable = true);
				mainSequence.Play();
				state = "Home";
				break;
            case "Shop":
				canvasGroup.interactable = false;
				shopObj.SetActive(false);
				Sequence sequence = HomeSequence(true);
                sequence.OnComplete(() => { canvasGroup.interactable = true; });
                sequence.Play();
				state = "Home";
				break;
            default:
                break;
        }

	}

    private void Btn3Pressed()
    {
        switch (state)
        {
			case "Role":
				canvasGroup.interactable = false;
				Sequence sequence1 = RoleSequence(false);
				sequence1.OnComplete(() => { shopObj.SetActive(true); canvasGroup.interactable = true; });
				sequence1.Play();
				state = "Shop";
				break;
			case "Home":
				canvasGroup.interactable = false;
				Sequence sequence2 = HomeSequence(false);
                sequence2.OnComplete(() => { shopObj.SetActive(true); canvasGroup.interactable = true; });
				sequence2.Play();
				state = "Shop";
				break;
			default:
				break;
		}
    }

    private void SetPressed()
    {
        setObj.parent.gameObject.SetActive(true);
		canvasGroup.interactable = false;
		Sequence sequence = SetSequence(true);
        sequence.OnComplete(() => canvasGroup.interactable = true);
        sequence.Play();
	}

	private void ExitSetPressed()
	{
		canvasGroup.interactable = false;
		Sequence sequence = SetSequence(false);
        sequence.OnComplete(() => { setObj.gameObject.SetActive(false); canvasGroup.interactable = true; });
		sequence.Play();
	}


	private void Transitions()
	{
		VertexGradient gradient = new VertexGradient();
		gradient.topLeft = new Color(1, 1, 1, 0);
		gradient.topRight = new Color(1, 1, 1, 0);
		gradient.bottomLeft = new Color(1, 1, 1, 0);
		gradient.bottomRight = new Color(1, 1, 1, 0);
		appName.colorGradient = gradient;

		Sequence mainSequence = DOTween.Sequence();
		Sequence sequence1 = TransitionSequence(true);
		sequence1.OnComplete(() => load.gameObject.SetActive(true));
		Sequence sequence2 = DOTween.Sequence();
		sequence2.Append(load.rectTransform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack));
		sequence2.Append(DOTween.To(() => 0f, x =>
		{
			gradient.topLeft = new Color(1, 1, 1, x);
			gradient.topRight = new Color(1, 1, 1, x);
		}, 1, 1).SetEase(Ease.Linear).OnUpdate(() => appName.colorGradient = gradient));
		sequence2.Append(DOTween.To(() => 0f, x =>
		{
			gradient.bottomLeft = new Color(1, 1, 1, x);
			gradient.bottomRight = new Color(1, 1, 1, x);
		}, 1, 1).SetEase(Ease.Linear).OnUpdate(() => appName.colorGradient = gradient));
		sequence2.Append(load.rectTransform.DOScale(new Vector3(0, 0, 1), 0.15f).SetEase(Ease.InBack).OnComplete(() => load.gameObject.SetActive(false)));
		Sequence sequence3 = TransitionSequence(false);
		mainSequence.Append(sequence1);
		mainSequence.Append(sequence2);
		mainSequence.Append(sequence3);
		mainSequence.OnComplete(() => mask.gameObject.SetActive(false));
		mainSequence.Play();
	}

}
