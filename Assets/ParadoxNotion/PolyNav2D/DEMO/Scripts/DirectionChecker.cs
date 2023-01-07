using UnityEngine;
using System.Collections;
using PolyNav;

/*
* This is an example script of how you could make stuff happen when the agent is changing direction.
* In this example 4 directions are supported + idle.
* Furthermore, if doFlip is checked, the gameobject will flip on it's x axis, usefull for 2D sprites moving left/right like for example in an adventure game.
* Once again, this is an example to see how it can be done, for you to take and customize to your needs :)
*/
public class DirectionChecker : MonoBehaviour
{
    public bool doFlip = true;
    private bool isMoving = false;
    
    private Vector2 lastDir;
    private float originalScaleX;

    private PolyNavAgent _agent;
    private PolyNavAgent agent {
        get { return _agent != null ? _agent : _agent = GetComponent<PolyNavAgent>(); }
    }

    void Awake() {
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

    public void TurnLeft()
    {
        if ( doFlip ) {
            var scale = transform.localScale;
            scale.x = -originalScaleX;
            transform.localScale = scale;
        }
    }

    public void TurnRight()
    {
        print("測試");
        if ( doFlip ) {
            var scale = transform.localScale;
            scale.x = originalScaleX;
            transform.localScale = scale;
        }
    }

    void Update() {

        var dir = agent.movingDirection;

        if ( dir != lastDir ) {

            if ( dir == Vector2.zero ) {
                // Debug.Log("IDLE");
            }

            if ( dir.x > 0 ) {
                // Debug.Log("RIGHT");
                TurnRight();
            }

            if ( dir.x < 0 ) {
                // Debug.Log("LEFT");
               TurnLeft();
            }

            lastDir = dir;
        }
    }
}
