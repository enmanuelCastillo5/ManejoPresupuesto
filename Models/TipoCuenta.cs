using System.ComponentModel.DataAnnotations;

namespace ManejoPresupuesto.Models
{
    public class TipoCuenta
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="El campo {0} es Requerido")]
        [StringLength(maximumLength:50, MinimumLength =3, ErrorMessage = "La longitud del campo {0} debe estar {2} y {1}")]
        [Display(Name = "Nombre del tipo cuenta")]
        public string Nombre { get; set; }
        public int UsuarioId { get; set; }
        public int Orden { get; set; }

        /*pruebas de otras validaciones*/
        [Required(ErrorMessage = "El campo {0} es Requerido")]
        [EmailAddress(ErrorMessage = "El campo debe ser un correo electronico valido")]
        public string Email { get; set;}

        [Range(minimum:18, maximum:130, ErrorMessage = "el valor debe estar entre {1} y {2}")]
        public int Edad { get; set; }

        [Url(ErrorMessage = "el campo debe ser una Url valida")]
        public string URL { get; set; }


        [CreditCard(ErrorMessage = "La tarjeta de credito no es valida")]
        [Display(Name = "Tarjeta de credito")]
        public int TarjetaDeCredito { get; set; }
    }
}
