using UnityEngine;
using System.Collections;

public class TitleScreen : MonoBehaviour {
    void PlayClicked()
    {
        Application.LoadLevel("Level1");
    }
}
