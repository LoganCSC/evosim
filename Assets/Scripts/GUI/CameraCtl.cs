using UnityEngine;
using System.Collections;


/*
 *		Author: 	Craig Lomax
 *		Date: 		06.09.2011
 *		URL:		clomax.me.uk
 *		email:		craig@clomax.me.uk
 *
 */


public class CameraCtl : MonoBehaviour {

	private float sensitivity;	
	private Transform _t;
	
	float hdg = 0F;
	float pitch = 0F;

	float invert;

	Settings settings;
	
	void Awake () {
		_t = transform;
	}
	
	void Start() {
		settings = Settings.getInstance();
		sensitivity = settings.camera_sensitivity;
		invert = settings.camera_invert;
		invert = invert == 0 ? 1 : -1;

		pitch = _t.localEulerAngles.x;
		GetComponent<Camera>().farClipPlane = 100000;
	}

	void LateUpdate() {
		float deltaX = Input.GetAxis("Mouse X") * sensitivity;
		float deltaY = Input.GetAxis("Mouse Y") * sensitivity;

		if(Input.GetMouseButton(2)) {
			pan (deltaX, deltaY);
		}
	
		if (Input.GetMouseButton(1) && !Input.GetMouseButton(0)) {
			moveForwards(-deltaY);
			setHeading(deltaX);
		}
		
		if(Input.GetMouseButton(1) && Input.GetMouseButton(0)) {
			setHeading(deltaX);
			setPitch(-deltaY);
		}

	}

	void pan(float dx, float dy) {
		Camera.main.transform.position += invert * (Camera.main.transform.right * dx * sensitivity);
		Camera.main.transform.position += invert * (Camera.main.transform.up * dy * sensitivity);
	}

	void moveForwards(float n) {
		Vector3 fwd = transform.forward;
		fwd.y = 0;
		fwd.Normalize();
		fwd.z *= sensitivity;
		fwd.x *= sensitivity;
		Camera.main.transform.position += invert * -(n * fwd);
	}

	void setHeight(float n) {
		transform.position += invert * (n * Vector3.up);
	}

	void setHeading(float n) {
		hdg += invert * n;
		wrapAngle(ref hdg);
		transform.localEulerAngles = new Vector3(pitch, hdg, 0);
	}

	void setPitch(float n) {
		pitch += invert * n;
		wrapAngle(ref pitch);
		transform.localEulerAngles = new Vector3(pitch, hdg, 0);
	}

	private static void wrapAngle(ref float angle) {
		if (angle < -360F)
			angle += 360F;
		if (angle > 360F)
			angle -= 360F;
	}
}
