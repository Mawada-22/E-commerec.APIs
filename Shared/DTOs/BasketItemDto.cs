using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public record BasketItemDto
    {
        public int Id { get; init; }
        public string ProductName { get; init; }
        public string PictureUrl { get; init; }
        [Range(1,double.MaxValue,ErrorMessage ="Enter valid Price")]
        public decimal Price { get; init; }
        public string Category { get; init; }
        public string Brand { get; init; }
        [Range(1,50,ErrorMessage = "Quantity can not be greater than 50")]
        public int Quantity { get; init; }
    }
}
