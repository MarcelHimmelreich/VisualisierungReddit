using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using QuickType;


public class Model : MonoBehaviour {

    //Controller
    public GameObject GraphManager;
    //View
    public GameObject UserInterface;

    public Subreddit subreddit { get; set; }

    public List<Submission> submissions { get; set; }

    public string file_path;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //Load Json Data and stores it
    //Create a Dictionary from Json
    public void LoadJson() {
        if (file_path != null)
        {
            Debug.Log("Loading Json Data...");
            var json = File.ReadAllText(file_path);

            subreddit = Subreddit.FromJson(json);
            if (subreddit != null)
            {
                UserInterface.GetComponent<UserInterfaceManager>().Loaded();
                Debug.Log("Loading Data Success!");
            }
            else
            {
                Debug.Log("Unable to load json data with given path!");
            }
        }
        else
        {
            Debug.Log("Unable to load json data with given path!");
        }

    }

    public void SetFilePath(string path)
    {
        file_path = path;
    }

    public void PrintAll()
    {
        Debug.Log(System.Environment.Version);
        /*foreach (Submission submission in Submissions.submission)
        {
            Debug.Log(submission);
            Debug.Log("Submission Name: " + submission.title);
            Debug.Log("Submission Author: " + submission.author);
            Debug.Log("Submission ID: " + submission.id);
            Debug.Log("Submission Comments: " + submission.comments.comment.Count);
            foreach (Comment comment in submission.comments.comment) {
                Debug.Log("Comment Author: " + comment.author);

                int value = 0;
                if (comment.replies!= null)
                {
                    value = comment.replies.comment.Count;
                }
                else {  value  =0; }
                Debug.Log("Comment Replies: " + value);
            }

        }*/
    }

    public void CreateDataObjects() {

    }
    
}
