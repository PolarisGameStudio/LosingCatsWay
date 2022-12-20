using Sirenix.OdinInspector;
using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class View_ClinicFunction : ViewBehaviour
{
    [Title("Spine")]
    [SerializeField] private string[] funcAnimNames;
    [SerializeField] private string[] catAnimNames;
    [SerializeField] private CatSkin catSkin;
    [SerializeField] private SkeletonGraphic tableGraphic;
    [SerializeField] private SkeletonGraphic functionGraphic;

    List<string> subjects = new List<string>();

    public override void Init()
    {
        base.Init();
        App.model.clinic.OnSelectedCatChange += OnSelectedCatChange;
        App.model.clinic.OnPaymentChange += OnPaymentChange;
    }

    private void OnSelectedCatChange(object value)
    {
        Cat cat = (Cat)value;
        catSkin.ChangeSkin(cat.cloudCatData);
        functionGraphic.transform.localScale = catSkin.transform.localScale;
    }

    private void OnPaymentChange(object value)
    {
        var payment = (Dictionary<string, int>)value;
        subjects = new List<string>();

        for (int i = 0; i < payment.Count; i++)
        {
            subjects.Add(payment.ElementAt(i).Key);
        }
    }

    public override void Open()
    {
        base.Open();

        tableGraphic.gameObject.SetActive(true);
        catSkin.SetActive(true);
        functionGraphic.gameObject.SetActive(true);

        #region Animation

        catSkin.skeletonGraphic.AnimationState.ClearTracks();
        functionGraphic.AnimationState.ClearTracks();

        List<Spine.Animation> catTracks = new List<Spine.Animation>();
        List<Spine.Animation> funcTracks = new List<Spine.Animation>();

        for (int i = 0; i < subjects.Count; i++)
        {
            if (subjects[i] == "CP007")
            {
                catTracks.Add(catSkin.skeletonGraphic.SkeletonData.FindAnimation(catAnimNames[5]));
                funcTracks.Add(functionGraphic.SkeletonData.FindAnimation(funcAnimNames[5]));
                continue;
            }

            if (subjects[i] == "CP001")
            {
                catTracks.Add(catSkin.skeletonGraphic.SkeletonData.FindAnimation(catAnimNames[0]));
                funcTracks.Add(functionGraphic.SkeletonData.FindAnimation(funcAnimNames[0]));
                continue;
            }

            if (subjects[i] == "CP002")
            {
                catTracks.Add(catSkin.skeletonGraphic.SkeletonData.FindAnimation(catAnimNames[1]));
                funcTracks.Add(functionGraphic.SkeletonData.FindAnimation(funcAnimNames[1]));
                continue;
            }

            if (subjects[i] == "CP003")
            {
                catTracks.Add(catSkin.skeletonGraphic.SkeletonData.FindAnimation(catAnimNames[2]));
                funcTracks.Add(functionGraphic.SkeletonData.FindAnimation(funcAnimNames[2]));
                continue;
            }

            if (subjects[i] == "CP004")
            {
                catTracks.Add(catSkin.skeletonGraphic.SkeletonData.FindAnimation(catAnimNames[3]));
                funcTracks.Add(functionGraphic.SkeletonData.FindAnimation(funcAnimNames[3]));
                continue;
            }

            if (subjects[i] == "CP005" || subjects[i] == "CP006")
            {
                catTracks.Add(catSkin.skeletonGraphic.SkeletonData.FindAnimation(catAnimNames[4]));
                funcTracks.Add(functionGraphic.SkeletonData.FindAnimation(funcAnimNames[4]));
            }
        }

        for (int i = 0; i < catTracks.Count; i++)
        {
            catTracks[i].Duration = funcTracks[i].Duration;
            catSkin.skeletonGraphic.AnimationState.AddAnimation(0, catTracks[i], false, 0);
        }

        for (int i = 0; i < funcTracks.Count; i++)
        {
            if (funcTracks.Count > 1)
            {
                if (i < funcTracks.Count - 1)
                {
                    functionGraphic.AnimationState.AddAnimation(0, funcTracks[i], false, 0);
                    continue;
                }

                var trackEntry = functionGraphic.AnimationState.AddAnimation(0, funcTracks[i], false, 0);
                trackEntry.Complete += FunctionComplete;
            }
            else
            {
                var trackEntry = functionGraphic.AnimationState.AddAnimation(0, funcTracks[i], false, 0);
                trackEntry.Complete += FunctionComplete;
            }
        }

        #endregion
    }

    private void FunctionComplete(TrackEntry trackEntry)
    {
        trackEntry.Complete -= FunctionComplete;
        Close();
        App.controller.clinic.OpenCheckResult();
    }

    public override void Close()
    {
        base.Close();

        tableGraphic.gameObject.SetActive(false);
        catSkin.SetActive(false);
        functionGraphic.gameObject.SetActive(false);
    }
}
