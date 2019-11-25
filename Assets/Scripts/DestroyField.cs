using UnityEngine;

public class DestroyField : MonoBehaviour
{
    public GameLogic gameLogic;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Field") gameLogic.DestroyBlock(other.gameObject);
    }
}
