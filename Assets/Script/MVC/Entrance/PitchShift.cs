using UnityEngine;
using System.Collections;

public class PitchShift : MonoBehaviour
{
    [Tooltip("Add a sound clip of a single tone in C4 (at 440Hz)")]
    public AudioClip clip;

    [Tooltip("Will play a chord (4 tones) of the key pressed")]
    public bool playChord;
    public enum Chord { Major, Minor };
    [Tooltip("Chose which chord to play")]
    public Chord chord;
    [Tooltip("Plays a chord in ascending sequence")]
    public bool arpeggiate;
    [Tooltip("Time between tones when arpeggiating")]
    public float arpeggioTime = 0.1f;

    public float KeyToPitch(int midiKey)
    {
        int c4Key = midiKey - 72;

        float pitch = Mathf.Pow(2, c4Key / 12f);
        return pitch;
    }

    public void Play(int index)
    {
        int midiKey = IndexToMidi(index);
        PlayNote(midiKey, 1);
    }

    void PlayChord(int midiKey)
    {
        if (arpeggiate)
        {
            StartCoroutine(Arpeggio(midiKey));
            return;
        }

        PlayNote(midiKey, 0.25f);

        if (chord == Chord.Major)
        {
            PlayNote(midiKey + 4, 0.25f);

        }
        else if (chord == Chord.Minor)
        {
            PlayNote(midiKey + 3, 0.25f);
        }

        PlayNote(midiKey + 7, 0.25f);
        PlayNote(midiKey + 12, 0.25f);
    }

    IEnumerator Arpeggio(int midiKey)
    {
        PlayNote(midiKey, 0.8f);
        yield return new WaitForSeconds(arpeggioTime);

        if (chord == Chord.Major)
        {
            PlayNote(midiKey + 4, 0.8f);
            yield return new WaitForSeconds(arpeggioTime);

        }
        else if (chord == Chord.Minor)
        {
            PlayNote(midiKey + 3, 0.8f);
            yield return new WaitForSeconds(arpeggioTime);
        }

        PlayNote(midiKey + 7, 0.8f);
        yield return new WaitForSeconds(arpeggioTime);

        PlayNote(midiKey + 12, 0.8f);
        yield return new WaitForSeconds(arpeggioTime);
    }

    void PlayNote(int midiKey, float volume)
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();

        source.clip = clip;
        source.spatialBlend = 0;
        source.volume = volume;
        source.pitch = KeyToPitch(midiKey);
        source.Play();

        Destroy(source, source.clip.length);
    }

    public int IndexToMidi(int index)
    {
        switch (index)
        {
            case 0:
                return 72;
            case 1:
                return 74;
            case 2:
                return 76;
            case 3:
                return 77;
            case 4:
                return 79;
            case 5:
                return 81;
            case 6:
                return 83;
            case 7:
                return 84;
            default:
                return 72;
        }
    }
}