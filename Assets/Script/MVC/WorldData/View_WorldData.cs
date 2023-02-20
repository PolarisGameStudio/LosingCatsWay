using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class View_WorldData : ViewBehaviour
{
    [Title("Left")] [SerializeField] private TextMeshProUGUI totalCatsText;
    [SerializeField] private DataVisualization_Circle visualCircle;

    [Title("Right")] [SerializeField] private TextMeshProUGUI bornCatsText;
    [SerializeField] private TextMeshProUGUI deadCatsText;
    [SerializeField] private TextMeshProUGUI sellCatsText;

    [Title("Age")] [SerializeField] private DataVisualization_Bar visualBar_Age;

    [Title("Ligation")] [SerializeField] private DataVisualization_Bar visualBar_Ligation;

    public override void Init()
    {
        base.Init();
        App.model.worldData.OnWorldDataChange += OnWorldDataChange;
    }

    private void OnWorldDataChange(object value)
    {
        var worldData = (Cloud_WorldData)value;

        int totalCount = worldData.CatCount;
        totalCatsText.text = totalCount.ToString("N0");
        
        int adoptedCount = worldData.AdoptedCount;
        int shelterCount = worldData.ShelterCount;
        int outsideCount = worldData.OutdoorCount;
        List<float> circleValues = new List<float> { adoptedCount, shelterCount, outsideCount };
        visualCircle.SetData(circleValues.ToArray());

        int childCount = worldData.ChildCount;
        int adultCount = worldData.AdultCount;
        int oldCount = worldData.OldCount;
        List<float> ageBarValues = new List<float> {childCount, adultCount, oldCount};
        visualBar_Age.SetData(ageBarValues.ToArray());

        int ligationCount = worldData.LigationCount;
        int nonLigationCount = totalCount - ligationCount;
        List<float> ligationBarValues = new List<float>() { ligationCount, nonLigationCount };
        visualBar_Ligation.SetData(ligationBarValues.ToArray());

        bornCatsText.text = worldData.AddCatCount.ToString("N0");
        deadCatsText.text = worldData.DeleteCatCount.ToString("N0");
        sellCatsText.text = worldData.BuyCatCount.ToString("N0");
    }
}
