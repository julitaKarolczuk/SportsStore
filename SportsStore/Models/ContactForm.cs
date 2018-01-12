using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SportsStore.Models
{
    public class ContactForm
    {
        [Required(ErrorMessage = "Podaj Imię")]
        [Display(Name = "Imię:")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Podaj Nazwisko")]
        [Display(Name = "Nazwisko:")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Podaj swój email")]
        [Display(Name = "Nadawca:")]
        [EmailAddress]
        public string CustomEmail { get; set; }

        [Required(ErrorMessage = "Podaj nr telefonu")]
        [Display(Name = "Numer telefonu:")]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Wpełnij obszar")]
        [Display(Name = "Pytanie:")]
        [DataType(DataType.MultilineText)]
        public string TextArea { get; set; }

    }
}