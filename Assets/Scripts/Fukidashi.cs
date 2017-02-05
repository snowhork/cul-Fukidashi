using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Fukidashi : MonoBehaviour
{
	public Canvas Canvas;
	public Text Text;

	public void StartMessage(string Message) {
		Canvas.enabled = true;
		StartCoroutine (FlowMessage (Message));
	}

	IEnumerator FlowMessage(string Message) {
		for (int i = 0; i < Message.Length; i++) {
			Text.text = Message.Substring (0, i + 1);
			yield return new WaitForSeconds (0.1f);
		}
	}

}
