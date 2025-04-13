using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using SmartHome.Domain;

namespace SmartHome.Serialization
{
    /// <summary>
    /// Кастомный редактор графа электрических устройств.
    /// Позволяет добавлять, соединять и сохранять устройства в ScriptableObject.
    /// </summary>
    public class GraphEditorWindow : EditorWindow
    {
        private List<GraphNode> _nodes = new();
        private List<GraphEdge> _edges = new();
        private GraphNode _selectedOutput;
        private ElectricNetworkAsset _currentAsset;
        private bool _dirty;
        private const float NODE_WIDTH = 210f;
        private const float NODE_HEIGHT = 170f;
        private const float HEIGHT_FOR_ADDITIONAL_INFO = 20f;
        private Vector2 _canvasOffset = Vector2.zero;
        private Vector2 scrollPosition;
        private Rect scrollAreaSize = new Rect(0, 0, 4000, 4000);

        [MenuItem("SmartHome/Graph Editor")]
        public static void ShowWindow()
        {
            var window = GetWindow<GraphEditorWindow>();
            window.titleContent = new GUIContent("Electric Graph");
            window.minSize = new Vector2(900, 600);
        }

        /// <summary>
        /// Рисует основной интерфейс окна.
        /// </summary>
        private void OnGUI()
        {
            // Нарисовать фон
            GUI.color = new Color(0.2f, 0.2f, 0.2f, 1f);
            GUI.DrawTexture(new Rect(0, 0, position.width, position.height), Texture2D.whiteTexture);
            GUI.color = Color.white;

            float sidebarWidth = 250f;

            // scrollView начинается после сайдбара
            Rect scrollRect = new Rect(sidebarWidth, 0, position.width - sidebarWidth, position.height);
            scrollPosition = GUI.BeginScrollView(scrollRect, scrollPosition, scrollAreaSize);

            DrawEdges(Vector2.zero);

            BeginWindows();
            for (int i = 0; i < _nodes.Count; i++)
            {
                var node = _nodes[i];
                node.rect = GUI.Window(i, node.rect, id => DrawNodeWindow(id), node.id);
            }
            EndWindows();
            HandleEdgeClick(Vector2.zero);

            GUI.EndScrollView();

            // Сайдбар как оверлей
            Rect sidebarRect = new Rect(0, 0, sidebarWidth, position.height);
            GUI.color = new Color(0.12f, 0.12f, 0.12f, 1f);
            GUI.DrawTexture(sidebarRect, Texture2D.whiteTexture);
            GUI.color = Color.white;

            GUILayout.BeginArea(sidebarRect);
            DrawSidebar();
            GUILayout.EndArea();
        }

        /// <summary>
        /// Рисует сайдбар с кнопками для добавления устройств.
        /// </summary>
        private void DrawSidebar()
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Devices", EditorStyles.boldLabel);

            foreach (ElectricDeviceType type in Enum.GetValues(typeof(ElectricDeviceType)))
            {
                if (GUILayout.Button($"Add {type}"))
                {
                    float sidebarWidth = 250f;
                    float centerX = scrollPosition.x + (position.width - sidebarWidth) / 2f - NODE_WIDTH / 2f;
                    float centerY = scrollPosition.y + position.height / 2f - NODE_HEIGHT / 2f;
                    var rect = new Rect(centerX, centerY, NODE_WIDTH, NODE_HEIGHT);

                    var node = new GraphNode(type, rect);

                    if (node.consumptionMode == EnergyConsumptionMode.PerUse)
                        node.rect.height += HEIGHT_FOR_ADDITIONAL_INFO;

                    _nodes.Add(node);
                    _dirty = true;
                }
            }

            GUILayout.Space(10);
            GUI.enabled = _dirty;
            if (GUILayout.Button("\ud83d\udcc2 Save Asset")) SaveAsset();
            GUI.enabled = true;

            if (_currentAsset != null)
            {
                if (GUILayout.Button("\ud83d\udcc2 Overwrite Loaded")) OverwriteLoadedAsset();
                if (GUILayout.Button("\ud83d\udcc2 Open in Project"))
                {
                    Selection.activeObject = _currentAsset;
                    EditorGUIUtility.PingObject(_currentAsset);
                }
            }

