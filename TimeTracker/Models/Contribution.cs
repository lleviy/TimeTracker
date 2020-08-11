using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace TimeTracker.Models
{
    public class Contribution
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public int TaskTypeId { get; set; }
        public TaskType TaskType { get; set; }

        [DataType(DataType.EmailAddress)]
        [DisplayName("Contributor e-mail")]
        public string ContributorEmail { get; set; }

        [DisplayName("Contribution confirmed")]
        public bool ContributionConfirmed { get; set; }
    }
}
