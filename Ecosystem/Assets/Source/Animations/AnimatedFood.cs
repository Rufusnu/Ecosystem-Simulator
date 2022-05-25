using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedFood : MonoBehaviour
{
    private bool ready = false;
    private float _animationElapsedTime;
    private Vector3 _animationStartPos;
    private Vector3 _animationEndPos;
    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (ready)
        {
            animate();
        }
    }

    public void updateAttributes(Sprite sprite, Color color, Vector3 localPosition, Vector3 scale, Vector3 targetPosition)
    {
        gameObject.AddComponent<SpriteRenderer>();    
        gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
        gameObject.GetComponent<SpriteRenderer>().color = color;
        gameObject.transform.SetParent(GridDomain.GridMap.currentGridInstance.getObject().transform);
        gameObject.transform.localPosition = localPosition;
        this._animationStartPos = localPosition;
        this._animationEndPos = targetPosition;
        gameObject.transform.localScale = scale;
        this._animationElapsedTime = 0;
        ready = true;
    }

    private void animate()
    {
        this._animationElapsedTime = Mathf.Min(1, this._animationElapsedTime + Time.deltaTime * 2);
        gameObject.transform.localPosition = Vector3.Lerp(this._animationStartPos, this._animationEndPos, this._animationElapsedTime);
        this.gameObject.transform.localScale -= new Vector3(0.1F, .1f, .1f) * Time.deltaTime * 30;
        if (this.gameObject.transform.localScale.x < 0 && this.gameObject.transform.localScale.y < 0 && this.gameObject.transform.localScale.z < 0)
            this.gameObject.transform.localScale = new Vector3(0,0,0);
        
        if (this._animationElapsedTime >= 1)
        {
            Destroy(gameObject);
        }
    }
}
