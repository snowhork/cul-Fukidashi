using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	void Update ()
	{
		if(Input.GetMouseButtonDown(0)){
			RayCast ();
		}
	}

	void RayCast() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit))
		{
			var character = hit.collider.gameObject.GetComponent<Character>();
			if (character == null)
			{
				return;
			}
			character.StartConversation();
		}
	}
}
