using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class WsMsgProcessor : MonoBehaviour
{
    [SerializeField] MenuUtils menuUtils;
    [SerializeField] RoomNetworkManager roomNetworkManager;
    [SerializeField] GameNetworkManager gameNetworkManager;
    [SerializeField] GameMenu gameMenu;
    [SerializeField] Msg msg;

    private (string command, string reference) ParseWsMessage(string message)
    {
        string[] parts = message.Split(':', 2);
        string command = parts[0].Trim();
        string reference = parts.Length > 1 ? parts[1].Trim() : string.Empty;
        return (command, reference);
    }

    private (string command, int[] values) ParseValuesWsMessage(string message)
    {
        string[] parts = message.Split(':', 2);
        string command = parts[0].Trim();

        int[] values = parts.Length > 1
            ? parts[1].Split(',').Select(s => int.Parse(s.Trim())).ToArray()
            : new int[0];

        return (command, values);
    }

    public void ProcessWsMessage(string message)
    {
        switch (ParseWsMessage(message).command)
        {
            // broadcast global
            case "room_created":
                HandleNewRoomCreated();
                break;
            case "user_joined":
                HandleUserJoined(ParseWsMessage(message).reference);
                break;
            case "user_left":
                HandleUserLeft(ParseWsMessage(message).reference);
                break;
            case "room_deleted":
                HandleRoomDeleted(ParseWsMessage(message).reference, "Room deleted", false);
                break;
            case "room_expired":
                HandleRoomDeleted(ParseWsMessage(message).reference, "Room expired", true);
                break;        
            // broadcast to room
            case "game_started":
                HandleGameStarted();
                break;
            case "next_turn":
                HandleNextTurn();
                break;
            case "player_left":
                HandlePlayerLeft(ParseWsMessage(message).reference);
                break;
            case "switch":
                HandleSwitch(ParseValuesWsMessage(message).values[0],
                    ParseValuesWsMessage(message).values[1],
                    ParseValuesWsMessage(message).values[2],
                    ParseValuesWsMessage(message).values[3],
                    ParseValuesWsMessage(message).values[4]);
                break;
            case "figure_discard":
                HandleFigureDiscard(ParseValuesWsMessage(message).values[0]);
                break;
            case "figure_block":
                HandleFigureBlock();
                break;
            case "timer":
                HandleTimer(ParseValuesWsMessage(message).values[0]);
                break;
            default:
                break;
        }
    }

    private void HandleNewRoomCreated()
    {
        if (menuUtils.joinRoomMenu.activeSelf)
        {
            JoinMenu joinMenu = menuUtils.joinRoomMenu.GetComponent<JoinMenu>();
            joinMenu.SearchRooms();
        }
    }

    private void HandleUserJoined(string room_id)
    {
        if (menuUtils.joinRoomMenu.activeSelf)
        {
            JoinMenu joinMenu = menuUtils.joinRoomMenu.GetComponent<JoinMenu>();
            joinMenu.SearchRooms();
        }
        else if (menuUtils.roomMenu.activeSelf)
        {
            bool userInRoom = RoomNetworkManager.publicRoomData.room_id == room_id;
            if (userInRoom)
            {
                RoomMenu roomMenu = menuUtils.roomMenu.GetComponent<RoomMenu>();
                UpdateRoomMenu(roomMenu);
            }
        }
    }

    private void HandleUserLeft(string room_id)
    {
        if (menuUtils.joinRoomMenu.activeSelf)
        {
            JoinMenu joinMenu = menuUtils.joinRoomMenu.GetComponent<JoinMenu>();
            joinMenu.SearchRooms();
        }
        else if (menuUtils.roomMenu.activeSelf)
        {
            bool userInRoom = RoomNetworkManager.publicRoomData.room_id == room_id;
            if (userInRoom)
            {
                RoomMenu roomMenu = menuUtils.roomMenu.GetComponent<RoomMenu>();
                UpdateRoomMenu(roomMenu);
            }
        }
    }

    private void HandleRoomDeleted(string room_id, string msg_txt, bool ownerIncluded)
    {
        if (menuUtils.joinRoomMenu.activeSelf)
        {
            JoinMenu joinMenu = menuUtils.joinRoomMenu.GetComponent<JoinMenu>();
            joinMenu.SearchRooms();
        }
        else if (menuUtils.roomMenu.activeSelf)
        {
            bool userInRoom = RoomNetworkManager.publicRoomData.room_id == room_id;
            if (userInRoom)
            {
                bool userIsOwner = RoomNetworkManager.publicRoomData.room_users[0].username == NetworkAuthentication.userPrivateData.username;
                menuUtils.ChangeMenu(menuUtils.initialMenu, MenuUtils.Side.Left);
                if (ownerIncluded || !userIsOwner)
                {
                    msg.Print(Msg.MsgType.Red, msg_txt);
                }
            }
        }       
    }

    private void UpdateRoomMenu(RoomMenu roomMenu)
    {
        roomNetworkManager.GetRoom(NetworkAuthentication.userPrivateData.user_id, RoomNetworkManager.publicRoomData.room_id, (RoomPublicData publicRoomData) =>
        {
            roomMenu.UpdateUsers(publicRoomData.room_users);
        });
    }

    private void HandleGameStarted()
    {
        gameNetworkManager.GetGame(NetworkAuthentication.userPrivateData.user_id, RoomNetworkManager.publicRoomData.room_id,
        (GamePublicData publicGameData) =>
        {
            msg.Deactivate();
            menuUtils.ChangeMenu(menuUtils.gameMenu, MenuUtils.Side.Down);
            gameMenu.InitGameMenu(publicGameData);
            gameMenu.publicGame.board.HighlightTiles();
        }
        );
    }

    private void HandleNextTurn()
    {
        gameNetworkManager.GetGame(NetworkAuthentication.userPrivateData.user_id, RoomNetworkManager.publicRoomData.room_id,
        (GamePublicData publicGameData) =>
        {
            gameMenu.publicGame.DestroyUsedCards();
            gameMenu.publicGame.UpdateGameData(publicGameData);
            gameMenu.publicGame.board.HighlightTiles();
            gameMenu.publicGame.GetMoreCards();
            StartCoroutine(HandleStateCoroutine(0.25f));
        }
        );
    }
    IEnumerator HandleStateCoroutine(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        gameMenu.ResetState();
    }

    private void HandlePlayerLeft(string username)
    {
        if (!gameMenu.isEnd)
        {
            bool isOtherPlayer = GameNetworkManager.publicGameData.other_players.Exists(otherPlayer => otherPlayer.username == username);
            if (isOtherPlayer)
            {
                gameNetworkManager.GetGame(NetworkAuthentication.userPrivateData.user_id, RoomNetworkManager.publicRoomData.room_id,
                (GamePublicData publicGameData) =>
                {
                    gameMenu.publicGame.DestroyLeftPlayer(username);
                    gameMenu.publicGame.UpdateGameData(publicGameData);
                    gameMenu.publicGame.board.HighlightTiles();
                    gameMenu.ResetState();
                    gameMenu.HandleEndOfGameForDefault();
                }
                );
            }
        }
    }

    private void HandleSwitch(int movCardIndex, int tile1_x, int tile1_y, int tile2_x, int tile2_y)
    {
        gameNetworkManager.GetGame(NetworkAuthentication.userPrivateData.user_id, RoomNetworkManager.publicRoomData.room_id,
        (GamePublicData publicGameData) =>
        {
            gameMenu.publicGame.UpdateSwitch(movCardIndex, tile1_x, tile1_y, tile2_x,tile2_y);
            gameMenu.publicGame.UpdateGameData(publicGameData);
            gameMenu.publicGame.board.HighlightTiles();
            gameMenu.ResetState();
        }
        );
    }

    private void HandleFigureDiscard(int figCardIndex)
    {
        gameNetworkManager.GetGame(NetworkAuthentication.userPrivateData.user_id, RoomNetworkManager.publicRoomData.room_id,
        (GamePublicData publicGameData) =>
        {
            gameMenu.publicGame.UpdateDiscardFigure(figCardIndex);
            gameMenu.publicGame.UpdateGameData(publicGameData);
            gameMenu.publicGame.UpdateBlockFigure();
            gameMenu.publicGame.board.HighlightTiles();
            gameMenu.publicGame.UpdateCardsLeft();
            gameMenu.publicGame.UpdateBlockedColor();
            gameMenu.ResetState();
            gameMenu.HandleEndOfGame();
        });
    }

    private void HandleFigureBlock()
    {
        gameNetworkManager.GetGame(NetworkAuthentication.userPrivateData.user_id, RoomNetworkManager.publicRoomData.room_id,
        (GamePublicData publicGameData) =>
        {
            gameMenu.publicGame.UpdateGameData(publicGameData);
            gameMenu.publicGame.UpdateBlockFigure();
            gameMenu.publicGame.board.HighlightTiles();
            gameMenu.publicGame.UpdateBlockedColor();
            gameMenu.ResetState();
        });
    }

    private void HandleTimer(int time)
    {
        if (gameMenu != null && gameMenu.publicGame != null)
        {
            gameMenu.publicGame.UpdateTimer(time);
        }
    }
}

