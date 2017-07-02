using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Genesis.idlib.Models
{
    public class ApplicationRole: IdentityRole<long>
    {
        public string RoleDescription { get; set;}
    }
}