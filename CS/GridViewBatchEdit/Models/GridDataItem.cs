using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GridViewBatchEdit.Models {
    public class GridDataItem {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Range(0,10)]
        public decimal Price { get; set; }
        public GridDataItem() { }
        public GridDataItem(int id, string name, decimal price) {
            this.Id = id;
            this.Name = name;
            this.Price = price;
        }
    }
}
