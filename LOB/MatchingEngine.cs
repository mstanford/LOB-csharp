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
    public class MatchingEngine
    {
        private int _orderId;

        public MatchingEngine(int numberOfSymbols, int initialOrderId)
        {
            OrderBooks = new OrderBook[numberOfSymbols];
            for (int i = 0; i < OrderBooks.Length; i++)
                OrderBooks[i] = new OrderBook(i);

            _orderId = initialOrderId;
        }

        public readonly OrderBook[] OrderBooks;

        public void ImmediateOrCancelOrder(int symbol, bool buyOrSell, int quantity, int limitPrice)
        {
            if (quantity < 1)
                throw new System.Exception();

            OrderBook orderBook = OrderBooks[symbol];
            lock (orderBook)
            {
                int remainingQuantity = quantity;

                if (buyOrSell)
                {
                    while (remainingQuantity > 0 && orderBook.OfferQuantity > 0 && orderBook.LowestSell.Price <= limitPrice)
                    {
                        int matchedQuantity = System.Math.Min(remainingQuantity, orderBook.LowestSell.HeadOrder.RemainingQuantity);
                        orderBook.Execute(!buyOrSell, matchedQuantity);
                        remainingQuantity -= matchedQuantity;

                        if (remainingQuantity == 0)
                        {
                            //TODO send agressor a fill.
                        }
                        else
                        {
                            //TODO send agressor a partial fill.
                        }
                    }
                }
                else
                {
                    while (remainingQuantity > 0 && orderBook.BidQuantity > 0 && orderBook.HighestBuy.Price >= limitPrice)
                    {
                        int matchedQuantity = System.Math.Min(remainingQuantity, orderBook.HighestBuy.HeadOrder.RemainingQuantity);
                        orderBook.Execute(!buyOrSell, matchedQuantity);
                        remainingQuantity -= matchedQuantity;

                        if (remainingQuantity == 0)
                        {
                            //TODO send agressor a fill.
                        }
                        else
                        {
                            //TODO send agressor a partial fill.
                        }
                    }
                }

                if (remainingQuantity > 0)
                {
                    //TODO send agressor a cancel.
                }
            }
        }

        public void LimitOrder(int symbol, bool buyOrSell, int quantity, int limitPrice)
        {
            if (quantity < 1)
                throw new System.Exception();

            OrderBook orderBook = OrderBooks[symbol];
            lock (orderBook)
            {
                int remainingQuantity = quantity;

                if (buyOrSell)
                {
                    while (remainingQuantity > 0 && orderBook.OfferQuantity > 0 && orderBook.LowestSell.Price <= limitPrice)
                    {
                        int matchedQuantity = System.Math.Min(remainingQuantity, orderBook.LowestSell.HeadOrder.RemainingQuantity);
                        orderBook.Execute(!buyOrSell, matchedQuantity);
                        remainingQuantity -= matchedQuantity;

                        if (remainingQuantity == 0)
                        {
                            //TODO send agressor a fill.
                        }
                        else
                        {
                            //TODO send agressor a partial fill.
                        }
                    }
                }
                else
                {
                    while (remainingQuantity > 0 && orderBook.BidQuantity > 0 && orderBook.HighestBuy.Price >= limitPrice)
                    {
                        int matchedQuantity = System.Math.Min(remainingQuantity, orderBook.HighestBuy.HeadOrder.RemainingQuantity);
                        orderBook.Execute(!buyOrSell, matchedQuantity);
                        remainingQuantity -= matchedQuantity;

                        if (remainingQuantity == 0)
                        {
                            //TODO send agressor a fill.
                        }
                        else
                        {
                            //TODO send agressor a partial fill.
                        }
                    }
                }

                if (remainingQuantity > 0)
                {
                    //TODO send agressor a cancel.
                    Order order = new Order(_orderId, buyOrSell, quantity, remainingQuantity, limitPrice);
                    _orderId++;
                    orderBook.Add(order);
                }
            }
        }

        public void MarketOrder(int symbol, bool buyOrSell, int quantity)
        {
            if (quantity < 1)
                throw new System.Exception();

            OrderBook orderBook = OrderBooks[symbol];
            lock (orderBook)
            {
                int remainingQuantity = quantity;

                if (buyOrSell)
                {
                    while (remainingQuantity > 0 && orderBook.OfferQuantity > 0)
                    {
                        int matchedQuantity = System.Math.Min(remainingQuantity, orderBook.LowestSell.HeadOrder.RemainingQuantity);
                        orderBook.Execute(!buyOrSell, matchedQuantity);
                        remainingQuantity -= matchedQuantity;

                        if (remainingQuantity == 0)
                        {
                            //TODO send agressor a fill.
                        }
                        else
                        {
                            //TODO send agressor a partial fill.
                        }
                    }
                }
                else
                {
                    while (remainingQuantity > 0 && orderBook.BidQuantity > 0)
                    {
                        int matchedQuantity = System.Math.Min(remainingQuantity, orderBook.HighestBuy.HeadOrder.RemainingQuantity);
                        orderBook.Execute(!buyOrSell, matchedQuantity);
                        remainingQuantity -= matchedQuantity;

                        if (remainingQuantity == 0)
                        {
                            //TODO send agressor a fill.
                        }
                        else
                        {
                            //TODO send agressor a partial fill.
                        }
                    }
                }

                if (remainingQuantity > 0)
                {
                    //TODO send agressor a cancel.
                }
            }
        }

    }
}
