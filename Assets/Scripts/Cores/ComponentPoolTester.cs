using DSPool;
using Editor.ButtonMethod;
using System.Collections.Generic;
using UnityEngine;

namespace Cores
{
    public class ComponentPoolTester : MonoBehaviour
    {
        private ComponentPool<MeshRenderer> pool;
        [SerializeField] private List<MeshRenderer> rentedElements;
        [SerializeField] private GameObject prefab;
        [SerializeField] private int prewarmAmount = 5000;
        [SerializeField] private int rentAmount = 1000;
        [SerializeField] private int returnAmount = 500;

        private void Awake()
        {
            this.pool = new() { Prefab = prefab };
        }

        [Button]
        private void Prewarm()
        {
            this.pool.Prewarm(this.prewarmAmount);
        }

        [Button]
        private void Rent()
        {
            for (int i = 0; i < this.rentAmount; i++)
            {
                var rentedElement = this.pool.Rent();
                rentedElement.gameObject.SetActive(true);
                this.rentedElements.Add(rentedElement);
            }
        }

        [Button]
        private void Return()
        {
            int rentedElementCount = this.rentedElements.Count;

            if (rentedElementCount < this.returnAmount)
            {
                Debug.LogWarning("rentedElements.Count < returnAmount");
                return;
            }

            int startIndex = rentedElementCount - this.returnAmount;

            for (int i = startIndex; i < startIndex + this.returnAmount; i++)
            {
                this.pool.Return(this.rentedElements[i]);
            }

            this.rentedElements.RemoveRange(rentedElementCount - this.returnAmount, this.returnAmount);
        }
    }
}
