using UnityEngine;
using System.Collections;
using Tobii.EyeX.Framework;
using Tobii.EyeX.Client;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
	private Transform myTransform;			// player transform
	private Vector3 destinationPosition;		// destination Point
	private float destinationDistance;			// distance between myTransform and destinationPosition
	
	private float moveSpeed;						// The Speed the character will move
	private float moveSpeed1;
	private Vector3 velocity;

	public Text countText;
	private int count;
	public Text winText;
	public Button menu;
	

	//Oppening a data stream
	private EyeXHost _eyeXHost;
	private IEyeXDataProvider<EyeXGazePoint> _gazePointProvider;

	public void Awake(){         
		_eyeXHost = EyeXHost.GetInstance();         
		_gazePointProvider = _eyeXHost.GetGazePointDataProvider(GazePointDataMode.LightlyFiltered);
	}  

	public void OnEnable(){         
		_gazePointProvider.Start();
	}  

	public void OnDisable(){         
		_gazePointProvider.Stop(); 
	}  
	//Code

	void Start () {
		velocity = Vector3.zero;
		myTransform = transform;							// sets myTransform to this GameObject.transform
		destinationPosition = myTransform.position;			// prevents myTransform reset
		count = 0;											// the count of cube to collect
		SetCountText ();
		winText.text = "";
	}
	
	void Update () {

		destinationDistance = Vector3.Distance(destinationPosition, myTransform.position); // keep track of the distance between this gameObject and destinationPosition
			var gazePoint = _gazePointProvider.Last; // get the last gaze point

		if(destinationDistance < 0.1f){		// prevent shakin behavior when near destination
			moveSpeed = 2;
		}
		else if(destinationDistance > 0.1f){			// reset Speed to default
			moveSpeed1 = 0.05f*destinationDistance+5;
			if(moveSpeed1 < 45){
				moveSpeed = moveSpeed1;
			}
			else{
				moveSpeed = 45;  // maximal Speed
			}
		}

		if (!float.IsNaN (_gazePointProvider.Last.Screen.x) && !float.IsNaN (_gazePointProvider.Last.Screen.y)) {
			
			Plane playerPlane = new Plane (Vector3.up, myTransform.position);
			Vector3 eyePosition = new Vector3 (gazePoint.Screen.x, gazePoint.Screen.y, 0);
			Ray ray = Camera.main.ScreenPointToRay (eyePosition);
			float hitdist = 0.0f;
			
			if (playerPlane.Raycast (ray, out hitdist)) {
				Vector3 targetPoint = ray.GetPoint (hitdist);
				destinationPosition = ray.GetPoint (hitdist);
				Vector3 targetRotation = Vector3.SmoothDamp (myTransform.position, targetPoint, ref velocity, 0.5f, 10.0f);
				transform.position = targetRotation;
			}
		}

		// To prevent code from running if not needed
		if(destinationDistance > 0.1f){
			myTransform.position = Vector3.MoveTowards(myTransform.position, destinationPosition, moveSpeed * Time.deltaTime);
		}
	}

	
	void OnTriggerEnter (Collider other) 
	{
		if (other.gameObject.CompareTag ("PickUp")) {
			other.gameObject.SetActive (false);
			count ++;
			SetCountText ();
		}
	}
	
	void SetCountText () {
		countText.text = "Count :   " + count.ToString ();
		if (count >= 31) {
			winText.text = "You Win !";
			
		}
	}
}
