using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Doozy.Runtime.UIManager.Containers;

public class InGameTutorial : MvcBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI contentText;
    public TextMeshProUGUI okText;
    [Space(10)]

    public UIView view;

    private void Start()
    {
        Time.timeScale = 0;
        SetContent();
    }

    public void ToggleView()
    {
        if (view.isVisible)
        {
            Time.timeScale = 1f;
            view.Hide();
        }
        else if (view.isHidden)
        {
            view.Show();
            Time.timeScale = 0;
        }
    }

    void SetContent()
    {
        //1.Title
        titleText.text = "Title";

        //2.Content
        contentText.text = "Description";
    }
}
