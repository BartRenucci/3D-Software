using UnityEngine;
using System.Collections;
using Tobii.EyeX.Framework;
using Tobii.EyeX.Client;

public class CameraMovement : MonoBehaviour {
	
	public GameObject player;
	
	private Vector3 offset;

	private int width;

	public float rotationSpeed;
	

	//Opening a data stream

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


		width = Screen.width;
		offset = transform.position - player.transform.position;
		
		
	}

	
	void LateUpdate () {
		var gazePoint = _gazePointProvider.Last;

		transform.LookAt(player.transform.position);
		if (gazePoint.Screen.x <= (0.35f*width)) {
			transform.position = player.transform.position + offset;
			rotationSpeed = (1-(gazePoint.Screen.x/width))*2;
			transform.RotateAround(player.transform.position, Vector3.up, -rotationSpeed*25*Time.deltaTime);
			offset = transform.position - player.transform.position;
		}

		if (gazePoint.Screen.x >= (0.65f*width)) {
			transform.position = player.transform.position + offset;
			rotationSpeed = 2*gazePoint.Screen.x/width;
			transform.RotateAround(player.transform.position, Vector3.up, rotationSpeed*25*Time.deltaTime);
			offset = transform.position - player.transform.position;
		}

		transform.position = player.transform.position + offset;
	}
}