using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FixButtonClicked : MonoBehaviour, IPointerClickHandler
{
    protected GameObject _lineOfCode;
    protected LineOfCodeBehavior _lineOfCodeBehavior;
    
    void Start()
    {
        _lineOfCode = transform.parent.gameObject;
        _lineOfCodeBehavior = _lineOfCode.GetComponent<LineOfCodeBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _lineOfCodeBehavior.FixLine();
    }
}
