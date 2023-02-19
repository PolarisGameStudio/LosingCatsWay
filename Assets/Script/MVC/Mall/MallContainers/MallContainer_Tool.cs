using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class MallContainer_Tool : MallContainer
{
    public Reward[] adsDiamond;
    public Reward[] adsMoney;
    public Reward[] adsTool;
    public Reward[] adsFeed;

    public Reward[] diamondFeed;
    public Reward[] diamondLosingCat;
    public Reward[] diamondChangeName;
    public Reward[] diamondFlower;

    private int _adsDiamondMaxCount = 5;
    private int _adsMoneyMaxCount = 1;
    private int _adsToolMaxCount = 1;
    private int _adsFeedMaxCount = 1;

    [Title("UI")] public TextMeshProUGUI adsDiamondCountText;
    public TextMeshProUGUI adsMoneyCountText;
    public TextMeshProUGUI adsToolCountText;
    public TextMeshProUGUI adsFeedCountText;

    public GameObject adsDiamondGetMask;
    public GameObject adsMoneyGetMask;
    public GameObject adsToolGetMask;
    public GameObject adsFeedGetMask;

    public override void Init()
    {
        base.Init();

        App.system.myTime.OnFirstLogin += () =>
        {
            PlayerPrefs.SetInt("adsDiamondCount", 0);
            PlayerPrefs.SetInt("adsMoneyCount", 0);
            PlayerPrefs.SetInt("adsToolCount", 0);
            PlayerPrefs.SetInt("adsFeedCount", 0);
        };
    }

    public override void Refresh()
    {
        int adsDiamondCount = PlayerPrefs.GetInt("adsDiamondCount");
        int adsMoneyCount = PlayerPrefs.GetInt("adsMoneyCount");
        int adsToolCount = PlayerPrefs.GetInt("adsToolCount");
        int adsFeedCount = PlayerPrefs.GetInt("adsFeedCount");

        adsDiamondGetMask.SetActive(adsDiamondCount >= _adsDiamondMaxCount);
        adsMoneyGetMask.SetActive(adsMoneyCount >= _adsMoneyMaxCount);
        adsToolGetMask.SetActive(adsToolCount >= _adsToolMaxCount);
        adsFeedGetMask.SetActive(adsFeedCount >= _adsFeedMaxCount);

        adsDiamondCountText.text = adsDiamondCount + "/" + _adsDiamondMaxCount;
        adsMoneyCountText.text = adsMoneyCount + "/" + _adsMoneyMaxCount;
        adsToolCountText.text = adsToolCount + "/" + _adsToolMaxCount;
        adsFeedCountText.text = adsFeedCount + "/" + _adsFeedMaxCount;
    }

    public void BuyDiamond_Ads()
    {
        App.system.ads.Active(AdsType.MallDiamond, () =>
        {
            int adsDiamondCount = PlayerPrefs.GetInt("adsDiamondCount");
            PlayerPrefs.SetInt("adsDiamondCount", adsDiamondCount + 1);

            GetItem(adsDiamond);
            Refresh();
        });
    }

    public void BuyMoney_Ads()
    {
        App.system.ads.Active(AdsType.MallCoin, () =>
        {
            int adsMoneyCount = PlayerPrefs.GetInt("adsMoneyCount");
            PlayerPrefs.SetInt("adsMoneyCount", adsMoneyCount + 1);

            GetItem(adsMoney);
            Refresh();
        });
    }

    public void BuyTool_Ads()
    {
        App.system.ads.Active(AdsType.MallTool, () =>
        {
            var randomIndex = Random.Range(0, adsTool.Length);

            var rewards = new Reward[1];
            rewards[0] = adsTool[randomIndex];

            int adsToolCount = PlayerPrefs.GetInt("adsToolCount");
            PlayerPrefs.SetInt("adsToolCount", adsToolCount + 1);

            GetItem(rewards);
            Refresh();
        });
    }

    public void BuyFeed_Ads()
    {
        App.system.ads.Active(AdsType.MallFeed, () =>
        {
            int adsFeedCount = PlayerPrefs.GetInt("adsFeedCount");
            PlayerPrefs.SetInt("adsFeedCount", adsFeedCount + 1);

            GetItem(adsFeed);
            Refresh();
        });
    }

    public void BuyFeed_Diamond()
    {
        App.system.confirm.Active(ConfirmTable.Hints_Buy2, () =>
        {
            if (App.system.player.ReduceDiamond(60))
                GetItem(diamondFeed);
            else
                App.system.confirm.Active(ConfirmTable.Hints_NoDiamond);
        });
    }

    public void BuyLosingCat_Diamond()
    {
        App.system.confirm.Active(ConfirmTable.Hints_Buy2, () =>
        {
            if (App.system.player.ReduceDiamond(300))
                GetItem(diamondLosingCat);
            else
                App.system.confirm.Active(ConfirmTable.Hints_NoDiamond);
        });
    }

    public void BuyChangeName_Diamond()
    {
        App.system.confirm.Active(ConfirmTable.Hints_Buy2, () =>
        {
            if (App.system.player.ReduceDiamond(120))
                GetItem(diamondChangeName);
            else
                App.system.confirm.Active(ConfirmTable.Hints_NoDiamond);
        });
    }

    public void BuyFlower_Diamond()
    {
        App.system.confirm.Active(ConfirmTable.Hints_Buy2, () =>
        {
            if (App.system.player.ReduceDiamond(900))
                GetItem(diamondFlower);
            else
                App.system.confirm.Active(ConfirmTable.Hints_NoDiamond);
        });
    }
}