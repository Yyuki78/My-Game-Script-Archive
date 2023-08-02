using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.Localization.Settings;

public class TutorialManager : MonoBehaviour
{
    public TitleManager _titleManager;
    [SerializeField] GameObject Board;
    [SerializeField] GameObject Controller;
    [SerializeField] GameObject SamplePiece;
    [SerializeField] GameObject PiecePutPlace;
    [SerializeField] GameObject FirstPanelButton;
    [SerializeField] GameObject[] GameStartButtons = new GameObject[2];

    [SerializeField] GameObject HeaderText;
    [SerializeField] TextMeshProUGUI ExplanationText;
    [SerializeField] TextMeshProUGUI ButtonGuideText;

    [TextArea]
    [SerializeField] string[] ExplanationTexts;
    [TextArea]
    [SerializeField] string[] ButtonGuideTexts;

    [SerializeField] GameObject[] UIButtonGuides = new GameObject[6];
    [SerializeField] GameObject MoveButtonGuide;
    [SerializeField] GameObject RotateButtonGuide;
    [SerializeField] GameObject[] GripButtonGuides = new GameObject[2];

    [Header("Language")]
    [SerializeField] GameObject SettingButton;
    [SerializeField] GameObject SettingPanel;
    [SerializeField] Image JapaneseButton;
    [SerializeField] Image EnglishButton;
    [SerializeField] Sprite[] buttonFlame = new Sprite[2];

    private bool once = true;

    public enum TutorialState
    {
        TelopEffect,
        UIClick,
        Moving,
        FinishMoving,
        RotatingView,
        FinishRotatingView,
        GrabPiece,
        GoStageSelect
    }
    public TutorialState _currentState { get; private set; }

    private void Awake()
    {
        Board.SetActive(false);
        Controller.SetActive(false);
        SamplePiece.SetActive(false);
        PiecePutPlace.SetActive(false);
        FirstPanelButton.SetActive(false);
        GameStartButtons[0].SetActive(false);
        GameStartButtons[1].SetActive(false);
        HeaderText.SetActive(false);
        HeaderText.transform.localScale = new Vector3(1, 0, 1);
        HeaderText.transform.localPosition = new Vector3(0, -0.2f, 0.0119f);
        ExplanationText.gameObject.SetActive(false);
        ExplanationText.transform.localScale = new Vector3(1, 0, 1);
        ButtonGuideText.gameObject.SetActive(false);
        for (int i = 0; i < UIButtonGuides.Length; i++)
            UIButtonGuides[i].gameObject.SetActive(false);
        MoveButtonGuide.SetActive(false);
        RotateButtonGuide.SetActive(false);
        GripButtonGuides[0].SetActive(false);
        GripButtonGuides[1].SetActive(false);

        SettingButton.SetActive(false);
        SettingPanel.SetActive(false);

        SetState(TutorialState.TelopEffect);
    }

