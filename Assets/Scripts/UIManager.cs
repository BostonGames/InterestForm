using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    //TODO: should this be stored elsewhere?
    public string userIDkey;
    [SerializeField]
    private List<int> _userIDkeys;
   

    // app panels
    public GameObject beginPanel, mainInfoPanel, optionalInfoPanel, infoReviewPanel, completePanel;

    // input fields to clear
    public InputField name0, name1, company1, title1, birth1, email1;


    // for testing, can comment out
    private void OnEnable()
    {
    }

    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            Instance = this;
        }
        else { Destroy(this.gameObject); }
    }
    private void Start()
    {
        ShowBeginPanel();
        Debug.Log(Application.persistentDataPath);
        Debug.Log(Application.persistentDataPath + "/user");

    }



    public UserProfile activeUserProfile;

    public void SubmitButton()
    {
        // create new userProfile to save
        // populate the userProfile data
        // open a data stream to turn that object (userProfile) into a file
        // begin AWS process

        UserProfile awsProfile = new UserProfile();
        awsProfile.userIDNumber = activeUserProfile.userIDNumber;
        awsProfile.firstName = activeUserProfile.firstName;
        awsProfile.lastName = activeUserProfile.lastName;
        awsProfile.birthday = activeUserProfile.birthday;
        awsProfile.email = activeUserProfile.email;
        awsProfile.companyName = activeUserProfile.companyName;
        awsProfile.companyTitle = activeUserProfile.companyTitle;


        // so each file has a unique name
        string userIDnum = activeUserProfile.userIDNumber.ToString();
        //use a binary formatter to serialize info 
        BinaryFormatter bf = new BinaryFormatter();

        string filepath = Application.persistentDataPath + "/user" + userIDnum + ".dat";
        // create file for the info
        FileStream file = File.Create(filepath);
        bf.Serialize(file, awsProfile);
        file.Close();

        string _fileName = "user" + userIDnum;

        AWSManager.Instance.UploadToS3(filepath, _fileName);
    }

    public void CreateUserProfile()
    {
        activeUserProfile = new UserProfile();
        CreateUserIDKey();
    }

    //generates a unique user ID number to help keep database seachable becuase idk anything about AWS lol
    public void CreateUserIDKey()
    {
        int randomIDKeyNumber0 = Random.Range(0, 10000);
        int randomIDKeyNumber1 = Random.Range(0, 10000);
        int randomIDKeyNumber = randomIDKeyNumber0 + randomIDKeyNumber1;
        

        if (_userIDkeys.Contains(randomIDKeyNumber))
        {
            CreateUserIDKey();
        }
        else
        {
            _userIDkeys.Add(randomIDKeyNumber);
            // assign the number itself to use User Profile
            activeUserProfile.userIDNumber = randomIDKeyNumber;
            userIDkey = "user" + randomIDKeyNumber.ToString();
        }
    }

    public void ClearPanelInputs()
    {
        //name0, name1, company1, title1, birth1, email1
        name0.text = "";
        name1.text = "";
        company1.text = "";
        title1.text = "";
        birth1.text = "";
        email1.text = "";
    }

    public void ShowNativeKEyboard()
    {
        TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
    }
    public void ShowBeginPanel()
    {
        beginPanel.SetActive(true);
        mainInfoPanel.SetActive(false);
        optionalInfoPanel.SetActive(false);
        infoReviewPanel.SetActive(false);
        completePanel.SetActive(false);
    }

    public void ShowMainInfoPanel()
    {
        beginPanel.SetActive(false);
        mainInfoPanel.SetActive(true);
        optionalInfoPanel.SetActive(false);
        infoReviewPanel.SetActive(false);
        completePanel.SetActive(false);
    }

    public void ShowOptionalInfoPanel()
    {
        beginPanel.SetActive(false);
        mainInfoPanel.SetActive(false);
        optionalInfoPanel.SetActive(true);
        infoReviewPanel.SetActive(false);
        completePanel.SetActive(false);
    }

    public void ShowInfoReviewPanel()
    {
        beginPanel.SetActive(false);
        mainInfoPanel.SetActive(false);
        optionalInfoPanel.SetActive(false);
        infoReviewPanel.SetActive(true);
        completePanel.SetActive(false);
    }

    public void ShowCompletePanel()
    {
        beginPanel.SetActive(false);
        mainInfoPanel.SetActive(false);
        optionalInfoPanel.SetActive(false);
        infoReviewPanel.SetActive(false);
        completePanel.SetActive(true);
    }
}
