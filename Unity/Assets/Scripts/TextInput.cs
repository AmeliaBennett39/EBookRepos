using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextInput : MonoBehaviour {

    //Input field 
    public InputField inputField;

    //Game object
    public GameObject textBox;

    public Button button;

    string words;

    Text thetext;

    public Text theWord;

    InputField theInput;


    void Awake()
    {
        inputField.image.enabled = false;
        inputField.placeholder.enabled = false;
        inputField.enabled = false;

        //After user enters word and exits input box or presses enter, 
        //function AcceptStringInput is executed
        button.onClick.AddListener(TaskOnClick);
        
    }

    void TaskOnClick()
    {
       
        Debug.Log("Clicked");
        if (inputField.enabled == false)
        {
            inputField.image.enabled = true;
            inputField.placeholder.enabled = true;
            inputField.enabled = true;
            inputField.onEndEdit.AddListener(AcceptStringInput);
        }
        else
        {
            inputField.image.enabled = false;
            inputField.placeholder.enabled = false;
            inputField.enabled = false;
        }

    }

    void AcceptStringInput(string userInput)
    {
      
        //Grabs the text from the game object
        thetext = textBox.GetComponent<Text>();
        
        //Stores the text in lower case in the string variable words
        words = thetext.text.ToLower();

        //Split input into seperate words (split by single space)
        string[] strings = inputField.text.ToLower().Split(' ');
        //If the text contains my input from the input field return found to unity console, else return not found
        foreach (var word in strings)
        {
            if (words.Contains(word))
            {
                Debug.Log("FOUND: " + word);
                theWord.text = inputField.text;
                //call function for finding definition with inputField.text.ToLower(); //lowercase ver of string
            }
            else
            {
                Debug.Log("NOT FOUND: " + word);
                theWord.text = "NOT FOUND: " + inputField.text;
                //output "Cannot find " + inputField.text.ToLower();
            }
        }

        //After word is found, input field is cleared and has focus
        InputComplete();

    }

    void InputComplete()
    {
        inputField.ActivateInputField();    //Puts focus on input field
        inputField.text = null;             //Clears input field
    }


}