    private void Start()
    {
        if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[1])
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1];
            JapaneseButton.sprite = buttonFlame[0];
            EnglishButton.sprite = buttonFlame[1];
            if (PlayerPrefs.GetInt("Language", 1) == 0)
            {
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
                JapaneseButton.sprite = buttonFlame[1];
                EnglishButton.sprite = buttonFlame[0];
            }
        }
        else
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
            JapaneseButton.sprite = buttonFlame[1];
            EnglishButton.sprite = buttonFlame[0];
            if (PlayerPrefs.GetInt("Language", 0) == 1)
            {
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1];
                JapaneseButton.sprite = buttonFlame[0];
                EnglishButton.sprite = buttonFlame[1];
            }
        }
        for (int i = 0; i < ExplanationTexts.Length; i++)
            ExplanationTexts[i] = LocalizationSettings.StringDatabase.GetLocalizedString(tableReference: "TextData", tableEntryReference: "TutrialText" + (i + 1).ToString());
        for (int i = 0; i < ButtonGuideTexts.Length; i++)
            ButtonGuideTexts[i] = LocalizationSettings.StringDatabase.GetLocalizedString(tableReference: "TextData", tableEntryReference: "TutrialUIText" + (i + 1).ToString());
    }

    public void ChangeActivePanel()
    {
        AudioManager.Instance.SE(0);
        if (SettingPanel.activeSelf)
            SettingPanel.SetActive(false);
        else
            SettingPanel.SetActive(true);
    }

    public void ChangeLanguage(int num)
    {
        if (PlayerPrefs.GetInt("Language", 2) == num)
            return;
        AudioManager.Instance.SE(0);
        switch (num)
        {
            case 0:
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
                JapaneseButton.sprite = buttonFlame[1];
                EnglishButton.sprite = buttonFlame[0];
                PlayerPrefs.SetInt("Language", 0);
                break;
            case 1:
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1];
                JapaneseButton.sprite = buttonFlame[0];
                EnglishButton.sprite = buttonFlame[1];
                PlayerPrefs.SetInt("Language", 1);
                break;
        }
        for (int i = 0; i < ExplanationTexts.Length; i++)
            ExplanationTexts[i] = LocalizationSettings.StringDatabase.GetLocalizedString(tableReference: "TextData", tableEntryReference: "TutrialText" + (i + 1).ToString());
        for (int i = 0; i < ButtonGuideTexts.Length; i++)
            ButtonGuideTexts[i] = LocalizationSettings.StringDatabase.GetLocalizedString(tableReference: "TextData", tableEntryReference: "TutrialUIText" + (i + 1).ToString());
        switch (_currentState)
        {
            case TutorialState.TelopEffect:
                break;
            case TutorialState.UIClick:
                ExplanationText.text = ExplanationTexts[0];
                ButtonGuideText.text = ButtonGuideTexts[0];
                break;
            case TutorialState.Moving:
                ExplanationText.text = ExplanationTexts[1];
                ButtonGuideText.text = ButtonGuideTexts[1];
                break;
            case TutorialState.FinishMoving:
                ExplanationText.text = ExplanationTexts[2];
                ButtonGuideText.text = ButtonGuideTexts[2];
                break;
            case TutorialState.RotatingView:
                ExplanationText.text = ExplanationTexts[2];
                ButtonGuideText.text = ButtonGuideTexts[2];
                break;
            case TutorialState.FinishRotatingView:
                ExplanationText.text = ExplanationTexts[2];
                ButtonGuideText.text = ButtonGuideTexts[2];
                break;
            case TutorialState.GrabPiece:
                ExplanationText.text = ExplanationTexts[3];
                ButtonGuideText.text = ButtonGuideTexts[3];
                break;
            case TutorialState.GoStageSelect:
                ExplanationText.text = ExplanationTexts[4];
                ButtonGuideText.text = ButtonGuideTexts[0];
                break;
        }
    }

    public void SetState(TutorialState state)
    {
        _currentState = state;
        switch (state)
        {
            case TutorialState.TelopEffect:
                HeaderText.SetActive(true);
                HeaderText.transform.DOScaleY(1f, 2.5f).SetEase(Ease.InQuart);
                HeaderText.transform.DOLocalMoveY(0.2f, 3f).SetDelay(3f)
                    .OnComplete(() => { SetState(TutorialState.UIClick); });
                break;
            case TutorialState.UIClick:
                Board.SetActive(true);
                ExplanationText.gameObject.SetActive(true);
                ExplanationText.text = ExplanationTexts[0];
                ExplanationText.transform.DOScaleY(1f, 1.5f).SetEase(Ease.OutCirc)
                    .OnComplete(() => {
                        SettingButton.SetActive(true);
                        ButtonGuideText.gameObject.SetActive(true);
                        ButtonGuideText.text = ButtonGuideTexts[0];
                        FirstPanelButton.SetActive(true);
                        Controller.SetActive(true);
                        for (int i = 0; i < UIButtonGuides.Length; i++)
                            UIButtonGuides[i].SetActive(true);
                    });
                break;
            case TutorialState.Moving:
                break;
            case TutorialState.FinishMoving:
                AudioManager.Instance.SE(24);
                ExplanationText.transform.DOScaleY(0f, 0.75f).SetEase(Ease.InQuart)
                    .OnComplete(() => {
                        ExplanationText.text = ExplanationTexts[2];
                        ExplanationText.transform.DOScaleY(1f, 1.5f).SetEase(Ease.OutCirc)
                            .OnComplete(() => {
                                MoveButtonGuide.SetActive(false);
                                ButtonGuideText.text = ButtonGuideTexts[2];
                                RotateButtonGuide.SetActive(true);
                                SetState(TutorialState.RotatingView);
                            });
                    });
                break;
            case TutorialState.RotatingView:
                break;
            case TutorialState.FinishRotatingView:
                AudioManager.Instance.SE(24);
                ExplanationText.transform.DOScaleY(0f, 0.75f).SetEase(Ease.InQuart)
                    .OnComplete(() => {
                        SetState(TutorialState.GrabPiece);
                    });
                break;
            case TutorialState.GrabPiece:
                ExplanationText.text = ExplanationTexts[3];
                ExplanationText.transform.DOScaleY(1f, 1.5f).SetEase(Ease.OutCirc)
                    .OnComplete(() => {
                        ButtonGuideText.text = ButtonGuideTexts[3];
                        RotateButtonGuide.SetActive(false);
                        GripButtonGuides[0].SetActive(true);
                        GripButtonGuides[1].SetActive(true);
                        SamplePiece.SetActive(true);
                        PiecePutPlace.SetActive(true);
                    });
                break;
            case TutorialState.GoStageSelect:
                AudioManager.Instance.SE(24);
                ExplanationText.transform.DOScaleY(0f, 0.75f).SetEase(Ease.InQuart)
                    .OnComplete(() => {
                        ExplanationText.text = ExplanationTexts[4];
                        ExplanationText.transform.DOScaleY(1f, 1.5f).SetEase(Ease.OutCirc)
                            .OnComplete(() => {
                                ButtonGuideText.text = ButtonGuideTexts[0];
                                RotateButtonGuide.SetActive(false);
                                for (int i = 0; i < UIButtonGuides.Length; i++)
                                    UIButtonGuides[i].SetActive(true);
                                GripButtonGuides[0].SetActive(false);
                                GripButtonGuides[1].SetActive(false);
                                GameStartButtons[0].SetActive(true);
                                GameStartButtons[1].SetActive(true);
                            });
                    });
                break;
        }
    }

    private void Update()
    {
        if(OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch).magnitude > 0f)
        {
            if (_currentState != TutorialState.Moving) return;
            SetState(TutorialState.FinishMoving);
        }
        if (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch).magnitude > 0f)
        {
            if (_currentState != TutorialState.RotatingView) return;
            SetState(TutorialState.FinishRotatingView);
        }
    }

    public void OnClickTutorialUI(int num)
    {
        switch (num)
        {
            case 0:
                AudioManager.Instance.SE(0);
                AudioManager.Instance.SE(24);
                FirstPanelButton.SetActive(false);
                ExplanationText.transform.DOScaleY(0f, 0.75f).SetEase(Ease.InQuart)
                    .OnComplete(() => {
                        ExplanationText.text = ExplanationTexts[1];
                        ExplanationText.transform.DOScaleY(1f, 1.5f).SetEase(Ease.OutCirc)
                            .OnComplete(() => {
                                ButtonGuideText.text = ButtonGuideTexts[1];
                                for (int i = 0; i < UIButtonGuides.Length; i++)
                                    UIButtonGuides[i].gameObject.SetActive(false);
                                MoveButtonGuide.SetActive(true);
                                SetState(TutorialState.Moving);
                            });
                    });
                break;
            case 1:
                if (!once) return;
                once = false;
                AudioManager.Instance.SE(3);
                ExplanationText.transform.DOScaleY(0f, 2f).SetEase(Ease.InQuart)
                    .OnComplete(() => {
                        _titleManager.gameObject.SetActive(true);
                        _titleManager.FinishTutorial();
                        Destroy(gameObject);
                    });
                break;
        }
    }
}
