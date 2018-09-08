using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QuickType;

public class NodeUI : MonoBehaviour {

    public Comment comment;
    public Image background;
    public Text author;

    public NodeUI(Comment _comment)
    {
        comment = _comment;
    }

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
