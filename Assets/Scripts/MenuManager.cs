using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    //make sure the button names are the same as the scene names
    public void MenuOption(string option)
    {
        SceneManager.LoadScene(option);
    }
}
