using UnityEngine;
using System.Collections;

public class Play : MonoBehaviour {

	public void OnClickPlay (){
		Application.LoadLevel ("Spaceshooter");
	}

	public void OnClickExit (){
		Application.Quit();
	}

	public void OnClickMenu (){
		Application.LoadLevel ("menu");
	}
}
