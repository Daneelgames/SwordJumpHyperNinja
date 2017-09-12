using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSolidPlatform : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player")
		{
			other.gameObject.transform.SetParent(transform);
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.tag == "Player")
		{
			other.gameObject.transform.SetParent(null);
		}
	}
}