namespace FashionStore.Entity.Entities
{
    public class Picture : EntityBase
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string PicturePath { get; set; }
        public string SeoFileName { get; set; }
        public string AltAttribute { get; set; }
        public string TitleAttribute { get; set; }

        public virtual Product Product { get; set; }
    }
}
