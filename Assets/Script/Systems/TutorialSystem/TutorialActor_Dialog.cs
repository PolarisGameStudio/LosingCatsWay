using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class TutorialActor_Dialog : TutorialActor
{
    public Dictionary<string, MultilineString> dialogContents;

    public override void Enter()
    {
        App.system.dialogue.OnDialogueEnd += Exit;
        
        string country = App.factory.stringFactory.GetCountryByLocaleIndex();
        string content = dialogContents[country].content;
        
        App.system.dialogue.StartSentence(content);
        base.Enter();
    }

    public override void Exit()
    {
        App.system.dialogue.OnDialogueEnd -= Exit;
        base.Exit();
    }
}

[System.Serializable]
public class MultilineString
{
    [TextArea(5, 10)] public string content;
}
