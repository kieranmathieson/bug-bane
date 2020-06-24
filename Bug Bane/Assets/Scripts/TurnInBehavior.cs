using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TurnInBehavior : MonoBehaviour, IPointerClickHandler
{
    public GameObject gameController;
    protected GameController _gameController;
    
    void Start()
    {
        _gameController = gameController.GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _gameController.TurnIn();
    }
}
