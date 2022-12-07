using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Sirenix.OdinInspector;

public class DialogueContainer : MvcBehaviour
{
    [Title("UI")]
    public TextMeshProUGUI speakerText;
    public TextMeshProUGUI dialogueText;

    [Title("Dialogues")]
    public Dialogue[] dialogues;

    [Title("Events")]
    public UnityEvent OnDialogueEnter;
    public UnityEvent OnDialogueExit;

    string country;
    Queue<string> speakers = new Queue<string>();
    Queue<string> contents = new Queue<string>();

    public void InitDialogue()
    {
        if (dialogues.Length <= 0) return;

        #region Get dialogue

        country = App.factory.stringFactory.GetCountryByLocaleIndex();
        print(country);

        Dialogue target = new Dialogue();
        for (int i = 0; i < dialogues.Length; i++)
        {
            if (dialogues[i].country != country) continue;
            target = dialogues[i];
        }

        #endregion

        #region Init queue

        contents.Clear();
        for (int i = 0; i < target.content.Count; i++)
        {
            speakers.Enqueue(target.speakerName);
            contents.Enqueue(target.content[i]);
        }

        #endregion

        ContinueDialogue();
    }

    public void ContinueDialogue()
    {
        if (contents.Count <= 0)
        {
            ExitDialogue();
            return;
        }

        EnterDialogue();

        speakerText.text = string.Empty;
        dialogueText.text = string.Empty;

        string speaker = speakers.Dequeue();
        string content = contents.Dequeue();

        if (speaker == "<$PlayerName>") 
            speaker = App.system.player.PlayerName;

        //TMP
        speakerText.text = speaker;
        dialogueText.text = content;

        //Debug
        //print($"{speaker} speaking:");
        //print(content);
    }

    void EnterDialogue()
    {
        print("Enter dialogue.");
        OnDialogueEnter?.Invoke();
    }

    void ExitDialogue()
    {
        print("Exit dialogue.");
        OnDialogueExit?.Invoke();
    }
}
