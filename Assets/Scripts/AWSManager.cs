using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Amazon;
using UnityEngine.UI;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;
using System.IO;
using System;
using Amazon.S3.Util;
using Amazon.CognitoIdentity;

public class AWSManager : MonoBehaviour
{
    public static AWSManager Instance;
    public string S3Region = RegionEndpoint.USEast1.SystemName;
    private RegionEndpoint _S3Region
    {
        get { return RegionEndpoint.GetBySystemName(S3Region); }
    }

    // Initialize the Amazon Cognito credentials provider
    private AmazonS3Client _s3Client;
    public AmazonS3Client S3Client
    {
        get
        {
            if(_s3Client == null)
            {
                _s3Client = new AmazonS3Client(new CognitoAWSCredentials(
                "us-east-1:3295bf09-8b63-4749-823e-4756a70ad881", // Identity Pool ID
                RegionEndpoint.USEast1), _S3Region);
            }

            return _s3Client;
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            Instance = this;
        }
        else { Destroy(this.gameObject); }


        UnityInitializer.AttachToGameObject(this.gameObject);
        // use unity web request as the http request
        AWSConfigs.HttpClient = AWSConfigs.HttpClientOption.UnityWebRequest;



        S3Client.ListBucketsAsync(new ListBucketsRequest(), (responseObject) =>
        {
            if (responseObject.Exception == null)
            {
                responseObject.Response.Buckets.ForEach((s3b) =>
                {
                    Debug.Log("AWS Bucket Name: " + s3b.BucketName + "Created On: " + s3b.CreationDate);
                });
            }
            else
            {
                Debug.Log("AWS Error: " + responseObject.Exception);
            }
        });
    }

    /// <summary>
    ///     
    /// </summary>
    /// <param name="filename">persistent data path to user profile .dat file</param>
    public void UploadToS3(string path, string filename)
    {
        FileStream stream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);


        //request to upload to stream
        PostObjectRequest request = new PostObjectRequest()
        {
            Bucket = "gdcformfiles",
            Key = filename,
            InputStream = stream,
            CannedACL = S3CannedACL.Private,
            Region = _S3Region
        };

        S3Client.PostObjectAsync(request, (responseObj) =>
         {
             if (responseObj.Exception == null)
             {
                 Debug.Log("Successfully posted to bucket");
             }
             else
             {
                 Debug.Log("Exception Occured during AWS uploading: " + responseObj.Exception);

             }
         });
    }

}
