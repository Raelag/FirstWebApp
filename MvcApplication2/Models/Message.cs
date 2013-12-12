using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcApplication2.Models
{
    public class Message
    {
        public Message(string Em, string Mess)
        {
            message = Mess;
            Email = Em;
        }
        public string message { get; set; }
        public string Email { get; set; }
    }
}