using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class JackpotView : MonoBehaviour
{
    [SerializeField] Button drawOnceBtn;
    [SerializeField] Button drawTenBtn;


    #region Register

    public void RegisterDrawOnceBtnPressed(UnityAction onClick)
    {
        drawOnceBtn.onClick.AddListener(onClick);
    }

    public void RegisterDrawTenBtnPressed(UnityAction onClick)
    {
        drawTenBtn.onClick.AddListener(onClick);
    }

    #endregion

    #region Unregister
    

    public void UnregisterDrawOnceBtnPressed(UnityAction onClick)
    {
        drawOnceBtn.onClick.RemoveListener(onClick);
    }

    public void UnregisterDrawTenBtnPressed(UnityAction onClick)
    {
        drawTenBtn.onClick.RemoveListener(onClick);
    }

    #endregion

}
    