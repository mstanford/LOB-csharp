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
using NUnit.Framework;

namespace LOB
{
	[TestFixture]
    public class TPriceLevel
	{

		[Test]
		public void AddRemove01()
		{
            int price = 1;
            PriceLevel priceLevel = new PriceLevel(price);


            Order order1 = new Order(1, true, 1, 1, price);
            priceLevel.Add(order1);
            Assert.AreEqual(1, priceLevel.OrderCount);
            Assert.AreEqual(1, priceLevel.TotalQuantity);
            Assert.AreEqual(order1, priceLevel.HeadOrder);
            Assert.AreEqual(order1, priceLevel.TailOrder);

            Assert.AreEqual(priceLevel, order1.PriceLevel);
            Assert.AreEqual(null, order1.PreviousOrder);
            Assert.AreEqual(null, order1.NextOrder);


            Order order2 = new Order(1, true, 2, 2, price);
            priceLevel.Add(order2);
            Assert.AreEqual(2, priceLevel.OrderCount);
            Assert.AreEqual(3, priceLevel.TotalQuantity);
            Assert.AreEqual(order1, priceLevel.HeadOrder);
            Assert.AreEqual(order2, priceLevel.TailOrder);

            Assert.AreEqual(null, order1.PreviousOrder);
            Assert.AreEqual(order2, order1.NextOrder);

            Assert.AreEqual(priceLevel, order2.PriceLevel);
            Assert.AreEqual(order1, order2.PreviousOrder);
            Assert.AreEqual(null, order2.NextOrder);


            priceLevel.Remove(order1);
            Assert.AreEqual(1, priceLevel.OrderCount);
            Assert.AreEqual(2, priceLevel.TotalQuantity);
            Assert.AreEqual(order2, priceLevel.HeadOrder);
            Assert.AreEqual(order2, priceLevel.TailOrder);

            Assert.AreEqual(null, order2.PreviousOrder);
            Assert.AreEqual(null, order2.NextOrder);


            priceLevel.Remove(order2);
            Assert.AreEqual(0, priceLevel.OrderCount);
            Assert.AreEqual(0, priceLevel.TotalQuantity);
            Assert.AreEqual(null, priceLevel.HeadOrder);
            Assert.AreEqual(null, priceLevel.TailOrder);
        }

        [Test]
        public void AddRemove02()
        {
            int price = 1;
            PriceLevel priceLevel = new PriceLevel(price);


            Order order1 = new Order(1, true, 1, 1, price);
            priceLevel.Add(order1);
            Assert.AreEqual(1, priceLevel.OrderCount);
            Assert.AreEqual(1, priceLevel.TotalQuantity);
            Assert.AreEqual(order1, priceLevel.HeadOrder);
            Assert.AreEqual(order1, priceLevel.TailOrder);

            Assert.AreEqual(priceLevel, order1.PriceLevel);
            Assert.AreEqual(null, order1.PreviousOrder);
            Assert.AreEqual(null, order1.NextOrder);


            Order order2 = new Order(1, true, 2, 2, price);
            priceLevel.Add(order2);
            Assert.AreEqual(2, priceLevel.OrderCount);
            Assert.AreEqual(3, priceLevel.TotalQuantity);
            Assert.AreEqual(order1, priceLevel.HeadOrder);
            Assert.AreEqual(order2, priceLevel.TailOrder);

            Assert.AreEqual(null, order1.PreviousOrder);
            Assert.AreEqual(order2, order1.NextOrder);

            Assert.AreEqual(priceLevel, order2.PriceLevel);
            Assert.AreEqual(order1, order2.PreviousOrder);
            Assert.AreEqual(null, order2.NextOrder);


            priceLevel.Remove(order2);
            Assert.AreEqual(1, priceLevel.OrderCount);
            Assert.AreEqual(1, priceLevel.TotalQuantity);
            Assert.AreEqual(order1, priceLevel.HeadOrder);
            Assert.AreEqual(order1, priceLevel.TailOrder);

            Assert.AreEqual(null, order1.PreviousOrder);
            Assert.AreEqual(null, order1.NextOrder);


            priceLevel.Remove(order1);
            Assert.AreEqual(0, priceLevel.OrderCount);
            Assert.AreEqual(0, priceLevel.TotalQuantity);
            Assert.AreEqual(null, priceLevel.HeadOrder);
            Assert.AreEqual(null, priceLevel.TailOrder);
        }

