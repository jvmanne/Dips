namespace Dips
{
    public class Account
    {
        public Money Balance { get; set; }
        
        public Person Owner { get; set; }
        
        public int AccountNumber { get; set; }

        public Account(Person owner)
        {
            Owner = owner;
            Owner.AddAccount(this);
            AccountNumber = Owner.NumberOfAccounts;
            Balance = new Money(0);
        }

        public override string ToString()
        {
            return Owner.Name + " " + AccountNumber;
        }
    }
}