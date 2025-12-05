using UnityEngine;

[CreateAssetMenu(menuName = "Audio/Sound Effect")]
public class SoundEffect : ScriptableObject
{
    public string id;
    public AudioClip clip;
    public float volume = 1f;
}
