// ------------------------------------------------------------------------
// 
// This is free and unencumbered software released into the public domain.
// 
// Anyone is free to copy, modify, publish, use, compile, sell, or
// distribute this software, either in source code form or as a compiled
// binary, for any purpose, commercial or non-commercial, and by any
// means.
// 
// In jurisdictions that recognize copyright laws, the author or authors
// of this software dedicate any and all copyright interest in the
// software to the public domain. We make this dedication for the benefit
// of the public at large and to the detriment of our heirs and
// successors. We intend this dedication to be an overt act of
// relinquishment in perpetuity of all present and future rights to this
// software under copyright law.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
// OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
// 
// For more information, please refer to <http://unlicense.org/>
// 
// ------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;

namespace LOB
{
	public class PriceLevel
	{
        public readonly int Price;
        public int OrderCount;
        public int TotalQuantity;
		public Order HeadOrder;
		public Order TailOrder;
        //public PriceLevel Parent;
        //public PriceLevel LeftChild;
        //public PriceLevel RightChild;

        public PriceLevel(int price) { Price = price; }

        public void Add(Order order)
        {
            if (order.Price != Price)
                throw new System.Exception();

            OrderCount++;
            TotalQuantity += order.RemainingQuantity;
            if (HeadOrder == null)
            {
                HeadOrder = order;
            }
            else
            {
                TailOrder.NextOrder = order;
                order.PreviousOrder = TailOrder;
            }
            TailOrder = order;
            order.PriceLevel = this;
        }

        public void Remove(Order order)
        {
            OrderCount--;
            TotalQuantity -= order.RemainingQuantity;

            if (order == HeadOrder)
            {
                if (order == TailOrder)
                {
                    if (OrderCount != 0 || TotalQuantity != 0)
                        throw new System.Exception();
                    HeadOrder = TailOrder = null;
                }
                else
                {
                    HeadOrder = order.NextOrder;
                    order.NextOrder.PreviousOrder = null;
                }
            }
            else if (order == TailOrder)
            {
                TailOrder = order.PreviousOrder;
                order.PreviousOrder.NextOrder = null;
            }
            else
            {
                order.PreviousOrder.NextOrder = order.NextOrder;
                order.NextOrder.PreviousOrder = order.PreviousOrder;
            }

            order.NextOrder = null;
            order.PreviousOrder = null;
            order.PriceLevel = null;
        }

    }
}
