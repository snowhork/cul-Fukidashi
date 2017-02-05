using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {
	public Fukidashi fukidashi;
    public void StartConversation()
    {
		fukidashi.StartMessage ("Hello, World");
    }
}