            if (GUILayout.Button("\ud83d\udcc1 Load Asset"))
            {
                string path = EditorUtility.OpenFilePanel("Select Asset", "Assets", "asset");
                if (!string.IsNullOrEmpty(path))
                {
                    path = path.Substring(path.IndexOf("Assets"));
                    _currentAsset = AssetDatabase.LoadAssetAtPath<ElectricNetworkAsset>(path);
                    LoadAsset();
                }
            }

            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// Рассчитывает размер канваса.
        /// </summary>
        private Rect CalculateCanvasRect()
        {
            if (_nodes.Count == 0)
            {
                _canvasOffset = Vector2.zero;
                return new Rect(0, 0, 2000, 2000);
            }

            float minX = _nodes.Min(n => n.rect.xMin);
            float minY = _nodes.Min(n => n.rect.yMin);
            float maxX = _nodes.Max(n => n.rect.xMax);
            float maxY = _nodes.Max(n => n.rect.yMax);

            float padding = 300f;

            _canvasOffset = new Vector2(minX - padding, minY - padding);
            float width = (maxX - minX) + 2 * padding;
            float height = (maxY - minY) + 2 * padding;

            return new Rect(0, 0, width, height);
        }

        private void FocusCanvas()
        {
            if (_nodes.Count == 0) return;

            var bounds = new Rect(
                _nodes.Min(n => n.rect.xMin),
                _nodes.Min(n => n.rect.yMin),
                _nodes.Max(n => n.rect.xMax) - _nodes.Min(n => n.rect.xMin),
                _nodes.Max(n => n.rect.yMax) - _nodes.Min(n => n.rect.yMin)
            );

            // Рассчитывает размер канваса
            var canvasRect = CalculateCanvasRect();
            float viewWidth = position.width - 250;
            float viewHeight = position.height;
        }

        /// <summary>
        /// Рендерит и обрабатывает окно одного узла графа.
        /// </summary>
        private void DrawNodeWindow(int id)
        {
            var node = _nodes[id];
            // ID — стандартная ширина
            GUILayout.BeginHorizontal();
            GUILayout.Label("ID", GUILayout.Width(30));
            node.id = EditorGUILayout.TextField(node.id);
            GUILayout.EndHorizontal();

            // Name — растянутая ширина
            GUILayout.BeginHorizontal();
            GUILayout.Label("Name", GUILayout.Width(40));
            GUIStyle nameFieldStyle = new GUIStyle(GUI.skin.textField);
            nameFieldStyle.fontStyle = FontStyle.Bold;
            node.displayName = EditorGUILayout.TextField(node.displayName, nameFieldStyle, GUILayout.MinWidth(100));
            GUILayout.EndHorizontal();

            node.consumptionMode = (EnergyConsumptionMode)EditorGUILayout.EnumPopup("Consumption", node.consumptionMode);
            float desiredHeight = NODE_HEIGHT;

            if (node.consumptionMode == EnergyConsumptionMode.PerUse)
                desiredHeight += HEIGHT_FOR_ADDITIONAL_INFO;
            else if (node.consumptionMode == EnergyConsumptionMode.BatteryPowered)
                desiredHeight += HEIGHT_FOR_ADDITIONAL_INFO * 2;

            if (Mathf.Abs(node.rect.height - desiredHeight) > 0.1f)
            {
                node.rect.height = desiredHeight;
                _dirty = true;
            }
            if (node.consumptionMode == EnergyConsumptionMode.PerTick)
            {
                node.energyRequired = EditorGUILayout.FloatField("Energy Required", node.energyRequired);
            }
            else if (node.consumptionMode == EnergyConsumptionMode.PerUse)
            {
                node.energyRequired = EditorGUILayout.FloatField("Energy Required", node.energyRequired);
                node.useDuration = EditorGUILayout.FloatField("Use Duration", node.useDuration);
            }
            else if (node.consumptionMode == EnergyConsumptionMode.BatteryPowered)
            {
                node.batteryCapacity = EditorGUILayout.FloatField("Battery Capacity", node.batteryCapacity);
                node.drainPerSecond = EditorGUILayout.FloatField("Drain Per Second", node.drainPerSecond);
                node.chargePerSecond = EditorGUILayout.FloatField("Charge Per Second", node.chargePerSecond);
            }

            GUILayout.Label(node.type.ToString());

            if (GUILayout.Button("Connect"))
            {
                if (_selectedOutput == null) _selectedOutput = node;
                else if (_selectedOutput != node && !_edges.Any(e => e.output == _selectedOutput && e.input == node))
                {
                    _edges.Add(new GraphEdge(_selectedOutput, node));
                    _dirty = true;
                    _selectedOutput = null;
                }
                else _selectedOutput = null;
            }

            if (GUILayout.Button("Delete Node"))
            {
                _edges.RemoveAll(e => e.output == node || e.input == node);
                _nodes.RemoveAt(id);
                GUIUtility.ExitGUI();
            }

            GUI.DragWindow();
        }

        /// <summary>
        /// Рендерит и обрабатывает связи графа.
        /// </summary>
        private void DrawEdges(Vector2 offset)
        {
            foreach (var edge in _edges)
            {
                var start = new Vector2(edge.output.rect.xMax, edge.output.rect.center.y);
                var end = new Vector2(edge.input.rect.xMin, edge.input.rect.center.y);
                var startTangent = start + Vector2.right * 50f;
                var endTangent = end + Vector2.left * 50f;

                Handles.DrawBezier(start, end, startTangent, endTangent, Color.yellow, null, 3f);
                DrawArrow(endTangent, end);
            }
        }

