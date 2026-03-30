using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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


	public Camera mainCamera;
	private CircularMaskEffect maskEffect;


	void Start()
    {
		mainCamera.clearFlags = CameraClearFlags.Depth;
		mainCamera.cullingMask = 0;
		mainCamera.depth = mainCamera.depth + 1;
		mainCamera.orthographic = true;
		mainCamera.orthographicSize = 1;


		// 添加效果到相机
		maskEffect = mainCamera.gameObject.AddComponent<CircularMaskEffect>();
		// 执行转场
		maskEffect.FullTransition(
			onMiddle: () => {
				Debug.Log("完全黑屏 - 可以切换场景");
				//LoadScene();
			},
			onComplete: () => {
				Debug.Log("转场完成");
			}
		).Play();


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
			sequence.Append(setObj.DOScale(Vector3.one, 1f));
		}
		else
		{
            sequence.Append(setObj.DOScale(new Vector3(0, 0, 1), 1f));
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
				Sequence sequence = HomeSequence(false);
                sequence.OnComplete(() => { shopObj.SetActive(true); canvasGroup.interactable = true; });
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
}
