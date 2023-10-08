using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSpecificCharacterController : MonoBehaviour
{
    [SerializeField] InputManager Input;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SunRotate();
    }

    #region LightController

    [Header("Sun Controller")]
    [SerializeField] Transform SunTransform;

    [SerializeField] float RotationSpeed = 1f;
    float CurrentMana;
    float MaxMana;
    float ManaCostPerSecond;

    void SunRotate()
    {
        float HorizontalRotation = Input.RotateInput;

        Vector3 SunsRotation = SunTransform.eulerAngles;
        SunsRotation.y += HorizontalRotation * Time.deltaTime * RotationSpeed;
        SunTransform.eulerAngles = SunsRotation;
    }

    #endregion
    
}
