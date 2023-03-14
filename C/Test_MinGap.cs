using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test
{
public class MyTestClass
{
    public void TestFindMinGap()
    {
        // Arrange
        BinarySearchTree<int> tree = new BinarySearchTree<int>();
        tree.Insert(5);
        tree.Insert(3);
        tree.Insert(9);
        tree.Insert(1);
        tree.Insert(4);
        tree.Insert(7);
        tree.Insert(10);
        int expected = 1;

        // Act
        int actual = tree.FindMinGap();

        // Assert
        Assert.AreEqual(expected, actual);
    }
}

};