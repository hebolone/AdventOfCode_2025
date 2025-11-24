namespace AdventOfCode.Tools; 

internal record Node<T>(T Item, Node<T>? Ancestor = null) { 
    public int Level { 
        get { 
            var retValue = 0; 
            var pointer = this; 
            while (pointer != null) { 
                retValue++; 
                pointer = pointer.Ancestor; 
            } 
            return retValue; 
        } 
    } 

    public override string ToString() => $"{Item} (level: {Level})"; 
  

    public IEnumerable<Node<T>> PathTo() { 
        List<Node<T>> retValue = []; 
        var current = this; 
        while (current != null) { 
            retValue.Add(current); 
            current = current.Ancestor; 
        } 
        retValue.Reverse(); 
        return retValue; 
    } 
} 

internal class Tree<T> : IEnumerable<Node<T>> { 
  
    protected readonly List<Node<T>> _Datas = []; 

    public Node<T> AddNode(T item, Node<T>? ancestor = null) { 
        var node = new Node<T>(item, ancestor); 
        _Datas.Add(node); 
        return node; 
    } 

    public IEnumerable<Node<T>> GetChildren(Node<T> node) => _Datas.Where(n => n.Ancestor == node); 

    public Node<T>? GetRoot() => _Datas.FirstOrDefault(i => i.Ancestor == null); 

    public static int GetLevel(Node<T> node) => node.Level;

    public int GetDepth() => _Datas.Max(n => n.Level);

    public static IEnumerable<Node<T>> GetPathToNode(Node<T> node) { 
        List<Node<T>> retValue = []; 
        var current = node; 
        while (current != null) { 
            retValue.Add(current); 
            current = current.Ancestor; 
        } 
        retValue.Reverse(); 
        return retValue; 
    } 

    public override string ToString() { 
        var sb = new StringBuilder(); 
        var root = GetRoot(); 
        if(root != null) { 
            ComposeTree(root, sb); 
        } else { 
            sb.AppendLine("No root defined"); 
        } 
        return sb.ToString(); 
    } 

    private StringBuilder ComposeTree(Node<T> node, StringBuilder stringBuilder) { 
        stringBuilder.AppendLine($"{new string(' ', (node.Level - 1) * 2)}{node.Item}"); 
        var children = GetChildren(node); 
        foreach (var child in children) { 
            ComposeTree(child, stringBuilder); 
        } 
        return stringBuilder; 
    } 

    public IEnumerator<Node<T>> GetEnumerator() => _Datas.GetEnumerator(); 

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator(); 

    IEnumerator<Node<T>> IEnumerable<Node<T>>.GetEnumerator() => GetEnumerator(); 

} 