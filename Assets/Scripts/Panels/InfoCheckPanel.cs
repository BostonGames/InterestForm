using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoCheckPanel : MonoBehaviour
{
    public Text userIDNumber0;
    public Text firstName0, lastName0, birthday0, email0, companyName0, companyTitle0;


    private void OnEnable()
    {
        userIDNumber0.text = UIManager.Instance.activeUserProfile.userIDNumber.ToString();
        firstName0.text = UIManager.Instance.activeUserProfile.firstName;
        lastName0.text = UIManager.Instance.activeUserProfile.lastName;
        birthday0.text = UIManager.Instance.activeUserProfile.birthday;
        email0.text = UIManager.Instance.activeUserProfile.email;
        companyName0.text = UIManager.Instance.activeUserProfile.companyName;
        companyTitle0.text = UIManager.Instance.activeUserProfile.companyTitle;
    }
}
