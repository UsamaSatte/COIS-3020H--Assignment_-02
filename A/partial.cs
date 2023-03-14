public List<string> PartialMatch(string pattern)
{
    List<string> matches = new List<string>();

    foreach (string key in dictionary.Keys)
    {
        if (key.Length != pattern.Length)
        {
            continue;
        }

        bool isMatch = true;
        for (int i = 0; i < key.Length; i++)
        {
            if (pattern[i] != '*' && pattern[i] != key[i])
            {
                isMatch = false;
                break;
            }
        }

        if (isMatch)
        {
            matches.Add(key);
        }
    }

    return matches;
}