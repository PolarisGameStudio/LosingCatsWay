using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class View_EntranceDiary : ViewBehaviour
{
    [Title("Book")] [SerializeField] private Card_EntranceDiary leftBook;
    [SerializeField] private Card_EntranceDiary rightBook;
    [SerializeField] private Card_EntranceDiary centerBook;

    public override void Init()
    {
        base.Init();
        App.model.entrance.OnAllDeadCatsChange += OnAllDeadCatsChange;
    }

    private void OnAllDeadCatsChange(object value)
    {
        //
    }
}
