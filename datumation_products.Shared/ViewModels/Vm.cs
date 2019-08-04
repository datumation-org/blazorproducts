using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace datumation_products.Shared.ViewModels {
    public class RegisterVM {
        public string UserName { get; set; }

        [DataType (DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType (DataType.Password)]
        public string Password { get; set; }

        [Compare ("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
    public class ResultVM {
        public Status Status { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }

    public enum Status {
        Success = 1,
        Error = 2
    }
}