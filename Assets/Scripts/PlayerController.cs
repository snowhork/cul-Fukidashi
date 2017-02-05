using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	void Update ()
	{
	    RaycastHit hit;
	    if (Physics.Raycast(transform.position, transform.forward, out hit))
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
