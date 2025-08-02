﻿using BuildingBlocks.Exceptions;

namespace Ordering.Application.Exceptions
{
    internal class OrderNotFoundException : NotFoundException
    {
        public OrderNotFoundException(Guid orderId) : base("Order", orderId)
        {
        }
    }
}
