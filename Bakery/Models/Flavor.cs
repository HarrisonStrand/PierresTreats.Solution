using System.Collections.Generic;

namespace Bakery.Models
{
  public class Flavor
  {
    public Flavor()
    {
      this.JoinEntries = new HashSet<FlavorTreat>();
    }
    public int FlavorId { get; set; }
    public string FlavorName { get; set; }
    public string StarRating { get; set; }
    public virtual ApplicationUser User { get; set; }
    public ICollection<FlavorTreat> JoinEntries { get; }
  }
}