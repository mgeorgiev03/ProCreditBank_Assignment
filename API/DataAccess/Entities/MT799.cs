namespace DataAccess.Entities
{
    public class MT799
    {
        public int Id { get; set; }

        public string Block1 { get; set; }
        public string Block2 { get; set; }
        public string Block3 { get; set; }


        public MT799() { }
        public MT799(int id, string block1, string block2, string block3)
        {
            Id = id;
            Block1 = block1;
            Block2 = block2;
            Block3 = block3;
        }

    }
}
