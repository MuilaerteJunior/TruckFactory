namespace TruckFactory.Web.Domain.Data
{
    public class TruckEntity
    {
        public int? Id { get; set; }
        
        public string? Name { get; set; }

        public TruckModelEntity? Model { get; set; }

        public int ProductionYear { get; set; }//Must be current
        public int ModelYear { get; set; }//Current or >
        public int ID_TRUCK_MODEL { get;  set; }
    }
}
