using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SC_LoadingScreen : MonoBehaviour
{
    static public SC_LoadingScreen Instance { get; private set; } = null;

    [SerializeField] private Text informationText = null;
    [SerializeField] private GameObject loadingScreenCanvas = null;

    private List<AsyncOperation> activeOperation = new List<AsyncOperation>();
    private List<AsyncOperationHandle> activeOperationHandle = new List<AsyncOperationHandle>();
    private bool IsLoadingLevel = false;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if(gameObject != Instance.gameObject)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (activeOperation.Count == 0 &&
            activeOperationHandle.Count == 0 &&
            loadingScreenCanvas.activeSelf && 
            IsLoadingLevel == false)
        {
            StopLoadingScreen();
        }
    }

    public void AddASyncOperation(AsyncOperation operation)
    {
        activeOperation.Add(operation);
        operation.completed += (obj) =>
        {
            activeOperation.Remove(operation);
        };
        StartLoadingScreen();
    }

    public void AddASyncOperationHandle(AsyncOperationHandle operation)
    {
        activeOperationHandle.Add(operation);
        operation.Completed += (obj) =>
        {
            activeOperationHandle.Remove(operation);
        };
        StartLoadingScreen();
    }

    private void StartLoadingScreen()
    {
        loadingScreenCanvas.SetActive(true);
    }

    private void StopLoadingScreen()
    {
        loadingScreenCanvas.SetActive(false);
    }

    public void StartLoadingLevel()
    {
        IsLoadingLevel = true;
        StartLoadingScreen();
    }

    public void LoadingLevelOver()
    {
        IsLoadingLevel = false;
    }
}
