using Oculus.Platform;
using Oculus.Platform.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseManager : MonoBehaviour
{
    [SerializeField] Button PurchaseButton;
    [SerializeField] GameObject PurchaseText1;
    [SerializeField] GameObject PurchaseText2;
    [SerializeField] GameObject ThankYouText;
    private GameInfomation _infomation;
    private OptionManager _optionManager;
    private StageSelectManager _stageSelectManager;

    void Start()
    {
        _infomation = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameInfomation>();
        _stageSelectManager = GetComponent<StageSelectManager>();
        _optionManager = GetComponent<OptionManager>();
        if (_infomation.isPay)
        {
            PurchaseButton.enabled = false;
            PurchaseText1.SetActive(false);
            PurchaseText2.SetActive(false);
            ThankYouText.SetActive(true);
        }
        else
        {
            PurchaseButton.enabled = true;
            PurchaseText1.SetActive(true);
            PurchaseText2.SetActive(true);
            ThankYouText.SetActive(false);
        }
    }
    
    //課金ボタンが押された時の処理(購入確認)
    public void OnClickPurchaseButton()
    {
        AudioManager.Instance.SE(0);
        //購入をしてもらう

        OnPurchasesCallback();
    }

    //正常に課金された時の処理(ステージ、オプション、戦闘方法の追加)
    private void OnPurchasesCallback()
    {
        _infomation.isPay = true;
        PlayerPrefs.SetInt("isPay", 1);
        //ステージを選択出来るようにする
        _stageSelectManager.Purchase();
        //オプションを選択出来るようにする
        _optionManager.Purchase();

        PurchaseButton.enabled = false;
        PurchaseText1.SetActive(false);
        PurchaseText2.SetActive(false);
        ThankYouText.SetActive(true);
    }
}
