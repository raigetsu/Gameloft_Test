using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SC_MainMenu : MonoBehaviour
{
   public void OpenLevel()
    {
        SceneManager.LoadScene(1);
    }
}
