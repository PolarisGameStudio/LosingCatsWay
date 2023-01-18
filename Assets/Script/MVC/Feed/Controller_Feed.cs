using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controller_Feed : ControllerBehavior
{
    public void Open()
    {
        App.system.bgm.Play("Lobby");
        App.view.feed.Open();
        App.model.feed.Cats = App.system.cat.GetCats();
    }

    public void Close()
    {
        App.system.soundEffect.Play("Button");
        App.view.feed.Close();
        App.controller.lobby.Open();
    }

    public void Select(int index)
    {
        App.system.soundEffect.Play("Button");
        App.system.bgm.FadeOut();
        App.system.transition.Active(0.25f, () =>
        {
            var cat = App.model.feed.Cats[index];
            App.model.cultive.SelectedCat = cat;
            App.model.cultive.OpenFromIndex = 0;
            App.controller.cultive.Open();
        });
    }
}
