using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QuickType;

public class AuthorUI : MonoBehaviour {

    public delegate void InterfaceAuthor(string author);
    public static event InterfaceAuthor SelectAuthor;

    public Comment comment;
    public Image background;
    public Text author;

    public AuthorUI(Comment _comment)
    {
        comment = _comment;       
    }

    public void SetText()
    {
        author.text = comment.Author;
    }

    public void SendSelectAuthor()
    {
        SelectAuthor(author.text);
    }

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
