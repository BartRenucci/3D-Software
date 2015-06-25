using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public GameObject hazard;
	public Vector3 spawnValues;
	public int hazardCount;
	public float spawnWait;
	public float startWait;
	public float waveWait;
	public GUIText scoreText;
	private int scoreCount;
	public GUIText restartText;
	public GUIText gameOverText;
	public GUIText quitText;
	private bool over, restart;
	private int level;

	IEnumerator SpawnWaves () {
		yield return new WaitForSeconds (startWait);
		while(true){
			hazardCount = level*5 + hazardCount;
			for (int i =0; i < hazardCount;i++) {
				Vector3 spawnPosition = new Vector3 (Random.Range (-10, 10), 0.0f, spawnValues.z);
				Quaternion spawnRotation = Quaternion.identity;
				Instantiate (hazard, spawnPosition, spawnRotation);
				yield return new WaitForSeconds(spawnWait);
			}
			yield return new WaitForSeconds(waveWait);

			if (over){
				restartText.text = ("Press 'R' for Restart");
				quitText.text =("Press 'Q' to quit");
				restart = true;
				break;
			}
			level ++;
		}
	}

	void Start () {
		scoreCount = 0;
		UpdateScore ();
		StartCoroutine (SpawnWaves ());
		scoreText.text = "Score : 0";
		restartText.text = "";
		gameOverText.text = "";
		quitText.text = "";
		over = false;
		restart = false;
		level = 1;
	}

	void Update(){
		if (restart) {
			if (Input.GetKeyDown(KeyCode.R)){
				Application.LoadLevel("Spaceshooter");
			}
			if (Input.GetKeyDown(KeyCode.Q)){
				Application.LoadLevel("Menu");
			}
		}
	}

	public void Score (int newScoreValue) {
		scoreCount += newScoreValue;
		UpdateScore ();

	}

	public void GameOver() {
		gameOverText.text = "Game Over";
		over = true;
	}

	void UpdateScore(){
		scoreText.text = "Score: " + scoreCount;
	}
}
