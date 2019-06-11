/****************************************************
	文件：GuideWnd.cs
	作者：David
	邮箱: 1785275942@qq.com
	日期：2019/05/31 14:26   	
	功能：引导界面
*****************************************************/

using Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuideWnd : WindowRoot
{
    private Image imgRole;
    private Text txtName;
    private Text txtDes;
    private Button btnNext;

    private CfgGuide curGuideData;
    private PlayerData playerData;
    private string[] dialogArr;
    private int index;

    private void Awake()
    {
        imgRole = transform.Find("BgBottom/ImgRole").GetComponent<Image>();
        txtName = transform.Find("BgBottom/TxtName").GetComponent<Text>();
        txtDes = transform.Find("BgBottom/TxtDes").GetComponent<Text>();
        btnNext = transform.Find("BgBottom/BtnNext").GetComponent<Button>();

        AddClickListener();
    } 

    protected override void InitWnd(object[] args = null)
    {
        base.InitWnd(args);
        playerData = mGameRoot.GetPlayerData();
        if (playerData != null) {
            curGuideData = mCfgSvc.GetCfgGuide(playerData.GuideID); 
        }
        
        UpdateData();
    }

    private void UpdateData() {
        if (curGuideData == null) {
            return;
        }
        dialogArr = curGuideData.dilogArr.Split('#');
        index = 1;

        SetData();
    }
    private void SetData()
    { 
        string[] oneDialog = dialogArr[index].Split('|');
        string sptPath = "";
        int lanIndex = 0;
        if (oneDialog[0] == "0")
        {
            //自己
            lanIndex = 26;
            sptPath = PathDefine.AssassinRoleImg;
            oneDialog[1] = oneDialog[1].Replace("$name", Language.GetString(20));
        }
        else {
            //其他人
            int npcID = curGuideData.npcID;
            switch (npcID)
            {
                case (int)GuideNpcIDType.NpcWiseman:
                    lanIndex = 21;
                    sptPath = PathDefine.WisemanRoleImg; 
                    break;
                case (int)GuideNpcIDType.NpcGeneral:
                    lanIndex = 22;
                    sptPath = PathDefine.GeneralRoleImg;
                    break;
                case (int)GuideNpcIDType.NpcArtisan:
                    lanIndex = 23;
                    sptPath = PathDefine.ArtisanRoleImg;
                    break;
                case (int)GuideNpcIDType.NpcTrader:
                    lanIndex = 24;
                    sptPath = PathDefine.TraderRoleImg;
                    break;
                default:
                    lanIndex = 25;
                    sptPath = PathDefine.DefaultGuideRoleImg;
                    break;
            }
        }
        
        SetText(txtName, Language.GetString(lanIndex));
        SetText(txtDes, oneDialog[1]);
        SetSprite(imgRole, sptPath);   
    }


    private void OnBtnNext() {
        index++;
        if (index >= dialogArr.Length)
        {
            SetWndState(false);
            //请求引导结束
            MainCitySys.Instance.ReqGuide(curGuideData.ID);
            return;
        }
        SetData();
    }


    private void AddClickListener() {
        btnNext.onClick.AddListener(OnBtnNext);
    }
}
