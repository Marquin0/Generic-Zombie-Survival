using Assets.Scripts.Systems.Entities;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class FeedbackController : MonoBehaviour, IFeedbackContext
    {
        private SkinnedMeshRenderer meshRenderer;
        private IEntityContext entityContext;
        private Material originalMaterial;
        private Color originalColor;
        private float flashDuration = 0.125f;

        public Material FlashMaterial;

        public SkinnedMeshRenderer Renderer => null;

        void Start()
        {
            meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
            entityContext = (IEntityContext)GetComponents(typeof(IEntityContext)).FirstOrDefault();
            originalMaterial = meshRenderer.material;
            originalColor = originalMaterial.color;

            entityContext.EntityStats.Health.StatChanged += Health_StatChanged;
        }

        public void InvokeCoroutine(System.Func<IEnumerator> func)
        {
            StartCoroutine(func());
        }

        private void Health_StatChanged(Systems.Entities.Stats.ModifiableStat<float> sender, Systems.Entities.Stats.StatChangedEventsArgs<float> e)
        {
            if(e.ValueChanged < 0)
            {
                StartCoroutine("DamangeFlash");
            }
        }

        private IEnumerator DamangeFlash()
        {
            meshRenderer.material = FlashMaterial;
            FlashMaterial.color = Color.white;
            yield return new WaitForSeconds(flashDuration);
            meshRenderer.material = originalMaterial;
        }
    }
}
