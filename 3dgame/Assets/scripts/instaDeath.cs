using UnityEngine;
using System.Collections;

public class instaDeath : MonoBehaviour {
	
	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.name == "bug"){
			Debug.Log ("you dead foo!");
			Application.LoadLevel(Application.loadedLevel);
		}
	} 
	// Update is called once per frame
	void Update () {
		
	}
}
