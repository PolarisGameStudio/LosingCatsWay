using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDynamicWall : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] walls;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("Enter");
        if (!collision.CompareTag("Cat")) return;
        for (int i = 0; i < walls.Length; i++)
        {
            print("0");
            walls[i].DOFade(0.3f, 0.35f).SetEase(Ease.OutExpo);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        print("Exit");
        if (!collision.CompareTag("Cat")) return;
        for (int i = 0; i < walls.Length; i++)
        {
            print("1");
            walls[i].DOFade(1, 0.25f).SetEase(Ease.InExpo);
        }
    }
}
