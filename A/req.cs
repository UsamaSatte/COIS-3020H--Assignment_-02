using System;
using System.Collections.Generic;

public class TrieNode
{
    public char Key { get; set; }
    public bool IsEndOfWord { get; set; }
    public Dictionary<char, TrieNode> Children { get; set; }
    public int Value { get; set; }

    public TrieNode(char key)
    {
        Key = key;
        IsEndOfWord = false;
        Children = new Dictionary<char, TrieNode>();
        Value = 0;
    }
}

public class Trie
{
    private TrieNode root;

    public Trie()
    {
        root = new TrieNode('^');
    }

    public void Insert(string word, int value)
    {
        TrieNode currentNode = root;

        foreach (char c in word)
        {
            if (!currentNode.Children.ContainsKey(c))
            {
                TrieNode newNode = new TrieNode(c);
                currentNode.Children[c] = newNode;
            }

            currentNode = currentNode.Children[c];
        }

        currentNode.IsEndOfWord = true;
        currentNode.Value = value;
    }

    public bool Search(string word)
    {
        TrieNode currentNode = root;

        foreach (char c in word)
        {
            if (!currentNode.Children.ContainsKey(c))
            {
                return false;
            }

            currentNode = currentNode.Children[c];
        }

        return currentNode.IsEndOfWord;
    }

    public bool StartsWith(string prefix)
    {
        TrieNode currentNode = root;

        foreach (char c in prefix)
        {
            if (!currentNode.Children.ContainsKey(c))
            {
                return false;
            }

            currentNode = currentNode.Children[c];
        }

        return true;
    }

    public List<string> Autocomplete(string prefix)
    {
        List<string> words = new List<string>();

        TrieNode currentNode = root;

        foreach (char c in prefix)
        {
            if (!currentNode.Children.ContainsKey(c))
            {
                return words;
            }

            currentNode = currentNode.Children[c];
        }

        AutocompleteHelper(currentNode, prefix, words);

        return words;
    }

    private void AutocompleteHelper(TrieNode node, string prefix, List<string> words)
    {
        if (node.IsEndOfWord)
        {
            words.Add(prefix);
        }

        foreach (TrieNode childNode in node.Children.Values)
        {
            AutocompleteHelper(childNode, prefix + childNode.Key, words);
        }
    }

    public List<string> Autocorrect(string word)
    {
        List<string> words = new List<string>();

        AutocorrectHelper(word, root, "", words);

        return words;
    }

    private void AutocorrectHelper(string word, TrieNode node, string prefix, List<string> words)
    {
        if (word.Length == 0 && node.IsEndOfWord)
        {
            words.Add(prefix);
        }

        if (node.Children.Count == 0)
        {
            return;
        }

        if (word.Length > 0)
        {
            char nextChar = word[0];
            string restOfWord = word.Substring(1);

            if (node.Children.ContainsKey(nextChar))
            {
                AutocorrectHelper(restOfWord, node.Children[nextChar], prefix + nextChar, words);
            }
        }
        else
        {
            foreach (TrieNode childNode in node.Children.Values)
            {
                AutocorrectHelper("", childNode, prefix + childNode.Key, words);
            }
        }
    }

    public List<string> PartialMatch(string pattern)
    {
        List<string> words = new List<string>();

        PartialMatchHelper(pattern, root, "", words);
  }
}
