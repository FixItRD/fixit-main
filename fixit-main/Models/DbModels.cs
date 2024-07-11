using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using fixit_main.Models.Templates;

namespace fixit_main.Models
{
    public class Ubicacion
    {
        [Key]
        public int ID_Ubicacion { get; set; }

        [Required]
        [StringLength(255)]
        public string Direccion { get; set; }

        public string Detalle { get; set; } = "";

        [Required]
        [ForeignKey("Cliente")]
        public int ID_Cliente { get; set; }
        public Cliente Cliente { get; set; }
    }

    public class Cliente : IUser
    {
        [Key]
        [Column("ID_Cliente")]
        public int ID { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(15)]
        public string Telefono { get; set; } = "";

        public DateTime Fecha_Registro { get; set; } = DateTime.Now;

        [Required]
        public string Password { get; set; }

        [Required]
        public string Salt { get; set; }

        public ICollection<Ubicacion> Ubicaciones { get; set; }
        public ICollection<Servicio> Servicios { get; set; }
    }

    public class Servicio
    {
        [Key]
        public int ID_Servicio { get; set; }

        public DateTime Fecha_Solicitud { get; set; } = DateTime.Now;

        public DateTime Fecha_Realizacion { get; set; } = DateTime.Now;

        public DateTime Fecha_Completado { get; set; } = DateTime.Parse("0001-01-01 00:00:00");

        [StringLength(50)]
        public string Estado { get; set; } = "pendiente";

        public string Descripcion { get; set; } = "";

        [Required]
        [ForeignKey("Cliente")]
        [Column("Cliente")]
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        [Required]
        [ForeignKey("Trabajador")]
        [Column("Trabajador")]
        public int TrabajadorId { get; set; }
        public Trabajador Trabajador { get; set; }

        [Required]
        [ForeignKey("CategoriaServicio")]
        [Column("Categoria_servicio")]
        public int Categoria_ServicioId { get; set; }
        public CategoriaServicio CategoriaServicio { get; set; }

        [ForeignKey("Calificacion")]
        public int ID_Calificacion { get; set; } = -1;
        public Calificacion Calificacion { get; set; }

        [Required]
        public int ID_Factura { get; set; }
        public Factura Factura { get; set; }
    }

    public class Calificacion
    {
        [Key]
        public int ID_Calificacion { get; set; }

        [Required]
        [Range(1, 5)]
        public int Puntuacion { get; set; }

        public string Comentario { get; set; } = "";

        public DateTime Fecha { get; set; } = DateTime.Now;

        [Required]
        [ForeignKey("Trabajador")]
        public int ID_Trabajador { get; set; }
        public Trabajador Trabajador { get; set; }

        [Required]
        [ForeignKey("Servicio")]
        public int ID_Servicio { get; set; }
        public Servicio Servicio { get; set; }
    }

    public class Trabajador : IUser
    {
        [Key]
        [Column("ID_Trabajador")]
        public int ID { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(15)]
        public string Telefono { get; set; } = "";

        [Required]
        public string Password { get; set; }

        [Required]
        public string Salt { get; set; }

        public string Disponibilidad { get; set; } = "";

        public DateTime Fecha_Registro { get; set; } = DateTime.Now;

        public ICollection<Servicio> Servicios { get; set; }
        public ICollection<Certificado> Certificados { get; set; }
        public ICollection<Calificacion> Calificaciones { get; set; }
        public ICollection<TrabajadorServicio> TrabajadorServicios { get; set; }
    }

    public class Certificado
    {
        [Key]
        public int ID_Certificado { get; set; }

        public DateTime Fecha_Otorgada { get; set; } = DateTime.Now;

        [Required]
        [StringLength(100)]
        [Column("Área")]
        public string Area { get; set; }

        [Required]
        [ForeignKey("Trabajador")]
        public int ID_Trabajador { get; set; }
        public Trabajador Trabajador { get; set; }

        [Required]
        public bool Validado { get; set; }
    }

    [Table("Trabajador_servicio")]
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

    [Table("Categoria_servicio")]
    public class CategoriaServicio
    {
        [Key]
        public int ID_Servicio { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        public string Descripcion { get; set; }

        [Required]
        public string Imagen { get; set; }

        [Required]
        public decimal Precio { get; set; }

        public ICollection<Servicio> Servicios { get; set; }
    }

    public class Factura
    {
        [Key]
        public int ID_Factura { get; set; }

        public DateTime Fecha_Emision { get; set; } = DateTime.Now;

        [Required]
        public decimal Monto_Total { get; set; }

        [StringLength(50)]
        public string Estado_Pago { get; set; } = "pendiente";

        [Required]
        [ForeignKey("Servicio")]
        public int ID_Servicio { get; set; }
        public Servicio Servicio { get; set; }
    }
}
