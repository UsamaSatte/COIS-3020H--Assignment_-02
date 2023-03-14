using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Based on http://igoro.com/archive/skip-lists-are-fascinating/

namespace SkipLists
{
    // Interface for SkipList

    interface ISkipList<T> where T : IComparable
    {
        void Insert(T item);     // Inserts item into the skip list (duplicates are permitted)
        bool Contains(T item);   // Returns true if item is found; false otherwise
        void Remove(T item);     // Removes one occurrence of item (if possible) from the skip list
    }

    // Class SkipList

    class SkipList<T> : ISkipList<T> where T : IComparable
    {
        private Node   head;          // Header node of height 32
        private int    maxHeight;     // Maximum height among non-header nodes
        private Random rand;          // For generating random heights

        // Class Node (used by SkipList)

        private class Node
        {
            public T Item { get; set; }
            public int Height { get; set; }
            public Node[] Next   { get; set; } 

            // Constructor
            public Node(T item, int height)
            {
                Item = item;
                Height = height;
                Next = new Node[Height];
            }
        }

        // Constructor
        // Creates an empty skip list with a header node of height 32
        // Time complexity: O(1)

        public SkipList()
        {
            head = new Node(default(T), 32);    // Set to NIL by default
            maxHeight = 0;                      // Current maximum height of the skip list
            rand = new Random();
        }

        // Insert
        // Inserts the given item into the skip list
        // Duplicate items are permitted
        // Expected time complexity: O(log n)

        ///////////////////////////////////////////////////////////////////////
        // Add method to get the rank of an item
      public int Rank(T item)
      {
        int rank = 0;
        Node current = head;

        while (current != null)
        {
            if (current.Item.CompareTo(item) < 0)
            {
                rank++;
            }

            current = current.Next[0];
        }

        return rank;
      }
       ///////////////////////////////////////////////////
       //////////////////////////////////////////////////
       public T GetItemByRank(int rank)
       {
        int currentRank = 0;
        Node current = head;

        while (current != null)
        {
            if (currentRank == rank)
            {
                return current.Item;
            }

            currentRank++;
            current = current.Next[0];
        }

        return default(T);
       }
       /////////////////////////////////////////////////
        public void Insert(T item)
        {
            // Randomly determine height of a node

            int height = 0;
            int R = rand.Next();   // R is a random 32-bit positive integer
            while ((height < maxHeight) && ((R & 1) == 1))
            {
                height++;   // Equal to the number consecutive lower order 1-bits
                R >>= 1;    // Right shift one bit
            }
            if (height == maxHeight) maxHeight++;

            // Create and insert node

            Node newNode = new Node(item, height + 1);
            Node cur = head;
            for (int i = maxHeight - 1; i >= 0; i--)  // for each level
            {
                while (cur.Next[i] != null && cur.Next[i].Item.CompareTo(item) < 0)
                    cur = cur.Next[i];

                // Adjust references at level i once the height is within range
                if (i < newNode.Height)
                {
                    newNode.Next[i] = cur.Next[i];
                    cur.Next[i] = newNode;
                }
            }

        }

        // Contains
        // Returns true if the given item is found in the skip list; false otherwise
        // Expected time complexity: O(log n)

        public bool Contains(T item)
        {
            Node cur = head;
            for (int i = maxHeight - 1; i >= 0; i--)  // for each level
            {
                while (cur.Next[i] != null && cur.Next[i].Item.CompareTo(item) < 0)
                    cur = cur.Next[i];

                if (cur.Next[i] != null && cur.Next[i].Item.CompareTo(item) == 0)
                    return true;
            }
            return false;
        }

        // Remove
        // Removes one occurrence (if possible) of the given item from the skip list
        // Expected time complexity: O(log n)

        public void Remove(T item)
        {
            Node cur = head;
            for (int i = maxHeight - 1; i >= 0; i--)  // for each level
            {
                while (cur.Next[i] != null && cur.Next[i].Item.CompareTo(item) < 0)
                    cur = cur.Next[i];

                if (cur.Next[i] != null && cur.Next[i].Item.CompareTo(item) == 0)
                {
                    cur.Next[i].Height--;                 // Decrease height by 1 when a level is removed
                    cur.Next[i] = cur.Next[i].Next[i];    // Remove reference at level i
                }

                // Decrease maximum height by 1 when the number of nodes at height i is 0
                if (head.Next[i] == null)
                    maxHeight--;
            }
        }

        // Print
        // Outputs the item in sorted order
        // Time complexity:  O(n)

        public void Print()
        {
            Node cur = head.Next[0];
            while (cur != null)
            {
                Console.Write(cur.Item + " ");
                cur = cur.Next[0];
            }
            Console.WriteLine();
        }

        // Profile
        // Prints out a "skyline" of the skip list
        // Time complexity: O(n)

        public void Profile()
        {
            Node cur = head;

            while (cur != null)
            {
                Console.WriteLine(new string('*', cur.Height));  // Outputs a string of *s
                cur = cur.Next[0];
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //Testing of Rank and GetItemByRank methods
            //////////////////////////////////////////////////////////////////////////////
            SkipList<int> S = new SkipList<int>();
            S.Insert(5);
            S.Insert(10);
            S.Insert(3);
            S.Insert(7);

            int rank = S.Rank(7);
            Console.WriteLine("Rank of 7 is: " + rank);

            S.Insert(1);
            S.Insert(2);
            S.Insert(3);
            S.Insert(4);

            // Test the GetItemByRank method
            int item = S.GetItemByRank(2);
            Console.WriteLine(item); // Output: 2

            item = S.GetItemByRank(0);
            Console.WriteLine(item); // Output: 0

            item = S.GetItemByRank(4);
            Console.WriteLine(item); // Output: 3
            /////////////////////////////////////////////////////////////////////////////////
            int i;
            for (i = 1; i <= 6; i++) { S.Insert(i); S.Insert(7 - i); }
            S.Print();
            S.Profile();

            for (i = 1; i <= 2; i++) { S.Remove(6); S.Remove(3);  }
            for (i = 1; i <= 6; i++) Console.WriteLine(S.Contains(i));
            S.Print();
            S.Profile();

            Console.ReadKey();
        }
    }
}
