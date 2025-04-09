using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Room : MonoBehaviour
{

    // Prefab Components
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI roomName;
    [SerializeField] List<TextMeshProUGUI> userNames;
    public TextButton joinButton;

    // Prefab Settings
    public AudioController audioController;
    public string roomNameText;
    public List<UserPublicData> usernames;
    public Color color;

    public void ApplyChanges()
    {
        image.color = color;
        roomName.text = roomNameText;
        SetUsernames();
        
    }

    void SetUsernames()
    {
        if (usernames.Count <= userNames.Count)
        {
            for (int i = 0; i < userNames.Count; i++)
            {
                if (i < usernames.Count)
                {
                    userNames[i].text = usernames[i].username;
                }
                else
                {
                    userNames[i].text = "";
                }
            }
        }
    }

}
