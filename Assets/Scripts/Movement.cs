using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    #region Variables
    public float inputX = 0;
    public float inputZ = 0;
    public Vector3 moveDirection;

    [SerializeField]
    private float speed = 5;

    private Vector3 initLocalPosition;
    #endregion

    #region MonoBehaviour methods
    // Start is called before the first frame update
    void Start()
    {
        initLocalPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition += moveDirection * speed * Time.deltaTime;//new Vector3(inputX, 0, inputZ) * speed * Time.deltaTime;
    }
    #endregion

    #region Custom methods
    public void ResetMovement()
    {
        transform.localPosition = initLocalPosition;
    }
    #endregion
}

