using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterAnimation : MonoBehaviour
{
    // use with animation frame
    public void DestroyAtFrame()
    {
        Destroy(this.gameObject);
    }
}
