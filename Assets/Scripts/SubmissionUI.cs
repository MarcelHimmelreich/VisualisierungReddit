using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QuickType;

public class SubmissionUI : MonoBehaviour {

    public Submission submission;
    public Image background;
    public Text title;

    public SubmissionUI(Submission _submission)
    {
        submission = _submission;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
