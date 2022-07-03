namespace Tree;

class BinarySearchTree<T> where T : IComparable
{
    T value;
    BinarySearchTree<T> left;
    BinarySearchTree<T> right;
    public BinarySearchTree(T value, BinarySearchTree<T> left, BinarySearchTree<T> right)
    {
        this.value = value;
        this.left = left;
        this.right = right;
    }

    public static BinarySearchTree<T> ArrayToTree(T[] items)
    {
        return ArrayToTree(items, 0, items.Length);
    }
    private static BinarySearchTree<T> ArrayToTree(T[] items, int inf, int sup)
    {
        if (inf >= sup) return null;
        int mid = (inf + sup) / 2;

        BinarySearchTree<T> left = ArrayToTree(items, inf, mid);
        BinarySearchTree<T> right = ArrayToTree(items, mid + 1, sup);

        BinarySearchTree<T> tree = new BinarySearchTree<T>(items[mid], left, right);
        return tree;
    }
    public bool Contains(T valueToFind)
    {
        if (value.Equals(valueToFind))
            return true;
        if (valueToFind.CompareTo(value) > 0)
        {
            if (right != null)
                return right.Contains(valueToFind);
        }
        else
        {
            if (left != null)
                return left.Contains(valueToFind);
        }
        return false;
    }
    public override string ToString()
    {
        return value.ToString();
    }
}

class Tree<T>
{
    List<Tree<T>> children { get; }
    T value;
    public int Height_ { get; }
    public Tree(T value, List<Tree<T>> nodes)
    {
        children = nodes;
        this.value = value;
        Height_ = Height();
    }
    public IEnumerable<Tree<T>> BFS()
    {
        Queue<Tree<T>> queue = new();
        queue.Enqueue(this);
        while (queue.Count > 0)
        {
            yield return queue.First();
            for (int i = 0; i < queue.First().children.Count; i++)
                queue.Enqueue(queue.First().children[i]);
            queue.Dequeue();
        }
    }
    public IEnumerable<Tree<T>> DFS()
    {
        yield return this;
        for (int i = 0; i < children.Count; i++)
            foreach (var item in children[i].DFS())
                yield return item;
    }
    public int Count()
    {
        return BFS().Count();
    }

    public bool IsLeaf => children.Count == 0;
    public bool EstaColgado(Tree<T> tree)
    {
        if (value.Equals(tree.value))
        {
            return true;
        }
        for (int i = 0; i < children.Count; i++)
        {
            if (children[i].EstaColgado(tree))
            {
                children[i].children.Add(this);
                children.Remove(children[i]);
                return true;
            }
        }
        return false;
    }

    public (int, Tree<T>) HeightAndDeeperNode()
    {
        int height = 0;
        Tree<T> auxTree = this;
        for (int i = 0; i < children.Count; i++)
        {
            (int, Tree<T>) aux = children[i].HeightAndDeeperNode();
            if (height < aux.Item1)
            {
                height = aux.Item1;
                auxTree = aux.Item2;
            }
        }
        return (height + 1, auxTree);
    }

    public int Diametrer()
    {
        Tree<T> deeperNode = HeightAndDeeperNode().Item2;
        EstaColgado(deeperNode);
        return deeperNode.HeightAndDeeperNode().Item1;
    }


    public int Height()
    {
        int height = 0;
        for (int i = 0; i < children.Count; i++)
            height = Math.Max(height, children[i].Height());
        return height + 1;
    }
    private void NearestCommonAncestor(Tree<T> tree1, Tree<T> tree2, ref bool finded, Tree<T>[] tree)
    {
        for (int i = 0; i < children.Count; i++)
        {
            children[i].NearestCommonAncestor(tree1, tree2, ref finded, tree);
            if (finded) return;
        }
        if (!finded && !(Equals(tree1) || Equals(tree2)) && Contains(tree1) && Contains(tree2))
        {
            tree[0] = this;
            finded = true;
        }
    }
    public Tree<T> NearestCommonAncestor(Tree<T> tree1, Tree<T> tree2)
    {
        bool finded = false;
        Tree<T>[] nearestCommonAncester = new Tree<T>[] { this };
        NearestCommonAncestor(tree1, tree2, ref finded, nearestCommonAncester);
        return nearestCommonAncester[0];
    }
    public bool Contains(Tree<T> tree)
    {
        if (tree.value.Equals(value))
            return true;
        for (int i = 0; i < children.Count; i++)
            if (children[i].Contains(tree))
                return true;
        return false;
    }

    public override string? ToString()
    {
        return value?.ToString();
    }
}

class Program
{
    static void Main(string[] args)
    {
        Tree<int> tree4 = new Tree<int>(4, new List<Tree<int>>());
        Tree<int> tree3 = new Tree<int>(3, new List<Tree<int>>());
        List<Tree<int>> aux = new List<Tree<int>>();
        aux.Add(tree4);
        aux.Add(tree3);
        Tree<int> tree2 = new Tree<int>(2, aux);
        Tree<int> tree1 = new Tree<int>(1, new List<Tree<int>>());
        List<Tree<int>> aux1 = new List<Tree<int>>();
        aux1.Add(tree2);
        aux1.Add(tree1);
        Tree<int> tree = new Tree<int>(0, aux1);

        //Console.WriteLine(tree1.Height_);
        // Console.WriteLine(tree.HeightAndDeeperNode());
        //Console.WriteLine(tree.Diametrer());
        //Console.WriteLine(tree.NearestCommonAncestor(tree1,tree4));
        BinarySearchTree<int> searchTree = BinarySearchTree<int>.ArrayToTree(new int[] { 1, 2, 3 ,4,5,6,7,8});
        Console.WriteLine(searchTree.Contains(6));
    }
}