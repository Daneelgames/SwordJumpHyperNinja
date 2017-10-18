using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorTriggerReciever : MonoBehaviour {

	public GameObject kiss;

	public void HidePlayer()
	{
		GameManager.instance.TogglePlayer(false);
	}

	public void HideGoal()
	{
		GameManager.instance.ToggleGoal(false);
	}

	public void BirthPlayer()
	{
		GameManager.instance.TogglePlayer(true);
	}

	public void UnhideGoal()
	{
		GameManager.instance.ToggleGoal(true);
	}

	public void MotherKiss()
	{
		Instantiate(kiss, transform.position, Quaternion.identity);
	}
}