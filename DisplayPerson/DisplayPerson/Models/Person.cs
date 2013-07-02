using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using DisplayPerson.Core;
using DisplayPerson.DAL;

namespace DisplayPerson.Models
{
    public class RemoteValidatePerson
    {
        public static ValidationResult ValidateUniqueKeys(string pNome)
        {
            using (var context = new ContextDAL())
            {
                var repository = new Repository<Person>(context);
                var query = repository.List(x => x.Nome.ToLower().Contains(pNome.ToLower())).ToList();

                return query.Count() > 0 ? new ValidationResult(string.Format("A pessoa {0} já está cadastrada no banco de dados.", pNome)) : ValidationResult.Success;
            }
        }
    }

    [Table("Person")]
    public class Person
    {
        public Person()
        {
            this.Nome = string.Empty;
            this.Email = string.Empty;
            this.DataNascimento = DateTime.MinValue;
            this.Celular = string.Empty;
            this.TelefoneResidencial = string.Empty;
        }

        public Person(string fileLine)
        {
            this.Nome = fileLine.Substring(0, 20).Trim();
            this.Email = fileLine.Substring(20, 20).Trim();
            this.DataNascimento = Common.GetValueOrNull<DateTime>(Common.FormateDateString(fileLine)).Value;
            this.Celular = fileLine.Substring(48, 15).Trim();
            this.TelefoneResidencial = fileLine.Substring(63, 15).Trim();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "Campo nome não informado."), CustomValidation(typeof(RemoteValidatePerson), "ValidateUniqueKeys")]
        public string Nome { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "Campo e-mail não informado.")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Campo e-mail informado incorretamente.")]
        public string Email { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessage = "Campo data de nascimento não informado.")]
        [DataType(DataType.DateTime, ErrorMessage = "Campo data de nascimento informado incorretamente")]
        public DateTime DataNascimento { get; set; }

        [StringLength(15)]
        [Required(ErrorMessage = "Campo celular não informado.")]
        public string Celular { get; set; }

        [StringLength(15)]
        [Required(ErrorMessage = "Campo telefone residencial não informado.")]
        public string TelefoneResidencial { get; set; }
    }
}