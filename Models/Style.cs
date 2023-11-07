using System;
using System.Collections.Generic;
#if NETCOREAPP || NET45 || NETSTANDARD
using System.ComponentModel.DataAnnotations;
#endif

namespace MediaStreamer.Domain.Models
{
    public class Style
    {
#if NETCOREAPP || NET45 || NETSTANDARD
        [Key]
#endif
        public Guid StyleId { get; set; }

#if NETCOREAPP || NET45 || NETSTANDARD
        [Required]
        [StringLength(256)]
        [DataType("vatchar(256)")]
        [MaxLength(256)]
#endif
        public string StyleName { get; set; }

        public virtual ICollection<Genre> Genres { get; set; }
    }
}