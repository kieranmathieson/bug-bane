using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class LineOfCodeBehavior : MonoBehaviour, IPointerClickHandler 
{
    const float MinTransmogrificationValue = 5f;
    const float MaxTransmogrificationValue = 20f;
    const float ProbabilityOfBug = 0.125f;
    
    protected enum TransmogrificationOperator
    {
        Add,
        Subtract
    }

    private GameController _gameController;
    private int _lineNumber;
    private int _inputValueNoBugs;
    private int _inputValueWithBugs;
    private int _correctOutputValue;
    private int _actualOutputValue;

    private TMP_Text _text;
    private TMP_Text _lineNumberDisplay;
    
    // Does this line of code have a bug?
    private bool _hasBug;
    
    // Transmogrification that is correct for this line of code.
    private TransmogrificationOperator _correctTransmogrification;
    // Transmogrification value that is correct for this line of code.
    private int _correctTransmogrificationValue;

    // Transmogrification that happens is this line of code has a bug.
    private TransmogrificationOperator _bugTransmogrification;
    // Transmogrification value if this line of code has a bug.
    private int _bugTransmogrificationValue;

    public void SetGameController(GameController gameControllerIn)
    {
        this._gameController = gameControllerIn;
    }

    public void SetLineNumber(int lineNumberIn)
    {
        _lineNumber = lineNumberIn;
        _lineNumberDisplay.text = (lineNumberIn + 1).ToString() + ".";
    }

    /**
     * Set the input value when there are no bugs in the entire program.
     */
    public void SetInputValueNoBugs(int inputValueIn)
    {
        this._inputValueNoBugs = inputValueIn;
    }
    
    /**
     * Set the input value when there are bugs in the entire program.
     * This is the actual output from the prior line.
     */
    public void SetInputValueWithBugs(int inputValueIn)
    {
        this._inputValueWithBugs = inputValueIn;
    }

    public int GetCorrectOutput()
    {
        return _correctOutputValue;
    }

    public int GetActualOutput()
    {
        return _actualOutputValue;
    }

    public void ComputeOutput()
    {
        // Correct.
        if (_correctTransmogrification == TransmogrificationOperator.Add)
        {
            _correctOutputValue = _inputValueNoBugs + _correctTransmogrificationValue;
        }
        else
        {
            _correctOutputValue = _inputValueNoBugs - _correctTransmogrificationValue;
        }
        // Actual.
        if (this._hasBug)
        {
            if (_bugTransmogrification == TransmogrificationOperator.Add)
            {
                _actualOutputValue = _inputValueWithBugs + _bugTransmogrificationValue;
            }
            else
            {
                _actualOutputValue = _inputValueWithBugs - _bugTransmogrificationValue;
            }
        }
        else
        {
            // No bug.
            if (_correctTransmogrification == TransmogrificationOperator.Add)
            {
                _actualOutputValue = _inputValueWithBugs + _correctTransmogrificationValue;
            }
            else
            {
                _actualOutputValue = _inputValueWithBugs - _correctTransmogrificationValue;
            }
        }
    }

    
    void Awake()
    {
        // Set up correct transmogrification.
        _correctTransmogrificationValue = (int)Random.Range(MinTransmogrificationValue, MaxTransmogrificationValue);
        _correctTransmogrification = ChooseRandomTransmogrificationOperator();
        
        // Is there a bug?
        _hasBug = Random.Range(0f, 1f) < ProbabilityOfBug;
        if (_hasBug)
        {
            // Set up buggy transmogrification.
            _bugTransmogrificationValue = (int)Random.Range(MinTransmogrificationValue, MaxTransmogrificationValue);
            _bugTransmogrification = ChooseRandomTransmogrificationOperator();
        }
        // Find the line text component.
        _text = this.transform.GetChild(0).GetComponent<TMP_Text>();
        // Find the line number component.
        _lineNumberDisplay = this.transform.GetChild(1).GetComponent<TMP_Text>();
    }

    protected TransmogrificationOperator ChooseRandomTransmogrificationOperator()
    {
        TransmogrificationOperator result = Random.Range(0f, 10f) < 5f 
            ? TransmogrificationOperator.Add 
            : TransmogrificationOperator.Subtract;
        return result;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        this._gameController.ResetCodeDisplay();
        this._gameController.ChargeForBreakpoint();
        this.ShowOutput();
    }

    protected void ShowOutput()
    {
        string output = "Correct: " + _correctOutputValue + "  Actual: " + _actualOutputValue;
        _text.text = output;
    }

    public void ResetDisplay()
    {
        _text.text = "";
    }

    public void FixLine()
    {
        this._hasBug = false;
        this._gameController.ResetCodeDisplay();
        this._gameController.RecomputeValues();
        this._gameController.ChargeForLineFix();
        this.ShowOutput();
    }

    public bool GetHasBug()
    {
        return this._hasBug;
    }
}
