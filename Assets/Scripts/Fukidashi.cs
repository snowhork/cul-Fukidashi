using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Fukidashi : MonoBehaviour
{
	public Canvas Canvas;
	public Text Text;

	public void StartMessage(string Message) {
		Canvas.enabled = true;
		Text.text = Message;
	}

}
