using System.Collections;
using System.Collections.Generic;
using PolyNav;
using Spine;
using Spine.Unity;
using UnityEngine;

public class AngelCat : MvcBehaviour
{
    public bool isFriendMode = false;
    private PolyNavAgent polyNavAgent;

    // Start is called before the first frame update
    void Start()
    {
        polyNavAgent = GetComponent<PolyNavAgent>();
        SetAngelRing();
    }

    public void MoveToRandomRoom()
    {
        Room room = null;

        if (!isFriendMode)
        {
            if (App.system.room.GetRoomCount() == 1)
            {
                RandomMoveAtRoom();
                return;
            }

            room = App.system.room.GetRandomRoom();
        }
        else
        {
            var friendRoomSystem = FindObjectOfType<FriendRoom_RoomSystem>();

            if (friendRoomSystem.GetRoomCount() == 1)
            {
                RandomMoveAtRoom();
                return;
            }

            room = friendRoomSystem.GetRandomRoom();
        }

        for (int i = 0; i < 50; i++)
        {
            Vector2 target = room.transform.position;

            target.x += Random.Range(1f, 5f) * Random.Range(0, 2);
            target.y += Random.Range(1f, 5f) * Random.Range(0, 2);

            polyNavAgent.SetDestination(target);
            if (polyNavAgent.hasPath)
                break;
        }
    }
    
    public void RandomMoveAtRoom()
    {
        FriendRoom_GridSystem friendGridSystem = null;

        if (isFriendMode)
            friendGridSystem = FindObjectOfType<FriendRoom_GridSystem>();

        for (int i = 0; i < 50; i++)
        {
            Vector2 target = transform.position;
            int x = (int)(target.x / 5.12f);
            int y = (int)(target.y / 5.12f);

            Room room = null;
            
            if (!isFriendMode)
                room = App.system.grid.GetGrid(x, y).Content.GetComponent<Room>();
            else
                room = friendGridSystem.GetGrid(x, y).Content.GetComponent<Room>();

            if (room == null)
                continue;

            target.x = (x + Random.Range(0, room.Width)) * 5.12f + Random.Range(1.024f, 4.096f);
            target.y = (y + Random.Range(0, room.Height)) * 5.12f + Random.Range(1.024f, 4.096f);

            polyNavAgent.SetDestination(target);
            if (polyNavAgent.hasPath)
                break;
        }
    }
    
    private void SetAngelRing()
    {
        Skeleton catSkeleton = GetComponent<SkeletonMecanim>().skeleton;
        catSkeleton.SetAttachment("Angel_halo", "Angel_halo");
    }
}
