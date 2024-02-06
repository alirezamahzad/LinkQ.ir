using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User
    {
        public enum Status
        {
            Active=1,
            Banned=0
        }
        public int Id { get; set; }
        public required Email Email { get; set; }
        public required string HashedPassword { get; set; }
        public DateTime RegDate { get; set; }
        public Status UserStatus { get; set; }
    }
}
