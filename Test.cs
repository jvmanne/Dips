using System;
using System.Linq;
using NUnit.Framework;

namespace Dips
{
    [TestFixture]
    public class NunitClass2
    {
        private Bank Bank { get; set; }
        
        private Person Person1 { get; set; }
        private Person Person2 { get; set; }
        private Person Person3 { get; set; }
        
        private Money InitialDeposit1 { get; set; }
        private Money InitialDeposit2 { get; set; }
        private Money InitialDeposit3 { get; set; }
        
        private Account Account1 { get; set; }
        private Account Account2 { get; set; }
        private Account Account3 { get; set; }
        
        [SetUp]
        public void Init()
        {
            Bank = new Bank();
            Person1 = new Person("person1", 250);
            Person2 = new Person("person2", 200);
            Person3 = new Person("person3");
            
            InitialDeposit1 = new Money(200);
            InitialDeposit2 = new Money(50);
            InitialDeposit3 = new Money(100);
            
            Account1 = Bank.CreateAccount(Person1, InitialDeposit1);
            Account2 = Bank.CreateAccount(Person1, InitialDeposit2);
            Account3 = Bank.CreateAccount(Person2, InitialDeposit3);
        }
        
        [Test]
        public void TestCreateAccount()
        {
            Assert.Throws(Is.TypeOf<ArgumentException>().And.
                    Message.EqualTo("Customer does not have enough money"),
                delegate { Bank.CreateAccount(Person1, new Money(100)); });
            
            var account4 = Bank.CreateAccount(Person2, new Money(0));
            
            Assert.AreEqual(0, Person1.Cash.Value);
            Assert.AreEqual(100, Person2.Cash.Value);
            
            Assert.AreEqual(200, Account1.Balance.Value);
            Assert.AreEqual(50, Account2.Balance.Value);
            Assert.AreEqual(100, Account3.Balance.Value);
            
            Assert.AreEqual("person1 1", Account1.ToString());
            Assert.AreEqual("person1 2", Account2.ToString());
            Assert.AreEqual("person2 1", Account3.ToString());
            Assert.AreEqual("person2 2", account4.ToString());
        }

        [Test]
        public void TestGetAccountForCustomer()
        {
            var expectedAccounts1 = new Account[2];
            expectedAccounts1[0] = Account1;
            expectedAccounts1[1] = Account2;
            
            var accounts1 = Bank.GetAccountsForCustomer(Person1);
            var accounts3 = Bank.GetAccountsForCustomer(Person3);
            
            CollectionAssert.AreEquivalent(expectedAccounts1, accounts1);
            CollectionAssert.IsEmpty(accounts3);
        }

        [Test]
        public void TestDeposit()
        {
            Assert.Throws(Is.TypeOf<ArgumentException>().And.
                    Message.EqualTo("There is not enough money left"),
                delegate { Bank.Deposit(Account1, new Money(50)); });
            Assert.AreEqual(0, Person1.Cash.Value);
            Assert.AreEqual(200, Account1.Balance.Value);
            
            Bank.Deposit(Account3, new Money(50));
            Assert.AreEqual(50, Person2.Cash.Value);
            Assert.AreEqual(150, Account3.Balance.Value);
        }
        
        [Test]
        public void TestWithdraw()
        {
            Assert.Throws(Is.TypeOf<ArgumentException>().And.
                    Message.EqualTo("There is not enough money left"),
                delegate { Bank.Withdraw(Account1, new Money(250)); });
            Assert.AreEqual(0, Person1.Cash.Value);
            Assert.AreEqual(200, Account1.Balance.Value);
            
            Bank.Withdraw(Account1, new Money(200));
            Assert.AreEqual(200, Person1.Cash.Value);
            Assert.AreEqual(0, Account1.Balance.Value);
            
            Bank.Withdraw(Account2, new Money(30));
            Assert.AreEqual(230, Person1.Cash.Value);
            Assert.AreEqual(20, Account2.Balance.Value);
        }
        
        [Test]
        public void TestTransfer()
        {
            Assert.Throws(Is.TypeOf<ArgumentException>().And.
                    Message.EqualTo("There is not enough money left"),
                delegate { Bank.Transfer(Account3, Account1, new Money(150)); });
            Assert.AreEqual(0, Person1.Cash.Value);
            Assert.AreEqual(100, Person2.Cash.Value);
            Assert.AreEqual(200, Account1.Balance.Value);
            Assert.AreEqual(100, Account3.Balance.Value);
            
            Bank.Transfer(Account1, Account3, new Money(100));
            Assert.AreEqual(100, Account1.Balance.Value);
            Assert.AreEqual(200, Account3.Balance.Value);
            
            Bank.Transfer(Account1, Account2, new Money(50));
            Assert.AreEqual(50, Account1.Balance.Value);
            Assert.AreEqual(100, Account2.Balance.Value);
            
            Assert.AreEqual(0, Person1.Cash.Value);
            Assert.AreEqual(100, Person2.Cash.Value);
        }
    }
}