using System;
using System.Collections.Generic;

namespace CCKit
{
    public class AdjacencyListNode<T>
    {
        public Dictionary<AdjacencyListNode<T>, int> mAdjacencyMap;
        public AdjacencyListNode<T> mPredecessor;
        public T mVal;
        public bool mVisited;
        public bool mOccupied;
        public int mDistance;

        public AdjacencyListNode(T _val)
        {
            mAdjacencyMap = new Dictionary<AdjacencyListNode<T>, int>();
            mPredecessor = null;
            mVal = _val;
            mOccupied = false;
        }

    }

    public class AdjacencyList<T>
    {
        LinkedList<AdjacencyListNode<T>> mNodes;

        public AdjacencyList()
        {
            mNodes = new LinkedList<AdjacencyListNode<T>>();
        }

        public AdjacencyListNode<T> AddVertex(T _val)
        {
            var node = new AdjacencyListNode<T>(_val);
            mNodes.AddLast(node);
            return node;
        }

        public void SetEdge(AdjacencyListNode<T> _head, AdjacencyListNode<T> _tail, int _weight)
        {
            _head.mAdjacencyMap[_tail] = _weight;
        }

        public bool IsEdge(AdjacencyListNode<T> _head, AdjacencyListNode<T> _tail)
        {
            return _head.mAdjacencyMap[_tail] != -1;
        }

        public void BFSearch(AdjacencyListNode<T> _src, int _range, Action<AdjacencyListNode<T>> _func)
        {
            BFSearch(_src, _range, _func, _func);
        }

        public void BFSearch(AdjacencyListNode<T> _src, int _range, Action<AdjacencyListNode<T>> _func0, Action<AdjacencyListNode<T>> _func1)
        {
            var queue = new Queue<AdjacencyListNode<T>>();
            queue.Enqueue(_src);
            foreach (var elem in mNodes) {
                elem.mVisited = false;
                elem.mDistance = 0;
            }
            _src.mVisited = true;
            _func0(_src);

            do {
                var currentNode = queue.Dequeue();
                if (currentNode.mDistance >= _range) return;
                
                foreach (var elem in currentNode.mAdjacencyMap) {
                    var adjacentNode = elem.Key;
                    if (!adjacentNode.mVisited && IsEdge(currentNode, adjacentNode) && !adjacentNode.mOccupied) {
                        adjacentNode.mPredecessor = currentNode;
                        adjacentNode.mVisited = true;
                        adjacentNode.mDistance = currentNode.mDistance + 1;
                        queue.Enqueue(adjacentNode);

                        _func1(adjacentNode);
                    }
                }
            } while (queue.Count > 0);
        }
    }
}
