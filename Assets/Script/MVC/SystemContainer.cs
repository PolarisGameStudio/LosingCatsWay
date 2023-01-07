using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class SystemContainer : MvcBehaviour
{
    public AbandonSystem abandon;
    
    public BgmSystem bgm;
    public BigGamesSystem bigGames;

    public CatSystem cat;
    public CatchCatSystem catchCat;
    public CatRenameSystem catRename;
    public CatNotifySystem catNotify;
    public ConfirmSystem confirm;
    public ChoosePlayerGenderSystem choosePlayerGenderSystem;
    
    public DialogueSystem dialogue;

    public FindCatSystem findCat;
    public FlowTaskSystem flowTask;

    public MyGridSystem grid;
    
    public HowToPlaySystem howToPlay;
    
    public LevelUpSystem levelUp;
    public LittleGameSystem littleGame;
    public InventorySystem inventory;
    
    public MailSystem mail;
    public MapSystem map;
    public MyTimeSystem myTime;

    public OpenFlowSystem openFlow;

    public PlayerSystem player;
    public PlayerRenameSystem playerRename;
    public PostSystem post;

    public QuestSystem quest;

    public RewardSystem reward;
    public RoomSystem room;
    
    public ScreenshotSystem screenshot;
    public SettleSystem settle;
    public ShortcutSystem shortcut;
    public SoundEffectSystem soundEffect;
    public SideMenuSystem sideMenu;

    public TnrSystem tnr;
    public TransitionsSystem transition;
    public TutorialSystem tutorial;

    public WaitingSystem waiting;
    
    [FormerlySerializedAs("useItem")] public UnlockGridSystem unlockGrid;
    
    [Title("Save")]
    public CloudSaveSystem cloudSave;
}
