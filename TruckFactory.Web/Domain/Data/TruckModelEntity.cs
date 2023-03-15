namespace TruckFactory.Web.Domain.Data
{
    public class TruckModelEntity
    {

        public int Id { get; set; }

        public string? Name { get; set; }

        public short? EnumValue { get; set; }

        public IEnumerable<TruckEntity>? Trucks { get; set; }
    }
}
