using QMVC;
using UnityEngine;

public class JackpotController : BaseController
{
	[SerializeField] JackpotView _view;
    



    override protected void OnInit()
    {
        base.OnInit();
        _view.RegisterDrawOnceBtnPressed(OnDrawOncePressed);
        _view.RegisterDrawTenBtnPressed(OnDrawTenPressed);
      
    }



    private void OnDrawOncePressed()
    {
        
    }

    private void OnDrawTenPressed()
    {

        //规则： 最高为五星也就是最高稀有度
        // 奖池中包含角色、道具、资源
        // 稀有度分为五星、四星、三星、二星、一星
        //五星： 最稀有，奖励最高 只会出角色
        //四星： 比五星稀有，奖励第二高 只会出角色或道具
        //三星： 比四星稀有，奖励第三高 会出低级道具，或较多资源
        //二星： 比三星稀有，奖励第四高 只会出资源
        //一星： 最稀有，奖励最低 只会出较少资源

        //奖品： 奖品稀有度 奖品类型 奖品数量
        //

        //生成规则
        //首先根据稀有度进行概率分配
        //随机获取当前抽的稀有度
        //根据不同稀有度中奖励的类型与概率，再次确定当前抽的奖品类型
        //根据稀有度与奖品类型，确定数量

        //一个奖励的数据包含：稀有度，类型，数量







        this.SendCommand(new ToShowLotteryResultCommand());
    }


    protected override void OnDeInit()
    {
        base.OnDeInit();
        _view.UnregisterDrawOnceBtnPressed(OnDrawOncePressed);
        _view.UnregisterDrawTenBtnPressed(OnDrawTenPressed);

    }

}