        /// <summary>
        /// Рендерит стрелку.
        /// </summary>
        private void DrawArrow(Vector2 from, Vector2 to)
        {
            Vector2 direction = (to - from).normalized;
            Vector2 perp = Vector2.Perpendicular(direction);
            float size = 10f;

            Vector2 p1 = to;
            Vector2 p2 = to - direction * size + perp * size * 0.5f;
            Vector2 p3 = to - direction * size - perp * size * 0.5f;

            Handles.color = Color.yellow;
            Handles.DrawAAConvexPolygon(p1, p2, p3);
        }

        /// <summary>
        /// Обрабатывает клик по связи. Удаляет ее при нажатии правой кнопки мыши.
        /// </summary>
        private void HandleEdgeClick(Vector2 offset)
        {
            Event e = Event.current;
            if (e.type == EventType.MouseDown && e.button == 1)
            {
                Vector2 mousePos = e.mousePosition;
                var toRemove = _edges.FirstOrDefault(edge =>
                {
                    float dist = HandleUtility.DistancePointBezier(mousePos, edge.output.rect.center, edge.input.rect.center,
                        edge.output.rect.center + Vector2.right * 50f,
                        edge.input.rect.center + Vector2.left * 50f);
                    return dist < 10f;
                });

                if (toRemove != null)
                {
                    _edges.Remove(toRemove);
                    _dirty = true;
                    e.Use();
                }
            }
        }

        /// <summary>
        /// Сохраняет текущий граф в ScriptableObject.
        /// </summary>
        private void SaveAsset()
        {
            if (_nodes == null || _edges == null)
            {
                Debug.LogError("Cannot save: nodes or edges are null.");
                return;
            }

            var asset = ScriptableObject.CreateInstance<ElectricNetworkAsset>();

            // Создаём словарь ID → Definition
            var definitions = new Dictionary<string, ElectricDeviceDefinition>();
            foreach (var node in _nodes)
            {
                if (string.IsNullOrEmpty(node.id))
                {
                    Debug.LogError("Node has empty ID. Skipping.");
                    continue;
                }

                var def = new ElectricDeviceDefinition
                {
                    id = node.id,
                    displayName = node.displayName,
                    type = node.type,
                    inputs = new List<string>(),
                    outputs = new List<string>(),
                    posX = node.rect.x,
                    posY = node.rect.y,
                    consumptionMode = node.consumptionMode,
                    energyRequired = node.energyRequired,
                    useDuration = node.useDuration,
                    batteryCapacity = node.batteryCapacity,
                    drainPerSecond = node.drainPerSecond,
                    chargePerSecond = node.chargePerSecond
                };

                definitions[node.id] = def;
                asset.devices.Add(def);
            }

            // Связываем
            foreach (var edge in _edges)
            {
                if (edge.output == null || edge.input == null) continue;

                if (definitions.TryGetValue(edge.output.id, out var from) &&
                    definitions.TryGetValue(edge.input.id, out var to))
                {
                    if (!from.outputs.Contains(to.id)) from.outputs.Add(to.id);
                    if (!to.inputs.Contains(from.id)) to.inputs.Add(from.id);
                }
            }

            string path = EditorUtility.SaveFilePanelInProject(
                "Save Electric Network",
                "NewElectricGraph",
                "asset",
                "Choose location to save the electric graph asset"
            );

            if (string.IsNullOrEmpty(path)) return;

            var existing = AssetDatabase.LoadAssetAtPath<ElectricNetworkAsset>(path);
            if (existing != null)
                AssetDatabase.DeleteAsset(path);

            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            _currentAsset = asset;
            _dirty = false;
        }

        private void OverwriteLoadedAsset()
        {
            if (_currentAsset == null) return;
            _currentAsset.devices.Clear();

            var definitions = new Dictionary<string, ElectricDeviceDefinition>();
            foreach (var node in _nodes)
            {
                var def = new ElectricDeviceDefinition
                {
                    id = node.id,
                    displayName = node.displayName,
                    type = node.type,
                    inputs = new List<string>(),
                    outputs = new List<string>(),
                    posX = node.rect.x,
                    posY = node.rect.y,
                    consumptionMode = node.consumptionMode,
                    energyRequired = node.energyRequired,
                    useDuration = node.useDuration,
                    batteryCapacity = node.batteryCapacity,
                    drainPerSecond = node.drainPerSecond,
                    chargePerSecond = node.chargePerSecond
                };

                definitions[node.id] = def;
                _currentAsset.devices.Add(def);
            }

            foreach (var edge in _edges)
            {
                if (definitions.TryGetValue(edge.output.id, out var from) &&
                    definitions.TryGetValue(edge.input.id, out var to))
                {
                    if (!from.outputs.Contains(to.id)) from.outputs.Add(to.id);
                    if (!to.inputs.Contains(from.id)) to.inputs.Add(from.id);
                }
            }

            EditorUtility.SetDirty(_currentAsset);
            AssetDatabase.SaveAssets();
            _dirty = false;
        }

