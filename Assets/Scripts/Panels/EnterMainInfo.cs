using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnterMainInfo : MonoBehaviour, IPanel
{
    public InputField firstNameInput, lastNameInput, birthdayDateInput, emailInput;
    
    public void ProcessInfo()
    {
    }

    public void TransferMainEnteredData()
    {
        UIManager.Instance.activeUserProfile.firstName = firstNameInput.text;
        UIManager.Instance.activeUserProfile.lastName = lastNameInput.text;
        UIManager.Instance.activeUserProfile.birthday = birthdayDateInput.text;
        UIManager.Instance.activeUserProfile.email = emailInput.text;
    }
}
