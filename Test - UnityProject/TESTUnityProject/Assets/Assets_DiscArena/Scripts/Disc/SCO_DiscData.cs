using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DiscData", menuName = "Create DiscData")]
public class SCO_DiscData : ScriptableObject
{
    [Header("KEY")]
    [SerializeField] private string addressablesKey = null;

    [Header("RANK")]
    [SerializeField] private SC_Rank.ERank unlockedRank = SC_Rank.ERank.Bronze;
    [SerializeField] private int unlockedRankLevel = 3;

    [Header("DATA")]
    [SerializeField] private GameObject prefab = null;
    [SerializeField] private Sprite iconTexture = null;
    [SerializeField, Tooltip("Set -1 for unlimited use")] private int usableCount = -1;

    [Header("INFORMATION")]
    [SerializeField] private string discName = "CLASSIC";
    [SerializeField, TextArea] private string discInformation = "Information";

    public SC_Rank.ERank UnlockedRank { get => unlockedRank; }
    public int UnlockedRankLevel { get => unlockedRankLevel; }
    public GameObject Prefab { get => prefab; }
    public Sprite IconTexture { get => iconTexture; }
    public int UsableCount { get => usableCount; }
    public string AddressablesKey { get => addressablesKey; }
    public string DiscName { get => discName; }
    public string DiscInformation { get => discInformation; }
}
