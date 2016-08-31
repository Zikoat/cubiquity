using UnityEngine;
using System.Collections;

public class collisionTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame

	void OnCollisionStay(Collision collision) {
		Vector3 average = new Vector3();
		foreach (ContactPoint contact in collision.contacts) {
			Debug.DrawRay(contact.point, contact.normal, Color.white, 0);
			average += contact.point;
		}
		average /= collision.contacts.Length;
		Debug.DrawRay(average, transform.position - average, Color.red, 1);

	}


	void fixedUpdate() {
		/*Transform parentTransform = GetComponentInParent<Transform> ();
		Transform thisTransform = GetComponent<Transform> ();
		transform.localPosition = Vector3.zero;
		thisTransform = parentTransform;*/
	}
}
