using System.ComponentModel.DataAnnotations.Schema;

namespace RestApiDotNet.Model.Base
{
    public class BaseEntity
    {
        [Column("id")]
        public long Id { get; set; }
    }
}
