using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {
	public Fukidashi fukidashi;
	public string Message;
    public void StartConversation()
    {
		fukidashi.StartMessage (Message);
    }
}
