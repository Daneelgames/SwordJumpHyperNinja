using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArenaCanvasController : MonoBehaviour {

    public static ArenaCanvasController instance;
	int playerScore = 0;
	int p2Score = 0;
	public Text playerText;
	public Text p2Text;
	public Animator anim;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

	void Start()
	{
		GameManager.instance.SetArenaCanvasController(this);
	}

	public void AddPoint(string player)
	{
		if (player == "Player")
		{
			playerScore += 1;
			playerText.text = "" + playerScore;
		}
		else if (player == "P2")
		{
			p2Score += 1;
			p2Text.text = "" + p2Score;
		}
		anim.SetTrigger("ShowScore");
	}
}