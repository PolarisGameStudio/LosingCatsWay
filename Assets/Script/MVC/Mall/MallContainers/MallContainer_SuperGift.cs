using System;
using System.Collections.Generic;
using Firebase.Firestore;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Purchasing;

public class MallContainer_SuperGift : MallContainer
{
    [Title("PriceText")]
    [SerializeField] private PriceTextHelper[] _priceTextHelpers;

    public override void Open()
    {
        base.Open();
        for (int i = 0; i < _priceTextHelpers.Length; i++)
        {
            _priceTextHelpers[i].SetText();
        }
    }
}