        [Test]
        public void AddRemove03()
        {
            int price = 1;
            PriceLevel priceLevel = new PriceLevel(price);


            Order order1 = new Order(1, true, 1, 1, price);
            priceLevel.Add(order1);
            Assert.AreEqual(1, priceLevel.OrderCount);
            Assert.AreEqual(1, priceLevel.TotalQuantity);
            Assert.AreEqual(order1, priceLevel.HeadOrder);
            Assert.AreEqual(order1, priceLevel.TailOrder);

            Assert.AreEqual(priceLevel, order1.PriceLevel);
            Assert.AreEqual(null, order1.PreviousOrder);
            Assert.AreEqual(null, order1.NextOrder);


            Order order2 = new Order(1, true, 2, 2, price);
            priceLevel.Add(order2);
            Assert.AreEqual(2, priceLevel.OrderCount);
            Assert.AreEqual(3, priceLevel.TotalQuantity);
            Assert.AreEqual(order1, priceLevel.HeadOrder);
            Assert.AreEqual(order2, priceLevel.TailOrder);

            Assert.AreEqual(null, order1.PreviousOrder);
            Assert.AreEqual(order2, order1.NextOrder);

            Assert.AreEqual(priceLevel, order2.PriceLevel);
            Assert.AreEqual(order1, order2.PreviousOrder);
            Assert.AreEqual(null, order2.NextOrder);


            Order order3 = new Order(1, true, 3, 3, price);
            priceLevel.Add(order3);
            Assert.AreEqual(3, priceLevel.OrderCount);
            Assert.AreEqual(6, priceLevel.TotalQuantity);
            Assert.AreEqual(order1, priceLevel.HeadOrder);
            Assert.AreEqual(order3, priceLevel.TailOrder);

            Assert.AreEqual(null, order1.PreviousOrder);
            Assert.AreEqual(order2, order1.NextOrder);

            Assert.AreEqual(order1, order2.PreviousOrder);
            Assert.AreEqual(order3, order2.NextOrder);

            Assert.AreEqual(priceLevel, order3.PriceLevel);
            Assert.AreEqual(order2, order3.PreviousOrder);
            Assert.AreEqual(null, order3.NextOrder);


            priceLevel.Remove(order2);
            Assert.AreEqual(2, priceLevel.OrderCount);
            Assert.AreEqual(4, priceLevel.TotalQuantity);
            Assert.AreEqual(order1, priceLevel.HeadOrder);
            Assert.AreEqual(order3, priceLevel.TailOrder);

            Assert.AreEqual(null, order1.PreviousOrder);
            Assert.AreEqual(order3, order1.NextOrder);

            Assert.AreEqual(order1, order3.PreviousOrder);
            Assert.AreEqual(null, order3.NextOrder);


            priceLevel.Remove(order1);
            Assert.AreEqual(1, priceLevel.OrderCount);
            Assert.AreEqual(3, priceLevel.TotalQuantity);
            Assert.AreEqual(order3, priceLevel.HeadOrder);
            Assert.AreEqual(order3, priceLevel.TailOrder);

            Assert.AreEqual(null, order3.PreviousOrder);
            Assert.AreEqual(null, order3.NextOrder);


            priceLevel.Remove(order3);
            Assert.AreEqual(0, priceLevel.OrderCount);
            Assert.AreEqual(0, priceLevel.TotalQuantity);
            Assert.AreEqual(null, priceLevel.HeadOrder);
            Assert.AreEqual(null, priceLevel.TailOrder);
        }

        [Test]
        public void AddRemove04()
        {
            int price = 1;
            PriceLevel priceLevel = new PriceLevel(price);


            Order order1 = new Order(1, true, 1, 1, price);
            priceLevel.Add(order1);
            Assert.AreEqual(1, priceLevel.OrderCount);
            Assert.AreEqual(1, priceLevel.TotalQuantity);
            Assert.AreEqual(order1, priceLevel.HeadOrder);
            Assert.AreEqual(order1, priceLevel.TailOrder);

            Assert.AreEqual(priceLevel, order1.PriceLevel);
            Assert.AreEqual(null, order1.PreviousOrder);
            Assert.AreEqual(null, order1.NextOrder);


            Order order2 = new Order(1, true, 2, 2, price);
            priceLevel.Add(order2);
            Assert.AreEqual(2, priceLevel.OrderCount);
            Assert.AreEqual(3, priceLevel.TotalQuantity);
            Assert.AreEqual(order1, priceLevel.HeadOrder);
            Assert.AreEqual(order2, priceLevel.TailOrder);

            Assert.AreEqual(null, order1.PreviousOrder);
            Assert.AreEqual(order2, order1.NextOrder);

            Assert.AreEqual(priceLevel, order2.PriceLevel);
            Assert.AreEqual(order1, order2.PreviousOrder);
            Assert.AreEqual(null, order2.NextOrder);


            Order order3 = new Order(1, true, 3, 3, price);
            priceLevel.Add(order3);
            Assert.AreEqual(3, priceLevel.OrderCount);
            Assert.AreEqual(6, priceLevel.TotalQuantity);
            Assert.AreEqual(order1, priceLevel.HeadOrder);
            Assert.AreEqual(order3, priceLevel.TailOrder);

            Assert.AreEqual(null, order1.PreviousOrder);
            Assert.AreEqual(order2, order1.NextOrder);

            Assert.AreEqual(order1, order2.PreviousOrder);
            Assert.AreEqual(order3, order2.NextOrder);

            Assert.AreEqual(priceLevel, order3.PriceLevel);
            Assert.AreEqual(order2, order3.PreviousOrder);
            Assert.AreEqual(null, order3.NextOrder);


            priceLevel.Remove(order2);
            Assert.AreEqual(2, priceLevel.OrderCount);
            Assert.AreEqual(4, priceLevel.TotalQuantity);
            Assert.AreEqual(order1, priceLevel.HeadOrder);
            Assert.AreEqual(order3, priceLevel.TailOrder);

            Assert.AreEqual(null, order1.PreviousOrder);
            Assert.AreEqual(order3, order1.NextOrder);

            Assert.AreEqual(order1, order3.PreviousOrder);
            Assert.AreEqual(null, order3.NextOrder);


            priceLevel.Remove(order3);
            Assert.AreEqual(1, priceLevel.OrderCount);
            Assert.AreEqual(1, priceLevel.TotalQuantity);
            Assert.AreEqual(order1, priceLevel.HeadOrder);
            Assert.AreEqual(order1, priceLevel.TailOrder);

            Assert.AreEqual(null, order1.PreviousOrder);
            Assert.AreEqual(null, order1.NextOrder);


            priceLevel.Remove(order1);
            Assert.AreEqual(0, priceLevel.OrderCount);
            Assert.AreEqual(0, priceLevel.TotalQuantity);
            Assert.AreEqual(null, priceLevel.HeadOrder);
            Assert.AreEqual(null, priceLevel.TailOrder);
        }

    }
}
