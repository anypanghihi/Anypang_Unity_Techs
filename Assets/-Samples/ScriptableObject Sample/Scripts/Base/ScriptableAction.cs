using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableAction", menuName = "SO/Base/Action")]
public abstract class ScriptableAction : ScriptableObject
{
    public abstract void DoAction(GameObject obj);
}
