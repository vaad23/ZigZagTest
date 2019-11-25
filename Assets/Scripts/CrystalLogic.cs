using UnityEngine;

public class CrystalLogic : MonoBehaviour
{
    public GameLogic gameLogic;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Sphere")
        {
            gameLogic.IntersectionOfSphereAndCrystal();
            Destroy(gameObject);
        }
    }
    void Update()
    {
        transform.Rotate(new Vector3(0, 1, 0));
    }
}
