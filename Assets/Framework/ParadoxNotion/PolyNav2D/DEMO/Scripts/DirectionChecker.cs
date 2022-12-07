using UnityEngine;
using System.Collections;

public class DirectionChecker : MonoBehaviour {

    public bool doFlip = true;
    private bool isMoving = false;

    private Vector2 lastDir;

    private float originalScaleX;

    private PolyNavAgent _agent;
    private PolyNavAgent agent{
        get {return _agent != null? _agent : _agent = GetComponent<PolyNavAgent>();}
    }
 
 	void Awake(){
 		originalScaleX = transform.localScale.x;

        agent.OnDestinationReached += () =>
        {
            isMoving = false;
        };
        
        agent.OnNavigationStarted += () =>
        {
            isMoving = true;
        };
    }

    public void Stop()
    {
        isMoving = false;
    }

    public void SetOriginalScaleX(float value)
    {
        originalScaleX = value;
    }

    public bool CheckIsMoving()
    {
        return isMoving;
    }

    void Update() {
 
        var dir = agent.movingDirection;
        var x = Mathf.Round(dir.x);
        var y = Mathf.Round(dir.y);
 
        //eliminate diagonals favoring x over y
        y = Mathf.Abs(y) == Mathf.Abs(x)? 0 : y;
     
        dir = new Vector2(x, y);
 
        if (dir != lastDir){
 
            if (dir == Vector2.zero){
            }
 
            if (dir.x == 1){
                if (doFlip){
                    var scale = transform.localScale;
                	scale.x = originalScaleX;
                	transform.localScale = scale;
                }
            }
 
            if (dir.x == -1){
                if (doFlip){
                    var scale = transform.localScale;
                	scale.x = -originalScaleX;
                	transform.localScale = scale;
                }
            }
 
            if (dir.y == 1){
            }
 
            if (dir.y == -1){
            }
 
            lastDir = dir;
        }
    }
}
