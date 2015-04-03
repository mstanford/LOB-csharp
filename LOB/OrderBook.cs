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
	public class OrderBook
	{
        public readonly int Symbol;
        public readonly Dictionary<int, Order> Orders = new Dictionary<int, Order>();
        public readonly Dictionary<int, PriceLevel> PriceLevels = new Dictionary<int, PriceLevel>();
        public readonly PriceLevelTree BuyTree = new PriceLevelTree();
        public readonly PriceLevelTree SellTree = new PriceLevelTree();
        public PriceLevel LowestSell;
		public PriceLevel HighestBuy;
        public int BidQuantity;
        public int OfferQuantity;

        public OrderBook(int symbol) { Symbol = symbol; }

		public void Add(Order order)
		{
            if (order.BuyOrSell)
                BidQuantity += order.RemainingQuantity;
            else
                OfferQuantity += order.RemainingQuantity;

            Orders.Add(order.OrderId, order);

            if (PriceLevels.ContainsKey(order.Price))
            {
                PriceLevels[order.Price].Add(order);
            }
            else
            {
                PriceLevel priceLevel = new PriceLevel(order.Price);
                PriceLevels.Add(order.Price, priceLevel);

                OnAddPriceLevel(priceLevel);

                if (order.BuyOrSell)
                {
                    BuyTree.Add(priceLevel);

                    if (HighestBuy == null || priceLevel.Price > HighestBuy.Price)
                    {
                        HighestBuy = priceLevel;
                        OnTopOfBook(HighestBuy, LowestSell);
                    }
                }
                else
                {
                    SellTree.Add(priceLevel);

                    if (LowestSell == null || priceLevel.Price < LowestSell.Price)
                    {
                        LowestSell = priceLevel;
                        OnTopOfBook(HighestBuy, LowestSell);
                    }
                }

                priceLevel.Add(order);
            }

            OnAdd(order);
        }

        public virtual void OnAdd(Order order) { }

        public virtual void OnAddPriceLevel(PriceLevel priceLevel) { }

        public virtual void OnTopOfBook(PriceLevel bid, PriceLevel ask) { }

        public void Cancel(int orderId)
		{
            Order order = Orders[orderId];

            if (order.BuyOrSell)
                BidQuantity -= order.RemainingQuantity;
            else
                OfferQuantity -= order.RemainingQuantity;

            Orders.Remove(orderId);

            PriceLevel priceLevel = order.PriceLevel;

            priceLevel.Remove(order);

            if (priceLevel.OrderCount == 0)
            {
                OnRemovePriceLevel(priceLevel);

                if (order.BuyOrSell)
                {
                    BuyTree.Remove(priceLevel);

                    if (HighestBuy == priceLevel)
                    {
                        HighestBuy = BuyTree.Max();
                        OnTopOfBook(HighestBuy, LowestSell);
                    }
                }
                else
                {
                    SellTree.Remove(priceLevel);

                    if (LowestSell == priceLevel)
                    {
                        LowestSell = SellTree.Min();
                        OnTopOfBook(HighestBuy, LowestSell);
                    }
                }
            }

            OnCancel(order);
        }

        public virtual void OnCancel(Order order) { }

        public virtual void OnRemovePriceLevel(PriceLevel priceLevel) { }

        public void Execute(bool buyOrSell, int quantity)
        {
            if (buyOrSell)
                BidQuantity -= quantity;
            else
                OfferQuantity -= quantity;

            PriceLevel priceLevel = buyOrSell ? HighestBuy : LowestSell;

            Order order = priceLevel.HeadOrder;

            order.RemainingQuantity -= quantity;
            priceLevel.TotalQuantity -= quantity;

            if (order.RemainingQuantity == 0)
            {
                Orders.Remove(order.OrderId);
                priceLevel.Remove(order);
                OnFill(order, quantity);

                if (priceLevel.OrderCount == 0)
                {
                    OnRemovePriceLevel(priceLevel);

                    if (buyOrSell)
                    {
                        BuyTree.Remove(priceLevel);

                        HighestBuy = BuyTree.Max();
                        OnTopOfBook(HighestBuy, LowestSell);
                    }
                    else
                    {
                        SellTree.Remove(priceLevel);

                        LowestSell = SellTree.Min();
                        OnTopOfBook(HighestBuy, LowestSell);
                    }
                }
            }
            else
            {
                OnPartial(order, quantity);
            }
        }

        public virtual void OnPartial(Order order, int quantity) { }

        public virtual void OnFill(Order order, int quantity) { }

        public int Volume(int price) { return PriceLevels[price].TotalQuantity; }

        public PriceLevel BestBid() { return HighestBuy; }

        public PriceLevel BestOffer() { return LowestSell; }

	}
}
