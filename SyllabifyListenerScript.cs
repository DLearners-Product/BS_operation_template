using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SyllabifyListenerScript : MonoBehaviour
{
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(
            () => { 
                BlendedOperations.instance.SendDataToSylabify((gameObject.GetComponent<Text>()) ? gameObject.GetComponent<Text>().text : gameObject.GetComponent<TMP_Text>().text);
            }
        );
    }
}
