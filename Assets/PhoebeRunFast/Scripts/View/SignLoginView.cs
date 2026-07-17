using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SignLoginView : MonoBehaviour
{
    [SerializeField] Button controlButton;
    [SerializeField] Button statusButton;

    [SerializeField] InputField usernameInput;
    [SerializeField] InputField passwordInput;


    [SerializeField] Text controlText;
    [SerializeField] Text statusText;


    public string GetUsername()
    {
        return usernameInput.text;
    }

    public string GetPassword()
    {
        return passwordInput.text;
    }


    public void SetControlInteractable(bool isInteractable)
    {
        controlButton.interactable = isInteractable;
    }


    public void SetStatus(bool isSign)
    {
        usernameInput.text = "";
        passwordInput.text = "";
        statusText.text = isSign ? "返回登陆" : "去注册";
        controlText.text = isSign ? "注册并登陆" : "登陆";
    }


    #region Register

    public void RegisterControlPressed(UnityAction onClick)
    {
        controlButton.onClick.AddListener(onClick);
    }

    public void RegisterStatusPressed(UnityAction onClick)
    {
        statusButton.onClick.AddListener(onClick);
    }

    #endregion


    #region UnRegister

    public void UnRegisterControlPressed(UnityAction onClick)
    {
        controlButton.onClick.RemoveListener(onClick);
    }

    public void UnRegisterStatusPressed(UnityAction onClick)
    {
        statusButton.onClick.RemoveListener(onClick);
    }

    #endregion
}
