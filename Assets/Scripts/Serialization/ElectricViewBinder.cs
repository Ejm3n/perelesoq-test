using System.Collections;
using System.Collections.Generic;
using SmartHome.Domain;
using UnityEngine;


namespace SmartHome.Serialization
{
    public class ElectricViewBinder : MonoBehaviour
    {
        [SerializeField] private ElectricNetworkAsset asset;
        [SerializeField] private List<BindableView> views;
        private Dictionary<string, IElectricNode> _nodes;

        private void Start()
        {
            _nodes = ElectricNetworkBuilder.BuildFromAsset(asset);

            foreach (var view in views)
            {
                if (_nodes.TryGetValue(view.id, out var logic))
                    view.Bind(logic);
                else
                    Debug.LogWarning($"Не найден узел с id: {view.id}");
            }
        }
    }
}