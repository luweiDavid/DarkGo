/****************************************************
    文件：StrongWnd.cs
	作者：David
    邮箱: 1785275942@qq.com
    日期：2019/6/1 14:34:44
	功能：Nothing
*****************************************************/

using Protocol;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class StrongWnd : WindowRoot 
{
    private Button btnClose;
    private Button btnStrong;
    private Image imgIcon;
    private Text txtCurStar;
    private Transform[] starTrs; 
    private Text txtHpPreDes; 
    private Text txtHurtPreDes; 
    private Text txtDefPreDes; 
    private Text txtHpAfterDes;
    private Text txtHurtAfterDes;
    private Text txtDefAfterDes; 
    private Text txtNeedLv;
    private Text txtConsumeCoin; 
    private Text txtConsumeCrystal;
    private Transform fullToHideTr;
    private Text txtTotalCoin;  
    public Toggle[] toggleArray;
    private int curToggleIndex;


    private PlayerData playerData;
    private CfgStrongData nextStrongCfg;

    private void Awake()
    {
        btnClose = transform.Find("BgBottom/BtnClose").GetComponent<Button>();
        btnStrong = transform.Find("RightPanel/Bottom/BtnStrong").GetComponent<Button>();
        imgIcon = transform.Find("RightPanel/Up/IconBg/ImgIcon").GetComponent<Image>();
        txtCurStar = transform.Find("RightPanel/Up/TxtStar").GetComponent<Text>();
        starTrs = new Transform[10];
        for (int i = 0; i < starTrs.Length; i++)
        {
            starTrs[i] = transform.Find("RightPanel/Up/StarContainer/StarBg" + i.ToString() + "/ImgStar").GetComponent<Transform>();
            starTrs[i].gameObject.SetActive(false);
        }
        txtHpPreDes = transform.Find("RightPanel/Middle/PropsCon/HpCon/TxtPreDes").GetComponent<Text>();
        txtHurtPreDes = transform.Find("RightPanel/Middle/PropsCon/HurtCon/TxtPreDes").GetComponent<Text>();
        txtDefPreDes = transform.Find("RightPanel/Middle/PropsCon/DefCon/TxtPreDes").GetComponent<Text>();
        txtHpAfterDes = transform.Find("RightPanel/Middle/PropsCon/FullToHideCon/HpCon/TxtAfterDes").GetComponent<Text>();
        txtHurtAfterDes = transform.Find("RightPanel/Middle/PropsCon/FullToHideCon/HurtCon/TxtAfterDes").GetComponent<Text>();
        txtDefAfterDes = transform.Find("RightPanel/Middle/PropsCon/FullToHideCon/DefCon/TxtAfterDes").GetComponent<Text>();
        txtNeedLv = transform.Find("RightPanel/Middle/PropsCon/FullToHideCon/LvDesCon/NeedLv").GetComponent<Text>();
        txtConsumeCoin = transform.Find("RightPanel/Middle/PropsCon/FullToHideCon/Con/CoinCon/ConsumeCoin").GetComponent<Text>();
        txtConsumeCrystal = transform.Find("RightPanel/Middle/PropsCon/FullToHideCon/Con/CrystalCon/ConsumeCrystal").GetComponent<Text>();
        fullToHideTr = transform.Find("RightPanel/Middle/PropsCon/FullToHideCon").GetComponent<Transform>();
        txtTotalCoin = transform.Find("RightPanel/Bottom/Bg/TotalCoin").GetComponent<Text>();

        AddClickListener();
        for (int i = 0; i < toggleArray.Length; i++)
        { 
            toggleArray[i].OnValueChgedExt(OnToggleValueChged);
            toggleArray[i].isOn = false;
        } 
    }

    private void InitTogglesState() {
        for (int i = 0; i < toggleArray.Length; i++)
        {
            toggleArray[i].isOn = false;
        }
    }

    /// <summary>
    /// 当点击的toggle的isOn为true时调用
    /// </summary>
    /// <param name="str">toggle名称</param>
    private void OnToggleValueChged(string str)
    {
        int pos = int.Parse(str.Substring(6, 1));
        curToggleIndex = pos; 
        Debug.Log(pos); 
        UpdateData(playerData);
    }

    protected override void InitWnd()
    {
        base.InitWnd();

        //每次打开界面刷新第一个toggle的数据 
        InitTogglesState();
        toggleArray[0].isOn = true;
        playerData = mGameRoot.GetPlayerData();
        UpdateData(playerData);
    }

    public void UpdateData(PlayerData data) {
        
        if (data == null) { 
            data = mGameRoot.GetPlayerData();
        }
        string iconPath = "";
        int curStar = data.Strong[curToggleIndex];
        CfgStrongData cfg = mCfgSvc.GetStrongData(curToggleIndex, curStar);
        if (cfg == null) {
            Debug.LogError("获取强化数据配置错误");
            return;
        }
        int addHp = mCfgSvc.GetStrongProTotalAddition(curToggleIndex, curStar,(int)PropertyType.Hp);
        int addHurt = mCfgSvc.GetStrongProTotalAddition(curToggleIndex, curStar, (int)PropertyType.Hurt);
        int addDef = mCfgSvc.GetStrongProTotalAddition(curToggleIndex, curStar, (int)PropertyType.Def);


        //下一个星级配置数据
        nextStrongCfg = mCfgSvc.GetStrongData(curToggleIndex, curStar + 1);
        int addHpAfter = 0;
        int addHurtAfter = 0;
        int addDefAfter = 0;
        int needLv = 0;
        int consumeCoin = 0;
        int consumeCrystal = 0;
        if (nextStrongCfg == null)
        {
            fullToHideTr.gameObject.SetActive(false); 
        }
        else {
            fullToHideTr.gameObject.SetActive(true);
            addHpAfter = nextStrongCfg.addHp;
            addHurtAfter = nextStrongCfg.addHurt;
            addDefAfter = nextStrongCfg.addDef;
            needLv = nextStrongCfg.minLv;
            consumeCoin = nextStrongCfg.coin;
            consumeCrystal = nextStrongCfg.crystal;
        } 
        
        int totalCoin = data.Coin; 
        switch (curToggleIndex)
        {
            case 0:
                //头部
                iconPath = PathDefine.TouKuiIcon;
                break;
            case 1:
                //身体
                iconPath = PathDefine.BodyIcon;
                break;
            case 2:
                //腰部
                iconPath = PathDefine.YaoBuIcon;
                break;
            case 3:
                //手臂
                iconPath = PathDefine.HandIcon;
                break;
            case 4:
                //腿部
                iconPath = PathDefine.LegIcon;
                break;
            case 5:
                //脚部
                iconPath = PathDefine.FootIcon;
                break;
        } 
        SetSprite(imgIcon, iconPath);
        SetText(txtCurStar, string.Format(Language.GetString(94), curStar));
        for (int i = 0; i < starTrs.Length; i++)
        {
            if (i <= curStar - 1)
            {
                starTrs[i].gameObject.SetActive(true);
            }
            else {
                starTrs[i].gameObject.SetActive(false);
            }
        }

        SetText(txtHpPreDes, string.Format(Language.GetString(95), addHp));
        SetText(txtHurtPreDes, string.Format(Language.GetString(96), addHurt));
        SetText(txtDefPreDes, string.Format(Language.GetString(97), addDef));
        SetText(txtHpAfterDes, string.Format(Language.GetString(98), addHpAfter));
        SetText(txtHurtAfterDes, string.Format(Language.GetString(98), addHurtAfter));
        SetText(txtDefAfterDes, string.Format(Language.GetString(98), addDefAfter));
        SetText(txtNeedLv, string.Format(Language.GetString(89), needLv));
        SetText(txtConsumeCoin, consumeCoin);
        SetText(txtConsumeCrystal, string.Format(Language.GetString(9), consumeCrystal, data.Crystal));
        SetText(txtTotalCoin, totalCoin); 
    }


    private void OnBtnClose()
    {
        SetWndState(false); 
    }


    private void OnBtnStrong()
    { 
        if (playerData != null && nextStrongCfg != null) { 
            if (playerData.Strong[curToggleIndex] < 10)
            {
                if (playerData.Level < nextStrongCfg.minLv)
                {
                    mGameRoot.AddTips(Language.GetString(60));
                    return;
                }
                else if(playerData.Coin < nextStrongCfg.coin)
                {
                    mGameRoot.AddTips(Language.GetString(62));
                    return;
                } 
                else if (playerData.Crystal < nextStrongCfg.crystal)
                {
                    mGameRoot.AddTips(Language.GetString(61));
                    return;
                }

                MainCitySys.Instance.ReqStrong(curToggleIndex);
            } 
        }
        else
        { 
            mGameRoot.AddTips(Language.GetString(63));
            return;
        }
    }

    private void AddClickListener()
    {
        btnClose.onClick.AddListener(OnBtnClose);
        btnStrong.onClick.AddListener(OnBtnStrong); 
    }
}