        /// <summary>
        /// Загружает граф из ScriptableObject. Для каждого EnergyConsumptionMode загружает только необходимые поля.
        /// </summary>
        private void LoadAsset()
        {
            if (_currentAsset == null)
            {
                Debug.LogError("LoadAsset called but _currentAsset is null.");
                return;
            }

            if (_currentAsset.devices == null)
            {
                Debug.LogError("Current asset has null devices list.");
                return;
            }

            _nodes.Clear();
            _edges.Clear();

            var map = new Dictionary<string, GraphNode>();

            foreach (var def in _currentAsset.devices)
            {
                if (string.IsNullOrEmpty(def.id))
                {
                    Debug.LogWarning("Skipping node with empty ID.");
                    continue;
                }
                if (def.consumptionMode == EnergyConsumptionMode.PerUse)
                {
                    var rect = new Rect(def.posX, def.posY, NODE_WIDTH, NODE_HEIGHT + HEIGHT_FOR_ADDITIONAL_INFO);
                    var node = new GraphNode(def.type, rect, def.id, def.displayName, def.consumptionMode, def.energyRequired, def.useDuration);
                    _nodes.Add(node);
                    map[def.id] = node;
                }
                else if (def.consumptionMode == EnergyConsumptionMode.BatteryPowered)
                {
                    var rect = new Rect(def.posX, def.posY, NODE_WIDTH, NODE_HEIGHT + HEIGHT_FOR_ADDITIONAL_INFO * 2);
                    var node = new GraphNode(def.type, rect, def.id, def.displayName, def.consumptionMode,
                    def.energyRequired, def.useDuration, def.batteryCapacity, def.drainPerSecond, def.chargePerSecond);
                    _nodes.Add(node);
                    map[def.id] = node;
                }
                else
                {
                    var rect = new Rect(def.posX, def.posY, NODE_WIDTH, NODE_HEIGHT);
                    var node = new GraphNode(def.type, rect, def.id, def.displayName, def.consumptionMode, def.energyRequired);
                    _nodes.Add(node);
                    map[def.id] = node;
                }
            }

            foreach (var def in _currentAsset.devices)
            {
                if (def.inputs == null) continue;

                foreach (var inputId in def.inputs)
                {
                    if (map.TryGetValue(def.id, out var inputNode) && map.TryGetValue(inputId, out var outputNode))
                    {
                        _edges.Add(new GraphEdge(outputNode, inputNode)); // <-- было наоборот
                    }
                    else
                    {
                        Debug.LogWarning($"Could not resolve edge from {inputId} -> {def.id}");
                    }
                }
            }

            // Центрируем канвас по содержимому
            var bounds = new Rect(
                _nodes.Min(n => n.rect.xMin),
                _nodes.Min(n => n.rect.yMin),
                _nodes.Max(n => n.rect.xMax) - _nodes.Min(n => n.rect.xMin),
                _nodes.Max(n => n.rect.yMax) - _nodes.Min(n => n.rect.yMin)
            );
            FocusCanvas();
            _dirty = false;
        }

        private class GraphNode
        {
            public string id;
            public string displayName; // ← новое поле
            public ElectricDeviceType type;
            public Rect rect;
            public EnergyConsumptionMode consumptionMode;
            public float energyRequired;
            public float useDuration;
            public float batteryCapacity;
            public float drainPerSecond;
            public float chargePerSecond;


            public GraphNode(ElectricDeviceType type, Rect rect, string id = null, string displayName = "", EnergyConsumptionMode consumptionMode = EnergyConsumptionMode.PerTick, float energyRequired = 0, float useDuration = 0, float batteryCapacity = 0, float drainPerSecond = 0, float chargePerSecond = 0)
            {
                this.type = type;
                this.rect = rect;
                this.id = id ?? Guid.NewGuid().ToString();
                this.displayName = displayName;
                this.consumptionMode = consumptionMode;
                this.energyRequired = energyRequired;
                this.useDuration = useDuration;
                this.batteryCapacity = batteryCapacity;
                this.drainPerSecond = drainPerSecond;
                this.chargePerSecond = chargePerSecond;
            }
        }

        private class GraphEdge
        {
            public GraphNode output;
            public GraphNode input;

            public GraphEdge(GraphNode output, GraphNode input)
            {
                this.output = output;
                this.input = input;
            }
        }
    }
}
