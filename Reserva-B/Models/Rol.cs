using Microsoft.AspNetCore.Identity;
using NT1_2022_1C_B_G2.Helpers;
using System.ComponentModel.DataAnnotations;

namespace NT1_2022_1C_B_G2.Models
{
    public class Rol : IdentityRole<int>
    {
        public Rol() : base()
        {
        }

        public Rol(string rolName) : base(rolName)
        {
        }

        [Display(Name=Alias.RolName)]
        public override string Name {
            get { return base.Name; }
            set { base.Name = value; }
        }
    }
}
