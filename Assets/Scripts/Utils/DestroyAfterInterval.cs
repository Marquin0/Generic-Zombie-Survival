using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Utils
{
    public class DestroyAfterInterval : MonoBehaviour
    {
        public float delay;

        private void OnEnable()
        {
            StartCoroutine("StartDelay");
        }

        private IEnumerator StartDelay()
        {
            yield return new WaitForSeconds(delay);
            Destroy(gameObject);
        }
    }
}
