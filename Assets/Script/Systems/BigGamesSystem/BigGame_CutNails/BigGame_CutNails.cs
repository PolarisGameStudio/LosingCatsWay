using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public class BigGame_CutNails : BigGameBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    #region Variables

    [SerializeField] private Canvas canvas;
    
    [Title("Paw")]
    [SerializeField] private RectTransform pawRect;
    [SerializeField] private CanvasGroup hurtMask;

    [Title("Nails")]
    [SerializeField] private RectTransform[] nails;

    [Title("UI")]
    [SerializeField] private Image fillBar;
    [SerializeField] private RectTransform barRect;
    [SerializeField] private RectTransform pauseRect;
    [SerializeField] private RectTransform howRect;
    [SerializeField] private Image pauseBg;
    [SerializeField] private RectTransform pauseMenuRect;

    [Title("Values")]
    [SerializeField] private int randomNailValue;
    [SerializeField] private int basicNailValue;

    [Title("Sensors")]
    [SerializeField] private Sensor_Nails[] nailSensors;
    [SerializeField] private Sensor_Paw pawSensor;

    [Title("CutEffect")]
    [SerializeField] private RectTransform cutEffect;

    [Title("DoTween")]
    [SerializeField] private Vector2 barOrigin;
    private Vector2 barOffset;
    [SerializeField] private List<Vector2> nailOrigins;
    [SerializeField] private Vector2 pauseOrigin;
    [SerializeField] private Vector2 howOrigin;
    [SerializeField] private Vector2 pawOrigin;
    
    [ReadOnly][SerializeField] private List<int> nailValues;
    private int maxNailValue;

    private Sequence tweenInSeq;

    #endregion

    #region Override

    public override void Open()
    {
        ResetTween();
        base.Open();
    }

    public override void Init()
    {
        base.Init();
        chance = hearts.Length;

        for (int i = 0; i < nailSensors.Length; i++)
            nailSensors[i].ResetNail();

        int total = randomNailValue;
        nailValues = new List<int>();
        for (int i = 0; i < nailSensors.Length; i++)
        {
            if (i >= nailSensors.Length - 1)
            {
                nailValues.Add(total + basicNailValue);
                nailSensors[i].Value = total + basicNailValue;
                break;
            }

            int rand = Random.Range(0, total);
            nailValues.Add(rand + basicNailValue);
            nailSensors[i].Value = rand + basicNailValue;
            total -= rand;
        }

        maxNailValue = randomNailValue + basicNailValue * nailSensors.Length;

        TweenIn();

        RefreshBar();
        CloseSensors();
    }

    #endregion

    #region Tween

    private void ResetTween()
    {
        barRect.DOKill();
        barRect.anchoredPosition = barOrigin;
        barRect.localScale = Vector2.zero;

        hurtMask.alpha = 0;
        
        for (int i = 0; i < nails.Length; i++)
        {
            Vector2 origin = nailOrigins[i];
            nails[i].DOKill();
            nails[i].DOAnchorPos(new Vector2(origin.x, origin.y - 150f), 0);
        }

        for (int i = 0; i < hearts.Length; i++)
            hearts[i].transform.localScale = Vector2.zero;

        Vector2 pauseOffset = new Vector2(pauseOrigin.x, pauseOrigin.y + pauseRect.sizeDelta.y * 2);
        pauseRect.anchoredPosition = pauseOffset;
        Vector2 howOffset = new Vector2(howOrigin.x, howOrigin.y + howRect.sizeDelta.y * 2);
        howRect.anchoredPosition = howOffset;

        pawRect.DOKill();
        Vector2 pawOffset = new Vector2(pawOrigin.x, pawOrigin.y - pawRect.sizeDelta.y * 2);
        pawRect.anchoredPosition = pawOffset;
    }
    
    private void TweenIn()
    {
        tweenInSeq = DOTween.Sequence();

        //Bar
        barOffset = new Vector2(barOrigin.x, barOrigin.y - barRect.sizeDelta.y * 2);

        // HeartTween
        for (int i = 0; i < hearts.Length; i++)
            hearts[hearts.Length - (i + 1)].transform.DOScale(Vector2.one, 0.25f).From(Vector2.zero)
                .SetDelay(i * 0.125f)
                .SetEase(Ease.OutBack);

        //Pause
        pauseRect.DOAnchorPos(pauseOrigin, 0.15f).SetEase(Ease.OutBack);

        //How
        howRect.DOAnchorPos(howOrigin, 0.15f).SetEase(Ease.OutBack).SetDelay(0.0625f);

        tweenInSeq
            .Append(barRect.DOAnchorPos(barOrigin, 0.25f).From(barOffset).SetEase(Ease.OutBack))
            .Join(barRect.DOScale(Vector2.one, 0.25f).From(Vector2.zero).SetEase(Ease.OutBack))
            .AppendInterval(0.0625f)
            .Join(pawRect.DOAnchorPos(pawOrigin, 0.5f).SetEase(Ease.OutExpo))
            .AppendInterval(0.03125f)
            .OnComplete(() =>
            {
                pawRect.DOShakeAnchorPos(duration:2f, strength:80, vibrato:1, randomness:180).SetLoops(-1, LoopType.Yoyo);
                for (int i = 0; i < nails.Length; i++)
                    nails[i].DOAnchorPos(nailOrigins[i], 0.25f).SetDelay(i * 0.0625f);
            });
    }

    #endregion

    #region Method

    public void CutNail(int index)
    {
        if (nailValues[index] == 0)
        {
            nailSensors[index].IsClean = true;
            return;
        }

        nailValues[index]--;

        RefreshBar();
        if (CheckIsNailsCut())
        {
            CloseSensors();
            DOVirtual.DelayedCall(1f, OpenSettle);
        }
    }

    public void CutPaw()
    {
        CloseSensors();
        
        hearts[hearts.Length - chance].transform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.InBack).From(Vector3.one);
        chance--;

        canvas.transform.DOShakePosition(0.15f, 10f)
            .OnComplete(() =>
            {
                if (chance <= 0)
                    DOVirtual.DelayedCall(1f, OpenSettle);
                else
                    OpenSensors();
            });
        hurtMask.DOFade(1, 0.15f).From(0).SetLoops(2, LoopType.Yoyo);
    }

    public void OpenSensors()
    {
        pawSensor.OpenSensor();

        for (int i = 0; i < nailSensors.Length; i++)
        {
            nailSensors[i].OpenSensor();
        }
    }

    public void CloseSensors()
    {
        pawSensor.CloseSensor();

        for (int i = 0; i < nailSensors.Length; i++)
        {
            nailSensors[i].CloseSensor();
        }
    }

    private bool CheckIsNailsCut()
    {
        if (nailValues.Max() > 0) return false;
        return true;
    }

    private void RefreshBar()
    {
        int value = nailValues.Sum();
        fillBar.DOFillAmount((1f / maxNailValue) * value, 0.25f).SetEase(Ease.OutExpo);
    }

    public override void OpenPause()
    {
        base.OpenPause();
        pauseBg.DOFade(1, 0.45f).From(0).OnStart(() =>
        {
            pauseBg.raycastTarget = true;
        });
        pauseMenuRect.DOScale(Vector2.one, 0.35f).From(Vector2.zero).SetEase(Ease.OutBack);
    }

    public override void ClosePause()
    {
        base.ClosePause();
        pauseBg.DOFade(0, 0.45f).From(1).OnComplete(() => 
        {
            pauseBg.raycastTarget = false;
        });
        pauseMenuRect.DOScale(Vector2.zero, 0.35f).From(Vector2.one).SetEase(Ease.InBack);
    }

    public void Exit()
    {
        ClosePause();
        CloseSensors();
        Close();
    }

    #endregion

    #region OnDrag

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (chance <= 0) return;
        if (CheckIsNailsCut()) return;

        OpenSensors();
        cutEffect.gameObject.SetActive(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (chance <= 0) return;
        if (CheckIsNailsCut()) return;

        cutEffect.anchoredPosition = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (chance <= 0) return;
        if (CheckIsNailsCut()) return;

        CloseSensors();
        cutEffect.gameObject.SetActive(false);
    }

    #endregion
}
