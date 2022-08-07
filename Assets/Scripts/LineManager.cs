using UnityEngine;
using UnityEngine.EventSystems;

public class LineManager : MonoBehaviour , IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform _RectTransform;
    [SerializeField] private GameObject _LinePrefab;
    [SerializeField] private Gradient _LineColor;
    [SerializeField] private GameObject _MeshParentObject;
    [Space ( 20f )]
    [SerializeField] private float _LinePointsMinDistance;
    [SerializeField] private float _LineWidth;

    private Camera _cam;
    private Line _currentLine;
    
    private Vector3 _tempMousePos;
    
    private void Start() 
    {
        _cam = Camera.main;
    }

    private void Update() 
    {
        if (_currentLine != null)
        {
            Draw();
        }
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        BeginDraw();
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        EndDraw();
    }
    
    private void BeginDraw() 
    {
        _currentLine = Instantiate(_LinePrefab, transform).GetComponent<Line>( );
        
        _currentLine.SetLineColor (_LineColor);
        _currentLine.SetPointsMinDistance (_LinePointsMinDistance);
        _currentLine.SetLineWidth (_LineWidth);
    }
    
    private void Draw()
    {
        _tempMousePos = Input.mousePosition;
        
        var anchorOffsetX = Screen.width / 2;
        _tempMousePos.x = Mathf.Clamp(_tempMousePos.x, _RectTransform.offsetMin.x + anchorOffsetX, _RectTransform.offsetMax.x +anchorOffsetX);
        _tempMousePos.y = Mathf.Clamp(_tempMousePos.y, _RectTransform.offsetMin.y, _RectTransform.offsetMax.y);
        _tempMousePos.z = 5f;
        
        var mousePosition = _cam.ScreenToWorldPoint(_tempMousePos);
        _currentLine.AddPoint (mousePosition);
    }

    private void EndDraw()
    {
        if (_currentLine == null) return;
        
        if (_currentLine.PointsCount < 2)   // If line has one point
        {
            Destroy(_currentLine.gameObject);
        } 
        else 
        {
            _currentLine.DrawObject(_LineWidth, _MeshParentObject);
            _currentLine = null;
        }
    }
}