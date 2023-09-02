using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private Camera m_camera;

    private Rigidbody m_rb;

    [SerializeField]
    private float m_accelerationValue;
    [SerializeField]
    private float m_maxVelocity;

    // Start is called before the first frame update
    void Start()
    {
        m_camera = Camera.main;
        m_rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var vectorOnFloor = Vector3.ProjectOnPlane(m_camera.transform.forward, Vector3.up);
        vectorOnFloor.Normalize();

        if (Input.GetKey(KeyCode.W))
        {
            m_rb.AddForce(vectorOnFloor * m_accelerationValue, ForceMode.Acceleration);
        }
        if (m_rb.velocity.magnitude > m_maxVelocity)
        {
            m_rb.velocity = m_rb.velocity.normalized;
            m_rb.velocity *= m_maxVelocity;
        }

        //TODO 31 AO�T:
                //Appliquer les d�placements relatifs � la cam�ra dans les 3 autres directions
                //Avoir des vitesses de d�placements maximales diff�rentes vers les c�t�s et vers l'arri�re
                //Lorsqu'aucun input est mis, d�c�l�rer le personnage rapidement

        Debug.Log(m_rb.velocity.magnitude);
    }
}
