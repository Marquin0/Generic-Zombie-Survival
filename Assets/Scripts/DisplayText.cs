using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayText : MonoBehaviour {

    public int delay = 0;
    public bool loopMessages = false;
    public MessageConstraint[] messages = new MessageConstraint[1];
    private int currentMessage = 0;
    private string text;
    private Color color;
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if (currentMessage >= messages.Length)
                return;

            var msg = messages[currentMessage++];
            if(other.gameObject.GetComponent<PlayerController>().powerCount >= (int)msg.constraint)
            {
                text = msg.message;
                color = msg.color;
                Invoke("DisplayMessage", delay);
            }
            else
            {
                currentMessage--;
            }

            if (loopMessages && currentMessage >= messages.Length)
                currentMessage = 0;
        }
    }

    private void DisplayMessage()
    {
        GameObject.Find("Message").GetComponent<TextAimation>().DisplayText(text, color, TextAnimation.Scrolling);
    }
}

[System.Serializable]
public class MessageConstraint
{
    [TextArea]
    public string message;
    public PlayerPower constraint;
    public Color color = Color.black;
}