using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    [SerializeField] private GameObject hpDisplay;
    [SerializeField] private GameObject scoreDisplay;
    public static int hp = 0;
    public static int score = 0;
    // Start is called before the first frame update

    // Update is called once per
    // 
    private void Start()
    {
        score = 0;
    }
    void Update()
    {
        hpDisplay.GetComponent<TextMeshProUGUI>().text = "HP: " + hp + "";
        scoreDisplay.GetComponent<TextMeshProUGUI>().text = "SCORE: " + score + "";
    }

    public void DisablePopup(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }
}
