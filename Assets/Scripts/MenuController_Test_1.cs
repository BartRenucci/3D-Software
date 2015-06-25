using UnityEngine;
using System.Collections;
using Tobii.EyeX.Framework;
using Tobii.EyeX.Client;
using UnityEngine.UI;

public class MenuController_Test_1 : MonoBehaviour {
	
	public Button menu;
	int a,b;
	
	public string name1;
	//Oppening a data stream
	private EyeXHost _eyeXHost;
	private IEyeXDataProvider<EyeXGazePoint> _gazePointProvider;
	private IEyeXDataProvider<EyeXEyePosition> _dataProvider;
	
	public void Awake(){         
		_eyeXHost = EyeXHost.GetInstance();
		_dataProvider = _eyeXHost.GetEyePositionDataProvider();
		_gazePointProvider = _eyeXHost.GetGazePointDataProvider(GazePointDataMode.LightlyFiltered);     
	}  
	
	public void OnEnable(){         
		_gazePointProvider.Start();
		_dataProvider.Start();
	}  
	
	public void OnDisable(){         
		_gazePointProvider.Stop();
		_dataProvider.Stop();
	}  
	//Code
	
	void Start () {
		a = 0;
		b = 0;
	}
	
	void Update () {


		var LastEyePosition = _dataProvider.Last;
		var gazePoint = _gazePointProvider.Last; // get the last gaze point
		Vector3 gaze = new Vector3 (gazePoint.Screen.x, gazePoint.Screen.y, 0);

		if (!float.IsNaN(gaze.x) && !float.IsNaN(gaze.y))
		{
			transform.position = gaze;
		}
		/*if (!(LastEyePosition.LeftEye.IsValid || LastEyePosition.RightEye.IsValid)){
			b++;
			if (b<30)
		if (!(LastEyePosition.LeftEye.IsValid || LastEyePosition.RightEye.IsValid)){

			if ((gazePoint.Screen.x < (-190+512) && 
			    gazePoint.Screen.x > 0) 
			    && (gazePoint.Screen.y >= (384 -300) && 
			    gazePoint.Screen.y >= (-220+384))){
					a++;
					if (a >= 50){
						Application.LoadLevel(name1);
						print(name1);
						a=0;
				}
			}*/

		if (Input.GetKeyDown(KeyCode.Q)){
			Application.LoadLevel("Menu");
		}
		}
	}	