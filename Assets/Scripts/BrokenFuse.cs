using UnityEngine;

public class BrokenFuse : MonoBehaviour
{
    [SerializeField] private FuseSlot fuseSlot;


    public void removeFuse()
    {
        if (fuseSlot != null)
        {
            fuseSlot.RemoveFuse();
        }
    }
}
