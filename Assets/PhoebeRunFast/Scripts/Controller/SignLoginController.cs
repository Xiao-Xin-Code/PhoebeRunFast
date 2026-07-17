
using System.Collections;
using System.Threading.Tasks;
using Frame;
using QMVC;
using UnityEngine;

public class SignLoginController : BaseController
{
    [SerializeField] SignLoginView _view;


    GlobalSystem _globalSystem;
    AccountSystem _accountSystem;

    bool isSign = false;

    protected override void OnInit()
    {
        base.OnInit();
        _globalSystem = this.GetSystem<GlobalSystem>();
        _accountSystem = this.GetSystem<AccountSystem>();

        this.RegisterEvent<SignLoginActiveEvent>(OnSignLoginActive);

        _view.RegisterControlPressed(OnControlPressed);
        _view.RegisterStatusPressed(OnStatusPressed);


        gameObject.SetActive(false);
    }



    private void OnSignLoginActive(SignLoginActiveEvent e)
    {
        gameObject.SetActive(e.isActive);
        isSign = e.isSign;
        _view.SetStatus(isSign);
    }

    private void OnStatusPressed()
    {
        isSign = !isSign;
        _view.SetStatus(isSign);
    }


    private void OnControlPressed()
    {
        //判断当前状态
        string username = _view.GetUsername();
        string password = _view.GetPassword();

        if (isSign)
        {
            //注册
            MonoService.Instance.StartCoroutine(SignAsync(username, password));
        }
        else
        {
            //登陆
            MonoService.Instance.StartCoroutine(LoginAsync(username, password));
        }

        //无论是登陆还是注册只要成功都直接进入

        //成功，-》进入主页
        //失败，-》提示用户输入错误


    }


    IEnumerator SignAsync(string username, string password)
    {
        Task<bool> task = _accountSystem.SignAccount(username, password);
        yield return new WaitUntil(() => task.IsCompleted);
        if (task.Result)
        {
            this.SendCommand(new OpenTransitionCommand(StageChanged));
        }
        //传递给专门检测并注册的方法
        yield return null;

    }

    IEnumerator LoginAsync(string username, string password)
    {
        //传递给专门检测的方法
        yield return null;
    }



    /// <summary>
    /// 阶段切换方法
    /// </summary>
    private void StageChanged()
    {
        Debug.Log("触发");
        _globalSystem.GlobalModel.Stage.Value = Stage.Main;
    }

}