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
    
    //�ۋ��{�^���������ꂽ���̏���(�w���m�F)
    public void OnClickPurchaseButton()
    {
        AudioManager.Instance.SE(0);
        //�w�������Ă��炤

        OnPurchasesCallback();
    }

    //����ɉۋ����ꂽ���̏���(�X�e�[�W�A�I�v�V�����A�퓬���@�̒ǉ�)
    private void OnPurchasesCallback()
    {
        _infomation.isPay = true;
        PlayerPrefs.SetInt("isPay", 1);
        //�X�e�[�W��I���o����悤�ɂ���
        _stageSelectManager.Purchase();
        //�I�v�V������I���o����悤�ɂ���
        _optionManager.Purchase();

        PurchaseButton.enabled = false;
        PurchaseText1.SetActive(false);
        PurchaseText2.SetActive(false);
        ThankYouText.SetActive(true);
    }
}
