using System;
using System.Data.SqlTypes;
using System.Security.Principal;
using NUnit.Framework;

namespace Dips
{
    public class Bank
    {
        public Account CreateAccount(Person customer, Money initialDeposit)
        {
            if (customer.Cash.Value < initialDeposit.Value)
            {
                throw new ArgumentException("Customer does not have enough money");
            }
            var account = new Account(customer);
            Deposit(account, initialDeposit);
            return account;
        }

        public Account[] GetAccountsForCustomer(Person customer)
        {
            return (Account[]) customer.Accounts.ToArray(typeof(Account));
        }

        public void Deposit(Account to, Money amount)
        {
            to.Owner.Cash.WithdrawMoney(amount.Value);
            to.Balance.AddMoney(amount.Value);
        }

        public void Withdraw(Account from, Money amount)
        {
            from.Balance.WithdrawMoney(amount.Value);
            from.Owner.Cash.AddMoney(amount.Value);
        }

        public void Transfer(Account from, Account to, Money amount)
        {
            from.Balance.WithdrawMoney(amount.Value);
            to.Balance.AddMoney(amount.Value);
        }
    }
}