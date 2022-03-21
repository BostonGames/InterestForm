using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionalInfoPanel : MonoBehaviour, IPanel
{
    public InputField companyNameInput, companyTitleInput;
    
    public void ProcessInfo()
    {
    }

    public void TransferOptionalData()
    {
        UIManager.Instance.activeUserProfile.companyName = companyNameInput.text;
        UIManager.Instance.activeUserProfile.companyTitle = companyTitleInput.text;
    }
}
