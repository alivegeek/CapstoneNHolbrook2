namespace CapstoneNHolbrook.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ColorCard
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string PreviousTint { get; set; }
        public string HairCondition { get; set; }
        public string NaturalColor { get; set; }
        public int PercentGrey { get; set; }
        public string Texture { get; set; }
        public string LightenerMixture { get; set; }
        public string ColorMixture { get; set; }

        // Foreign Key for Client
        public int ClientId { get; set; }
        [ForeignKey("ClientId")]
        public Client Client { get; set; }
    }
}
