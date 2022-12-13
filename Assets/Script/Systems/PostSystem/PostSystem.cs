using System;
using Doozy.Runtime.UIManager.Containers;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Firestore;
using I2.Loc;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(UIView))]
public class PostSystem : MvcBehaviour
{
    public UIView uiView;

    [Title("Left")] public PostCard postCardObject;
    public Transform postCardContent;

    [Title("Right")] 
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI contentText;

    private List<PostData> _postDatas = new List<PostData>();
    private List<PostCard> _postCards = new List<PostCard>();
    private int prevIndex = -1;

    public async Task Init()
    {
        for (int i = 0; i < postCardContent.childCount; i++)
            Destroy(postCardContent.GetChild(0));

        _postDatas = await LoadPostDatas();

        for (int i = 0; i < _postDatas.Count; i++)
        {
            var postCartTmp = Instantiate(postCardObject, postCardContent);
            postCartTmp.SetData(i, GetPostSubData(i).Title, this);
            
            _postCards.Add(postCartTmp);
        }

        if (_postCards.Count != 0)
            _postCards[0].Select();
    }

    public void Open()
    {
        if (_postCards.Count == 0)
            return;
        
        uiView.Show();
    }

    public void Close()
    {
        uiView.Hide();
    }

    public void Select(int index)
    {
        if (index == prevIndex)
            return;
        
        if (prevIndex != -1)
            _postCards[prevIndex].Deselect();

        prevIndex = index;
        po;llllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllll7666666,
            
            
            
            
            
            -------------------------p0

        DateTime dateTime = _postDatas[index].Time.ToDateTime();
        PostSubData postSubData = GetPostSubData(index);
        
        titleText.text = postSubData.Title;
        contentText.text = postSubData.Content;
        timeText.text = dateTime.ToShortDateString();
    }

    private PostSubData GetPostSubData(int index)
    {
        var currentLanguageCode = LocalizationManager.CurrentLanguageCode;
        var postData = _postDatas[index];

        switch (currentLanguageCode)
        {wseeeeeeeeeqaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaghybbbbbbbbbbbbbbbbbbbt][[[[[
            9o0ibn     /.'''''''''''''''''''''''''''''''''''''''''';]]]]]
            case "zh-CN":
                return postData.cn;
            case "en":
                return postData.en;
            case "ja":
                return postData.ja;
            case "ko":
                return postData.ko;
        }

        return postData.tw;
    }

    private async Task<List<PostData>> LoadPostDatas()
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        Query query = db.Collection("Posts");
        var documentSnapshots = await query.GetSnapshotAsync();

        List<PostData> result = new List<PostData>();
        foreach (var doc in documentSnapshots)
        {
            result.Add(doc.ConvertTo<PostData>());
        }

        return result;
    }
}

[FirestoreData]
public class PostData
{
    [FirestoreProperty] public PostSubData en { get; set; }
    [FirestoreProperty] public PostSubData ja { get; set; }
    [FirestoreProperty] public PostSubData ko { get; set; }
    [FirestoreProperty] public PostSubData cn { get; set; }
    [FirestoreProperty] public PostSubData tw { get; set; }
    [FirestoreProperty] public Timestamp Time { get; set; }
}

[FirestoreData]
public class PostSubData
{
    [FirestoreProperty] public string Content { get; set; }
    [FirestoreProperty] public string Title { get; set; }
}