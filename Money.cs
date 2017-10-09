using System;

namespace Dips
{
    public class Money
    {
        public float Value { get; private set; }

        public Money(float amount)
        {
            Value = amount;
        }

        public void AddMoney(float amount)
        {
            Value += amount;
        }

        public void WithdrawMoney(float amount)
        {
            if (Value < amount)
            {
                throw new ArgumentException("There is not enough money left");
            }
            Value -= amount;
        }
    }
}