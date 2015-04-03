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
    public class TMatchingEngine
    {
        private static readonly int PRICE_MULTIPLIER = 100;

        [Test]
        public void MarketOrder01()
        {
            MatchingEngine matchingEngine = LoadState();
            Assert.AreEqual(12, matchingEngine.OrderBooks[0].BidQuantity);
            Assert.AreEqual(0, matchingEngine.OrderBooks[0].OfferQuantity);

            matchingEngine.MarketOrder(0, false, 10);
            Assert.AreEqual(2, matchingEngine.OrderBooks[0].BidQuantity);
            Assert.AreEqual(0, matchingEngine.OrderBooks[0].OfferQuantity);
        }

        [Test]
        public void MarketOrder02()
        {
            MatchingEngine matchingEngine = LoadState();
            Assert.AreEqual(12, matchingEngine.OrderBooks[0].BidQuantity);
            Assert.AreEqual(0, matchingEngine.OrderBooks[0].OfferQuantity);

            matchingEngine.MarketOrder(0, false, 13);
            Assert.AreEqual(0, matchingEngine.OrderBooks[0].BidQuantity);
            Assert.AreEqual(0, matchingEngine.OrderBooks[0].OfferQuantity);
        }

        [Test]
        public void ImmediateOrCancelOrder01()
        {
            MatchingEngine matchingEngine = LoadState();
            Assert.AreEqual(12, matchingEngine.OrderBooks[0].BidQuantity);
            Assert.AreEqual(0, matchingEngine.OrderBooks[0].OfferQuantity);

            matchingEngine.ImmediateOrCancelOrder(0, false, 3, 10961);
            Assert.AreEqual(10, matchingEngine.OrderBooks[0].BidQuantity);
            Assert.AreEqual(0, matchingEngine.OrderBooks[0].OfferQuantity);
        }

        [Test]
        public void LimitOrder01()
        {
            MatchingEngine matchingEngine = LoadState();
            Assert.AreEqual(12, matchingEngine.OrderBooks[0].BidQuantity);
            Assert.AreEqual(0, matchingEngine.OrderBooks[0].OfferQuantity);

            matchingEngine.LimitOrder(0, false, 3, 10961);
            Assert.AreEqual(10, matchingEngine.OrderBooks[0].BidQuantity);
            Assert.AreEqual(1, matchingEngine.OrderBooks[0].OfferQuantity);
        }

        private static MatchingEngine LoadState()
        {
            MatchingEngine matchingEngine = new MatchingEngine(1, 1);
            System.IO.StreamReader reader = LoadReader("Orders00.txt");
            while (reader.Peek() != -1)
            {
                string[] row = reader.ReadLine().Split('\t');
                matchingEngine.LimitOrder(0, row[0] == "B", int.Parse(row[1]), (int)(decimal.Parse(row[2]) * PRICE_MULTIPLIER));
            }
            return matchingEngine;
        }

        private static System.IO.StreamReader LoadReader(string name)
        {
            return new System.IO.StreamReader(typeof(TOrderBook).Assembly.GetManifestResourceStream(typeof(TOrderBook).Namespace + ".samples." + name));
        }
    }
}
