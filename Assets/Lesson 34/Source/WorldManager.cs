using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldManager : MonoBehaviour
{
    [SerializeField] private string _sceneNameTemplate;
    [SerializeField] private Vector2Int _startPosition;
    [SerializeField] private Vector2Int _gridSize;
    [SerializeField] private Vector2Int _loadSize;
    [SerializeField] private Vector2 _tileSize;
    [SerializeField] private Transform _playerTarnsform;

    private bool[] _loadedScenes;
    private bool[] _loadingScenes;

    private void Start()
    {
        _loadedScenes = new bool[(_gridSize.y * 2 + 1) * (_gridSize.x * 2 + 1)];
        _loadingScenes = new bool[(_gridSize.y * 2 + 1) * (_gridSize.x * 2 + 1)];

        //SceneManager.LoadSceneAsync(GetSceneName(_startPosition.x, _startPosition.y), LoadSceneMode.Additive);
        int startX = _startPosition.x - _loadSize.x;
        int startY = _startPosition.y - _loadSize.y;
        int endX = _startPosition.x + _loadSize.x;
        int endY = _startPosition.y + _loadSize.y;

        for (int i = startY; i <= endY; ++i)
        {
            if (i < -_gridSize.y || i > _gridSize.y)
                continue;
            for (int j = startX; j <= endX; ++j)
            {
                if (j < -_gridSize.x || j > _gridSize.x)
                    continue;

                _loadedScenes[(i + _gridSize.y) * (_gridSize.x * 2 + 1) + (j + _gridSize.x)] = true;
                SceneManager.LoadSceneAsync(GetSceneName(j, i), LoadSceneMode.Additive);
            }
        }
    }

    private void Update()
    {
        Vector3 playerPosition = _playerTarnsform.position;

        _startPosition.x = Mathf.RoundToInt(playerPosition.x / _tileSize.x);
        _startPosition.y = Mathf.RoundToInt(playerPosition.z / _tileSize.y);

        TestReload();
    }

    [ContextMenu("Test Reload")]
    private void TestReload()
    {
        for (int i = 0; i < _loadingScenes.Length; ++i)
            _loadingScenes[i] = false;

        int startX = _startPosition.x - _loadSize.x;
        int startY = _startPosition.y - _loadSize.y;
        int endX = _startPosition.x + _loadSize.x;
        int endY = _startPosition.y + _loadSize.y;

        for (int i = startY; i <= endY; ++i)
        {
            if (i < -_gridSize.y || i > _gridSize.y)
                continue;
            for (int j = startX; j <= endX; ++j)
            {
                if (j < -_gridSize.x || j > _gridSize.x)
                    continue;

                _loadingScenes[(i + _gridSize.y) * (_gridSize.x * 2 + 1) + (j + _gridSize.x)] = true;

                //if (_loadScenes[(i + _gridSize.y) * (_gridSize.x * 2 + 1) + (j + _gridSize.x)])
                //continue;

                //SceneManager.LoadSceneAsync(GetSceneName(j, i), LoadSceneMode.Additive).completed += (AsyncOperation obj) => WorldManager_completed(obj, j, i);
            }
        }

        for (int i = 0; i < _loadedScenes.Length; ++i)
        {
            if (_loadedScenes[i] && !_loadingScenes[i])
            {
                string sceneName = GetSceneName(i % (_gridSize.x * 2 + 1) - _gridSize.x, i / (_gridSize.x * 2 + 1) - _gridSize.y);
                Debug.Log($"Unloading scene: '{sceneName}'");
                SceneManager.UnloadSceneAsync(sceneName);
            }
            if (!_loadedScenes[i] && _loadingScenes[i])
            {
                SceneManager.LoadSceneAsync(GetSceneName(i % (_gridSize.x * 2 + 1) - _gridSize.x, i / (_gridSize.x * 2 + 1) - _gridSize.y), LoadSceneMode.Additive);
            }
            _loadedScenes[i] = _loadingScenes[i];
        }
    }

    [ContextMenu("Test")]
    private void Test()
    {
        Debug.Log(GetSceneName(-2, 2));
    }

    private string GetSceneName(int x, int y)
    {
        return string.Format(_sceneNameTemplate, x.ToString("00"), y.ToString("00"));
    }
}
