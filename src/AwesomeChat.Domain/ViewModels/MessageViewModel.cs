using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeChat.Domain.ViewModels
{
    public class MessageViewModel
    {
        public string Id { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public string From { get; set; }
        [Required]
        public string Room { get; set; }
    }
}
