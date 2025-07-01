using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eventLib.Models
{
    public class Image
    {
        [HiddenInput]
        public int? IDImage {  get; set; }
        public string? ImageName { get; set; }
        public byte[]? ImageData { get; set; }

    }
}
