using UnityEngine;

[RequireComponent(typeof(Camera))]
public class OrbitCamera : MonoBehaviour {

	[SerializeField]
	Transform focus = default;

	[SerializeField, Range(1f, 20f)]
	float distance = 5f;

	[SerializeField, Min(0f)]
	float focusRadius = 1f;

	[SerializeField, Range(0f, 1f)] //higher value = faster focusing
	float focusCentering = 0.5f;

	Vector3 focusPoint;

	void Awake()
	{
		focusPoint = focus.position;
	}

	void LateUpdate()
	{
		UpdateFocusPoint();
		Vector3 lookDirection = transform.forward;
		Vector3 lookPosition = focusPoint - lookDirection * distance;

		if (Physics.Raycast(
			focusPoint, -lookDirection, out RaycastHit hit, distance
		))
		{
            //TODO: How to handle when an object is in between camera and player
			//lookPosition = focusPoint - lookDirection * hit.distance;
		}

		transform.localPosition = lookPosition;

	}

	/*If distance between the target and current focus points is greater than
     * the radius, pull the focus toward the target, using (1−c)^t as the
     * interpolator
     */
	void UpdateFocusPoint()
	{
		Vector3 targetPoint = focus.position;
		if (focusRadius > 0f)
		{
			float distance = Vector3.Distance(targetPoint, focusPoint);
			if (distance > focusRadius)
			{
				focusPoint = Vector3.Lerp(
					targetPoint, focusPoint, focusRadius / distance
				);
			}
			if (distance > 0.01f && focusCentering > 0f)
			{
				focusPoint = Vector3.Lerp(
					targetPoint, focusPoint,
					Mathf.Pow(1f - focusCentering, Time.unscaledDeltaTime)
				);
			}
		}
		else
		{
			focusPoint = targetPoint;
		}
	}

}