using UnityEngine;
using System.Collections;
using Tobii.EyeX.Framework;
using Tobii.EyeX.Client;
using UnityEngine.UI;

[System.Serializable]
public class Boundaries{

	public float xmin, xmax, zmin, zmax;
		
}

public class PlayerControllerShip : MonoBehaviour {


	private Transform myTransform;	
	private Vector3 movement;
	private float nextFire;
	public Boundaries boundary;
	public float tiltz, tiltx;
	public float fireRate;

	private Vector3 velocity;
	private Vector3 destinationPosition;		// destination Point
	private float destinationDistance;	

	public GameObject shot;
	public Transform shotsSpawn;


	public float speed;

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
		_gazePointProvider.Stop ();
	}

	// Use this for initialization
	void Start () {
		fireRate = 0.5f;
		nextFire = 0.0f;
		myTransform = transform;
	}

	void Update () {
		if (Time.time > nextFire) {
			nextFire = Time.time + fireRate;
			Instantiate (shot, shotsSpawn.position, shotsSpawn.rotation);
		}

		var gazePoint = _gazePointProvider.Last;

		Plane playerPlane = new Plane (Vector3.up, myTransform.position);
		Vector3 eyePosition = new Vector3 (gazePoint.Screen.x, 0, 0);
		Ray ray = Camera.main.ScreenPointToRay (eyePosition);
		float hitdist = 0.0f;
		
		if (playerPlane.Raycast (ray, out hitdist)) {
			Vector3 targetPoint = ray.GetPoint (hitdist);
			destinationPosition = ray.GetPoint (hitdist);
			Vector3 targetRotation = Vector3.SmoothDamp (myTransform.position, Vector3.Scale (targetPoint, Vector3.right), ref velocity, 0.5f, 10.0f);
			transform.position = targetRotation;
			GetComponent<Rigidbody> ().position = new Vector3 
				(
					Mathf.Clamp (GetComponent<Rigidbody> ().position.x, boundary.xmin, boundary.xmax),
					0.0f,
					Mathf.Clamp (GetComponent<Rigidbody> ().position.z, boundary.zmin, boundary.zmax)
			);
			GetComponent<Rigidbody> ().rotation = Quaternion.Euler (GetComponent<Rigidbody> ().velocity.z * -tiltx, 0.0f, GetComponent<Rigidbody> ().velocity.x * -tiltz);
		}
		
		// To prevent code from running if not needed
		if (destinationDistance > 0.1f) {
			myTransform.position = Vector3.MoveTowards (myTransform.position, destinationPosition, speed * Time.deltaTime);
		}
	}
}
