using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG_Project
{
    public class MessageLog : MonoBehaviour
    {
        [SerializeField] Text messageText;
        [TextArea(2, 4)]
        [SerializeField] List<string> messages = new List<string>();
        int messageCap = 60;

        public void LogMessage(string message)
        {
            messageText.text = message;

            messages.Add(message);
            if (messages.Count > messageCap) messages.RemoveAt(0);
        }
    }
}