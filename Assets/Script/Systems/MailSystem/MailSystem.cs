using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Doozy.Runtime.UIManager.Containers;
using Firebase.Firestore;
using I2.Loc;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class MailSystem : MvcBehaviour
{
    public List<string> mailReceivedDatas = new List<string>();

    [Title("UI")] 
    public UIView view;
    public GameObject content;
    public GameObject receiveButtonMask;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI contentText;
    public TextMeshProUGUI dateText;
    [SerializeField] private TextMeshProUGUI expiredDayText;
    
    public MailRewardUI mailRewardUIObject;
    public Transform mailRewardContent;
    
    public Transform mailButtonsContent;
    public MailButton mailButton;
    
    private int prevIndex = -1;
    private List<MailData> _mailDatas = new List<MailData>();
    private List<MailButton> _mailButtons = new List<MailButton>();
    private List<Reward> _rewards = new List<Reward>();

    public async Task Init()
    {
        _mailDatas = await LoadMailDatas();
        Timestamp nowTime = Timestamp.GetCurrentTimestamp();

        for (int i = 0; i < _mailDatas.Count; i++)
        {
            MailData mailData = _mailDatas[i];

            if (mailData.StartTime > nowTime || nowTime > mailData.EndTime)
                _mailDatas.Remove(mailData);
        }
        
        for (int i = 0; i < _mailDatas.Count; i++)
        {
            MailButton tmp = Instantiate(mailButton, mailButtonsContent);
            tmp.SetData(_mailDatas[i], i);
            _mailButtons.Add(tmp);
        }

        if (_mailDatas.Count != 0)
        {
            content.SetActive(true);
            
            for (int i = 0; i < _mailDatas.Count; i++)
            {
                if (!_mailDatas[i].IsReceived(mailReceivedDatas))
                    _mailButtons[i].SetStatus(1);
                else
                    _mailButtons[i].SetStatus(2);
            }

            Select(0);
        }
        else
            content.SetActive(false);
        
        RefreshPoint();
    }

    public void Open()
    {
        view.Show();
    }

    public void Close()
    {
        view.InstantHide();
        App.controller.lobby.ActiveBuffer();
    }

    public void Select(int index)
    {
        print(index);
        
        if (index == prevIndex)
            return;

        if (prevIndex != -1)
        {
            int status = _mailDatas[prevIndex].IsReceived(mailReceivedDatas) ? 1 : 2;
            _mailButtons[prevIndex].SetStatus(status);
        }

        receiveButtonMask.SetActive(_mailDatas[index].IsReceived(mailReceivedDatas));
        var currentLanguageCode = LocalizationManager.CurrentLanguageCode;

        MailData mailData = _mailDatas[index];
        titleText.text = mailData.Content[currentLanguageCode].Title;
        contentText.text = mailData.Content[currentLanguageCode].Content;
        dateText.text = mailData.StartTime.ToDateTime().ToShortDateString();
        expiredDayText.text = (mailData.EndTime.ToDateTime() - Timestamp.GetCurrentTimestamp().ToDateTime()).Days.ToString();

        _rewards.Clear();
        
        for (int i = 0; i < mailRewardContent.childCount; i++)
            Destroy(mailRewardContent.GetChild(i).gameObject);

        for (int i = 0; i < mailData.Rewards.Count; i++)
        {
            MailReward mailReward = mailData.Rewards[i];
            Reward reward = new Reward();

            reward.item = App.factory.itemFactory.GetItem(mailReward.Id);
            reward.count = mailReward.Count;

            _rewards.Add(reward);
            
            MailRewardUI mailRewardUI = Instantiate(mailRewardUIObject, mailRewardContent);
            mailRewardUI.SetData(reward);
        }
        
        _mailButtons[index].SetStatus(0);
        prevIndex = index;
    }

    public void Receive()
    {
        mailReceivedDatas.Add(_mailDatas[prevIndex].Id);
        App.system.reward.Open(_rewards.ToArray());
        receiveButtonMask.SetActive(true);

        RefreshPoint();
    }

    private async Task<List<MailData>> LoadMailDatas()
    {
        List<MailData> tmp = new List<MailData>();

        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        CollectionReference catsRef = db.Collection("Mails");

        Query query1 = catsRef.WhereEqualTo("IsPrivate", false);
        var documentSnapshots1 = await query1.GetSnapshotAsync();

        foreach (var doc in documentSnapshots1.Documents)
            tmp.Add(doc.ConvertTo<MailData>());

        Query query2 = catsRef.WhereEqualTo("IsPrivate", true)
            .WhereArrayContains("PrivateMembers", App.system.player.PlayerId);
        var documentSnapshots2 = await query2.GetSnapshotAsync();

        foreach (var doc in documentSnapshots2.Documents)
            tmp.Add(doc.ConvertTo<MailData>());

        return tmp;
    }

    private void RefreshPoint()
    {
        for (int i = 0; i < _mailDatas.Count; i++)
        {
            MailData mailData = _mailDatas[i];
            
            if (!mailData.IsReceived(mailReceivedDatas))
            {
                App.view.lobby.mailRedPoint.SetActive(true);
                return;
            }
        }
        
        App.view.lobby.mailRedPoint.SetActive(false);
    }
}

[FirestoreData]
public class MailData
{
    [FirestoreProperty] public string Id { get; set; }
    [FirestoreProperty] public Dictionary<string, MailContent> Content { get; set; }
    [FirestoreProperty] public List<MailReward> Rewards { get; set; }
    [FirestoreProperty] public Timestamp EndTime { get; set; }
    [FirestoreProperty] public Timestamp StartTime { get; set; }

    public bool IsReceived(List<string> mailReceivedDatas)
    {
        return mailReceivedDatas.Contains(Id);
    }
}

[FirestoreData]
public class MailContent
{
    [FirestoreProperty] public string Content { get; set; }
    [FirestoreProperty] public string Title { get; set; }
}

[FirestoreData]
public class MailReward
{
    [FirestoreProperty] public string Id { get; set; }
    [FirestoreProperty] public int Count { get; set; }
}