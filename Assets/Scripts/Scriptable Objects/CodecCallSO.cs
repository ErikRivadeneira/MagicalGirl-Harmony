using UnityEngine;

[CreateAssetMenu(fileName = "CodecCallSO", menuName = "Scriptable Objects/CodecCallSO")]
public class CodecCallSO : ScriptableObject
{
    [SerializeField] private string callID;
    [SerializeField] private CodecLine[] lines;

    public string GetCallID()
    {
        return callID;
    }

    public CodecLine[] GetCodecLines()
    {
        return lines;
    }
    public CodecLine GetCodecLineByIndex(int index)
    {
        return lines[index];
    }
}
