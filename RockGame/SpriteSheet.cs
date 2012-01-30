using System.Collections;
using UnityEngine;

public class SpriteSheet : MonoBehaviour 
{
    public int _uvTieX = 1;
    public int _uvTieY = 1;
    public int _fps = 10;
	
	public bool loop = false;
    
    private Vector2 _size;
    private Renderer _myRenderer;
    private int _lastIndex = -1;
	
	private float animationStart;
    
    void Start () 
    {
        _size = new Vector2 (1.0f / _uvTieX , 1.0f / _uvTieY);
        _myRenderer = renderer;
        if(_myRenderer == null)
            enabled = false;
    }
	
	void OnEnable()
	{
		_lastIndex = -1;
		animationStart = Time.time;
	}
	
//	void OnDisable()
//	{
//		
//	}
	
    // Update is called once per frame
    void Update()
    {
        // Calculate index
        int index = (int)((Time.time - animationStart) * _fps) % (_uvTieX * _uvTieY);
        if( index > _lastIndex || (loop && index != _lastIndex))
        {
            // split into horizontal and vertical index
            int uIndex = index % _uvTieX;
            int vIndex = index / _uvTieY;
      
            // build offset
            // v coordinate is the bottom of the image in opengl so we need to invert.
            Vector2 offset = new Vector2 (uIndex * _size.x, 1.0f - _size.y - vIndex * _size.y);
            
            _myRenderer.material.SetTextureOffset ("_MainTex", offset);
            _myRenderer.material.SetTextureScale ("_MainTex", _size);
            
            _lastIndex = index;
        }
    }
}