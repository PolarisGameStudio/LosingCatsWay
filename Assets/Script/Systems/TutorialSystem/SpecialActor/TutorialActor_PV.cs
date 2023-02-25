using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class TutorialActor_PV : TutorialActor_Movie
{
    [Title("Character", "Walk")]
    [SerializeField] private Image character_walk;
    [SerializeField] private Sprite walk_boy;
    [SerializeField] private Sprite walk_girl;
    
    [Title("Character", "Train")]
    [SerializeField] private Image character_train;
    [SerializeField] private Image character_train02;
    [SerializeField] private Sprite train_boy;
    [SerializeField] private Sprite train_girl;

    public override void Enter()
    {
        CheckGender();
        base.Enter();
    }

    private void CheckGender()
    {
        int index = App.system.player.PlayerGender; //0 boy 1 girl
        
        // walk
        Sprite walkSprite = index == 0 ? walk_boy : walk_girl;
        character_walk.sprite = walkSprite;
        
        // train
        Sprite trainSprite = index == 0 ? train_boy : train_girl;
        character_train.sprite = trainSprite;
        character_train02.sprite = trainSprite;
    }
}
