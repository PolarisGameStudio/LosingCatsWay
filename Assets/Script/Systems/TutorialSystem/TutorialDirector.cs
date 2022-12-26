using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDirector : MvcBehaviour
{
    public List<TutorialActor> actors = new List<TutorialActor>();

    private int stepIndex;

    public void Init()
    {
        stepIndex = -1;
    }

    private void Action(int index)
    {
        if (index < 0 || index >= actors.Count)
            return;

        actors[stepIndex].Exit();
        stepIndex = index;
        actors[stepIndex].Enter();
    }
}
