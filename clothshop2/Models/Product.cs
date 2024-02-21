using System.ComponentModel.DataAnnotations.Schema;

namespace clothshop2.Models
{
    public class Product
    {

        public int Id { get; set; }

        public string Name { get; set; }



        public string description { get; set; }

        public int Price { get; set; }


        public string imgName { get; set; }


        public int Catid { get; set; }

        [ForeignKey("Catid")]

        public Category Category { get; set; }
    }
}
