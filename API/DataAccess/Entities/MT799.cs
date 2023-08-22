namespace DataAccess.Entities
{
    public class MT799
    {
        public int Id { get; set; }

        //for testing database, will be changed for the code below at a later stage
        public string Message { get; set; }

        public MT799() { }
        public MT799(int id, string message)
        {
            Id = id;
            Message = message;
        }


        ////MT799 Block 1
        //public string SenderBIC { get; set; }
        //public string SenderLTAddress { get; set; }

        ////MT799 Block 2
        //public string MessageType { get; set; } //is this needed, all messages will be mt799? 
        //public string ReceiverBIC { get; set; }
        //public DateOnly Date { get; set; }

        ////MT 799 Block 4
        //public string TransactionId { get; set; }
        //public string AnotherId { get; set; } //??
        //public string TextBlock { get; set; }

        ////MT799 Block 5
        //public string MAC { get; set; }
        //public string CHK { get; set; }
    }
}
