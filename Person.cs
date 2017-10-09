using System.Collections;

namespace Dips
{
    public class Person
    {
        public string Name { get; set; }

        public Money Cash { get; set; }

        public ArrayList Accounts { get; set; }

        public int NumberOfAccounts => Accounts.Count;

        public Person(string name, float amount = 0)
        {
            Name = name;
            Cash = new Money(amount);
            Accounts = new ArrayList();
        }
        
        public void AddAccount(Account account)
        {
            Accounts.Add(account);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}