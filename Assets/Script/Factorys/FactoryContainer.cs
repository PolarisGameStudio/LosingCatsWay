using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FactoryContainer : MvcBehaviour
{
    public RoomFactory roomFactory;
    public ConfirmFactory confirmFactory;
    public ItemFactory itemFactory;
    public CatFactory catFactory;
    public StringFactory stringFactory;
    public SickFactory sickFactory;
    public QuestFactory questFactory;
    public DiaryFactory diaryFactory;
    public PediaFactory pediaFactory;
}