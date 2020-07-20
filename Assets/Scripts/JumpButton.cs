using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JumpButton : MonoBehaviour
{
    // config
    [SerializeField] float fillSpeed = .1f;
    // max jump force is defined on slider in editor
    [SerializeField] Player player;
    [SerializeField] Slider jumpSlider;
        
    // other
    bool isButtonPressed = false;
    JumpButtonState state = JumpButtonState.Waiting;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isButtonPressed && !JumpSliderFull())
        {
            state = JumpButtonState.PoweringUp;
            jumpSlider.value = jumpSlider.value + (Time.deltaTime * fillSpeed);
        }
        else
        {
            if (state == JumpButtonState.PoweringUp)
            {
                Jump();
            }
        }
    }

    private void Jump()
    {
        player.Jump(jumpSlider.value);
        jumpSlider.value = 0;
        state = JumpButtonState.Waiting;
    }

    private bool JumpSliderFull()
    {
        return jumpSlider.value >= jumpSlider.maxValue - Mathf.Epsilon;
    }

    public void onButtonDown()
    {
        isButtonPressed = true;
    }

    public void onButtonUp()
    {
        isButtonPressed = false;
    }
}

enum JumpButtonState
{
    Waiting,
    PoweringUp
}