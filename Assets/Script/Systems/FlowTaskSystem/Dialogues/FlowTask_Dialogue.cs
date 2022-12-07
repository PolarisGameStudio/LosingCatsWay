using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class FlowTask_Dialogue : FlowTask
{
    public Dictionary<string, DialogueContent> contents = new Dictionary<string, DialogueContent>();

    public override void Enter()
    {
        base.Enter();
        App.system.dialogue.OnDialogueEnd += Exit;

        string country = App.factory.stringFactory.GetCountryByLocaleIndex();
        string content = contents[country].content;
        
        App.system.dialogue.StartSentence(content);
    }

    public override void Exit()
    {
        App.system.dialogue.OnDialogueEnd -= Exit;
        base.Exit();
    }
}


[System.Serializable]
public class DialogueContent
{
    [TextArea(5, 10)] public string content;
}