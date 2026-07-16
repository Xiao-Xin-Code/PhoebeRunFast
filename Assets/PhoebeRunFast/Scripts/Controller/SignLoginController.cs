
using QMVC;
using UnityEngine;

public class SignLoginController : BaseController
{
    [SerializeField] SignLoginView _view;


    protected override void OnInit()
    {
        base.OnInit();

        this.RegisterEvent<SignLoginActiveEvent>(OnSignLoginActive);



        gameObject.SetActive(false);
    }



    private void OnSignLoginActive(SignLoginActiveEvent e)
    {
        gameObject.SetActive(e.isActive);
    }



    private void OnControlPressed()
    {
        //判断当前状态

        //无论是登陆还是注册只要成功都直接进入

        //成功，-》进入主页
        //失败，-》提示用户输入错误


    }


    private void OnSignPressed()
    {
        //传递给专门检测并注册的方法

    }
    
    private void OnLoginPressed()
    {
        //传递给专门检测的方法
    }


}