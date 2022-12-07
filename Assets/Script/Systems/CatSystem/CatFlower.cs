using Firebase.Firestore;
using Sirenix.OdinInspector;
using Spine;
using Spine.Unity;
using UnityEngine;

public class CatFlower : MonoBehaviour
{
    public SkeletonGraphic skeletonGraphic;
    [SpineSlot] public string slotName;
    [SpineAttachment] public string spineName;

    #region Slot

    private readonly string slot_bow = "Bow";
    private readonly string slot_bow_tail = "Bow_Tail_";
    private readonly string slot_decoration = "Decoration";
    private readonly string slot_folderC = "Folder_C";
    private readonly string slot_folderA = "Folder_A";
    private readonly string slot_folderO = "Folder_O";
    private readonly string slot_pot = "Pot";
    private readonly string slot_subCatFlower = "SubCatFlower";
    private readonly string slot_timothy = "Timothy";
    
    #endregion

    [Button]
    public async void Test()
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("LosingCats").Document("ed948bb51cf340efa168e7f1ac10f080");
    
        DocumentSnapshot result = await docRef.GetSnapshotAsync();
        var data = result.ConvertTo<CloudLosingCatData>();
        
        ChangeSkin(data);
    }

    public void ChangeSkin(CloudLosingCatData cloudLosingCatData)
    {
        Skeleton catSkeleton = skeletonGraphic.Skeleton;

        CloudSave_CatData catData = cloudLosingCatData.CatData;
        CloudSave_CatSkinData catSkinData = cloudLosingCatData.CatSkinData;
        CloudSave_CatServerData catServerData = cloudLosingCatData.CatServerData;
        
        var variety = catData.Variety;
        
        // 王若呈那邊ID在靠北
        if (variety.Contains("Siamese") && !variety.Contains("GT") && !variety.Contains("CT"))
        {
            var t = variety.Split('-');
            variety = t[1] + '-' + t[0];
        }

        variety = variety.Replace('_', '-');
        
        catSkeleton.SetSkin(variety);
        
        var sex = catData.Sex;
        var tailIndex = catSkinData.TailIndex;

        if (CatExtension.IsPedigreeCat(catData.Variety))
        {
            if (sex == 0)
            {
                catSkeleton.SetAttachment(slot_bow, "M_B");
                catSkeleton.SetAttachment(slot_bow_tail + "Big", "M_B_B");
            }
            else
            {
                catSkeleton.SetAttachment(slot_bow, "F_B");
                catSkeleton.SetAttachment(slot_bow_tail + "Big", "F_B_B");
            }
        }
        else
        {
            catSkeleton.SetAttachment(slot_bow, null);
            catSkeleton.SetAttachment(slot_bow_tail + "Big", null);
            catSkeleton.SetAttachment(slot_bow_tail + "Small", null);
        }
        

        // Folder
        var catAgeLevel = CatExtension.GetCatAgeLevel(catData.SurviveDays);

        catSkeleton.SetAttachment(slot_folderC, null);
        catSkeleton.SetAttachment(slot_folderA, null);
        catSkeleton.SetAttachment(slot_folderO, null);

        if (catAgeLevel >= 0)
        {
            if (sex == 0)
                catSkeleton.SetAttachment(slot_folderC, "M_C");
            else
                catSkeleton.SetAttachment(slot_folderC, "F_C");
        }

        if (catAgeLevel >= 1)
        {
            if (sex == 0)
                catSkeleton.SetAttachment(slot_folderA, "M_A");
            else
                catSkeleton.SetAttachment(slot_folderA, "F_A");
        }

        if (catAgeLevel >= 2)
        {
            if (sex == 0)
                catSkeleton.SetAttachment(slot_folderO, "M_O");
            else
                catSkeleton.SetAttachment(slot_folderO, "Fr_O");
        }

        // SubCatFlower
        if (catData.PersonalityTypes.Count >= 1)
        {
            var personalityType = catData.PersonalityTypes[0];
            var personalityLevel = catData.PersonalityLevels[0];

            catSkeleton.SetAttachment(slot_subCatFlower,
                "A" + CatExtension.ConvertPersonality(personalityType, personalityLevel).ToString("00"));
            
            catSkeleton.SetAttachment(slot_pot,
                "B" + CatExtension.ConvertPersonality(personalityType, personalityLevel).ToString("00"));
            
            catSkeleton.SetAttachment(slot_decoration,
                "C" + CatExtension.ConvertPersonality(personalityType, personalityLevel).ToString("00"));
        }
        
        // Pot
        if (catData.PersonalityTypes.Count >= 2)
        {
            var personalityType = catData.PersonalityTypes[1];
            var personalityLevel = catData.PersonalityLevels[1];

            catSkeleton.SetAttachment(slot_pot,
                "B" + CatExtension.ConvertPersonality(personalityType, personalityLevel).ToString("00"));
        }
        
        // Decoration
        if (catData.PersonalityTypes.Count >= 3)
        {
            var personalityType = catData.PersonalityTypes[2];
            var personalityLevel = catData.PersonalityLevels[2];

            catSkeleton.SetAttachment(slot_decoration,
                "C" + CatExtension.ConvertPersonality(personalityType, personalityLevel).ToString("00"));
        }
        
        // Timothy
        if (tailIndex == 0)
            catSkeleton.SetAttachment(slot_timothy, "Timothy");
        else if (tailIndex == 1)
            catSkeleton.SetAttachment(slot_timothy, "Timothy_L");
        
        // Reset pose
        Spine.Animation anim = skeletonGraphic.SkeletonData.FindAnimation("A_IDLE");
        skeletonGraphic.AnimationState.ClearTracks();
        skeletonGraphic.AnimationState.SetAnimation(0, anim, true);
        skeletonGraphic.AnimationState.TimeScale = 0;
    }

    public void DoAnimation(bool value)
    {
        if (value)
            skeletonGraphic.AnimationState.TimeScale = 1;
    }
}