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
    public class TOrderBook
	{
        private static readonly int PRICE_MULTIPLIER = 100;

		[Test]
        public void AddCancel01()
		{
            Apply("Orders01.txt");
        }

        [Test]
        public void AddExecute01()
        {
            Apply("Orders02.txt");
        }

        private void Apply(string name)
        {
            System.IO.StringWriter output = new System.IO.StringWriter();
            OrderBook orderBook = new TestOrderBook(0, PRICE_MULTIPLIER, output);
            System.IO.StreamReader reader = LoadReader(name);
            while (reader.Peek() != -1)
            {
                string[] row = reader.ReadLine().Split('\t');

                switch (row[0])
                {
                    case "ADD":
                        orderBook.Add(
                            new Order(
                                int.Parse(row[1]), 
                                row[2] == "B",
                                int.Parse(row[3]),
                                int.Parse(row[3]),
                                (int)(decimal.Parse(row[4]) * PRICE_MULTIPLIER)));
                        break;
                    case "CANCEL":
                        orderBook.Cancel(int.Parse(row[1]));
                        break;
                    case "PARTIAL":
                    case "FILL":
                        orderBook.Execute(row[2] == "B", int.Parse(row[3]));
                        break;
                    default:
                        throw new System.Exception();
                }
            }

            output.Flush();
            reader.BaseStream.Position = 0;
            string s = reader.ReadToEnd();
            reader.Close();

            System.Console.WriteLine(output.ToString());
            Assert.AreEqual(s, output.ToString());
        }

        private class TestOrderBook : OrderBook
        {
            private readonly decimal _priceMultiplier;
            private readonly System.IO.TextWriter _writer;

            public TestOrderBook(int symbol, decimal priceMultiplier, System.IO.TextWriter writer)
                : base(symbol)
            {
                _priceMultiplier = priceMultiplier;
                _writer = writer;
            }

            public override void OnAdd(Order order)
            {
                _writer.WriteLine("ADD	" + order.OrderId + "	" + (order.BuyOrSell ? "B" : "S") + "	" + order.Quantity + "	" + ((decimal)order.Price / _priceMultiplier).ToString("N2"));

                if (BuyTree.Max() != HighestBuy || SellTree.Min() != LowestSell)
                    throw new System.Exception();
            }

            public override void OnCancel(Order order)
            {
                _writer.WriteLine("CANCEL	" + order.OrderId);

                if (BuyTree.Max() != HighestBuy || SellTree.Min() != LowestSell)
                    throw new System.Exception();
            }

            public override void OnPartial(Order order, int quantity)
            {
                _writer.WriteLine("PARTIAL	" + order.OrderId + "	" + (order.BuyOrSell ? "B" : "S") + "	" + quantity);

                if (BuyTree.Max() != HighestBuy || SellTree.Min() != LowestSell)
                    throw new System.Exception();
            }

            public override void OnFill(Order order, int quantity)
            {
                _writer.WriteLine("FILL	" + order.OrderId + "	" + (order.BuyOrSell ? "B" : "S") + "	" + quantity);

                if (BuyTree.Max() != HighestBuy || SellTree.Min() != LowestSell)
                    throw new System.Exception();
            }
        }

        private static System.IO.StreamReader LoadReader(string name)
        {
            return new System.IO.StreamReader(typeof(TOrderBook).Assembly.GetManifestResourceStream(typeof(TOrderBook).Namespace + ".samples." + name));
        }
    }
}
