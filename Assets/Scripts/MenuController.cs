using UnityEngine;
using System.Collections;
using Tobii.EyeX.Framework;
using Tobii.EyeX.Client;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {
	
	public Button menu;
	public Button spaceShooter;
	public Button quit;
	int a,b,c;
	
	public string name1, name2;
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
		c = 0;
		var LastEyePosition = _dataProvider.Last;
		var gazePoint = _gazePointProvider.Last; // get the last gaze point
		Vector3 gaze = new Vector3 (gazePoint.Screen.x, gazePoint.Screen.y, 0);
	}
	
	void Update () {
		
		var LastEyePosition = _dataProvider.Last;
		var gazePoint = _gazePointProvider.Last; // get the last gaze point
		Vector3 gaze = new Vector3 (gazePoint.Screen.x, gazePoint.Screen.y, 0);

		if (!float.IsNaN(gaze.x) && !float.IsNaN(gaze.y))
		{
			transform.position = gaze;
		}

		if (Input.GetKeyDown(KeyCode.P)) {
			print (gaze.x);
		}



		if (!(LastEyePosition.LeftEye.IsValid || LastEyePosition.RightEye.IsValid)){


			if ((transform.position.x >= -412 && 
			     transform.position.x <= 612)
			    && (transform.position.y >= 384) && 
			    transform.position.y <= 484){
				b++;
				if (b >= 20){
					Application.LoadLevel(name2);
					print(name2);
					b=0;
				}
			}

			if ((transform.position.x > -412 && 
			     transform.position.x < 642)
			    && (transform.position.y > (-145+384)) && 
			    transform.position.y < (-45+384)){
					a++;
					if (a >= 20){
						Application.LoadLevel(name1);
						print(name1);
						a=0;
				}
			}

			if ((transform.position.x > -412 && 
			     transform.position.x < 642)
			    && (transform.position.y > (-300+384)) && 
			    transform.position.y < (-200+384)){
				a++;
				if (a >= 10){
					Application.Quit();
					print(name1);
					a=0;
				}
			}


		}
	}
}