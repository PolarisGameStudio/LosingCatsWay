using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using TMPro;
using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class DialogueSystem : MvcBehaviour
{
    public Callback OnDialogueEnd;

    [Title("NPC")]
    public Image npcImage;
    public Sprite[] npcFaces;
    
    [Title("UI")]
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI[] chooseTexts;
    public Image nextIcon;
    public GameObject mainNameObject;
    public GameObject npcNameObject;
    public TextMeshProUGUI mainNameText;
    public TextMeshProUGUI npcNameText;

    [Title("UIView")]
    public UIView dialogueView;
    public UIView chooseView;

    [Title("Choose")] 
    [SerializeField] private Button button_0;
    [SerializeField] private Button button_1;

    private string[] answers;
    private List<string> sentences;
    private int checkpoint;
    private bool isComplete;

    private int chooseInsertIndex;

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
        bool isMainCharacter = characterIndex == "0";

        if (characterIndex == "C") // 選項
        {
            answers = content.Split(';');

            #region 選項文字賦值

            chooseTexts[0].text = answers[0];
            chooseTexts[1].text = answers[1];

            #endregion

            checkpoint++;
            chooseInsertIndex = checkpoint;

            OpenChoose();
            isComplete = true; //對話結束
            return;
        }

        if (sentence.Split(':').Length >= 3)
        {
            string specialString = sentence.Split(':')[2];
            if (specialString.Contains("F")) //表情
            {
                int faceIndex = int.Parse(specialString.Replace("F", ""));
                npcImage.sprite = npcFaces[faceIndex];
            }
            npcImage.gameObject.SetActive(specialString != "NPC-1"); //關掉NPC
        }

        if (isMainCharacter)
        {
            mainNameText.text = App.system.player.PlayerName;

            if (String.IsNullOrEmpty(App.system.player.PlayerName))
                mainNameText.text = "???";

            Color32 color = new Color32(113, 113, 113, 255);
            npcImage.DOColor(color, 0.25f);
        }
        else
        {
            npcNameText.text = App.factory.stringFactory.GetCharacterName(characterIndex);
            
            npcImage.DOColor(Color.white, 0.25f);
            float y = npcImage.rectTransform.anchoredPosition.y;
            npcImage.rectTransform.DOAnchorPosY(y + 50f, .15f).SetLoops(2, LoopType.Yoyo);
        }

        npcNameObject.SetActive(!isMainCharacter);
        mainNameObject.SetActive(isMainCharacter);

        dialogueText.text = content;

        dialogueView.Show();

        dialogueText.DOFade(1, 0.75f).From(0).OnComplete(() => 
        {
            isComplete = true;
        });

        checkpoint++;
    }

    private void EndDialogue()
    {
        mainNameText.text = string.Empty;
        npcNameText.text = string.Empty;

        dialogueText.text = string.Empty;

        dialogueView.InstantHide();
        OnDialogueEnd?.Invoke();
    }

    private void OpenChoose()
    {
        button_0.interactable = true;
        button_1.interactable = true;
        chooseView.Show();
    }

    public void Choose(int index)
    {
        button_0.interactable = false;
        button_1.interactable = false;
        
        App.system.soundEffect.Play("Button");
        
        // sentences.Add("0:" + answers[index]);
        sentences.Insert(chooseInsertIndex, "0:" + answers[index]);
        chooseView.Hide();
        Invoke("NextSentence", 0.4f);
    }
}