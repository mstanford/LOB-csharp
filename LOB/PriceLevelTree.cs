// Based on http://algs4.cs.princeton.edu/
using System;
using System.Collections.Generic;
using System.Text;

namespace LOB
{
    public class PriceLevelTree
    {
        public RedBlackNode Root;

        public PriceLevelTree() { }

        public IEnumerable<PriceLevel> Ascending()
        {
            foreach (PriceLevel priceLevel in Ascending(Root))
                yield return priceLevel;
        }

        private IEnumerable<PriceLevel> Ascending(RedBlackNode x)
        {
            if (x != null)
            {
                foreach (PriceLevel priceLevel in Ascending(x.Left))
                    yield return priceLevel;
                yield return x.PriceLevel;
                foreach (PriceLevel priceLevel in Ascending(x.Right))
                    yield return priceLevel;
            }
        }

        public PriceLevel Min()
        {
            if (Root == null) return null;
            return Min(Root);
        }

        private PriceLevel Min(RedBlackNode x)
        {
            if (x.Left == null)
                return x.PriceLevel;
            return Min(x.Left);
        }

        public IEnumerable<PriceLevel> Descending()
        {
            foreach (PriceLevel priceLevel in Descending(Root))
                yield return priceLevel;
        }

        private IEnumerable<PriceLevel> Descending(RedBlackNode x)
        {
            if (x != null)
            {
                foreach (PriceLevel priceLevel in Ascending(x.Right))
                    yield return priceLevel;
                yield return x.PriceLevel;
                foreach (PriceLevel priceLevel in Ascending(x.Left))
                    yield return priceLevel;
            }
        }

        public PriceLevel Max()
        {
            if (Root == null) return null;
            return Max(Root);
        }

        private PriceLevel Max(RedBlackNode x)
        {
            if (x.Right == null)
                return x.PriceLevel;
            return Max(x.Right);
        }

        public void Add(PriceLevel priceLevel)
        {
            Root = Add(Root, priceLevel);
            Root.Color = BLACK;
        }

        private RedBlackNode Add(RedBlackNode h, PriceLevel priceLevel)
        {
            if (h == null)
                return new RedBlackNode(priceLevel);
            int cmp = priceLevel.Price.CompareTo(h.PriceLevel.Price);
            if (cmp < 0) h.Left = Add(h.Left, priceLevel);
            else if (cmp > 0) h.Right = Add(h.Right, priceLevel);
            else throw new System.Exception(); //h.Value = value;
            if (IsRed(h.Right) && !IsRed(h.Left)) h = RotateLeft(h);
            if (IsRed(h.Left) && IsRed(h.Left.Left)) h = RotateRight(h);
            if (IsRed(h.Left) && IsRed(h.Right)) FlipColors(h);
            return h;
        }

        public void Remove(PriceLevel priceLevel)
        {
            if (!IsRed(Root.Left) && !IsRed(Root.Right))
                Root.Color = RED;
            Root = Remove(Root, priceLevel);
            if (Root != null) Root.Color = BLACK;
        }

        private static RedBlackNode Remove(RedBlackNode h, PriceLevel priceLevel)
        {
            if (priceLevel.Price.CompareTo(h.PriceLevel.Price) < 0)
            {
                if (!IsRed(h.Left) && !IsRed(h.Left.Left))
                    h = MoveRedLeft(h);
                h.Left = Remove(h.Left, priceLevel);
            }
            else
            {
                if (IsRed(h.Left))
                    h = RotateRight(h);
                if (priceLevel.Price.CompareTo(h.PriceLevel.Price) == 0 && (h.Right == null))
                    return null;
                if (!IsRed(h.Right) && !IsRed(h.Right.Left))
                    h = MoveRedRight(h);
                if (priceLevel.Price.CompareTo(h.PriceLevel.Price) == 0)
                {
                    throw new System.Exception();

                    //RedBlackNode x = min(h.Right);
                    //h.Key = x.key;
                    //h.Val = x.val;
                    //// h.val = get(h.right, min(h.right).key);
                    //// h.key = min(h.right).key;
                    //h.Right = deleteMin(h.Right);
                }
                else h.Right = Remove(h.Right, priceLevel);
            }
            return Balance(h);
        }

        private static RedBlackNode MoveRedLeft(RedBlackNode h)
        {
            FlipColors(h);
            if (IsRed(h.Right.Left))
            {
                h.Right = RotateRight(h.Right);
                h = RotateLeft(h);
            }
            return h;
        }

        private static RedBlackNode MoveRedRight(RedBlackNode h)
        {
            FlipColors(h);
            if (IsRed(h.Left.Left))
                h = RotateRight(h);
            return h;
        }

        private static RedBlackNode Balance(RedBlackNode h)
        {
            if (IsRed(h.Right)) h = RotateLeft(h);
            if (IsRed(h.Left) && IsRed(h.Left.Left)) h = RotateRight(h);
            if (IsRed(h.Left) && IsRed(h.Right)) FlipColors(h);
            return h;
        }

        private static void FlipColors(RedBlackNode h)
        {
            h.Color = !h.Color;
            h.Left.Color = !h.Left.Color;
            h.Right.Color = !h.Right.Color;
        }

        private static bool IsRed(RedBlackNode node)
        {
            if (node == null)
                return false;
            return (node.Color == RED);
        }

        private static RedBlackNode RotateLeft(RedBlackNode h)
        {
            RedBlackNode x = h.Right;
            h.Right = x.Left;
            x.Left = h;
            x.Color = x.Left.Color;
            x.Left.Color = RED;
            return x;
        }

        private static RedBlackNode RotateRight(RedBlackNode h)
        {
            RedBlackNode x = h.Left;
            h.Left = x.Right;
            x.Right = h;
            x.Color = x.Right.Color;
            x.Right.Color = RED;
            return x;
        }

        private static readonly bool RED = true;
        private static readonly bool BLACK = false;

        public class RedBlackNode
        {
            public readonly PriceLevel PriceLevel;
            public bool Color = RED;
            public RedBlackNode Left;
            public RedBlackNode Right;

            public RedBlackNode(PriceLevel priceLevel)
            {
                PriceLevel = priceLevel;
            }
        }

    }
}
