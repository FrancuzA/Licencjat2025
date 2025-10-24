using System.Collections.Generic;
using UnityEngine;

public class DictionaryManager : MonoBehaviour
{
    public static DictionaryManager Instance { get; private set; }

    private readonly Dictionary<string, Translation> words = new Dictionary<string, Translation>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }


    public void AddOrUpdate(string original, Translation translation)
    {
        if (string.IsNullOrEmpty(original) || translation == null)
            return;

        words[original] = translation;

        Debug.Log($"Saving the word /{original}/ as /{words[original].translatedText ?? string.Empty}/");
    }

    public bool TryGetTranslation(string original, out Translation translation)
    {
        if (original == null)
        {
            translation = null;
            return false;
        }

        return words.TryGetValue(original, out translation);
    }

    public Translation GetTranslation(string original)
    {
        return TryGetTranslation(original, out var translation) ? translation : null;
    }

    public bool Remove(string original)
    {
        if (original == null)
            return false;

        return words.Remove(original);
    }

    public bool Contains(string original)
    {
        if (original == null)
            return false;

        return words.ContainsKey(original);
    }

    public void Clear()
    {
        words.Clear();
    }

    // Expose a read-only view of all entries
    public IReadOnlyDictionary<string, Translation> GetAll()
    {
        return words;
    }
}
