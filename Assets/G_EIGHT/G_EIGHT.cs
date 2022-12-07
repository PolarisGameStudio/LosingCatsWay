using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
using Firebase.Firestore;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class G_EIGHT : MonoBehaviour
{
    [Title("UI")] public TextMeshProUGUI totalCatText;
    public TextMeshProUGUI totalHatText;

    public G_EIGHT_LeadBoard[] gEightLeadBoards;

    // Data
    private List<CloudCatData> cloudCatDatas = new List<CloudCatData>();

    async void Start()
    {
        await FirstLoadCats();
        SetListener();
    }

    #region Data

    private async Task FirstLoadCats()
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        Query query = db.Collection("Cats");
        var docs = await query.GetSnapshotAsync();
        var result = new List<CloudCatData>();

        foreach (DocumentSnapshot documentSnapshot in docs.Documents)
        {
            CloudCatData cloudCatData = documentSnapshot.ConvertTo<CloudCatData>();
            result.Add(cloudCatData);
        }

        RefreshUI(result, cloudCatDatas);
        cloudCatDatas = result;
    }

    private void SetListener()
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        Query query = db.Collection("Cats");

        ListenerRegistration listener = query.Listen(snapshot => {
            var tmp = new List<CloudCatData>();

            foreach (DocumentSnapshot documentSnapshot in snapshot.Documents)
            {
                var cloudCatData = documentSnapshot.ConvertTo<CloudCatData>();
                tmp.Add(cloudCatData);
            }

            RefreshUI(tmp, cloudCatDatas);
            cloudCatDatas = tmp;
        });
        
        // ListenerRegistration listener = query.Listen(snapshot =>
        // {
        //     foreach (DocumentChange change in snapshot.GetChanges())
        //     {
        //         if (change.ChangeType == DocumentChange.Type.Added)
        //         {
        //             var cloudCatData = change.Document.ConvertTo<CloudCatData>();
        //         
        //             var newCloudCatDatas = cloudCatDatas;
        //             if (newCloudCatDatas.Exists(x => x.CatData.CatId == cloudCatData.CatData.CatId))
        //                 continue;
        //         
        //             newCloudCatDatas.Add(cloudCatData);
        //         
        //             RefreshUI(newCloudCatDatas, cloudCatDatas);
        //             cloudCatDatas = newCloudCatDatas;
        //         }
        //         else if (change.ChangeType == DocumentChange.Type.Modified)
        //         {
        //             var cloudCatData = change.Document.ConvertTo<CloudCatData>();
        //             var newCloudCatDatas = cloudCatDatas;
        //             
        //             var index = newCloudCatDatas.FindIndex(x => x.CatData.CatId == cloudCatData.CatData.CatId);
        //             newCloudCatDatas[index] = cloudCatData;
        //             
        //             RefreshUI(newCloudCatDatas, cloudCatDatas);
        //             cloudCatDatas = newCloudCatDatas;
        //         }
        //         else if (change.ChangeType == DocumentChange.Type.Removed)
        //         {
        //             var cloudCatData = change.Document.ConvertTo<CloudCatData>();
        //             var newCloudCatDatas = cloudCatDatas;
        //         
        //             if (!newCloudCatDatas.Exists(x => x.CatData.CatId == cloudCatData.CatData.CatId))
        //                 continue;
        //         
        //             var index = newCloudCatDatas.FindIndex(x => x.CatData.CatId == cloudCatData.CatData.CatId);
        //             newCloudCatDatas.RemoveAt(index);
        //         
        //             RefreshUI(newCloudCatDatas, cloudCatDatas);
        //             cloudCatDatas = newCloudCatDatas;
        //         }
        //     }
        // });
    }

    #endregion

    #region UI

    public void RefreshUI(List<CloudCatData> catDatas, List<CloudCatData> prevCatDatas)
    {
        int totalCatCount = catDatas.Count;
        int prevTotalCatCount = prevCatDatas.Count;

        int totalHatCount = GetHatCount(catDatas);
        int prevTotalHatCount = GetHatCount(prevCatDatas);

        List<LeaderBoard> prevLeadBoard = GetLeaderBoards(prevCatDatas);
        List<LeaderBoard> leadBoard = GetLeaderBoards(catDatas);

        int delayCount = 0;

        for (int i = 0; i < 10; i++)
        {
            if (i > leadBoard.Count - 1)
            {
                // 關閉那一條
                gEightLeadBoards[i].Out(delayCount * 0.125f);
                delayCount++;
                continue;
            }

            if (i > prevLeadBoard.Count - 1)
            {
                // 直接顯示leadBoard新的
                gEightLeadBoards[i].In(delayCount * 0.125f, leadBoard[i]);
                delayCount++;
                continue;
            }

            if (leadBoard[i].Variety != prevLeadBoard[i].Variety)
            {
                // 離開在近來
                gEightLeadBoards[i].OutIn(delayCount * 0.125f, leadBoard[i]);
                delayCount++;
                continue;
            }

            if (leadBoard[i].HatCount != prevLeadBoard[i].HatCount ||
                leadBoard[i].CatCount != prevLeadBoard[i].CatCount)
            {
                // 單純換數字
                gEightLeadBoards[i].ChangeValue(leadBoard[i], prevLeadBoard[i]);
            }
        }

        DOTween.To(() => prevTotalCatCount, x => prevTotalCatCount = x, totalCatCount, 0.25f).OnUpdate(() =>
        {
            totalCatText.text = prevTotalCatCount.ToString();
        });

        DOTween.To(() => prevTotalHatCount, x => prevTotalHatCount = x, totalHatCount, 0.25f).OnUpdate(() =>
        {
            totalHatText.text = prevTotalHatCount.ToString();
        });
    }

    private int GetHatCount(List<CloudCatData> catDatas)
    {
        int result = 0;

        for (int i = 0; i < catDatas.Count; i++)
        {
            var catData = catDatas[i];

            if (catData.CatSkinData.UseSkinId == "SantaHat")
                result++;
        }

        return result;
    }

    private List<LeaderBoard> GetLeaderBoards(List<CloudCatData> catDatas)
    {
        List<LeaderBoard> leaderBoards = new List<LeaderBoard>();

        for (int i = 0; i < catDatas.Count; i++)
        {
            var catData = catDatas[i];
            var variety = catData.CatData.Variety;
            int santaHat = catData.CatSkinData.UseSkinId == "SantaHat" ? 1 : 0;

            if (leaderBoards.Count == 0)
            {
                LeaderBoard leaderBoard = new LeaderBoard();
                leaderBoard.Variety = variety;
                leaderBoard.CatCount = 1;
                leaderBoard.HatCount = santaHat;
                leaderBoards.Add(leaderBoard);
                continue;
            }

            LeaderBoard tmp = leaderBoards.Find(x => x.Variety == variety);

            if (tmp != null)
            {
                tmp.CatCount++;
                tmp.HatCount += santaHat;
            }
            else
            {
                LeaderBoard leaderBoard = new LeaderBoard();
                leaderBoard.Variety = variety;
                leaderBoard.CatCount = 1;
                leaderBoard.HatCount = santaHat;
                leaderBoards.Add(leaderBoard);
            }
        }

        leaderBoards = leaderBoards.FindAll(x => x.HatCount != 0);
        leaderBoards = leaderBoards.OrderByDescending(x => x.HatRatio).ThenByDescending(x => x.CatCount).ThenBy(x => x.Variety).ToList();

        if (leaderBoards.Count > 10)
            leaderBoards.RemoveRange(10, leaderBoards.Count - 10);

        return leaderBoards;
    }

    #endregion
}

public class LeaderBoard
{
    public string Variety;
    public int CatCount;
    public int HatCount;

    public float HatRatio
    {
        get { return (float)HatCount / CatCount; }
    }
}