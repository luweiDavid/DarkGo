/****************************************************
	文件：FuBenWnd.cs
	作者：David
	邮箱: 1785275942@qq.com
	日期：2019/06/11 15:20   	
	功能：副本界面
*****************************************************/
using Protocol;
using UnityEngine;
using UnityEngine.UI;


public class FuBenWnd : WindowRoot
{
    private Button btnClose; 
    private Button[] btnTaskArr;
    private Transform imgPointerTr;
    private PlayerData pd;

    private void Awake()
    {
        btnClose = transform.Find("BtnClose").GetComponent<Button>();
        btnTaskArr = new Button[6];
        for (int i = 0; i < btnTaskArr.Length; i++)
        {
            btnTaskArr[i]= transform.Find("BtnTask"+(i+1).ToString()).GetComponent<Button>();
        }
        imgPointerTr = transform.Find("ImgPointer").transform;


        for (int i = 0; i < btnTaskArr.Length; i++)
        {
            btnTaskArr[i].OnClickExt(OnBtnTaskClick);
        }
        AddClickListener();
    }

    protected override void InitWnd(object[] args = null)
    {
        base.InitWnd(args);

        pd = mGameRoot.GetPlayerData();
        UpdateData();
    }
    private void UpdateData() {
        int curfbId = (pd.FuBenId % 10000);

        Vector3 pos = btnTaskArr[curfbId - 1].transform.localPosition;
        imgPointerTr.transform.localPosition = new Vector3(pos.x + 10, pos.y + 85);

        for (int i = 0; i < btnTaskArr.Length; i++)
        {
            if (i < curfbId)
            {
                btnTaskArr[i].interactable = true;
            }
            else {
                btnTaskArr[i].interactable = false;
            }
        }
    }

    private void OnBtnTaskClick(string name) { 
        int idx = int.Parse(name.Substring(7, 1));
        idx += 10000;
        CfgMap cfg = mCfgSvc.GetCfgMap(idx); 
        if (cfg == null) {
            Debug.LogError("获取地图配置失败:   " + idx);
            return;
        }
        if (pd.Power < cfg.power) {
            mGameRoot.AddTips("体力不足");
            return;
        }

        FuBenSys.Instance.ReqFuBen(idx);
    }


    private void OnBtnClose()
    {
        SetWndState(false);
    }


    private void AddClickListener()
    {
        btnClose.onClick.AddListener(OnBtnClose); 
    }
}
