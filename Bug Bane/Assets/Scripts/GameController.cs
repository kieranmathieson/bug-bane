using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private const int NumberOfLinesOfCode = 15;
    protected const float LineOfCodeLeftOffset = 40f;
    protected const float LineOfCodeTopOffset = 90f;
    protected const float LineOfCodeGap = 10f;
    protected const int InitialGalCredits = 10;
    protected const int BreakpointFee = 1;
    protected const int LineFixFee = 5;
    
    public GameObject lineOfCodePrefab;
    public GameObject linesOfCodeParent;
    public GameObject thumbsUp;
    public GameObject thumbsDown;

    // Object showing the galactic credits.
    public GameObject galaticCreditsDisplay;
    protected TMP_Text galaticCreditsDisplayText;

    protected int galCredits = InitialGalCredits;

    protected int _numberOfBugs;
    
    protected GameObject[] LinesOfCode = new GameObject[NumberOfLinesOfCode];
    protected LineOfCodeBehavior[] LineOfCodeBehaviors = new LineOfCodeBehavior[NumberOfLinesOfCode];
    
    private void Awake()
    {
        for (int counter = 0; counter < NumberOfLinesOfCode; counter++)
        {
            // Make a line of code object.
            GameObject lineOfCode = Instantiate(lineOfCodePrefab, new Vector3(0, 0, 0), Quaternion.identity);
            // Child of panel.
            lineOfCode.transform.SetParent(linesOfCodeParent.transform, false);
            // Set its position.
            RectTransform t = lineOfCode.GetComponent<RectTransform>();
            float top = LineOfCodeTopOffset + counter * (t.rect.height + LineOfCodeGap);
            t.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, LineOfCodeLeftOffset, t.rect.width);
            t.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, top, t.rect.height);
            // Tell it about its context.
            LineOfCodeBehavior b = lineOfCode.GetComponent<LineOfCodeBehavior>();
            b.SetGameController(this);
            b.SetLineNumber(counter);
            // Remember the creations.
            LinesOfCode[counter] = lineOfCode;
            LineOfCodeBehaviors[counter] = b;
        }
    }
    
    void Start()
    {
        galaticCreditsDisplayText = galaticCreditsDisplay.GetComponent<TMP_Text>();
        ShowGalCredits();
        RecomputeValues();
    }

    public void RecomputeValues()
    {
        _numberOfBugs = 0;
        int inputCorrectValue = 4;
        int inputActualValue = 4;
        for (int counter = 0; counter < NumberOfLinesOfCode; counter++)
        {
            LineOfCodeBehavior b = LineOfCodeBehaviors[counter];
            // Debug.Log("Counter " + counter);
            // Debug.Log("Correct " + inputCorrectValue);
            // Debug.Log("Actual " + inputActualValue);
            b.SetInputValueNoBugs(inputCorrectValue);
            b.SetInputValueWithBugs(inputActualValue);
            b.ComputeOutput();
            // Debug.Log("compute");            
            // Set values for next line of code.
            inputCorrectValue = b.GetCorrectOutput();
            inputActualValue = b.GetActualOutput();
            // Debug.Log("Correct " + inputCorrectValue);
            // Debug.Log("Actual " + inputActualValue);
            // Update bug count.
            if (b.GetHasBug())
            {
                // Debug.Log("Bug!!!!");
                _numberOfBugs++;
            }
            // Debug.Log("-----------------");
        }
        // Debug.Log("Bugs: " + _numberOfBugs);
    }

    public void ResetCodeDisplay()
    {
        for (int counter = 0; counter < NumberOfLinesOfCode; counter++)
        {
            LineOfCodeBehaviors[counter].ResetDisplay();
        }
    }

    public void ChargeForBreakpoint()
    {
        galCredits -= BreakpointFee;
        ShowGalCredits();
    }

    public void ChargeForLineFix()
    {
        galCredits -= LineFixFee;
        ShowGalCredits();
    }

    public void ShowGalCredits()
    {
        galaticCreditsDisplayText.text = galCredits.ToString();
    }

    public void TurnIn()
    {
        // How many bugs left?
        int bugs = 0;
        for (int index = 0; index < NumberOfLinesOfCode; index++)
        {
            if (this.LineOfCodeBehaviors[index].GetHasBug())
            {
                bugs++;
            }
        }
        // Show right image.
        if (bugs == 0)
        {
            thumbsUp.SetActive(true);
        }
        else
        {
            thumbsDown.SetActive(true);
        }
    }
}
