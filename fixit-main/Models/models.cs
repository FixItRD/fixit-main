using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace fixit_main.Models
{
    public class models
    {
    }
    public class Ubicacion
    {
        [Key]
        public int ID_Ubicacion { get; set; }

        [Required]
        [StringLength(255)]
        public string Direccion { get; set; }

        public string Detalle { get; set; }

        [ForeignKey("Cliente")]
        public int ID_Cliente { get; set; }
        public Cliente Cliente { get; set; }
    }

    public class Cliente
    {
        [Key]
        public int ID_Cliente { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(15)]
        public string Telefono { get; set; }

        [Required]
        public DateTime Fecha_Registro { get; set; }

        public string Password { get; set; }

        public string Salt { get; set; }

        public ICollection<Ubicacion> Ubicaciones { get; set; }
        public ICollection<Servicio> Servicios { get; set; }
    }

    public class Servicio
    {
        [Key]
        public int ID_Servicio { get; set; }

        [Required]
        public DateTime Fecha_Solicitud { get; set; }

        public DateTime? Fecha_Realizacion { get; set; }

        [Required]
        [StringLength(50)]
        public string Estado { get; set; }

        [Required]
        public string Descripcion { get; set; }

        [ForeignKey("Cliente")]
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        [ForeignKey("Trabajador")]
        public int? TrabajadorId { get; set; }
        public Trabajador Trabajador { get; set; }

        [ForeignKey("CategoriaServicio")]
        public int Categoria_ServicioId { get; set; }
        public CategoriaServicio CategoriaServicio { get; set; }

        [ForeignKey("Calificacion")]
        public int? ID_Calificacion { get; set; }
        public Calificacion Calificacion { get; set; }

        public Factura Factura { get; set; }
    }

    public class Calificacion
    {
        [Key]
        public int ID_Calificacion { get; set; }

        [Required]
        [Range(1, 5)]
        public int Puntuacion { get; set; }

        public string Comentario { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [ForeignKey("Trabajador")]
        public int ID_Trabajador { get; set; }
        public Trabajador Trabajador { get; set; }

        [ForeignKey("Servicio")]
        public int ID_Servicio { get; set; }
        public Servicio Servicio { get; set; }
    }

    public class Trabajador
    {
        [Key]
        public int ID_Trabajador { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(15)]
        public string Telefono { get; set; }

        public string Password { get; set; }

        public string Salt { get; set; }

        [Required]
        public DateTime Fecha_Registro { get; set; }

        public ICollection<Servicio> Servicios { get; set; }
        public ICollection<Certificado> Certificados { get; set; }
        public ICollection<Calificacion> Calificaciones { get; set; }
        public ICollection<TrabajadorServicio> TrabajadorServicios { get; set; }
    }

    public class Certificado
    {
        [Key]
        public int ID_Certificado { get; set; }

        [Required]
        public DateTime Fecha_Otorgada { get; set; }

        [Required]
        [StringLength(100)]
        public string Area { get; set; }

        [ForeignKey("Trabajador")]
        public int ID_Trabajador { get; set; }
        public Trabajador Trabajador { get; set; }

        public bool Validado { get; set; }
    }

    [PrimaryKey("ID_Servicio", "ID_Trabajador")]
    public class TrabajadorServicio
    {
        [Column(Order = 1)]
        [ForeignKey("Servicio")]
        public int ID_Servicio { get; set; }
        public Servicio Servicio { get; set; }

        [Column(Order = 2)]
        [ForeignKey("Trabajador")]
        public int ID_Trabajador { get; set; }
        public Trabajador Trabajador { get; set; }
    }

    public class CategoriaServicio
    {
        [Key]
        public int ID_Servicio { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public string Imagen { get; set; }

        public decimal Precio { get; set; }

        public ICollection<Servicio> Servicios { get; set; }
    }

    public class Factura
    {
        [Key]
        public int ID_Factura { get; set; }

        [Required]
        public DateTime Fecha_Emision { get; set; }

        [Required]
        public decimal Monto_Total { get; set; }

        [Required]
        [StringLength(50)]
        public string Estado_Pago { get; set; }

        [ForeignKey("Servicio")]
        public int ID_Servicio { get; set; }
        public Servicio Servicio { get; set; }
    }
}
