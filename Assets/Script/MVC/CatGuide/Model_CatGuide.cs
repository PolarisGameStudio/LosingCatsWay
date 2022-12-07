using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_CatGuide : ModelBehavior
{
    private Reward currentLevelBestReward;

    public Reward CurrentLevelBestReward
    {
        get => currentLevelBestReward;
        set
        {
            currentLevelBestReward = value;
            OnCurrentLevelBestRewardChange(value);
        }
    }

    public ValueChange OnCurrentLevelBestRewardChange;
}
