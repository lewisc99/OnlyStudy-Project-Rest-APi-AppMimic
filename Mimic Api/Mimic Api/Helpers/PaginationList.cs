using Mimic_Api.V1.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mimic_Api.Helpers
{
  /*  public class PaginationList<T>:List<T> */
  public class PaginationList<T>
    {
        public List<T> Resuls { get; set; } = new List<T>();
        public Paginacao Paginacao { get; set; }

        public List<LinkDTO> Links { get; set; } = new List<LinkDTO>();


    }
}
