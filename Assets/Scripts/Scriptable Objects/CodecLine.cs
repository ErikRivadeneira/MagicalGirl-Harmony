using UnityEngine;

[System.Serializable]
public class CodecLine : ScriptableObject
{
    public string speakerName;
    [TextArea] public string text;
    public Sprite portrait;
    public AudioClip voiceClip;
}
