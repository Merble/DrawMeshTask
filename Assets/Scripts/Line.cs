using UnityEngine;

public class Line : MonoBehaviour 
{
	[SerializeField] private LineRenderer _LineRenderer;
	[SerializeField] private GameObject _MeshObject;

	private float _pointsMinDistance;
	private float _distance;
	private float _angle;

	public int PointsCount { get; private set; }

	public void AddPoint(Vector2 newPoint)
	{
		if (PointsCount >= 1 && Vector2.Distance(newPoint, GetLastPoint()) < _pointsMinDistance)  return;

		PointsCount++;
		
		_LineRenderer.positionCount = PointsCount;
		_LineRenderer.SetPosition(PointsCount - 1, newPoint);
	}

	private Vector2 GetLastPoint()
	{
		return _LineRenderer.GetPosition(PointsCount - 1);
	}

	public void SetLineColor(Gradient colorGradient) 
	{
		_LineRenderer.colorGradient = colorGradient;
	}

	public void SetPointsMinDistance(float distance)
	{
		_pointsMinDistance = distance;
	}

	public void SetLineWidth(float width) 
	{
		_LineRenderer.startWidth = width;
		_LineRenderer.endWidth = width;
	}

	public void DrawObject(float width, GameObject parentObject)
	{
		if (parentObject.transform.childCount < 1)
		{
			GenerateObject(width,parentObject);
		}
		else
		{
			foreach (Transform child in parentObject.transform)
			{
				Destroy(child.gameObject);
			}
			
			GenerateObject(width, parentObject);
		}
	}

	private void GenerateObject(float width, GameObject parentObject)
	{
		for (var i = 0; i < _LineRenderer.positionCount-1; i++)
		{
			var vector1 = _LineRenderer.GetPosition(i);
			var vector2 = _LineRenderer.GetPosition(i + 1);

			_distance = (vector2 - vector1).magnitude;
			_angle = Mathf.Acos((vector2-vector1).x/_distance) * (180f / Mathf.PI);
			
			if (vector2.y < vector1.y)
				_angle = -_angle;
			
			var newObject = Instantiate(_MeshObject, ((vector1 + vector2) / 2f) + new Vector3(0, 5, 3), Quaternion.Euler(0, 0, _angle));
			newObject.transform.localScale = new Vector3(_distance, width, width);
			newObject.transform.parent = parentObject.transform;
		}
		
		Destroy(gameObject);
	}

}