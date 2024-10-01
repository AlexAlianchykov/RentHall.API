﻿using System.ComponentModel.DataAnnotations;

namespace RentHall.API.Contracts
{
    public class HallDtoRequest
    {
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        [Required]
        public int Capacity { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public string Currency { get; set; }
        public List<string>? additionalServices { get; set; }
    }
}
