using Newtonsoft.Json;
using System.Collections.Generic;

[System.Serializable]
public class UserPublicData
{
    public string username;
}

[System.Serializable]
public class UserPrivateData
{
    public string user_id;
    public string username;
    public string password;
}

[System.Serializable]
public class RoomPublicData
{
    public string room_id;
    public string room_name;
    public GameColor room_color;
    public List<UserPublicData> room_users;
}


[System.Serializable]
public class GamePublicData
{
    public RoomPublicData room;
    public PlayerData my_player;
    public List<PublicPlayerData> other_players;
    public BoardData board;
}

[System.Serializable]
public class BoardData
{
    public TileColor blocked_color;
    public List<TileData> tiles;
}

[System.Serializable]
public class PlayerData
{
    public string username;
    public List<MovCardData> mov_cards;
    public List<FigCardData> player_deck;
    public int fig_cards_left;
    public bool in_turn;
    public GameColor player_color;
}

[System.Serializable]
public class PublicPlayerData
{
    public string username;
    public List<FigCardData> fig_cards_in_hand;
    public int fig_cards_left;
    public bool in_turn;
    public GameColor player_color;
}

[System.Serializable]
public class TileData
{
    public TileColor color;
    public FigType figure;
}

[System.Serializable]
public class MovCardData
{
    public MovType mov_type;
    public bool is_used;
}

[System.Serializable]
public class FigCardData
{
    public FigType fig_type;
    public bool in_hand;
    public bool is_blocked;
    public bool is_used;
}

[System.Serializable]
public enum TileColor
{
    red, yellow, green, blue, none
}

[System.Serializable]
public enum MovType
{
    mov1, mov2, mov3, mov4, mov5, mov6, mov7
}

[System.Serializable]
public enum FigType
{
    fige1, fige2, fige3, fige4, fige5, fige6, fige7,
    fig01, fig02, fig03, fig04, fig05, fig06, fig07,
    fig08, fig09, fig10, fig11, fig12, fig13, fig14,
    fig15, fig16, fig17, fig18, none
}

[System.Serializable]
public enum GameColor
{
    color1,
    color2,
    color3,
    color4,
    color5,
    color6,
    color7,
    color8,
    color9,
    color10,
}
