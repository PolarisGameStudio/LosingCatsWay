using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Doozy.Runtime.UIManager.Containers;
using PolyNav;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class LittleGameSystem : MvcBehaviour
{
    [Title("Basic")] public LittleGame[] littleGames;
    private UIView view;

    public Callback OnClose;

    private Cat selectedCat;

    [Title("Tutorial")] 
    public UIView tutorialGameObject;
    public TextMeshProUGUI tutorialTitleText;
    public TextMeshProUGUI tutorialDescriptionText;

    private void Start()
    {
        view = GetComponent<UIView>();
    }

    public void Active(Cat cat)
    {
        if (App.model.build.IsCanMoveOrRemove)
            return;

        App.system.cat.PauseCatsGame(true);
        selectedCat = cat;

        view.Show();

        cat.CloseLittleGame();

        App.controller.lobby.Close();
        App.controller.followCat.SelectByOnlyFollw(cat);
        App.controller.followCat.Close();
        
        tutorialGameObject.Show();
        SetTutorialUI();
    }

    public void StartGame()
    {
        int index = selectedCat.littleGameIndex;

        littleGames[index].gameObject.SetActive(true);
        littleGames[index].StartGame(selectedCat);

        selectedCat.GetComponent<PolyNavAgent>().Stop();
        
        tutorialGameObject.InstantHide();
    }

    public void Close()
    {
        for (int i = 0; i < littleGames.Length; i++)
            littleGames[i].gameObject.SetActive(false);

        view.Hide();
        OnClose?.Invoke();
        App.system.cat.PauseCatsGame(false);
    }

    public void SetTutorialUI()
    {
        LittleGame littleGame = littleGames[selectedCat.littleGameIndex];

        tutorialTitleText.text = App.factory.stringFactory.GetGameString(littleGame.titleId);
        tutorialDescriptionText.text = App.factory.stringFactory.GetGameString(littleGame.descriptionId);
    }

    public void SetLittleGame(Cat cat)
    {
        int index = Random.Range(0, littleGames.Length);
        LittleGame littleGame = littleGames[index];
        cat.littleGameIndex = index;
        cat.catNotifyId = littleGame.notifyId;
    }
}