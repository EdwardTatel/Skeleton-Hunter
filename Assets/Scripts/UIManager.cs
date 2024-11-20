using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI sliderText = null;
    [SerializeField] private float maxSliderAmount = 100.0f;
    public void SliderChange(float value)
    {
        float localValue = value * maxSliderAmount;
        sliderText.text = localValue.ToString("0");
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
