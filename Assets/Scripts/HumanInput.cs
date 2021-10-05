using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanInput : MonoBehaviour
{
    #region Variables
    private Movement movement;
    #endregion

    #region MonoBehaviour methods
    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (movement != null)
        {
            movement.inputX = Input.GetAxis("Horizontal");
            movement.inputZ = Input.GetAxis("Vertical");
        }
    }
    #endregion
}
