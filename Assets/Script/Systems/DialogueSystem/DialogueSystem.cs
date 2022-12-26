using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using TMPro;
using Doozy.Runtime.UIManager.Containers;
using UnityEngine.UI;

public class DialogueSystem : MvcBehaviour
{
    public Callback OnDialogueEnd;

    public Image npcImage;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI[] chooseTexts;
    public Image nextIcon;
    [Space(10)]

    public GameObject mainNameObject;
    public GameObject npcNameObject;
    public TextMeshProUGUI mainNameText;
    public TextMeshProUGUI npcNameText;
    public UIView dialogueView;

    public UIView chooseView;
    private string[] answers;

    List<string> sentences;
    int checkpoint;

    bool isComplete;

    public void StartSentence(string content)
    {
        isComplete = true;

        checkpoint = 0;
        sentences = content.Split('\n').ToList();

        //dialogueView.Show();
        Invoke("NextSentence", 0.5f);
    }

    public void NextSentence()
    {
        if (checkpoint >= sentences.Count)
        {
            EndDialogue();
            return;
        }

        if (!isComplete) return;
        isComplete = false;

        nextIcon.DOFade(1, .5f).From(0).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);

        string sentence = sentences[checkpoint];

        string characterIndex = sentence.Split(':')[0];
        //string characterName = "";
        string content = sentence.Split(':')[1];
        bool isMainCharacter = (characterIndex == "0");

        if (characterIndex == "C") // 選項
        {
            answers = content.Split(';');

            #region 選項文字賦值

            chooseTexts[0].text = answers[0];
            chooseTexts[1].text = answers[1];

            #endregion

            checkpoint++;

            chooseView.Show();
            isComplete = true; //對話結束
            return;
        }

        if (isMainCharacter)
        {
            mainNameText.text = App.system.player.PlayerName;

            if (String.IsNullOrEmpty(App.system.player.PlayerName))
                mainNameText.text = "???";

            //npcImage.DOFade(0.25f, 0.25f);
            Color32 color = new Color32(113, 113, 113, 255);
            npcImage.DOColor(color, 0.25f);
        }
        else
        {
            npcNameText.text = App.factory.stringFactory.GetCharacterName(characterIndex);
            //npcImage.DOFade(1f, 0.25f);
            npcImage.DOColor(Color.white, 0.25f);
            float y = npcImage.rectTransform.anchoredPosition.y;
            npcImage.rectTransform.DOAnchorPosY(y + 50f, .15f).SetLoops(2, LoopType.Yoyo);
        }

        npcNameObject.SetActive(!isMainCharacter);
        mainNameObject.SetActive(isMainCharacter);

        //npcNameText.text = characterName;
        dialogueText.text = content;

        dialogueView.Show();

        dialogueText.DOFade(1, 0.75f).From(0).OnComplete(() => 
        {
            isComplete = true;
        });

        checkpoint++;
    }

    void EndDialogue()
    {
        mainNameText.text = string.Empty;
        npcNameText.text = string.Empty;

        dialogueText.text = string.Empty;

        dialogueView.InstantHide();
        OnDialogueEnd?.Invoke();
    }

    public void Choose(int index)
    {
        sentences.Add("0:" + answers[index]);
        chooseView.Hide();

        Invoke("NextSentence", 0.4f);
    }
}