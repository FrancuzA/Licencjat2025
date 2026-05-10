using Mono.Cecil.Cil;
using System.Collections.Generic;
using UnityEngine;

public class DictionaryManager : MonoBehaviour
{
    public static DictionaryManager Instance { get; private set; }
    public List<string> originalWords = new List<string>();
    public List<string> translatedWords = new List<string>();
    

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
        string lWord = original.ToLower();
        string lTranslation = translation.ToLower();
        if (string.IsNullOrEmpty(lWord) || translation == null)
            return;

        words[original] = lTranslation;
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
        string lWord = original.ToLower();
        if (lWord == null)
            return false;

        return words.ContainsKey(lWord);
    }

    public void Clear()
    {
        words.Clear();
    }



    public bool CheckTranslation(string original)
    {
        string lWord = original.ToLower();
        if (!originalWords.Contains(lWord)) return false;
        int index = originalWords.IndexOf(lWord);
        string goodTranslation = translatedWords[index];
        if (goodTranslation == null) return false;
        if (GetTranslation(lWord) == goodTranslation)
        {
            return true;
        }
        else
        {
            return false;

        }
    }
}
