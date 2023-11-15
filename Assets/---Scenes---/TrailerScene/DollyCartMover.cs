using UnityEngine;
using Cinemachine;

public class DollyCartMover : MonoBehaviour
{
    public float speed = 5f;
    private CinemachineDollyCart dollyCart;

    void Start()
    {
        dollyCart = GetComponent<CinemachineDollyCart>();
    }

    void Update()
    {
        void Update()
        {
            if (dollyCart)
            {
                dollyCart.m_Position = (dollyCart.m_Position + speed * Time.deltaTime) % dollyCart.m_Path.PathLength;
            }
        }

    }
}
