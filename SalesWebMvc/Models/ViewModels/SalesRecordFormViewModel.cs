using SalesWebMvc.Models;
using System.Collections.Generic;

public class SalesRecordFormViewModel
{
    public SalesRecord SalesRecord { get; set; }
    public IEnumerable<Seller> Sellers { get; set; }
}
