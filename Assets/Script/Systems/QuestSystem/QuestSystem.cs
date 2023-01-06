using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Firebase.Firestore;
using UnityEngine;
using Sirenix.OdinInspector;

public class QuestSystem : SerializedMonoBehaviour
{
    public Dictionary<string, int> QuestProgressData;
    public Dictionary<string, int> QuestReceivedStatusData; // 0 還沒領 1 領了
}