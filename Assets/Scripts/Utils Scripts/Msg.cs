using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Msg : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI msg;

    void Start()
    {
        gameObject.SetActive(false);
    }


    public enum MsgType
    {
        Red,
        Green
    }

    public void Print(MsgType msgType, string description)
    {
        gameObject.SetActive(true);
        
        switch (msgType)
        {
            case MsgType.Red:
                msg.color = Color.red;
                break;
            case MsgType.Green:
                msg.color = Color.green;
                break;
        }
        msg.text = description.ToUpper();

        if (msgType == MsgType.Red)
        {
            StartCoroutine(DeactivateCoroutine());
        }
    }

    IEnumerator DeactivateCoroutine()
    {
        yield return new WaitForSeconds(5);
        gameObject.SetActive(false);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
