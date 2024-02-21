using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace clothshop2.Models
{
    public class log
    {
        public int Id { get; set; }

        public string username { get; set; }
       
        public string date { get; set; }
        public int op_id { get; set; }

        [ForeignKey("op_id")]

        public operations Operations { get; set; }
    }
}
