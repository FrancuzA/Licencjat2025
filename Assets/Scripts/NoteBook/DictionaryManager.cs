using System.Collections.Generic;
using UnityEngine;

public class DictionaryManager : MonoBehaviour
{
    public static DictionaryManager Instance { get; private set; }

    private readonly Dictionary<string, string> words = new Dictionary<string, string>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void AddOrUpdate(string original, string translation)
    {
        if (string.IsNullOrEmpty(original))
            return;

        words[original] = translation ?? string.Empty;

        Debug.Log(words[original]);
    }

    public bool TryGetTranslation(string original, out string translation)
    {
        if (original == null)
        {
            translation = null;
            return false;
        }

        return words.TryGetValue(original, out translation);
    }

    public string GetTranslation(string original)
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
    public IReadOnlyDictionary<string, string> GetAll()
    {
        return words;
    }
}
