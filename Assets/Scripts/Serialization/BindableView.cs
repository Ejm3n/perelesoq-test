
using SmartHome.Domain;
using UnityEngine;


namespace SmartHome.Serialization
{
    public abstract class BindableView : MonoBehaviour
    {
        public string id;
        public abstract void Bind(IElectricNode node);
    }
}