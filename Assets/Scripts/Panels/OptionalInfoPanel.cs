using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionalInfoPanel : MonoBehaviour, IPanel
{
    public InputField companyName, companyTitle;
    public Toggle rPG, shooter, adventure, puzzle, racing, mystery, mmo, sandbox; 
    
    public void ProcessInfo()
    {
    }
}
