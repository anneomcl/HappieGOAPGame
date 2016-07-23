using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour 
{
	public Transform target;
	public float damping = .5f;
	public float lookAheadFactor = .5f;
	public float lookAheadReturnSpeed = 0.1f;
	public float lookAheadMoveThreshold = 0.0325f;

	private float offsetZ;
	private Vector3 lastTargetPosition;
	private Vector3 currentVelocity;
	private Vector3 lookAheadPos;

	// Use this for initialization
	void Start()
	{
		lastTargetPosition = target.position;
		offsetZ = transform.position.z - target.position.z;
		transform.parent = null;
		float s_baseOrthographicSize = Screen.height / 128.0f / 2.0f;
		Camera.main.orthographicSize = s_baseOrthographicSize;
	}

	// Update is called once per frame
	void LateUpdate()
	{
		float xMoveDelta = target.position.x - lastTargetPosition.x;
		bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;
		if (updateLookAheadTarget)
		{
			lookAheadPos = lookAheadFactor *Vector3.right *Mathf.Sign(xMoveDelta);
		}
		else
		{
			lookAheadPos = Vector3.MoveTowards(lookAheadPos, Vector3.zero, Time.deltaTime * lookAheadReturnSpeed);
		}
		Vector3 aheadTargetPos = target.position + lookAheadPos +Vector3.forward * offsetZ;
		Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref currentVelocity, damping);
		newPos = RoundToPixelUnits (128.0f, newPos);
		transform.position = newPos;
		lastTargetPosition = target.position;
	}
		
	Vector3 RoundToPixelUnits(float pixelUnits, Vector3 v){
		return new Vector3 (Mathf.Round (v.x * pixelUnits) / pixelUnits, 
			Mathf.Round (v.y * pixelUnits) /pixelUnits, Mathf.Round (v.z * pixelUnits) / pixelUnits);
	}

	void OnGUI(){
		float horizontalRatio = Screen.width / GetComponentInParent<Camera>().pixelWidth;
		float verticalRatio = Screen.height / GetComponentInParent<Camera> ().pixelHeight;
		GUI.matrix = Matrix4x4.TRS (Vector3.zero, Quaternion.identity, new Vector3 (horizontalRatio, verticalRatio, 1.0f));
	}
}