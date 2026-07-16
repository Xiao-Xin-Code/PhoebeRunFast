using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SignLoginView : MonoBehaviour
{
    [SerializeField] Button controlButton;

    [SerializeField] InputField usernameInput;
    [SerializeField] InputField passwordInput;


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



    #region Register

    public void RegisterControlPressed(UnityAction onClick)
    {
        controlButton.onClick.AddListener(onClick);
    }

    #endregion


    #region UnRegister

    public void UnRegisterControlPressed(UnityAction onClick)
    {
        controlButton.onClick.RemoveListener(onClick);
    }

    #endregion
}
