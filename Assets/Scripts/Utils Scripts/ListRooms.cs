using System;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ListRooms : MonoBehaviour
{
    // Scene components
    [SerializeField] RoomNetworkManager roomNetworkManager;
    [SerializeField] PrefabUtils prefabUtils;
    [SerializeField] Transform roomConteiner;
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] MenuUtils menuUtils;
    [SerializeField] Msg msg;

    private void Start()
    {
        scrollRect.gameObject.SetActive(true);
    }

    public List<Room> SearchRooms()
    {
        CleanRooms();
        List<Room> rooms = new List<Room>();
        roomConteiner.transform.localPosition = new Vector2(0, -280);
        roomNetworkManager.GetRooms(NetworkAuthentication.userPrivateData.user_id, 
        (List<RoomPublicData> publicRoomsData) =>
        {
            for (int i = 0; i < publicRoomsData.Count; i++)
            {
                RoomPublicData publicRoom = publicRoomsData[i];
                Room room = prefabUtils.InstantiateRoom(
                    roomName: publicRoom.room_name,
                    usernames: publicRoom.room_users,
                    color: GameColorUtils.GetColor(publicRoom.room_color),
                    parent: roomConteiner
                );
                room.transform.localPosition = new Vector2(0, 975 - 550 * i);
                rooms.Add(room);
                room.joinButton.GetComponent<Button>().onClick.AddListener(() =>
                     roomNetworkManager.JoinRoom(NetworkAuthentication.userPrivateData.user_id, publicRoom.room_id,
                    (RoomPublicData publicRoom) =>
                    {
                        menuUtils.ChangeMenu(menuUtils.roomMenu, MenuUtils.Side.Right);
                        menuUtils.roomMenu.GetComponent<RoomMenu>().SetJoinedRoomMenu(publicRoom);
                        msg.Deactivate();
                    }));
            }
        });
        return rooms;
    }

    void CleanRooms()
    {
        foreach (Transform child in roomConteiner.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void Update()
    {
        SetScrollMovementType();
    }

    void SetScrollMovementType()
    {
        int indexLastRoom = roomConteiner.transform.childCount - 1;
        if (indexLastRoom >= 0)
        {
            float scrollLimitUp = -280;
            float lastRoomY = roomConteiner.GetChild(indexLastRoom).transform.localPosition.y;
            float scrollLimitDown = Math.Abs(lastRoomY) - 675;
            if (roomConteiner.localPosition.y < scrollLimitUp || indexLastRoom < 3)
            {
                scrollRect.inertia = false;
                roomConteiner.transform.localPosition = new Vector2(0, scrollLimitUp);
            }
            else if (roomConteiner.localPosition.y > scrollLimitDown)
            {
                scrollRect.inertia = false;
                roomConteiner.transform.localPosition = new Vector2(0, scrollLimitDown);
            }
            else
            {
                scrollRect.inertia = true;
            }
        }

    }
}
