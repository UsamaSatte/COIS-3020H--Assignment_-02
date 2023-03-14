public List<string> Autocorrect(string key)
{
    List<string> corrections = new List<string>();

    // Check for deletions
    for (int i = 0; i < key.Length; i++)
    {
        string candidate = key.Remove(i, 1);
        if (dictionary.ContainsKey(candidate))
        {
            corrections.Add(candidate);
        }
    }

    // Check for insertions
    for (int i = 0; i <= key.Length; i++)
    {
        foreach (char c in alphabet)
        {
            string candidate = key.Insert(i, c.ToString());
            if (dictionary.ContainsKey(candidate))
            {
                corrections.Add(candidate);
            }
        }
    }

    // Check for substitutions
    for (int i = 0; i < key.Length; i++)
    {
        foreach (char c in alphabet)
        {
            if (c == key[i])
            {
                continue;
            }

            string candidate = key.Remove(i, 1).Insert(i, c.ToString());
            if (dictionary.ContainsKey(candidate))
            {
                corrections.Add(candidate);
            }
        }
    }

    // Check for transpositions
    for (int i = 0; i < key.Length - 1; i++)
    {
        string candidate = key.Substring(0, i) + key[i + 1] + key[i] + key.Substring(i + 2);
        if (dictionary.ContainsKey(candidate))
        {
            corrections.Add(candidate);
        }
    }

    return corrections;
}
