using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Doozy.Runtime.UIManager.Containers;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FlowTask_E3_Cloud : FlowTask
{
    public UIView islandTitleView;

    public TextMeshProUGUI clickText;
    public Image cloud0;
    public Image cloud1;
    public Image cloud2;
    public Image cloud3;
    
    private UIView view;

    public override void Enter()
    {
        base.Enter();

        view = GetComponent<UIView>();
        view.Show();
    }

    public void OpenCloud()
    {
        clickText.DOFade(0, 0.35f).OnComplete(() =>
        {
            cloud0.transform.DOLocalMove(new Vector3(1960, 1740, 0), 1f);
            cloud1.transform.DOLocalMove(new Vector3(1960, -1740, 0), 1f);
            cloud2.transform.DOLocalMove(new Vector3(-687.5f, 1740, 0), 1f);
            cloud3.transform.DOLocalMove(new Vector3(-687.5f, -1740, 0), 1f);

            DOVirtual.DelayedCall(0.35f, () => { islandTitleView.Show(); });

            DOVirtual.DelayedCall(2.35f, () =>
            {
                // islandTitleView.Hide();
                view.Hide();
                DOVirtual.DelayedCall(0.35f, () => { Exit(); });
            });
        });
    }
}
